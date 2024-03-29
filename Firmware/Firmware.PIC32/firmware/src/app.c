#include "app.h"
#include "math.h"

#include "delay.h"
#include "audio.h"
#include "ios.h"
#include "memory.h"
#include "sounds_allocation.h"
#include "parallel_bus.h"

// *****************************************************************************
// *****************************************************************************
// Section: Global Data Definitions
// *****************************************************************************
// *****************************************************************************
DRV_HANDLE i2sDriverHandle;
DRV_I2S_BUFFER_EVENT_HANDLER i2sBufferEventHandler;
DRV_I2S_BUFFER_HANDLE i2sBufferHandle0;
DRV_I2S_BUFFER_HANDLE i2sBufferHandle1;

/* Allocate memory for the envelopes.
 * 32KBytes -> ~167 ms @ 192KHz
 * 32KBytes -> ~334 ms @ 96KHz
 */
#define ENVELOPE_LENGTH 1024*32
uint16_t envelope_internal[ENVELOPE_LENGTH];
uint16_t envelope_user[ENVELOPE_LENGTH];


#define AUDIO_BUFFER_LEN 2048/4
int audio_first_buffer[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer0[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer1[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer0_length;
int audio_buffer1_length;
//int audio_buffer0_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));
//int audio_buffer1_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));

int audio_sinewave[AUDIO_BUFFER_LEN] __attribute__((coherent));
float right_tetha = 0;
float left_tetha = 0;

int audio_all_first_buffers[32][AUDIO_BUFFER_LEN];
int audio_all_second_buffers[32][AUDIO_BUFFER_LEN];
Sound_Metadata audio_all_metadata[32];
bool audio_sound_exists[32];
unsigned int audio_sound_exists_bitmask = 0;
unsigned char audio_user_metadata[32][2048];

#define AUDIO_BUFFER_IS_EMPTY 0
#define AUDIO_BUFFER_HAS_DATA 1
volatile int audio_buffer0_state = AUDIO_BUFFER_IS_EMPTY;
volatile int audio_buffer1_state = AUDIO_BUFFER_IS_EMPTY;

#define NEW_SOUND_STATE_STANDBY 0
#define NEW_SOUND_STATE_IS_AVAILABLE 1
#define NEW_SOUND_STATE_FIRST_BUFFER_DONE 2
Sound_Metadata play_metadata;
int sound_length_produced;
bool sound_is_playing = false;
int new_sound_to_start = NEW_SOUND_STATE_STANDBY;
int new_sound_index;

#define SET_SOUND_IS_ON_WHEN_POSSIBLE 0
#define SET_SOUND_IS_ON_DONE 1
int set_sound_is_on_when_possible = SET_SOUND_IS_ON_DONE;
#define CLR_SOUND_IS_ON_WHEN_POSSIBLE 0
#define CLR_SOUND_IS_ON_DONE 1
int clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_DONE;
int clr_sound_is_on_num_samples;

/* Receive data buffer */
uint8_t receivedDataBuffer[APP_READ_BUFFER_SIZE] APP_MAKE_BUFFER_DMA_READY;
/* Transmit data buffer */
uint8_t  transmitDataBuffer[APP_READ_BUFFER_SIZE] APP_MAKE_BUFFER_DMA_READY;
/* The endpoint size is 64 for FS and 512 for HS */
uint16_t endpointSize;

int sound_index_to_write;
int max_sound_data_index;

volatile int dma_i2s_handle0_timeout = 0;
volatile int dma_i2s_handle1_timeout = 0;

bool metadataCmd_received = false;
bool dataCmd_received = false;
bool readMetadataCmd_received = false;

void handle_USB_writing(void);
void process_metadataCmd(void);
void process_dataCmd(void);
void prepare_metadataCmd(void);
void prepare_dataCmd(void);

int current_sample_rate = 96000;


// *****************************************************************************
/* Application Data

  Summary:
    Holds application data

  Description:
    This structure holds the application's data.

  Remarks:
    This structure should be initialized by the APP_Initialize function.
    
    Application strings and buffers are be defined outside this structure.
*/

APP_DATA appData;

// *****************************************************************************
// *****************************************************************************
// Section: Application Callback Functions
// *****************************************************************************
// *****************************************************************************
void I2SBufferEventHandler( DRV_I2S_BUFFER_EVENT event,
                            DRV_I2S_BUFFER_HANDLE bufferHandle,
                            uintptr_t context )
{
    switch (event)
    {
        case DRV_I2S_BUFFER_EVENT_COMPLETE:
        {
            if (bufferHandle == i2sBufferHandle0)
            {
                if (audio_buffer0_state == AUDIO_BUFFER_HAS_DATA)
                    audio_buffer0_state = AUDIO_BUFFER_IS_EMPTY;
                
                if (set_sound_is_on_when_possible == SET_SOUND_IS_ON_WHEN_POSSIBLE)
                {
                    clr_SOUND_IS_ON;
                    set_sound_is_on_when_possible = SET_SOUND_IS_ON_DONE;
                    trigger_pin_sound_is_on(current_sample_rate);
                }
                
                if (clr_sound_is_on_when_possible == CLR_SOUND_IS_ON_WHEN_POSSIBLE)
                {
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_DONE;
                    tgl_TP0;
                    trigger_pin_sound_is_off(current_sample_rate, clr_sound_is_on_num_samples);
                }

                dma_i2s_handle0_timeout = 0;
            }
            if (bufferHandle == i2sBufferHandle1)
            {
                if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
                    audio_buffer1_state = AUDIO_BUFFER_IS_EMPTY;
                
                if (set_sound_is_on_when_possible == SET_SOUND_IS_ON_WHEN_POSSIBLE)
                {
                    clr_SOUND_IS_ON;
                    set_sound_is_on_when_possible = SET_SOUND_IS_ON_DONE;
                    trigger_pin_sound_is_on(current_sample_rate);
                }
                
                if (clr_sound_is_on_when_possible == CLR_SOUND_IS_ON_WHEN_POSSIBLE)
                {
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_DONE;
                    tgl_TP0;
                    trigger_pin_sound_is_off(current_sample_rate, clr_sound_is_on_num_samples);
                }
                
                dma_i2s_handle1_timeout = 0;
            }
        }
    }
}

void APP_USBDeviceEventHandler(USB_DEVICE_EVENT event, void * eventData, uintptr_t context)
{
    uint8_t * configurationValue;
    USB_SETUP_PACKET * setupPacket;
    switch(event)
    {
        case USB_DEVICE_EVENT_RESET:
        case USB_DEVICE_EVENT_DECONFIGURED:

            /* Device is reset or deconfigured. Provide LED indication.*/

            appData.deviceIsConfigured = false;

            break;

        case USB_DEVICE_EVENT_CONFIGURED:

            /* Check the configuration */
            configurationValue = (uint8_t *)eventData;
            if(*configurationValue == 1 )
            {
                /* The device is in configured state. Update LED indication */
                clr_LED_USB;

                /* Reset endpoint data send & receive flag  */
                appData.deviceIsConfigured = true;
            }
            break;

        case USB_DEVICE_EVENT_SUSPENDED:

            /* Device is suspended. Update LED indication */
            set_LED_USB;
            
            break;


        case USB_DEVICE_EVENT_POWER_DETECTED:

            /* VBUS is detected. Attach the device */
            USB_DEVICE_Attach(appData.usbDevHandle);
            break;

        case USB_DEVICE_EVENT_POWER_REMOVED:
            
            /* VBUS is removed. Detach the device */
            USB_DEVICE_Detach (appData.usbDevHandle);
            break;

        case USB_DEVICE_EVENT_CONTROL_TRANSFER_SETUP_REQUEST:
            /* This means we have received a setup packet */
            setupPacket = (USB_SETUP_PACKET *)eventData;
            if(setupPacket->bRequest == USB_REQUEST_SET_INTERFACE)
            {
                /* If we have got the SET_INTERFACE request, we just acknowledge
                 for now. This demo has only one alternate setting which is already
                 active. */
                USB_DEVICE_ControlStatus(appData.usbDevHandle,USB_DEVICE_CONTROL_STATUS_OK);
            }
            else if(setupPacket->bRequest == USB_REQUEST_GET_INTERFACE)
            {
                /* We have only one alternate setting and this setting 0. So
                 * we send this information to the host. */

                USB_DEVICE_ControlSend(appData.usbDevHandle, &appData.altSetting, 1);
            }
            else
            {
                /* We have received a request that we cannot handle. Stall it*/
                USB_DEVICE_ControlStatus(appData.usbDevHandle, USB_DEVICE_CONTROL_STATUS_ERROR);
            }
            break;

        case USB_DEVICE_EVENT_ENDPOINT_READ_COMPLETE:
           /* Endpoint read is complete */
            appData.epDataReadPending = false;
            break;

        case USB_DEVICE_EVENT_ENDPOINT_WRITE_COMPLETE:
            /* Endpoint write is complete */
            appData.epDataWritePending = false;
            break;

        /* These events are not used in this demo. */
        case USB_DEVICE_EVENT_RESUMED:
        case USB_DEVICE_EVENT_ERROR:
        default:
            break;
    }
}

// *****************************************************************************
// *****************************************************************************
// Section: Application Local Functions
// *****************************************************************************
// *****************************************************************************
void reset_PIC32(void)
{
    // MUST disable interrupts and dma, as reset timing is critical
    // use the compiler built-in function to disable interrupts
    SYS_INT_StatusGetAndDisable();
    // use the Peripheral Library Function to suspend the DMA
    PLIB_DMA_SuspendEnable(DMA_ID_0); // only required if you are using DMA
    // use the System Service Library Function to initiate the reset
    SYS_RESET_SoftwareReset(); // to perform the reset
}

void populate_envelope_internal(void)
{
    float pi = 3.14159265359;
    float sample;
    int i = 0;

    for (i; i < ENVELOPE_LENGTH; i++)
    {
        //envelope_internal[i] = 0.5 * (1 - cos((2 * pi * i) / (ENVELOPE_LENGTH - 1 )));
        sample = sin((pi * i) / (ENVELOPE_LENGTH * 2 - 1 )) * sin((pi * i) / (ENVELOPE_LENGTH * 2 - 1 )) * 32767;
        envelope_internal[i] = sample;
        
        if (sample - ((uint16_t) sample) >= 0.5)
            envelope_internal[i]++;
                
    }
}

void update_sound_buffers (void);

/* Start a new sound if:
 * - the selected index has a sound,
 * - no sound is initiating, and
 * - no sound is being produced.
 */
bool check_cmd_start(int index)
{
    if (!audio_sound_exists[index])
        return false;    
    
    //if (new_sound_to_start != NEW_SOUND_STATE_STANDBY)
        //return false;
    
    //if (sound_is_playing)
    //    return false;
    
    return true;
}

int launch_sound_v3(void/*int index*/)
{
    //clr_AUDIO_MUTE;
    new_sound_to_start = NEW_SOUND_STATE_IS_AVAILABLE;    
    //new_sound_index = index;
    play_metadata = audio_all_metadata[new_sound_index];
    
    return 0;
}


//http://www.dcs.gla.ac.uk/~jhw/cordic/

//Cordic in 32 bit signed fixed point math
//Function is valid for arguments in range -pi/2 -- pi/2
//for values pi/2--pi: value = half_pi-(theta-half_pi) and similarly for values -pi---pi/2
//
// 1.0 = 1073741824
// 1/k = 0.6072529350088812561694
// pi = 3.1415926535897932384626
//Constants
#define cordic_1K 0x26DD3B6A
#define half_pi 0x6487ED51
#define MUL 1073741824.000000
#define CORDIC_NTAB 32
int cordic_ctab [] = {0x3243F6A8, 0x1DAC6705, 0x0FADBAFC, 0x07F56EA6, 0x03FEAB76, 0x01FFD55B, 
0x00FFFAAA, 0x007FFF55, 0x003FFFEA, 0x001FFFFD, 0x000FFFFF, 0x0007FFFF, 0x0003FFFF, 
0x0001FFFF, 0x0000FFFF, 0x00007FFF, 0x00003FFF, 0x00001FFF, 0x00000FFF, 0x000007FF, 
0x000003FF, 0x000001FF, 0x000000FF, 0x0000007F, 0x0000003F, 0x0000001F, 0x0000000F, 
0x00000008, 0x00000004, 0x00000002, 0x00000001, 0x00000000, };

int cordic(int theta, /*int *s, */ /*int *c, */int n)
{
  int k, d, tx, ty, tz;
  int x=cordic_1K,y=0,z=theta;
  // n = (n>CORDIC_NTAB) ? CORDIC_NTAB : n;
  for (k=0; k<n; ++k)
  {
    d = z>>31;
    //get sign. for other architectures, you might want to use the more portable version
    //d = z>=0 ? 0 : -1;
    tx = x - (((y>>k) ^ d) - d);
    ty = y + (((x>>k) ^ d) - d);
    tz = z - ((cordic_ctab[k] ^ d) - d);
    x = tx; y = ty; z = tz;
  }  
 /*c = x;*/ /**s = y;*/
            return y;   // sine
}

int right_sinewave_freq = 0;
int left_sinewave_freq = 0;

int new_right_sinewave_freq;
bool new_sine_gen_frequency_is_available = false;
bool stop_sine_gen = false;

int right_mult = 1;
int left_mult = 1;

bool right_positive = true;
bool left_positive = true;


#define SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N 64
#define CORDIC_ITERACTIONS 12

int freqs[4] = {1000, 1234, 1000, 1234};
int freqs_index = 0;
int counter = 0;

void proc_sinewave_generator(void)
{
    int i = 0;
    
    if (new_sine_gen_frequency_is_available)
        if (right_sinewave_freq == 0)
        {
            new_sine_gen_frequency_is_available = false;

            right_sinewave_freq = new_right_sinewave_freq;
            
            //set_SOUND_IS_ON;
            trigger_pin_sinewave_is_on(96000, 0);
        }
    
    if (right_sinewave_freq != 0)
    {
        set_LED_AUDIO;
            
        for (; i < SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N/2; i++)
        {                
            right_tetha = right_tetha + (right_mult)*(half_pi/(96000.0/(right_sinewave_freq<<2)));

            if (right_tetha > half_pi)
            {
                right_mult = -1;
                right_tetha = half_pi - (right_tetha - half_pi);
            }
            if (right_tetha < -half_pi)
            {
                right_mult = 1;
                right_tetha = -half_pi - (right_tetha + half_pi);
            }

            audio_sinewave[i*2+1] = cordic((int)(right_tetha), CORDIC_ITERACTIONS);
            
            if (right_tetha > 0)
            {   
                //clr_SOUND_IS_ON;
                
                if (right_positive == false)
                {
                    if (new_sine_gen_frequency_is_available)
                    {
                        new_sine_gen_frequency_is_available = false;
                        
                        if (right_sinewave_freq != new_right_sinewave_freq)
                        {
                            /* Update only if the new frequency is different */
                            right_sinewave_freq = new_right_sinewave_freq;

                            //set_SOUND_IS_ON;
                            trigger_pin_sinewave_is_on(96000, i);
                        }
                    }
                    
                    if (stop_sine_gen)
                    {
                        right_sinewave_freq = 0;
                    }
                }
                
                right_positive = true;
            }
            else
            {
                right_positive = false;
            }
        }
    }
    else
    {
        if (new_sine_gen_frequency_is_available)
        {
            new_sine_gen_frequency_is_available = false;

            right_sinewave_freq = new_sound_index;
        }
        
        if (stop_sine_gen)
        {
            stop_sine_gen = false;
            
            clr_LED_AUDIO;

            for (; i < SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N/2; i++)
            {
                audio_sinewave[i*2+1] = 0;
            }

            right_tetha = 0;
            right_mult = 1;
        }
    }
}

void update_sound_buffers (void)
{
    int i = 0;
    
    if (/*!sound_is_playing && */new_sound_to_start == NEW_SOUND_STATE_IS_AVAILABLE)
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY || audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            new_sound_to_start = NEW_SOUND_STATE_FIRST_BUFFER_DONE;
            set_sound_is_on_when_possible = SET_SOUND_IS_ON_WHEN_POSSIBLE;
            sound_is_playing = true;
            set_page_and_sound_index(1, new_sound_index);
            
            config_audio_dac(play_metadata.sample_rate, (play_metadata.sample_rate != current_sample_rate) ? true : false);
            current_sample_rate = play_metadata.sample_rate;
            
            if (play_metadata.sound_length > AUDIO_BUFFER_LEN)
            {
                sound_length_produced = AUDIO_BUFFER_LEN;
                
                if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_all_first_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    set_LED_AUDIO;
                }
                else
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_first_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    set_LED_AUDIO;
                }
            }
            else
            {
                sound_length_produced = play_metadata.sound_length;
                
                if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_all_first_buffers[new_sound_index], play_metadata.sound_length * 4);
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    set_LED_AUDIO;
                }
                else
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_first_buffers[new_sound_index], play_metadata.sound_length * 4);
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    set_LED_AUDIO;
                }                
            }
        }
    }
    
    if (new_sound_to_start == NEW_SOUND_STATE_FIRST_BUFFER_DONE)
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
        {
            new_sound_to_start = NEW_SOUND_STATE_STANDBY;
            
            if (play_metadata.sound_length > sound_length_produced)
            {
                if (play_metadata.sound_length - sound_length_produced > AUDIO_BUFFER_LEN)
                {
                    //audio_buffer0_length = AUDIO_BUFFER_LEN;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_all_second_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    sound_length_produced += AUDIO_BUFFER_LEN;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                }
                else
                {
                    //audio_buffer0_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_all_second_buffers[new_sound_index], (play_metadata.sound_length - sound_length_produced) * 4);                    
                    clr_sound_is_on_num_samples = play_metadata.sound_length - sound_length_produced;
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_WHEN_POSSIBLE;
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_LED_AUDIO;
            }
            
            //if (audio_buffer0_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, audio_buffer0_length * 4);
            //}
        }
        
        if (audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            new_sound_to_start = NEW_SOUND_STATE_STANDBY;
            
            if (play_metadata.sound_length > sound_length_produced)
            {
                if (play_metadata.sound_length - sound_length_produced > AUDIO_BUFFER_LEN)
                {                    
                    //audio_buffer1_length = AUDIO_BUFFER_LEN;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_second_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    sound_length_produced += AUDIO_BUFFER_LEN;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                }
                else
                {
                    //audio_buffer1_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_second_buffers[new_sound_index], (play_metadata.sound_length - sound_length_produced) * 4);                    
                    clr_sound_is_on_num_samples = play_metadata.sound_length - sound_length_produced;
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_WHEN_POSSIBLE;
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_LED_AUDIO;
            }
            
            //if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, audio_buffer1_length * 4);
            //}
        }
    }
    
    
    if (/*sound_is_playing && */new_sound_to_start == NEW_SOUND_STATE_STANDBY)
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (play_metadata.sound_length > sound_length_produced)
            {
                set_LED_MEMORY;
                read_next_sound_page(audio_buffer0);
                clr_LED_MEMORY;

                if (play_metadata.sound_length - sound_length_produced > AUDIO_BUFFER_LEN)
                {
                    //audio_buffer0_length = AUDIO_BUFFER_LEN;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, AUDIO_BUFFER_LEN * 4);
                    sound_length_produced += AUDIO_BUFFER_LEN;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
            
                    handle_USB_writing();
                }
                else
                {
                    //audio_buffer0_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, (play_metadata.sound_length - sound_length_produced) * 4);                    
                    clr_sound_is_on_num_samples = play_metadata.sound_length - sound_length_produced;
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_WHEN_POSSIBLE;
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                //clr_SOUND_IS_ON;
                clr_LED_AUDIO;
                //trigger_delayed_stop(current_sample_rate, 256);
            }
            
            //if (audio_buffer0_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, audio_buffer0_length * 4);
            //}
        }
        
        if (audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (play_metadata.sound_length > sound_length_produced)
            {
                set_LED_MEMORY;
                read_next_sound_page(audio_buffer1);
                clr_LED_MEMORY;

                if (play_metadata.sound_length - sound_length_produced > AUDIO_BUFFER_LEN)
                {                    
                    //audio_buffer1_length = AUDIO_BUFFER_LEN;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, AUDIO_BUFFER_LEN * 4);
                    sound_length_produced += AUDIO_BUFFER_LEN;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
            
                    handle_USB_writing();
                }
                else
                {
                    //audio_buffer1_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, (play_metadata.sound_length - sound_length_produced) * 4);
                    clr_sound_is_on_num_samples = play_metadata.sound_length - sound_length_produced;
                    clr_sound_is_on_when_possible = CLR_SOUND_IS_ON_WHEN_POSSIBLE;
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                //clr_SOUND_IS_ON;
                clr_LED_AUDIO;
                //trigger_delayed_stop(current_sample_rate, 256);
            }
            
            //if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, audio_buffer1_length * 4);
            //}
        }
    }
    
    if (!sound_is_playing)
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (current_sample_rate == 96000)
            {
                //DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer_zeros, 32 * 4);
                //DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer_zeros, 64 * 4);
                
                /*
                for (; i < 256/2; i++)
                {
                    _X_ = _X_ + 2.0*3.141592654/(96000.0/3000.0);
                    audio_sinewave[i*2] = 16777216/2 * sin(_X_);
                    //audio_sinewave[i*2+1] = audio_sinewave[i*2];                    
                }
                */
                        
                proc_sinewave_generator();
                
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_sinewave, SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N * 4);
            }
            else
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer_zeros, 128 * 4); // 8 works well
            
            //clr_SOUND_IS_ON;
            audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
        }
        
        if (audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (current_sample_rate == 96000)
            {
                //DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer_zeros, 32 * 4);
                //DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer_zeros, 64 * 4);
                
                /*for (; i < 256/2; i++)
                {
                    _X_ = _X_ + 2.0*3.141592654/(96000.0/3000.0);
                    audio_sinewave[i*2] = 16777216/2 * sin(_X_);
                    //audio_sinewave[i*2+1] = audio_sinewave[i*2];
                }
                */
                
                proc_sinewave_generator();
                
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_sinewave, SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N * 4);
            }
            else
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer_zeros, 128 * 4); // 8 works well
            
            //clr_SOUND_IS_ON;
            audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
            
            handle_USB_writing();
        }
    }
}


void fill_audio_first_and_second_buffers()
{
    int n_sounds = get_available_sounds();

    int i = 2;
    for (; i < n_sounds; i++)
    {
        audio_sound_exists[i] = (read_first_sound_page(i, audio_all_first_buffers[i], &audio_all_metadata[i]) != -1) ? true : false;
        
        if (audio_sound_exists[i])
            read_next_sound_page(audio_all_second_buffers[i]);
    }
}

void fill_audio_user_metadata()
{
    int n_sounds = get_available_sounds();
    
    audio_sound_exists[0] = false;
    audio_sound_exists[1] = false;
    
    int i = 2;
    for (; i < n_sounds; i++)
    {   
        if (audio_sound_exists[i])
        {
            audio_sound_exists_bitmask |= (1 << i);
            read_user_metadata(i, audio_user_metadata[i]);
        }
    }
}

void reply_USB(int nBytes)
{
     USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                            appData.endpointTx, &transmitDataBuffer[0],
                            nBytes,
                            USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);

    appData.epDataReadPending = true ;

    /* Place a new read request. */
    USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                        appData.endpointRx, &receivedDataBuffer[0],
                        sizeof(receivedDataBuffer) );
    
    clr_LED_USB;
}

void handle_USB_writing(void)
{
    if (dataCmd_received == true)
        process_dataCmd();

    if (metadataCmd_received == true)
        process_metadataCmd();
}

#define METADATACMD_STATE_SAVE_USER_METADATA 0
#define METADATACMD_STATE_SAVE_PREPARE_MEMORY 1
#define METADATACMD_STATE_SAVE_ALLOCATE_METADATA 2
int prepare_metadataCmd_state;

int prepare_metadataCmd_sound_index;

void prepare_metadataCmd(void)
{
    int i;
                                
    Sound_Metadata * ptr = (Sound_Metadata*)(receivedDataBuffer + 8);
    prepare_metadataCmd_sound_index = ptr->sound_index;
    int *error = (int*)(transmitDataBuffer + 8);
    *error = ERROR_NOERROR;

    ptr->sound_length = ptr->sound_length & 0xFFFFFFFC; // Samples must be multiple of 4
                                                        // The DMA stops if loaded with 2 samples at 192KHz

    if (ptr->sound_index < 0 || ptr->sound_index > get_available_sounds()) *error = ERROR_BADSOUNDINDEX;
    if (ptr->sound_length < 16) *error = ERROR_BADSOUNDLENGTH;
    if (ptr->sample_rate != 96000 && ptr->sample_rate != 192000) *error = ERROR_BADSAMPLERATE;
    if (ptr->data_type != 0 && ptr->data_type != 1) *error = ERROR_BADDATATYPE;
    if (ptr->sound_index == 0 && ptr->data_type != 1) *error = ERROR_BADDATATYPEMATCH;
    if (ptr->sound_index == 1 && ptr->data_type != 1) *error = ERROR_BADDATATYPEMATCH;
    if (ptr->sound_index > 1 && ptr->data_type != 0) *error = ERROR_BADDATATYPEMATCH;

    if (current_sample_rate == 192000)
        if (sound_is_playing)
            *error = ERROR_PRODUCINGSOUND;
    
    if (prepare_memory_check(ptr->sound_index, ptr->sound_length) == false) *error = ERROR_BADSOUNDLENGTH;
    
    for (i = 8; i != 0; i--)
        transmitDataBuffer[i-1] = receivedDataBuffer[i-1];

    if (*error == ERROR_NOERROR)
    {
        prepare_metadataCmd_state = METADATACMD_STATE_SAVE_USER_METADATA;
        
        audio_all_metadata[ptr->sound_index] = *ptr;
        audio_sound_exists[ptr->sound_index] = true;
        audio_sound_exists_bitmask |= (1 << ptr->sound_index);
        
        for (i = 2048; i != 0; i--)
            audio_user_metadata[ptr->sound_index][i-1] = receivedDataBuffer[4+4+16+32768+i-1];
        
        int * first_buffer_index = ((int*)(&receivedDataBuffer[4+4+16]));
        for (i = AUDIO_BUFFER_LEN; i != 0; i--)
        {
            audio_all_first_buffers[ptr->sound_index][i-1] = *(first_buffer_index +i-1);
            audio_all_second_buffers[ptr->sound_index][i-1] = *( ((int*)(&receivedDataBuffer[4+4+16+AUDIO_BUFFER_LEN*4])) +i-1);
        }
        
        audio_all_metadata[0] = *ptr;
        
        for (i = AUDIO_BUFFER_LEN; i != 0; i--)
        {
            audio_all_first_buffers[0][i-1] = 0;
            audio_all_second_buffers[0][i-1] = 0;
        }
        
        //set_LED_MEMORY;

        //save_user_metadata(ptr->sound_index, &receivedDataBuffer[4+4+16+32768]);

        /* Erase memory that will be used for the sound data */
        //if (prepare_memory(ptr->sound_index, ptr->sound_length) == -1) *error = ERROR_BADSOUNDLENGTH;

        //allocate_metadata_command (*ptr, &receivedDataBuffer[4+4+16]);

        //clr_LED_MEMORY;

        sound_index_to_write = ptr->sound_index;
        max_sound_data_index = 0x7FFFFFFF;  // Code to be done        
        
        metadataCmd_received = true;
    }
    else
    {
        sound_index_to_write = -1;
        
        reply_USB(12);
    }
    
    //reply_USB(8);
}

void process_metadataCmd(void)
{
    set_LED_MEMORY;
    
    int *error = (int*)(transmitDataBuffer + 8);
    
    if (current_sample_rate == 192000)
        if (sound_is_playing)
        {
            *error = ERROR_PRODUCINGSOUND;
            
            metadataCmd_received = false;
            
            clr_LED_MEMORY;

            reply_USB(12);
            return;
        }
    
    if(prepare_metadataCmd_state == METADATACMD_STATE_SAVE_USER_METADATA)
    {
        if (save_user_metadata(prepare_metadataCmd_sound_index, &receivedDataBuffer[4+4+16+32768]) == true)
        {
            prepare_metadataCmd_state = METADATACMD_STATE_SAVE_PREPARE_MEMORY;
        }
        
        return;
    }
    
    if (prepare_metadataCmd_state == METADATACMD_STATE_SAVE_PREPARE_MEMORY)
    {
        if (prepare_memory_erase() == true)
        {
            prepare_metadataCmd_state = METADATACMD_STATE_SAVE_ALLOCATE_METADATA;
        }
        
        return;
    }
    
    if (prepare_metadataCmd_state == METADATACMD_STATE_SAVE_ALLOCATE_METADATA)
    {
        Sound_Metadata * ptr = (Sound_Metadata*)(receivedDataBuffer + 8);
        if (allocate_metadata_command (*ptr, &receivedDataBuffer[4+4+16]) == true)
        {        
            metadataCmd_received = false;
            
            clr_LED_MEMORY;

            reply_USB(12);
        }
    }
}

int process_dataCmd_data_index;

void prepare_dataCmd(void)
{
    int i;
    
    int *error = (int*)(transmitDataBuffer + 8); 
    process_dataCmd_data_index = *((int*)(receivedDataBuffer + 8));
    *error = ERROR_NOERROR;
   
    /*if (process_dataCmd_data_index > max_sound_data_index) *error = ERROR_BADDATAINDEX;    
    if (current_sample_rate == 192000)
        if (sound_is_playing)
            *error = ERROR_PRODUCINGSOUND;
    */
    
    for (i = 8; i != 0; i--)
        transmitDataBuffer[i-1] = receivedDataBuffer[i-1];

    if (*error == ERROR_NOERROR)
    {
        allocate_data_command_reset();
        dataCmd_received = true;
    }
    else
    {
        reply_USB(12);
    }
}

void process_dataCmd(void)
{
    set_LED_MEMORY;
    
    int *error = (int*)(transmitDataBuffer + 8);
    
    if (current_sample_rate == 192000)
        if (sound_is_playing)
        {
            *error = ERROR_PRODUCINGSOUND;
            
            dataCmd_received = false;
            
            clr_LED_MEMORY;

            reply_USB(12);
            return;
        }
    
    if (allocate_data_command(sound_index_to_write, process_dataCmd_data_index, &receivedDataBuffer[4+4+4]) == 32768/BYTES_PER_PAGE)
    {
        dataCmd_received = false;
        
        clr_LED_MEMORY;
        
        reply_USB(12);
    }
}

void process_readMetadataCmd(void)
{    
    int i;
    int *error = (int*)(transmitDataBuffer + 8);
    int *index = (int*)(receivedDataBuffer + 8);
    *error = ERROR_NOERROR;
    
    if (*index < 0 || *index > 31) *error = ERROR_BADSOUNDINDEX;                                
    //if (sound_is_playing) *error = ERROR_PRODUCINGSOUND;

    if (*error == ERROR_NOERROR)
    {
        /* Load user's metadata */
        for (i = 2048; i != 0; i--)
            transmitDataBuffer[28 + i-1] = audio_user_metadata[*index][i-1];

        /* Load audio exists bitmask */
        *((unsigned int*)(&transmitDataBuffer[12])) = audio_sound_exists_bitmask;

        /* Load sound's metadata */
        for (i = 3*4; i != 0; i--)
            transmitDataBuffer[16 + i-1] = *( ((unsigned char *)(&audio_all_metadata[*index])) + 4 + i-1);
    }

    for (i = 8; i != 0; i--)
        transmitDataBuffer[i-1] = receivedDataBuffer[i-1];    
    
    reply_USB(2076);
}

// *****************************************************************************
// *****************************************************************************
// Section: Application Initialization and State Machine Functions
// *****************************************************************************
// *****************************************************************************

/*******************************************************************************
  Function:
    void APP_Initialize ( void )

  Remarks:
    See prototype in app.h.
 */

void APP_Initialize ( void )
{    
    int i = 0;
    /*
     * Place the App state machine in its initial state.
     */
    appData.state = APP_STATE_INIT;

    appData.usbDevHandle = USB_DEVICE_HANDLE_INVALID;
    appData.deviceIsConfigured = false;
    appData.endpointRx = (APP_EP_BULK_OUT | USB_EP_DIRECTION_OUT);
    appData.endpointTx = (APP_EP_BULK_IN | USB_EP_DIRECTION_IN);
    appData.epDataReadPending = false;
    appData.epDataWritePending = false;
    appData.altSetting = 0;
    
    /* 
     * Read the reset reason.
     * 
     * Typical reasons:
     *  RESET_REASON_POWERON 3
     *  RESET_REASON_WDT_TIMEOUT 16
     *  RESET_REASON_SOFTWARE 64
     *  RESET_REASON_MCLR 128
     */    
    RESET_REASON reasonType;
    reasonType = SYS_RESET_ReasonGet();    
    SYS_RESET_ReasonClear(reasonType);
    
    /* 
     * Initialize IOs.
     * 
     *  The MEMORY LED will stay ON if no communication with the memory.
     *  The AUDIO LED will be always OFF.
     *  The USB LED will be maintained ON. Will be OFF when communication is detected.
     */
    initialize_ios((int) reasonType);   // Will leave all LEDs on
    
    /*
    for (; i < 64/2; i++)
    {
        _X_ = _X_ + 2.0*3.141592654/(96000.0/3000.0);
        audio_sinewave[i*2] = 16777216/2 * sin(_X_);
        audio_sinewave[i*2+1] = audio_sinewave[i*2];
    }
    */
    
    //right_sinewave_freq = 200;
    
    /*
     * Clear the sinewave generator's buffers
     */
    for (; i < SINEWAVE_GEN_96KHZ_LOAD_SAMPLE_N/2; i++)
    {
        audio_sinewave[i*2+0] = 0;  // Left
        audio_sinewave[i*2+1] = 0;  // Right
    }
    
    initialize_audio_ios((int) reasonType);
    clr_LED_AUDIO;                      // There is no way to check the audio circuit
                                        // Turn AUDIO LED off         
    initialize_memory_ios();
    if (check_memory_connection() != -1) 
    {
        fill_audio_first_and_second_buffers();
        fill_audio_user_metadata();
        clr_LED_MEMORY;                 // Memory is OK, turn MEMORY LED off
    }
    
    initialize_par_ios();
    
    /*
    for (i = 0; i < 50/2; i++)
    {
        set_LED_AUDIO;
        _ms_delay(100);
        clr_LED_AUDIO;
        _ms_delay(100);
    }
    */
    
    /*
     * Debug purpose only.
     * Next code lines should be commented for the code to work properly.
     */
    // To run USB on board v0.1
    //ANSELG  &= ~(1 << 9);   // R/B# @ RG9 (pin 16) as input    
    // To run USB on board v1.0
    //cfg_AUDIO_RESET;
    //set_AUDIO_RESET;
    
    /* 
     * Configure audio DAC IC.
     */
    config_audio_dac(current_sample_rate, true);
    
    /* 
     * Initialize internal envelope.
     */
    populate_envelope_internal();
    
    /* 
     * Clean all metadata in the memory.
     */
    //clean_memory();
    
    /*
     * Initialize timers     
     */
    INTCONbits.MVEC = 1;
    config_timer_for_pin_sound_is_on();
    config_timer_for_pin_sound_is_off();
    
    /* 
     * Create the I2S driver handle.
     */
    i2sDriverHandle = DRV_I2S_Open(DRV_I2S_INDEX_0, DRV_IO_INTENT_WRITE);
    if (i2sDriverHandle != DRV_HANDLE_INVALID)
    {
        //DRV_I2S_TransmitErrorIgnore(i2sDriverHandle, true);
        //DRV_I2S_ReceiveErrorIgnore(i2sDriverHandle, true);        
        //DRV_I2S_BaudSet(i2sDriverHandle, (samplingRate*BCLK_BIT_CLK_DIVISOR), samplingRate);
    }
    else
    {
        ;
    }
     
    /* 
     * Register I2S callback routine for handler events.
     */
    DRV_I2S_BufferEventHandlerSet(i2sDriverHandle, I2SBufferEventHandler, NULL);
}


/******************************************************************************
  Function:
    void APP_Tasks ( void )

  Remarks:
    See prototype in app.h.
 */

int command_received = 0;
//

void APP_Tasks ( void )
{   
    update_sound_buffers();
    
    if (command_received != 0)
    {
        switch(command_received)
        {
            case CMD_START:
            case CMD_UPDATE_FREQUENCY:
                
                switch(command_received)
                {
                    case CMD_START:
                        new_sound_index = par_bus_process_command_start();
                        break;
                        
                    case CMD_UPDATE_FREQUENCY:
                        new_sound_index = par_bus_process_command_update_frequency();
                        break;
                }
                
                if (new_sound_index < 32)
                {
                    if (check_cmd_start(new_sound_index))
                    {
                        launch_sound_v3(/*new_sound_index*/);
                    
                        stop_sine_gen = true;
                    }
                }
                else if (new_sound_index < 40000)
                {
                    new_right_sinewave_freq = new_sound_index;
                    new_sine_gen_frequency_is_available = true;
                }
                
                break;
            
            case CMD_STOP:
                if (right_sinewave_freq == 0)   // Sinewave generator is not working
                {
                    par_bus_process_command_stop();
                }
                else
                {
                    stop_sine_gen = true;
                }
                break;
        }
        
        command_received = 0;
    }
    
    
    
    /*
    set_TP1;
    int i = 0;
    for (; i < 512; i++)
    {
        audio_buffer0[i] = audio_all_first_buffers[2][i] * (envelope_user[i] / 32768);
        audio_buffer1[i] = audio_all_first_buffers[3][i] * (envelope_internal[i] / 32768);
    }
    clr_TP1;
    */
        
    
    command_received = par_bus_check_if_command_is_available();
    
    /* 
     * Checks if the I2S DMA is working.
     * Issue a software reset if not.
     * Usually, this test is performed each ~1.5 us
     */
    if (++dma_i2s_handle0_timeout == 20000) // Around 30 ms
    {
        clr_AUDIO_RESET;
        reset_PIC32();
        while(1);
    }
    if (++dma_i2s_handle1_timeout == 20000) // Around 30 ms
    {
        clr_AUDIO_RESET;
        reset_PIC32();
        while(1);
    }
    
    /* 
     * Check for bootloader enable.
     */
    if (read_BOOTLOADER_EN)
    {
        clr_AUDIO_RESET;
        reset_PIC32();
        while(1);        
    }
    
    //if (send_USB_packet)
    //{
        //send_USB_packet = false;
        /*
        USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    12,
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                                
        appData.epDataReadPending = true ;
                                
        /* Place a new read request. */
        /*
        USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                            appData.endpointRx, &receivedDataBuffer[0],
                            sizeof(receivedDataBuffer) );
        */
    //}
    
    /* Check the application's current state. */
    switch ( appData.state )
    {
        /* Application's initial state. */
        case APP_STATE_INIT:
            
            /* Open the device layer */
            appData.usbDevHandle = USB_DEVICE_Open( USB_DEVICE_INDEX_0,
                    DRV_IO_INTENT_READWRITE );

            if(appData.usbDevHandle != USB_DEVICE_HANDLE_INVALID)
            {
                /* Register a callback with device layer to get event notification (for end point 0) */
                USB_DEVICE_EventHandlerSet(appData.usbDevHandle,  APP_USBDeviceEventHandler, 0);

                appData.state = APP_STATE_WAIT_FOR_CONFIGURATION;
            }
            else
            {
                /* The Device Layer is not ready to be opened. We should try
                 * again later. */
            }

            break;

        case APP_STATE_WAIT_FOR_CONFIGURATION:

            /* Check if the device is configured */
            if(appData.deviceIsConfigured == true)
            {
                if (USB_DEVICE_ActiveSpeedGet(appData.usbDevHandle) == USB_SPEED_FULL)
                {
                    endpointSize = 64;
                }
                else if (USB_DEVICE_ActiveSpeedGet(appData.usbDevHandle) == USB_SPEED_HIGH)
                {
                    endpointSize = 512;
                }
                if (USB_DEVICE_EndpointIsEnabled(appData.usbDevHandle, appData.endpointRx) == false )
                {
                    /* Enable Read Endpoint */
                    USB_DEVICE_EndpointEnable(appData.usbDevHandle, 0, appData.endpointRx,
                            USB_TRANSFER_TYPE_BULK, endpointSize);
                }
                if (USB_DEVICE_EndpointIsEnabled(appData.usbDevHandle, appData.endpointTx) == false )
                {
                    /* Enable Write Endpoint */
                    USB_DEVICE_EndpointEnable(appData.usbDevHandle, 0, appData.endpointTx,
                            USB_TRANSFER_TYPE_BULK, endpointSize);
                }
                /* Indicate that we are waiting for read */
                appData.epDataReadPending = true;

                /* Place a new read request. */
                USB_DEVICE_EndpointRead(appData.usbDevHandle, &appData.readTranferHandle,
                        appData.endpointRx, &receivedDataBuffer[0], sizeof(receivedDataBuffer) );

                /* Device is ready to run the main task */
                appData.state = APP_STATE_MAIN_TASK;
            }
            break;

        case APP_STATE_MAIN_TASK:

            if(!appData.deviceIsConfigured)
            {
                /* This means the device got deconfigured. Change the
                 * application state back to waiting for configuration. */
                appData.state = APP_STATE_WAIT_FOR_CONFIGURATION;

                /* Disable the endpoint*/
                USB_DEVICE_EndpointDisable(appData.usbDevHandle, appData.endpointRx);
                USB_DEVICE_EndpointDisable(appData.usbDevHandle, appData.endpointTx);
                appData.epDataReadPending = false;
                appData.epDataWritePending = false;
            }
            else if (receivedDataBuffer[0] == 'c') 
            {
                if (receivedDataBuffer[1] == 'm' && receivedDataBuffer[2] == 'd')
                {                    
                    switch (receivedDataBuffer[3])
                    {
                        case 0x80:
                            if (receivedDataBuffer[32792+2048] == 'f')
                            {
                                set_LED_USB;
                                prepare_metadataCmd();
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                //receivedDataBuffer[12] = 0;
                                //receivedDataBuffer[32780] = 0;
                                //receivedDataBuffer[32792] = 0;
                                receivedDataBuffer[32792+2048] = 0;
                            }    
                            
                            break;
                            
                        case 0x81:
                            if (receivedDataBuffer[32780] == 'f')
                            {
                                set_LED_USB;                                
                                prepare_dataCmd();
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                //receivedDataBuffer[12] = 0;
                                receivedDataBuffer[32780] = 0;
                                //receivedDataBuffer[32792] = 0;
                                //receivedDataBuffer[32792+2048] = 0;
                            }    
                            
                            break;
                            
                        case 0x84:
                            if (receivedDataBuffer[12] == 'f')
                            {
                                set_LED_USB;
                                process_readMetadataCmd();
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                receivedDataBuffer[12] = 0;
                                //receivedDataBuffer[32780] = 0;
                                //receivedDataBuffer[32792] = 0;
                                //receivedDataBuffer[32792+2048] = 0;
                            }    
                            
                            break;
                    }
                }
                /*
                USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    64,//sizeof(transmitDataBuffer),
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                    
                receivedDataBuffer[0] = 0;
                receivedDataBuffer[65536-1] = 0;
                //}
                */

                //appData.epDataReadPending = true ;

                /* Place a new read request. */
                //USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                //        appData.endpointRx, &receivedDataBuffer[0], sizeof(receivedDataBuffer) );
            }
                
                
            else if (1==0)//    if (appData.epDataReadPending == false)
            {
                /* Look at the data the host sent, to see what kind of
                 * application specific command it sent. */

                //switch(receivedDataBuffer[0])
                //{
                    int i = 0;
                    for (; i < APP_READ_BUFFER_SIZE; i++)
                        transmitDataBuffer[i] = receivedDataBuffer[i];
                    
                    USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    sizeof(transmitDataBuffer),
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                    
                    
                //}

                appData.epDataReadPending = true ;

                /* Place a new read request. */
                USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                        appData.endpointRx, &receivedDataBuffer[0], sizeof(receivedDataBuffer) );
            }
            break;

        case APP_STATE_ERROR:
            break;
        

        /* The default state should never be executed. */
        default:
        {
            /* TODO: Handle error in application's state machine. */
            break;
        }
    }
}

 

/*******************************************************************************
 End of File
 */
