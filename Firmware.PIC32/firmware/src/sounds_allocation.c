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

#define SAVE_METADATA_STATE_STANDBY 0
#define SAVE_METADATA_STATE_CHECK_ERASE 1
#define SAVE_METADATA_STATE_ERASE_IS_DONE 2
static int save_user_metadata_state = SAVE_METADATA_STATE_STANDBY;

bool save_user_metadata(int sound_index, unsigned char * user_metadata)
{
    switch (save_user_metadata_state)
    {   
        case SAVE_METADATA_STATE_STANDBY:
            block_erase_start(sound_index);            
            save_user_metadata_state = SAVE_METADATA_STATE_CHECK_ERASE;
            break;
        
        case SAVE_METADATA_STATE_CHECK_ERASE:
            if (block_erase_check())
            {
                block_erase_finish();
                save_user_metadata_state = SAVE_METADATA_STATE_ERASE_IS_DONE;
            }
            else
            {
                save_user_metadata_state = SAVE_METADATA_STATE_CHECK_ERASE;
            }
            break;
        
        case SAVE_METADATA_STATE_ERASE_IS_DONE:
            program_memory_without_spare(sound_index * PAGES_PER_BLOCK, user_metadata);
            save_user_metadata_state = SAVE_METADATA_STATE_STANDBY;
            return true;
    }
    
    return false;
}

/*
int save_user_metadata(int sound_index, unsigned char * user_metadata)
{
    if (sound_index > get_available_sounds())
        return -1;
    
    block_erase(sound_index);    
    program_memory_without_spare(sound_index * PAGES_PER_BLOCK, user_metadata);
    
    return 0;
}
*/

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
#define PREPARE_MEMORY_STATE_STANDBY 0
#define PREPARE_MEMORY_STATE_CHECK_ERASE 1
#define PREPARE_MEMORY_STATE_ERASE_IS_DONE 2
static int prepare_memory_state = PREPARE_MEMORY_STATE_STANDBY;

static int number_of_blocks_index;
static int number_of_blocks_to_erase;
static int sound_index_to_erase;

bool prepare_memory_check(int sound_index, int sound_size)
{
    int number_of_pages = sound_size * 4 / BYTES_PER_PAGE;
    if (((sound_size * 4) % BYTES_PER_PAGE) > 0)
        number_of_pages++;
    
    number_of_blocks_to_erase = number_of_pages / PAGES_PER_BLOCK;
    if ((number_of_pages % PAGES_PER_BLOCK) > 0)
        number_of_blocks_to_erase++;
    
    sound_index_to_erase = sound_index;
    number_of_blocks_index = 0;
    
    if (sound_index * PAGES_PER_SOUND + number_of_pages > get_available_sounds() * PAGES_PER_SOUND)
        return false;
    
    return true;
}

bool prepare_memory_erase(void)
{
    switch (prepare_memory_state)
    {   
        case PREPARE_MEMORY_STATE_STANDBY:
            block_erase_start(sound_index_to_erase * BLOCKS_PER_SOUND + number_of_blocks_index);            
            prepare_memory_state = PREPARE_MEMORY_STATE_CHECK_ERASE;
            break;
        
        case PREPARE_MEMORY_STATE_CHECK_ERASE:
            if (block_erase_check())
            {
                block_erase_finish();
                prepare_memory_state = PREPARE_MEMORY_STATE_STANDBY;
                
                number_of_blocks_index++;
                
                if (number_of_blocks_to_erase == number_of_blocks_index)
                    return true;
            }
            break;
    }
    
    return false;
}

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

#define ALLOCATE_METADATA_STATE_STANDBY 0
#define ALLOCATE_METADATA_PROGRAM_MEMORY 1
static int allocate_metadata_state = ALLOCATE_METADATA_STATE_STANDBY;


static int number_of_pages_index;

bool allocate_metadata_command (Sound_Metadata metadata, unsigned char *sound_array)
{
    switch (allocate_metadata_state)
    {
        case ALLOCATE_METADATA_STATE_STANDBY:
            program_memory(
                metadata.sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK,
                sound_array,
                (unsigned char*)(&metadata)
            );
            
            number_of_pages_index = 1;
            
            allocate_metadata_state = ALLOCATE_METADATA_PROGRAM_MEMORY;
            
            break;
         
        case ALLOCATE_METADATA_PROGRAM_MEMORY:
            program_memory_without_spare(
                metadata.sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + number_of_pages_index,
                sound_array + BYTES_PER_PAGE * number_of_pages_index
            );
            
            number_of_pages_index++;
            
            if (number_of_pages_index == 32768/BYTES_PER_PAGE)
            {
                allocate_metadata_state = ALLOCATE_METADATA_STATE_STANDBY;
                return true;
            }
            
            break;
    }
    
    return false;
}

/*
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
*/

static int allocate_data_command_counter;

int allocate_data_command (int sound_index, int data_index, unsigned char *sound_array)
{
    program_memory_without_spare(
        sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + allocate_data_command_counter + data_index * 32768 / BYTES_PER_PAGE,
        sound_array + BYTES_PER_PAGE * allocate_data_command_counter
    );
    
    allocate_data_command_counter++;
    return allocate_data_command_counter;
}

void allocate_data_command_reset (void)
{
    allocate_data_command_counter = 0;
}

/*
void allocate_data_command (int sound_index, int data_index, unsigned char *sound_array)
{
    int i;
    
    for (i = 0; i < 32768/BYTES_PER_PAGE; i++)
        program_memory_without_spare(
            sound_index * BLOCKS_PER_SOUND * PAGES_PER_BLOCK + i + data_index * 32768 / BYTES_PER_PAGE,
            sound_array + BYTES_PER_PAGE * i
        );
}
*/

void clean_memory (void)
{
    int i = 0;
    
    for (; i < get_available_sounds(); i++)
        block_erase(i * BLOCKS_PER_SOUND);
}