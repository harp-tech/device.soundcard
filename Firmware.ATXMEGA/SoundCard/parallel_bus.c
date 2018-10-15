#include "parallel_bus.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Simple functions for the first tests                                 */
/************************************************************************/
void par_cmd_start_sound(uint8_t sound_index)
{
   PORTA_OUT = sound_index;
   set_CMD_WRITE;
   //while (!read_CMD_LATCHED);
   //clr_CMD_WRITE;
}

void par_cmd_stop(uint8_t sound_index)
{
   PORTA_OUT = 127;
   set_CMD_WRITE;
   //while (!read_CMD_LATCHED);
   //clr_CMD_WRITE;
}

/************************************************************************/
/* CMD_LATCHED & SOUND_IS_ON                                            */
/************************************************************************/
bool sound_is_on_state = false;
ISR(PORTC_INT1_vect, ISR_NAKED)
{
   if (read_CMD_LATCHED)
      clr_CMD_WRITE;
   
   if (read_SOUND_IS_ON)
   {
      if (!sound_is_on_state)
      {
         sound_is_on_state = true;
         
         /* The SOUND_IS_ON rising edge arrives around 415 us before the sound really starts */
         timer_type0_enable(&TCC0, TIMER_PRESCALER_DIV1024, 13, INT_LEVEL_LOW);   // 416 us
      }
   }
   else
   {
      if (sound_is_on_state)
      {
         sound_is_on_state = false;
         clr_DOUT0;
      }
   }
   
   reti();
}

ISR(TCC0_OVF_vect, ISR_NAKED)
{
   set_DOUT0;
   timer_type0_stop(&TCC0);
   reti();
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