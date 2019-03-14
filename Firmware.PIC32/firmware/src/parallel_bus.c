#include <xc.h>
#include <stdbool.h>
#include "ios.h"
#include "parallel_bus.h"
#include "audio.h"

bool check_cmd_start(int index);

unsigned char cmd_stop[CMD_STOP_LEN]   = {CMD_STOP, 0};
unsigned char cmd_start[CMD_START_LEN] = {CMD_START, 0, 0, 0, 0, 0, 0};

#define PAR_RECEIVE_BYTE(byte)  while (!read_PAR_CMD_WRITE); \
                                byte = read_PAR_BUS; \
                                set_PAR_CMD_LATCH; \
                                while (read_PAR_CMD_WRITE); \
                                clr_PAR_CMD_LATCH

#define PAR_RECEIVE_LAST_BYTE(byte) while (!read_PAR_CMD_WRITE); \
                                    byte = read_PAR_BUS

#define PAR_RECEIVE_LAST_BYTE_REPLY(error)  if (error) set_PAR_CMD_ERROR; else clr_PAR_CMD_ERROR; \
                                            set_PAR_CMD_LATCH; \
                                            while (read_PAR_CMD_WRITE); \
                                            clr_PAR_CMD_LATCH

void initialize_par_ios(void)
{
    cfg_PAR_BUS;
    cfg_PAR_CMD_WRITE;
    cfg_PAR_CMD_LATCH;
    cfg_PAR_CMD_ERROR;
    
    clr_PAR_CMD_LATCH;
    clr_PAR_CMD_ERROR;
}

int par_bus_check_if_command_is_available(void)
{
    if (read_PAR_CMD_WRITE)
    {
        int command_received;
        int i;
        
        clr_PAR_CMD_ERROR;
        command_received = read_PAR_BUS;
        set_PAR_CMD_LATCH;
        while (read_PAR_CMD_WRITE);
        clr_PAR_CMD_LATCH;
        
        if ((command_received & 0xF0) != 0xF0)
            return 0;
        
        switch (command_received)
        {
            case CMD_START:
                PAR_RECEIVE_BYTE(cmd_start[1]);
                PAR_RECEIVE_BYTE(cmd_start[2]);
                PAR_RECEIVE_BYTE(cmd_start[3]);
                PAR_RECEIVE_BYTE(cmd_start[4]);
                PAR_RECEIVE_BYTE(cmd_start[5]);
                //PAR_RECEIVE_BYTE(cmd_start[6]);
                PAR_RECEIVE_LAST_BYTE(cmd_start[6]);
                
                unsigned char checksum = 0;
                for (i = CMD_START_LEN - 1; i != 0; i--)
                {
                   checksum += cmd_start[i-1];
                }
                
                if ((checksum == cmd_start[CMD_START_LEN - 1]) && (check_cmd_start(cmd_start[1])))
                {
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);
                clr_LED_USB;
                    return command_received;
                }
                
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                set_LED_USB;
                break;
                
            case CMD_STOP:
                PAR_RECEIVE_BYTE(cmd_stop[1]);
                
                if (cmd_stop[0] == cmd_stop[1])
                {
                    return command_received;
                }
                
                break;
        }
    }
    
    return 0;
}

int par_bus_process_command_start(void)
{
    int att_left = cmd_start[3];
    int att_right = cmd_start[5];
    att_left = (att_left << 8) | cmd_start[2];
    att_right = (att_right << 8) | cmd_start[4];
    
    /* Update output amplitude */
    update_audio_volume_int(att_left, att_right);
    
    /* Return sound index */
    return cmd_start[1];
}

void par_bus_process_command_stop(void)
{    
    /* Mute the device */
    //set_AUDIO_MUTE;
}