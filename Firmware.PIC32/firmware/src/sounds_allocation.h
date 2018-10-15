#ifndef SOUNDS_ALLOCATION_H
#define	SOUNDS_ALLOCATION_H

#include "memory.h"

#define SOUNDS_PER_MEMORY_2G 32
#define SOUNDS_PER_MEMORY_4G SOUNDS_PER_MEMORY_2G * 2

#define PAGES_PER_SOUND PAGES_PER_MEM_2G/SOUNDS_PER_MEMORY_2G
//#define POINTS_PER_SOUND PAGES_PER_SOUND*BYTES_PER_PAGE/4
#define BLOCKS_PER_SOUND PAGES_PER_SOUND/PAGES_PER_BLOCK

/*
 * Structure to accommodate the metadata of each sound.
 * 
 * Note: If samples_length is equal to PAGES_PER_SOUND the board will
 * continue the sound production using the next sound.
 */
typedef struct {
    int sound_index;
    int sound_length;
    int sample_rate;
    int data_type;      // 0: int32, 1: float(4B)
} Sound_Metadata;
#define SOUND_METADATA_LENGTH 16

#define ERROR_NOERROR 0
#define ERROR_BADSOUNDINDEX -1010
#define ERROR_BADSOUNDLENGTH -1011
#define ERROR_BADSAMPLERATE -1012
#define ERROR_BADDATATYPE -1013
#define ERROR_BADDATATYPEMATCH -1014
#define ERROR_BADDATAINDEX -1015
#define ERROR_PRODUCINGSOUND -1020
#define ERROR_STARTEDPRODUCINGSOUND -1021

int get_available_sounds(void);

int save_user_metadata(int sound_index, unsigned char * user_metadata);
int read_user_metadata(int sound_index, unsigned char * user_metadata);

int prepare_memory(int sound_index, int sound_size);

int read_first_sound_page(int sound_index, int *page, Sound_Metadata * metadata);
void set_page_and_sound_index(int page_index, int sound_index);
void read_next_sound_page(int *page);

void allocate_metadata_command (Sound_Metadata metadata, unsigned char *sound_array);
void allocate_data_command (int sound_index, int data_index, unsigned char *sound_array);

#endif	/* SOUNDS_ALLOCATION_H */