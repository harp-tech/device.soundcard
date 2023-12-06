#include "hwbp_core.h"

#include "parallel_bus.h"
#include "app_ios_and_regs.h"

extern AppRegs app_regs;


/************************************************************************/
/* Protocol                                                             */
/************************************************************************/
/* DONE STOP                 11110000                                             checksum(1)
 * DONE START                11110001  index(1)   A_left(2)  A_right(2)           checksum(1)
 * X    START W/ FREQUENCY   11110010             A_left(2)  A_right(2)  Freq(2)  checksum(1)
 * DONE DELETE_SOUND         11110100  index(1)                                   checksum(1)
 * TODO UPDATE AMP           11111001             A_left(2)  A_right(2)           checksum(1)
 * X    UPDATE AMP. & FREQ.  11111010             A_left(2)  A_right(2)  Freq(2)  checksum(1)
 * TODO UPDATE FREQUENCY     11110011                                    Freq(2)  checksum(1)
 * TODO UPDATE AMP LEFT      11111100             A_left(2)                       checksum(1)
 * TODO UPDATE AMP RIGHT     11111101                        A_right(2)           checksum(1)
 */
#define CMD_STOP 0xF0
#define CMD_START 0xF1
#define CMD_START_W_FREQUENCY 0xF2
#define CMD_DELETE_SOUND 0xF7
#define CMD_UPDATE_AMPLITUDE 0xF9
#define CMD_UPDATE_AMPLITUDE_AND_FREQUENCY 0xFA
#define CMD_UPDATE_FREQUENCY 0xFB
#define CMD_UPDATE_AMPLITUDE_LEFT 0xFC
#define CMD_UPDATE_AMPLITUDE_RIGHT 0xFD

#define CMD_STOP_LEN 2
#define CMD_DELETE_SOUND_LEN 3
#define CMD_START_LEN 8
#define CMD_UPDATE_AMPLITUDE_LEN 6
#define CMD_UPDATE_FREQUENCY_LEN 4
#define CMD_UPDATE_AMPLITUDE_LEFT_LEN 4
#define CMD_UPDATE_AMPLITUDE_RIGHT_LEN 4

uint8_t cmd_stop[CMD_STOP_LEN]                                     = {CMD_STOP, 0};
uint8_t cmd_start[CMD_START_LEN]                                   = {CMD_START, 0, 0, 0, 0, 0, 0, 0};
uint8_t cmd_delete_sound[CMD_DELETE_SOUND_LEN]                     = {CMD_DELETE_SOUND, 0, 0};
uint8_t cmd_update_amplitude[CMD_UPDATE_AMPLITUDE_LEN]             = {CMD_UPDATE_AMPLITUDE, 0, 0, 0, 0, 0};
uint8_t cmd_update_frequency[CMD_UPDATE_FREQUENCY_LEN]             = {CMD_UPDATE_FREQUENCY, 0, 0, 0};
uint8_t cmd_update_amplitude_left[CMD_UPDATE_AMPLITUDE_LEFT_LEN]   = {CMD_UPDATE_AMPLITUDE_LEFT, 0, 0, 0};
uint8_t cmd_update_amplitude_right[CMD_UPDATE_AMPLITUDE_RIGHT_LEN] = {CMD_UPDATE_AMPLITUDE_RIGHT, 0, 0, 0};


bool command_available = false;
uint8_t command_to_send;

#define send_byte(byte) PORTA_OUT = byte; \
                        set_CMD_WRITE; \
                        while (!read_CMD_LATCHED); \
                        clr_CMD_WRITE; \
                        while (read_CMD_LATCHED)

#define send_last_byte(byte)    PORTA_OUT = byte; \
                                set_CMD_WRITE; \
                                while (!read_CMD_LATCHED); \
                                if (read_CMD_NOT_EXEC)  { clr_CMD_WRITE; while (read_CMD_LATCHED); set_DOUT2; return false;} \
                                else                    { clr_CMD_WRITE; while (read_CMD_LATCHED); clr_DOUT2; return true; }


/************************************************************************/
/* CMD_LATCHED & SOUND_IS_ON                                            */
/************************************************************************/
extern uint16_t last_sound_triggered;

ISR(PORTC_INT1_vect, ISR_NAKED)
{
   if (read_CMD_LATCHED)
   {  
      if (command_available)
      {
         while (!read_CMD_LATCHED);
         clr_CMD_WRITE;      
         while (read_CMD_LATCHED);

         /* Choose callback */
         switch (command_to_send)
         {
               
            case CMD_STOP:
               par_cmd_stop_callback();
               break;
            
			case CMD_START:
               if (par_cmd_start_sound_callback() == false)
			      
				  /* If command wasn't accepted by PIC32 */
			      last_sound_triggered = 0;
			      
               break;
               
            case CMD_DELETE_SOUND:
               par_cmd_delete_sound_callback();
               break;
               
            case CMD_UPDATE_AMPLITUDE:
               par_cmd_update_amplitude_callback();
               break;
               
            case CMD_UPDATE_FREQUENCY:
               if(par_cmd_update_frequency_callback() == false)
			      
				  /* If command wasn't accepted by PIC32 */
			      last_sound_triggered = 0;
				  
               break;
               
            case CMD_UPDATE_AMPLITUDE_LEFT:
               par_cmd_update_amplitude_left_callback();
               break;
               
            case CMD_UPDATE_AMPLITUDE_RIGHT:
               par_cmd_update_amplitude_right_callback();
               break;
         }
           
         /* Update global */
         command_available = false;
      }
   }
   
   reti();
}
   
ISR(PORTC_INT0_vect, ISR_NAKED)
{
   if (read_SOUND_IS_ON)
   {
      if (last_sound_triggered != 0)
      {
         app_regs.REG_PLAY_SOUND_OR_FREQ = last_sound_triggered;
	     core_func_send_event(ADD_REG_PLAY_SOUND_OR_FREQ, true);
	     last_sound_triggered = 0; // The event was sent and a new sound can be trigger
	  }
	  
      //set_DOUT0;
   }
   else
   {
      //clr_DOUT0;
   }
   
   reti();
}

ISR(TCD0_OVF_vect, ISR_NAKED)
{     
   timer_type0_stop(&TCD0);
   
   PORTA_OUT = command_to_send;
   set_CMD_WRITE;
   
   reti();
}


/************************************************************************/
/* COMMAND: STOP                                                        */
/************************************************************************/
void par_cmd_stop(void)
{
   /* Calculate checksum */
   cmd_stop[CMD_STOP_LEN - 1] = cmd_stop[0];
   
   /* Update globals */
   command_available = true;
   command_to_send = CMD_STOP;
   
   /* Create an interrupt to be addressed as soon as possible */
   timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_stop_callback (void)
{
   send_last_byte(cmd_stop[CMD_STOP_LEN - 1]);
}

/************************************************************************/
/* COMMAND: START                                                       */
/************************************************************************/
void par_cmd_start_sound(uint16_t sound_index, int16_t amplitude_left, int16_t amplitude_right)
{	
	/* Prepare command */
	cmd_start[1] = *(((uint8_t*)(&sound_index)) + 0);
	cmd_start[2] = *(((uint8_t*)(&sound_index)) + 1);
	cmd_start[3] = *(((uint8_t*)(&amplitude_left)) + 0);
	cmd_start[4] = *(((uint8_t*)(&amplitude_left)) + 1);
	cmd_start[5] = *(((uint8_t*)(&amplitude_right)) + 0);
	cmd_start[6] = *(((uint8_t*)(&amplitude_right)) + 1);
	
	/* Calculate checksum */
	cmd_start[CMD_START_LEN - 1] = cmd_start[0];
	for (uint8_t i = CMD_START_LEN - 1; i != 1; i--)
	{
		cmd_start[CMD_START_LEN - 1] += cmd_start[i-1];
	}
	
	/* Update globals */
	command_available = true;
	command_to_send = CMD_START;
	
	/* Create an interrupt to be addressed as soon as possible */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_start_sound_callback (void)
{
	send_byte(cmd_start[1]);
	send_byte(cmd_start[2]);
	send_byte(cmd_start[3]);
	send_byte(cmd_start[4]);
	send_byte(cmd_start[5]);
	send_byte(cmd_start[6]);
	send_last_byte(cmd_start[7]);
}

/************************************************************************/
/* COMMAND: DELETE_SOUND                                                */
/************************************************************************/
void par_cmd_delete_sound(uint8_t sound_index, bool delete_all)
{
    /* Prepare command */
    if (delete_all)
        cmd_delete_sound[1] = 0xAA;
    else
        cmd_delete_sound[1] = sound_index;
    
    /* Calculate checksum */
    cmd_delete_sound[2] = CMD_DELETE_SOUND + cmd_delete_sound[1];
    
    /* Update globals */
    command_available = true;
    command_to_send = CMD_DELETE_SOUND;
    
    /* Create an interrupt to be addressed as soon as possible */
    timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_delete_sound_callback (void)
{
    send_byte(cmd_delete_sound[1]);
    send_last_byte(cmd_delete_sound[2]);
}

/************************************************************************/
/* COMMAND: CMD_UPDATE_FREQUENCY                                        */
/************************************************************************/
void par_cmd_update_frequency(uint16_t sound_index)
{
	/* Prepare command */
	cmd_update_frequency[1] = *(((uint8_t*)(&sound_index)) + 0);
	cmd_update_frequency[2] = *(((uint8_t*)(&sound_index)) + 1);
	
	/* Calculate checksum */
	cmd_update_frequency[3] = CMD_UPDATE_FREQUENCY + cmd_update_frequency[1] + cmd_update_frequency[2];
	
	/* Update globals */
	command_available = true;
	command_to_send = CMD_UPDATE_FREQUENCY;
	
	/* Create an interrupt to be addressed as soon as possible */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_update_frequency_callback (void)
{
	send_byte(cmd_update_frequency[1]);
	send_byte(cmd_update_frequency[2]);
	send_last_byte(cmd_update_frequency[3]);
}

/************************************************************************/
/* COMMAND: CMD_UPDATE_AMPLITUDE                                        */
/************************************************************************/
void par_cmd_update_amplitude(int16_t amplitude_left, int16_t amplitude_right)
{
	/* Prepare command */
	cmd_update_amplitude[1] = *(((uint8_t*)(&amplitude_left)) + 0);
	cmd_update_amplitude[2] = *(((uint8_t*)(&amplitude_left)) + 1);
	cmd_update_amplitude[3] = *(((uint8_t*)(&amplitude_right)) + 0);
	cmd_update_amplitude[4] = *(((uint8_t*)(&amplitude_right)) + 1);
	
	/* Calculate checksum */
	cmd_update_amplitude[5] = CMD_UPDATE_AMPLITUDE;
	cmd_update_amplitude[5] += cmd_update_amplitude[1] + cmd_update_amplitude[2];
	cmd_update_amplitude[5] += cmd_update_amplitude[3] + cmd_update_amplitude[4];
	
	/* Update globals */
	command_available = true;
	command_to_send = CMD_UPDATE_AMPLITUDE;
	
	/* Create an interrupt to be addressed as soon as possible */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_update_amplitude_callback (void)
{
	send_byte(cmd_update_amplitude[1]);
	send_byte(cmd_update_amplitude[2]);
	send_byte(cmd_update_amplitude[3]);
	send_byte(cmd_update_amplitude[4]);
	send_last_byte(cmd_update_amplitude[5]);
}

/************************************************************************/
/* COMMAND: CMD_UPDATE_AMPLITUDE_LEFT                                   */
/************************************************************************/
void par_cmd_update_amplitude_left(int16_t amplitude_left)
{
	/* Prepare command */
	cmd_update_amplitude_left[1] = *(((uint8_t*)(&amplitude_left)) + 0);
	cmd_update_amplitude_left[2] = *(((uint8_t*)(&amplitude_left)) + 1);
	
	/* Calculate checksum */
	cmd_update_amplitude_left[3] = CMD_UPDATE_AMPLITUDE_LEFT;
	cmd_update_amplitude_left[3] += cmd_update_amplitude_left[1] + cmd_update_amplitude_left[2];
	
	/* Update globals */
	command_available = true;
	command_to_send = CMD_UPDATE_AMPLITUDE_LEFT;
	
	/* Create an interrupt to be addressed as soon as possible */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_update_amplitude_left_callback (void)
{
	send_byte(cmd_update_amplitude_left[1]);
	send_byte(cmd_update_amplitude_left[2]);
	send_last_byte(cmd_update_amplitude_left[3]);
}

/************************************************************************/
/* COMMAND: CMD_UPDATE_AMPLITUDE_RIGHT                                  */
/************************************************************************/
void par_cmd_update_amplitude_right(int16_t amplitude_right)
{
	/* Prepare command */
	cmd_update_amplitude_right[1] = *(((uint8_t*)(&amplitude_right)) + 0);
	cmd_update_amplitude_right[2] = *(((uint8_t*)(&amplitude_right)) + 1);
	
	/* Calculate checksum */
	cmd_update_amplitude_right[3] = CMD_UPDATE_AMPLITUDE_RIGHT;
	cmd_update_amplitude_right[3] += cmd_update_amplitude_right[1] + cmd_update_amplitude_right[2];
	
	/* Update globals */
	command_available = true;
	command_to_send = CMD_UPDATE_AMPLITUDE_RIGHT;
	
	/* Create an interrupt to be addressed as soon as possible */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV1, 1, INT_LEVEL_LOW);
}

bool par_cmd_update_amplitude_right_callback (void)
{
	send_byte(cmd_update_amplitude_right[1]);
	send_byte(cmd_update_amplitude_right[2]);
	send_last_byte(cmd_update_amplitude_right[3]);
}

/************************************************************************/
/* Functions for the future                                             */
/* Consider using a simple bytes circular buffer                        */
/************************************************************************/

#define PAR_MAX_PAYLOAD_LENGTH 4 // 4 bytes
#define PAR_CMDS_ON_BUFFER 5     // Buffer can accept 5 commands

#define PAR_CMD_STATE_EMPTY 0
#define PAR_CMD_STATE_READY_TO_TX 1

typedef struct {
   uint8_t command_state;
   uint8_t header;
   uint8_t payload[PAR_MAX_PAYLOAD_LENGTH];
   uint8_t payload_length;
} par_command_t;

par_command_t par_commands[PAR_CMDS_ON_BUFFER];

void _par_add_command(uint8_t header, uint8_t * payload, uint8_t payload_length)
{
   for (uint8_t i = PAR_CMDS_ON_BUFFER; i != 0; i--)
   {
      if (par_commands[i-1].command_state == PAR_CMD_STATE_EMPTY)
      {
         par_commands[i-1].command_state = PAR_CMD_STATE_READY_TO_TX;
         par_commands[i-1].header = header;
         par_commands[i-1].payload_length = payload_length;
         
         for (uint8_t j = payload_length; j != 0; j--)
            par_commands[i-1].payload[j-1] = payload[j-1];
         
         continue;
      }
   }
}

void _par_add_command_without_payload(uint8_t header)
{
   for (uint8_t i = PAR_CMDS_ON_BUFFER; i != 0; i--)
   {
      if (par_commands[i-1].command_state == PAR_CMD_STATE_EMPTY)
      {
         par_commands[i-1].command_state = PAR_CMD_STATE_READY_TO_TX;
         par_commands[i-1].header = header;
         par_commands[i-1].payload_length = 0;
         
         continue;
      }
   }
}


void par_cmd_start_sound_XXX(uint8_t sound_index, uint16_t right, uint16_t left)
{
   uint8_t auxiliary_array[4];
   auxiliary_array[0] = *(((uint8_t*)(&right)) + 0);
   auxiliary_array[1] = *(((uint8_t*)(&right)) + 1);
   auxiliary_array[2] = *(((uint8_t*)(&left)) + 0);
   auxiliary_array[3] = *(((uint8_t*)(&left)) + 1);
   
   _par_add_command(sound_index, auxiliary_array, 4);
}

void par_cmd_stop_XXX(void)
{
   _par_add_command_without_payload(127);
}
   
void par_cmd_set_attenuation(uint16_t right, uint16_t left)
{
   uint8_t auxiliary_array[4];
   auxiliary_array[0] = *(((uint8_t*)(&right)) + 0);
   auxiliary_array[1] = *(((uint8_t*)(&right)) + 1);
   auxiliary_array[2] = *(((uint8_t*)(&left)) + 0);
   auxiliary_array[3] = *(((uint8_t*)(&left)) + 1);
   
   _par_add_command(128, auxiliary_array, 4);
}