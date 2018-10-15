#include "sounds_allocation.h"
#include "memory.h"

int get_available_sounds(void)
{
    static int available_sounds = -1;
    if (available_sounds == -1)
    {
        if (read_memory_size() == 2)
            available_sounds = SOUNDS_PER_MEMORY_2G;
        else
            available_sounds = SOUNDS_PER_MEMORY_4G;
    }
    
    return available_sounds;
}

int save_user_metadata(int sound_index, unsigned char * user_metadata)
{
    if (sound_index > get_available_sounds())
        return -1;
    
    block_erase(sound_index);    
    program_memory_without_spare(sound_index * PAGES_PER_BLOCK, user_metadata);
    
    return 0;
}

int read_user_metadata(int sound_index, unsigned char * user_metadata)
{
    if (sound_index > get_available_sounds())
        return -1;
    
    read_memory_without_spare(sound_index * PAGES_PER_BLOCK, user_metadata);
    
    return 0;
}

/*
 sound_size is the number of samples of 4 bytes each
 */
int prepare_memory(int sound_index, int sound_size)
{
    int number_of_pages = sound_size * 4 / BYTES_PER_PAGE;
    if (((sound_size * 4) % BYTES_PER_PAGE) > 0)
        number_of_pages++;
    
    if (sound_index * PAGES_PER_SOUND + number_of_pages > get_available_sounds() * PAGES_PER_SOUND)
        return -1;
    
    int number_of_blocks = number_of_pages / PAGES_PER_BLOCK;
    if ((number_of_pages % PAGES_PER_BLOCK) > 0)
        number_of_blocks++;
    
    int i = 0;
    for (; i < number_of_blocks; i++)
    {
        block_erase(sound_index * BLOCKS_PER_SOUND + i);
    }
    
    return 0;
}

int _sound_index;
int _page_index;

int read_first_sound_page(int sound_index, int *page, Sound_Metadata * metadata)
{
    if (sound_index >= get_available_sounds()) return -1;
    
    read_memory(
        sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK,
        (unsigned char*)(page),
        (unsigned char*)(metadata)
    );
    
    if (metadata->sample_rate != 96000 && metadata->sample_rate != 192000) return -1;
    if (metadata->data_type != 0 && metadata->data_type != 1) return -1;
    if (metadata->sound_index != sound_index) return -1;
    
    _sound_index = sound_index;
    _page_index = 0;
    
    return 0;
}

void set_page_and_sound_index(int page_index, int sound_index)
{
    _page_index = page_index;
    _sound_index = sound_index;
}

void read_next_sound_page(int *page)
{
    _page_index++;
    
    read_memory_without_spare(
        _sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + _page_index,
        (unsigned char*)(page)
    );
}

void allocate_metadata_command (Sound_Metadata metadata, unsigned char *sound_array)
{
    int i;
    
    program_memory(
        metadata.sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK,
        sound_array,
        (unsigned char*)(&metadata)
    );
            
    for (i = 1; i < 32768/BYTES_PER_PAGE; i++)
        program_memory_without_spare(
            metadata.sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + i,
            sound_array + BYTES_PER_PAGE * i
        );
}

void allocate_data_command (int sound_index, int data_index, unsigned char *sound_array)
{
    int i;
    
    for (i = 0; i < 32768/BYTES_PER_PAGE; i++)
        program_memory_without_spare(
            sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + i + data_index * 32768 / BYTES_PER_PAGE,
            sound_array + BYTES_PER_PAGE * i
        );
}