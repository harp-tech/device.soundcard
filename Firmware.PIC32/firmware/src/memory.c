#include <xc.h>
#include <stdbool.h>
#include "memory.h"

/*
 * Initializes the uC digital pins.
 */
void initialize_memory_ios (void) {
    cfg_MEM_DATA;
    cfg_MEM_CLE;
    cfg_MEM_ALE;
    cfg_MEM_WP;
    cfg_MEM_BUSY;
    cfg_MEM_CE;
    cfg_MEM_WE;
    cfg_MEM_RE;
    
    set_MEM_WP;     // Removes hardware protection against modification
    set_MEM_CE;     // Un-select the memory    
    clr_MEM_CLE;    // Disable Command Latch Enable
    clr_MEM_ALE;    // Disable Address Latch Enable
    set_MEM_WE;     // Set Write Enable
    set_MEM_RE;     // Set Read Enable
}

/*
 * Check if memory can be accessed.
 */
int check_memory_connection (void)
{
    return read_memory_size();
}

/*
 * Test the functions used to read the memory.
 * The function will be in an infinite loop until find an error
 * which issues the return of -1.
 */
int test_read_routines (void)
{
    unsigned char mem_size;

    
    unsigned char page0_w[2048];
    unsigned char page1_w[2048];
    unsigned char spare0_w[64];
    unsigned char spare1_w[64];
    unsigned char page0_r[2048];
    unsigned char page1_r[2048];
    unsigned char spare0_r[64];
    unsigned char spare1_r[64];
 
    int i;
    for (i = 0; i < 2048; i++)
    {
        page0_w[i] = (i & 0xFF);
        page1_w[i] = 255 - (i & 0xFF);
    }
    for (i = 0; i < 64; i++)
    {
        spare0_w[i] = i;
        spare1_w[i] = 128 | i;
    }
    
    block_erase(1);
    program_memory(PAGES_PER_BLOCK+0, page0_w, spare0_w);        
    program_memory(PAGES_PER_BLOCK+1, page1_w, spare1_w);
    
    while(1)
    {
        mem_size = read_memory_size();    
        if (mem_size != 2 && mem_size != 4)
            return -1;
            
        read_memory(0, page0_r, spare0_r);
        read_memory(1, page1_r, spare1_r);

        for (i = 0; i < 2048; i++)
        {
            if (page0_w[i] != page0_r[i])
            {
                return -1;
            }
            if (page1_w[i] != page1_r[i])
            {
                return -1;
            }
        }
        for (i = 0; i < 64; i++)
        {
            if (spare0_w[i] != spare0_r[i])
            {
                return -1;
            }
            if (spare1_w[i] != spare1_r[i])
            {
                return -1;
            }
        }
    }
}

/*
 * Return the memory size in Gbits of the available memory.
 */
int read_memory_size (void)
{
    int manufacturer_code;
    int device_identifier;
    int byte3;
    int byte4;
    int byte5;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;

    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_ID);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable

    /* Write Address 1 Cycle */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_ALE;    // Each instruction is 20 ns
    clr_MEM_ALE;    // Each instruction is 20 ns
    clr_MEM_ALE;    // Each instruction is 20 ns

    /* Configure data port to input */
    to_input_MEM_DATA;

    /* Read Maker Code */
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    manufacturer_code = read_MEM_DATA;
    set_MEM_RE;

    /* Read Device Code */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    device_identifier = read_MEM_DATA;
    set_MEM_RE;       

    /* Read Byte 3 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte3 = read_MEM_DATA;
    set_MEM_RE;

     /* Read Byte 4 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte4 = read_MEM_DATA;
    set_MEM_RE;

    /* Read Byte 5 */
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte5 = read_MEM_DATA;
    set_MEM_RE;

    /* De-select memory */
    set_MEM_CE;
    
    switch (device_identifier)
    {
        case 0xDA: return 2;
        case 0xDC: return 4;
        default: return -1;
    }
}

/*
 * Deletes a memory Block.
 */
unsigned char block_erase (int block_index)
{
    int block_address = block_index * 64;
    
    unsigned char row_add_1 = block_address & 0xFF;
    unsigned char row_add_2 = (block_address >> 8) & 0xFF;
    unsigned char row_add_3 = (block_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_BLOCK_ERASE);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0xD0);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns  
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);
    
    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
      
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    unsigned char byte;
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

void block_erase_start (int block_index)
{
    int block_address = block_index * 64;
    
    unsigned char row_add_1 = block_address & 0xFF;
    unsigned char row_add_2 = (block_address >> 8) & 0xFF;
    unsigned char row_add_3 = (block_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_BLOCK_ERASE);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0xD0);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
}

bool block_erase_check (void)
{
    return read_MEM_BUSY ? true : false;
}

unsigned char block_erase_finish (void)
{
    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
      
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    unsigned char byte;
    to_input_MEM_DATA;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

/*
 * Writes a Page.
 * 298 us @ 200MHz
 */
unsigned char program_memory (int page_address, unsigned char *page, unsigned char *spare)
{
    unsigned char row_add_1 = page_address & 0xFF;
    unsigned char row_add_2 = (page_address >> 8) & 0xFF;
    unsigned char row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_PROGRAM);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Wait tADL (min. 70 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Program Page */
    int i = 0;
    for (i = 0; i < 2048; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(page[i]);
        set_MEM_WE;
    }    
    
    /* Program Header */
    for (i = 0; i < 64; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(spare[i]);
        set_MEM_WE;
    }
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x10);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);

    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    unsigned char byte;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

/*
 * Writes a Page.
 * 296 us @ 200MHz
 */
unsigned char program_memory_without_spare (int page_address, unsigned char *page)
{
    unsigned char row_add_1 = page_address & 0xFF;
    unsigned char row_add_2 = (page_address >> 8) & 0xFF;
    unsigned char row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_PROGRAM);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Wait tADL (min. 70 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Program Page */
    int i = 0;
    for (i = 0; i < 2048; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(page[i]);
        set_MEM_WE;
    }    
    
    /* Program empty Header */
    for (i = 0; i < 64; i++)
    {
        clr_MEM_WE;
        write_MEM_DATA(0);
        set_MEM_WE;
    }
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x10);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);

    /* Write command to read the status register */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_READ_STATUS_REG);
    set_MEM_WE;        
    clr_MEM_CLE;
    
    /* Wait tWHR (min. 60 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read Byte 3 */
    unsigned char byte;
    clr_MEM_RE;
    clr_MEM_RE;
    clr_MEM_RE;
    byte = read_MEM_DATA;
    set_MEM_RE;
    
    /* De-select memory */
    set_MEM_CE;
    
    /* Return status */
    return byte & 0x03;
}

/*
 * Reads a page.
 * 348 us @ 200 MHz
 */
void read_memory (int page_address, unsigned char *page, unsigned char *spare)
{
    unsigned char row_add_1 = page_address & 0xFF;
    unsigned char row_add_2 = (page_address >> 8) & 0xFF;
    unsigned char row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_READ);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable

    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x30);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read page */
    int i;
    for (i = 0; i < 2048; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;
        page[i] = (unsigned char) (read_MEM_DATA & 0xFF);
        set_MEM_RE;
    }
 
    /* Read spare */
    for (i = 0; i < 64; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;        
        spare[i] = (unsigned char) (read_MEM_DATA & 0xFF);
        set_MEM_RE;
    }
    
    /* De-select memory */
    set_MEM_CE;
}

void read_memory_without_spare (int page_address, unsigned char *page)
{
    unsigned char row_add_1 = page_address & 0xFF;
    unsigned char row_add_2 = (page_address >> 8) & 0xFF;
    unsigned char row_add_3 = (page_address >> 16) & 0xFF;
    
    /* Configure data port to output */
    to_output_MEM_DATA;

    /* Select memory */
    clr_MEM_CE;
    
    /* Write command */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(MEM_REG_PAGE_READ);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable

    /* Write Address Column Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Column Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 1 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_1);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 2 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_2);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write Address Row Address 3 */
    set_MEM_ALE;    // Enable Address Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(row_add_3);
    set_MEM_WE;        
    clr_MEM_ALE;    // Disable Address Latch Enable
    
    /* Write command second cycle */
    set_MEM_CLE;    // Enable Command Latch Enable
    clr_MEM_WE;
    write_MEM_DATA(0x30);
    set_MEM_WE;        
    clr_MEM_CLE;    // Disable Command Latch Enable
    
    /* Wait tWB (max. 100 ns) */
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    clr_MEM_CLE;    // Each instruction is 20 ns
    
    /* Wait until the BUSY line is set */
    while(!read_MEM_BUSY);
    
    /* Configure data port to input */
    to_input_MEM_DATA;
    
    /* Read page */
    int i;
    for (i = 0; i < 2048; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;
        page[i] = (unsigned char) (read_MEM_DATA & 0xFF);
        set_MEM_RE;
    }
 
    /* Read spare */
    for (i = 0; i < 64; i++)
    {
        /* Read Byte */
        clr_MEM_RE;
        clr_MEM_RE;
        clr_MEM_RE;
        set_MEM_RE;
    }
    
    /* De-select memory */
    set_MEM_CE;
}