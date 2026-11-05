#include <xc.h>
#include <stdbool.h>
#include "ios.h"
#include "parallel_bus.h"
#include "audio.h"
#include "sounds_allocation.h"

bool check_cmd_start(int index);

extern bool audio_sound_exists[32];

unsigned char cmd_stop[CMD_STOP_LEN]                                     = {CMD_STOP, 0};
unsigned char cmd_delete_sound[CMD_DELETE_SOUND_LEN]                     = {CMD_DELETE_SOUND, 0, 0};
unsigned char cmd_start[CMD_START_LEN]                                   = {CMD_START, 0, 0, 0, 0, 0, 0};
unsigned char cmd_update_amplitude[CMD_UPDATE_AMPLITUDE_LEN]             = {CMD_UPDATE_AMPLITUDE, 0, 0, 0, 0, 0};
unsigned char cmd_update_frequency[CMD_UPDATE_FREQUENCY_LEN]             = {CMD_UPDATE_FREQUENCY, 0, 0, 0};
unsigned char cmd_update_amplitude_left[CMD_UPDATE_AMPLITUDE_LEFT_LEN]   = {CMD_UPDATE_AMPLITUDE_LEFT, 0, 0, 0};
unsigned char cmd_update_amplitude_right[CMD_UPDATE_AMPLITUDE_RIGHT_LEN] = {CMD_UPDATE_AMPLITUDE_RIGHT, 0, 0, 0};

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
        unsigned char checksum = 0;
        int att_left;
        int att_right;
        
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
                PAR_RECEIVE_BYTE(cmd_start[6]);
                PAR_RECEIVE_LAST_BYTE(cmd_start[7]);                
                
                for (i = CMD_START_LEN - 1; i != 0; i--)
                {
                   checksum += cmd_start[i-1];
                }
                
                //if ((checksum == cmd_start[CMD_START_LEN - 1]) && (check_cmd_start(cmd_start[1])))
                if (checksum == cmd_start[CMD_START_LEN - 1])
                {
                    int index = cmd_start[2];
                    index = (index << 8) | cmd_start[1];
                    
                    if (index < 32)
                        if(audio_sound_exists[index] == false)
                        {
                            /* Return error */
                            PAR_RECEIVE_LAST_BYTE_REPLY(true);
                            return 0;
                        }
                    
                    /* Return success */
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);                    
                    return command_received;
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
                
            case CMD_STOP:
                PAR_RECEIVE_BYTE(cmd_stop[1]);
                
                if (cmd_stop[0] == cmd_stop[1])
                {
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);
                    
                    return command_received;
                }
                
                break;
                
            case CMD_DELETE_SOUND:
                PAR_RECEIVE_BYTE(cmd_delete_sound[1]);
                PAR_RECEIVE_LAST_BYTE(cmd_delete_sound[2]);
                
                if (cmd_delete_sound[2] == (unsigned char)(CMD_DELETE_SOUND + cmd_delete_sound[1]))
                {
                    if ((cmd_delete_sound[1] < 32) || (cmd_delete_sound[1] == 0xAA))
                    {
                        /* Return success */
                        PAR_RECEIVE_LAST_BYTE_REPLY(false);
                    
                        if (cmd_delete_sound[1] < 32)
                        {
                            if (audio_sound_exists[cmd_delete_sound[1]] == true)
                            {
                                block_erase(cmd_delete_sound[1] * BLOCKS_PER_SOUND);
                                
                                clr_AUDIO_RESET;
                                reset_PIC32();
                                while(1);
                            }
                        }
                        
                        if (cmd_delete_sound[1] == 0xAA)
                        {
                            bool have_sounds = false;
                            
                            for (i = 0; i < get_available_sounds(); i++)
                            {
                                if (audio_sound_exists[i] == true)
                                {
                                    block_erase(i * BLOCKS_PER_SOUND);
                                    have_sounds = true;
                                }
                            }
                            
                            if (have_sounds)
                            {
                                clr_AUDIO_RESET;
                                reset_PIC32();
                                while(1);
                            }
                        }
                    }
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
            
            case CMD_UPDATE_AMPLITUDE:
                PAR_RECEIVE_BYTE(cmd_update_amplitude[1]);
                PAR_RECEIVE_BYTE(cmd_update_amplitude[2]);
                PAR_RECEIVE_BYTE(cmd_update_amplitude[3]);
                PAR_RECEIVE_BYTE(cmd_update_amplitude[4]);
                PAR_RECEIVE_LAST_BYTE(cmd_update_amplitude[5]);
                
                unsigned char checksum = 0;
                for (i = CMD_UPDATE_AMPLITUDE_LEN - 1; i != 0; i--)
                   checksum += cmd_update_amplitude[i-1];
                
                if (checksum == cmd_update_amplitude[CMD_UPDATE_AMPLITUDE_LEN - 1])
                {
                    /* Return success */
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);
                    
                    att_left = cmd_update_amplitude[2];
                    att_right = cmd_update_amplitude[4];
                    att_left = (att_left << 8) | cmd_update_amplitude[1];
                    att_right = (att_right << 8) | cmd_update_amplitude[3];

                    /* Update output amplitude */
                    update_audio_volume_int(att_left, att_right);
                    
                    return 0;
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
                
            case CMD_UPDATE_FREQUENCY:
                PAR_RECEIVE_BYTE(cmd_update_frequency[1]);
                PAR_RECEIVE_BYTE(cmd_update_frequency[2]);
                PAR_RECEIVE_LAST_BYTE(cmd_update_frequency[3]);
                
                checksum = 0;
                for (i = CMD_UPDATE_FREQUENCY_LEN - 1; i != 0; i--)
                   checksum += cmd_update_frequency[i-1];
                
                if (checksum == cmd_update_frequency[CMD_UPDATE_FREQUENCY_LEN - 1])
                {
                    int index = cmd_update_frequency[2];
                    index = (index << 8) | cmd_update_frequency[1];
                    
                    if (index < 32)
                        if(audio_sound_exists[index] == false)
                        {
                            /* Return error */
                            PAR_RECEIVE_LAST_BYTE_REPLY(true);
                            return 0;
                        }
    
                    /* Return success */
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);                    
                    return command_received;
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
                
            case CMD_UPDATE_AMPLITUDE_LEFT:
                PAR_RECEIVE_BYTE(cmd_update_amplitude_left[1]);
                PAR_RECEIVE_BYTE(cmd_update_amplitude_left[2]);
                PAR_RECEIVE_LAST_BYTE(cmd_update_amplitude_left[3]);
                
                checksum = 0;
                for (i = CMD_UPDATE_AMPLITUDE_LEFT_LEN - 1; i != 0; i--)
                   checksum += cmd_update_amplitude_left[i-1];
                
                if (checksum == cmd_update_amplitude_left[CMD_UPDATE_AMPLITUDE_LEFT_LEN - 1])
                {
                    /* Return success */
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);
                    
                    att_left = cmd_update_amplitude_left[2];
                    att_left = (att_left << 8) | cmd_update_amplitude_left[1];

                    update_audio_volume_left_int(att_left);
                    
                    return 0;
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
                
            case CMD_UPDATE_AMPLITUDE_RIGHT:
                PAR_RECEIVE_BYTE(cmd_update_amplitude_right[1]);
                PAR_RECEIVE_BYTE(cmd_update_amplitude_right[2]);
                PAR_RECEIVE_LAST_BYTE(cmd_update_amplitude_right[3]);
                
                checksum = 0;
                for (i = CMD_UPDATE_AMPLITUDE_RIGHT_LEN - 1; i != 0; i--)
                   checksum += cmd_update_amplitude_right[i-1];
                
                if (checksum == cmd_update_amplitude_right[CMD_UPDATE_AMPLITUDE_RIGHT_LEN - 1])
                {
                    /* Return success */
                    PAR_RECEIVE_LAST_BYTE_REPLY(false);
                    
                    att_right = cmd_update_amplitude_right[2];
                    att_right = (att_right << 8) | cmd_update_amplitude_right[1];

                    update_audio_volume_right_int(att_right);
                    
                    return 0;
                }
                
                /* Return error */
                PAR_RECEIVE_LAST_BYTE_REPLY(true);
                return 0;
        }
    }
    
    return 0;
}

int par_bus_process_command_start(void)
{
    int index = cmd_start[2];
    int att_left = cmd_start[4];
    int att_right = cmd_start[6];
    index = (index << 8) | cmd_start[1];
    att_left = (att_left << 8) | cmd_start[3];
    att_right = (att_right << 8) | cmd_start[5];
    
    /* Update output amplitude */
    update_audio_volume_int(att_left, att_right);
    
    /* Return sound index */
    return index;
}

void par_bus_process_command_stop(void)
{    
    /* Mute the device */
    //set_AUDIO_MUTE;
}

int par_bus_process_command_update_frequency(void)
{
    int index = cmd_update_frequency[2];
    index = (index << 8) | cmd_update_frequency[1];
    
    /* Return sound index */
    return index;
}