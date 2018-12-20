#include <avr/io.h>
#include "uart1.h"
#include "parallel_bus.h"

//https://sites.google.com/site/bpoddocumentation/bpod-user-guide/serial-interfaces/audioplayer

uint8_t bpod_reply [32];
uint8_t bpod_reply_length;
uint8_t bpod_cmd_received = 0;

void load_device_name_to_bpod_cmds (uint8_t fwH, uint8_t *device_name, uint8_t name_length)
{
   bpod_reply[0] = 65;                       // Acknowledge
   *((int32_t*)(&bpod_reply[1])) = fwH;      // 4-byte firwmare version as 32-bit unsigned
   bpod_reply[5] = 9;                        // Length of module's name   
      
   for (uint8_t i = 0; i < name_length; i++)
      bpod_reply[i+6] = device_name[i];      // Module's name
   
   bpod_reply[1 + 4 + 1 + name_length] = 0;  // 1 if more info follows. 0 if not   
   
   bpod_reply_length = 1 + 4 + 1 + name_length + 1;
}

void uart1_rcv_byte_callback(uint8_t byte)
{
   switch (byte)
   {
      case 'P':
         bpod_cmd_received = 'P';
         return;
      case 'X':
         par_cmd_stop();
         return;
      case 'x':
         bpod_cmd_received = 'x';
         return;
   }
   
   if (byte == 255)
   {      
      uart1_xmit(bpod_reply, bpod_reply_length);      
      return;
   }
   
   if (bpod_cmd_received == 'P')
   {
      bpod_cmd_received = 0;
      
      if(byte > 1 && byte < 31)
      {
         par_cmd_start_sound(byte, 0, 0);
      }
   }
   
   if (bpod_cmd_received == 'x')
   {
      bpod_cmd_received = 0;
      
      if(byte > 1 && byte < 31)
      {
         
      }
   }
}