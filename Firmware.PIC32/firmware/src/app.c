#include "app.h"
#include "math.h"

#include "delay.h"
#include "audio.h"
#include "ios.h"
#include "memory.h"
#include "sounds_allocation.h"

// *****************************************************************************
// *****************************************************************************
// Section: Global Data Definitions
// *****************************************************************************
// *****************************************************************************
DRV_HANDLE i2sDriverHandle;
DRV_I2S_BUFFER_EVENT_HANDLER i2sBufferEventHandler;
DRV_I2S_BUFFER_HANDLE i2sBufferHandle0;
DRV_I2S_BUFFER_HANDLE i2sBufferHandle1;

#define AUDIO_BUFFER_LEN 2048/4
int audio_first_buffer[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer0[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer1[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer0_length;
int audio_buffer1_length;
//int audio_buffer0_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));
//int audio_buffer1_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));
int audio_buffer_zeros[AUDIO_BUFFER_LEN] __attribute__((coherent));

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

bool usb_is_receiving_data = false;

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
                
                dma_i2s_handle0_timeout = 0;
                tgl_TP1;
            }
            if (bufferHandle == i2sBufferHandle1)
            {
                if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
                    audio_buffer1_state = AUDIO_BUFFER_IS_EMPTY;
                
                dma_i2s_handle1_timeout = 0;
                tgl_TP1;
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
    
int launch_sound(int index)
{
    audio_buffer0_length = 0;
    audio_buffer1_length = 0;
    audio_buffer0_state = AUDIO_BUFFER_IS_EMPTY;
    audio_buffer1_state = AUDIO_BUFFER_IS_EMPTY;
    sound_is_playing = false;
    sound_length_produced = 0;
    
    if (read_first_sound_page(index, audio_buffer0, &play_metadata) != 0) return -1;
    sound_is_playing = true;
    
    if (play_metadata.sound_length > AUDIO_BUFFER_LEN)
    {
        audio_buffer0_length = AUDIO_BUFFER_LEN;
        audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
        
        read_next_sound_page(audio_buffer1);
        
        if (play_metadata.sound_length - AUDIO_BUFFER_LEN > AUDIO_BUFFER_LEN)
        {
            audio_buffer1_length = AUDIO_BUFFER_LEN;
            audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
        }
        else
        {
            audio_buffer1_length = play_metadata.sound_length - AUDIO_BUFFER_LEN;
            audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
        }
    }
    else
    {
        audio_buffer0_length = play_metadata.sound_length;
        audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
    }
    
    sound_length_produced += audio_buffer0_length;
    sound_length_produced += audio_buffer1_length;
    
    if (audio_buffer0_state == AUDIO_BUFFER_HAS_DATA)
        DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, audio_buffer0_length * 4);
    
    if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
        DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, audio_buffer1_length * 4);
    
    return 0;
}

void update_sound_buffers (void);

int launch_sound_v2(int index)
{
    set_LED_AUDIO;
    
    audio_buffer0_state = AUDIO_BUFFER_IS_EMPTY;
    audio_buffer1_state = AUDIO_BUFFER_IS_EMPTY;
    sound_is_playing = false;
    sound_length_produced = 0;
    
    set_LED_MEMORY;
    if (read_first_sound_page(index, audio_buffer0, &play_metadata) != 0) return -1;
    clr_LED_MEMORY;
    
    sound_is_playing = true;
    
    if (play_metadata.sound_length > AUDIO_BUFFER_LEN)
    {
        DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, AUDIO_BUFFER_LEN * 4);
        sound_length_produced = AUDIO_BUFFER_LEN;
        audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
    }
    else
    {
        DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, play_metadata.sound_length * 4);
        sound_length_produced = play_metadata.sound_length;
        audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
    }
    
    update_sound_buffers();
    
    return 0;
}

/* Start a new sound if:
 * - the selected index has a sound,
 * - no sound is initiating, and
 * - no sound is being produced.
 */
int launch_sound_v3(int index)
{
    if (!audio_sound_exists[index])
        return -1;    
    
    if (new_sound_to_start != NEW_SOUND_STATE_STANDBY)
        return -1;
    
    if (sound_is_playing)
        return -1;
    
    new_sound_to_start = NEW_SOUND_STATE_IS_AVAILABLE;    
    new_sound_index = index;
    play_metadata = audio_all_metadata[index];
    
    return 0;
}

void update_sound_buffers (void)
{
    if (!sound_is_playing && new_sound_to_start == NEW_SOUND_STATE_IS_AVAILABLE)
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY || audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            new_sound_to_start = NEW_SOUND_STATE_FIRST_BUFFER_DONE;
            sound_is_playing = true;
            set_page_and_sound_index(1, new_sound_index);
            
            if (play_metadata.sample_rate != current_sample_rate)
            {
                current_sample_rate = play_metadata.sample_rate;
                config_audio_dac(current_sample_rate);
            }
            
            if (play_metadata.sound_length > AUDIO_BUFFER_LEN)
            {
                sound_length_produced = AUDIO_BUFFER_LEN;
                
                if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_all_first_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    set_SOUND_IS_ON;
                    set_LED_AUDIO;
                }
                else
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_first_buffers[new_sound_index], AUDIO_BUFFER_LEN * 4);
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    set_SOUND_IS_ON;
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
                    set_SOUND_IS_ON;
                    set_LED_AUDIO;
                }
                else
                {
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_all_first_buffers[new_sound_index], play_metadata.sound_length * 4);
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    set_SOUND_IS_ON;
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
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_SOUND_IS_ON;
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
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_SOUND_IS_ON;
                clr_LED_AUDIO;
            }
            
            //if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, audio_buffer1_length * 4);
            //}
        }
    }
    
    
    if (sound_is_playing && new_sound_to_start == NEW_SOUND_STATE_STANDBY)
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
                }
                else
                {
                    //audio_buffer0_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer0, (play_metadata.sound_length - sound_length_produced) * 4);
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_SOUND_IS_ON;
                clr_LED_AUDIO;
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
                }
                else
                {
                    //audio_buffer1_length = play_metadata.sound_length - sound_length_produced;
                    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, (play_metadata.sound_length - sound_length_produced) * 4);
                    sound_length_produced += play_metadata.sound_length - sound_length_produced;
                    audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
                    
                    //sound_is_playing = false;
                    //clr_LED_AUDIO;
                }
            }
            else
            {
                sound_is_playing = false;
                clr_SOUND_IS_ON;
                clr_LED_AUDIO;
            }
            
            //if (audio_buffer1_state == AUDIO_BUFFER_HAS_DATA)
            //{   
            //    DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer1, audio_buffer1_length * 4);
            //}
        }
    }
    else
    {
        if (audio_buffer0_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (current_sample_rate == 96000)
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer_zeros, 32 * 4);
            else
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle0, audio_buffer_zeros, 64 * 4); // 8 works well
            
            audio_buffer0_state = AUDIO_BUFFER_HAS_DATA;
        }
        
        if (audio_buffer1_state == AUDIO_BUFFER_IS_EMPTY)
        {
            if (current_sample_rate == 96000)
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer_zeros, 32 * 4);
            else
                DRV_I2S_BufferAddWrite(i2sDriverHandle, &i2sBufferHandle1, audio_buffer_zeros, 64 * 4); // 8 works well
            
            audio_buffer1_state = AUDIO_BUFFER_HAS_DATA;
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

void clean_memory (void)
{
    int i = 0;
    
    for (; i < get_available_sounds(); i++)
        block_erase(i * BLOCKS_PER_SOUND);
    
    while(1)
    {
        _ms_delay(100);
        tgl_LED_MEMORY;
    }
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
    
    set_TP0;
    
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
    config_audio_dac(current_sample_rate);
    
    /* 
     * Clean all metadata in the memory.
     */
    //clean_memory();
    
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
int a = 0;
int b = 0;
int command_received = 0xFF;
void APP_Tasks ( void )
{   
    update_sound_buffers();
    command_received = par_bus_check_for_command();
    
    if (command_received > 0)
    {
        if (command_received < 32)
        {
            // Start a sound
            launch_sound_v3(command_received);
        }
    }
    
    /* 
     * Checks if the I2S DMA is working.
     * Issue a software reset if not.
     * Usually, this test is performed each ~1.5 us
     */
    if (usb_is_receiving_data == false)
    {
        if (++dma_i2s_handle0_timeout == 20000) // Around 30 ms
        {
            clr_AUDIO_RESET;
            while (1) tgl_TP1;
            reset_PIC32();
            while(1);
        }
        if (++dma_i2s_handle1_timeout == 20000) // Around 30 ms
        {
            clr_AUDIO_RESET;
            while (1) tgl_TP1;
            reset_PIC32();
            while(1);
        }
    }    
    
    a++;
    if (a == 0x80000 && b == 0)
    {
        //set_LED_AUDIO;
        //launch_sound_v2(17);
    }
    
    if (a == 0x7FFFFF && b == 0)
    {
        //set_LED_AUDIO;
        //launch_sound_v2(17);
        b = 1;
    }   
    
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
                                usb_is_receiving_data = true;
                                
                                Sound_Metadata * ptr = (Sound_Metadata*)(receivedDataBuffer + 8);
                                int *error = (int*)(transmitDataBuffer + 8);
                                int i;
                                
                                *error = ERROR_NOERROR;
                                if (ptr->sound_index < 0 || ptr->sound_index > get_available_sounds()) *error = ERROR_BADSOUNDINDEX;
                                if (ptr->sound_length < 16) *error = ERROR_BADSOUNDLENGTH;
                                if (ptr->sample_rate != 96000 && ptr->sample_rate != 192000) *error = ERROR_BADSAMPLERATE;
                                if (ptr->data_type != 0 && ptr->data_type != 1) *error = ERROR_BADDATATYPE;
                                if (ptr->sound_index == 0 && ptr->data_type != 1) *error = ERROR_BADDATATYPEMATCH;
                                if (ptr->sound_index == 1 && ptr->data_type != 1) *error = ERROR_BADDATATYPEMATCH;
                                if (ptr->sound_index > 1 && ptr->data_type != 0) *error = ERROR_BADDATATYPEMATCH;
                                
                                if (sound_is_playing) *error = ERROR_PRODUCINGSOUND;
                                
                                if (*error == ERROR_NOERROR)
                                {
                                    set_LED_MEMORY;
                                    
                                    save_user_metadata(ptr->sound_index, &receivedDataBuffer[4+4+16+32768]);
                                    
                                    /* Erase memory that will be used for the sound data */
                                    if (prepare_memory(ptr->sound_index, ptr->sound_length) == -1) *error = ERROR_BADSOUNDLENGTH;

                                    allocate_metadata_command (*ptr, &receivedDataBuffer[4+4+16]);
                                    
                                    clr_LED_MEMORY;

                                    sound_index_to_write = ptr->sound_index;
                                    max_sound_data_index = 0x7FFFFFFF;  // Code to be done
                                }
                                else
                                {
                                    sound_index_to_write = -1;
                                }
                                
                                for (i = 8; i != 0; i--)
                                    transmitDataBuffer[i-1] = receivedDataBuffer[i-1];                    
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                receivedDataBuffer[12] = 0;
                                receivedDataBuffer[32780] = 0;
                                receivedDataBuffer[32792] = 0;
                                
                                USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    12,
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                                
                                appData.epDataReadPending = true ;
                                
                                /* Place a new read request. */
                                USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                                                    appData.endpointRx, &receivedDataBuffer[0],
                                                    sizeof(receivedDataBuffer) );
                                
                                clr_LED_USB;
                            }    
                            
                            break;
                            
                        case 0x81:
                            if (receivedDataBuffer[32780] == 'f')
                            {
                                set_LED_USB;
                                
                                int dataIndex = *((int*)(receivedDataBuffer + 8));
                                int *error = (int*)(transmitDataBuffer + 8);
                                int i;
                                
                                *error = ERROR_NOERROR;
                                if (dataIndex > max_sound_data_index) *error = ERROR_BADDATAINDEX;
                                
                                if (sound_is_playing) *error = ERROR_STARTEDPRODUCINGSOUND;
                                
                                if (*error == ERROR_NOERROR)
                                {
                                    set_LED_MEMORY;
                                    allocate_data_command (sound_index_to_write, dataIndex, &receivedDataBuffer[4+4+4]);
                                    clr_LED_MEMORY;
                                }
                                
                                for (i = 8; i != 0; i--)
                                    transmitDataBuffer[i-1] = receivedDataBuffer[i-1];                    
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                receivedDataBuffer[12] = 0;
                                receivedDataBuffer[32780] = 0;
                                receivedDataBuffer[32792] = 0;
                                
                                USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    12,
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                                
                                appData.epDataReadPending = true ;
                                
                                /* Place a new read request. */
                                USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                                                    appData.endpointRx, &receivedDataBuffer[0],
                                                    sizeof(receivedDataBuffer) );
                                
                                clr_LED_USB;
                            }    
                            
                            break;
                            
                        case 0x84:
                            if (receivedDataBuffer[12] == 'f')
                            {
                                set_LED_USB;
                                usb_is_receiving_data = true;
                                
                                int i;
                                int *error = (int*)(transmitDataBuffer + 8);
                                int *index = (int*)(receivedDataBuffer + 8);                                
                                
                                *error = ERROR_NOERROR;
                                if (*index < 0 || *index > 31) *error = ERROR_BADSOUNDINDEX;                                
                                if (sound_is_playing) *error = ERROR_PRODUCINGSOUND;
                                
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
                                
                                receivedDataBuffer[0] = 0;
                                receivedDataBuffer[1] = 0;
                                receivedDataBuffer[2] = 0;
                                receivedDataBuffer[3] = 0;
                                receivedDataBuffer[12] = 0;
                                receivedDataBuffer[32780] = 0;
                                receivedDataBuffer[32792] = 0;
                                
                                USB_DEVICE_EndpointWrite ( appData.usbDevHandle, &appData.writeTranferHandle,
                                    appData.endpointTx, &transmitDataBuffer[0],
                                    2076,
                                    USB_DEVICE_TRANSFER_FLAGS_MORE_DATA_PENDING);
                                
                                appData.epDataReadPending = true ;
                                
                                /* Place a new read request. */
                                USB_DEVICE_EndpointRead ( appData.usbDevHandle, &appData.readTranferHandle,
                                                    appData.endpointRx, &receivedDataBuffer[0],
                                                    sizeof(receivedDataBuffer) );
                                
                                usb_is_receiving_data = false;
                                clr_LED_USB;
                            }    
                            
                            break;
                        
                        case 0x88:
                            if (receivedDataBuffer[4] == 'f')
                            {
                                set_LED_USB;
                                clr_AUDIO_RESET;
                                reset_PIC32();
                                while(1);
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
