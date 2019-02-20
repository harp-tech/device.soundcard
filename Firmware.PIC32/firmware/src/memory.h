#ifndef MEMORY_H
#define	MEMORY_H

#define PAGES_PER_MEM_2G 131072
#define PAGES_PER_MEM_4G 262144
#define BYTES_PER_PAGE 2048
#define PAGES_PER_BLOCK 64

#include <xc.h>
#include <stdbool.h>

// CLE @ RD13 as output
#define cfg_MEM_CLE TRISDCLR = (1 << 13)
#define set_MEM_CLE  LATDSET = (1 << 13)
#define clr_MEM_CLE  LATDCLR = (1 << 13)
#define tgl_MEM_CLE  LATDINV = (1 << 13)

// ALE @ RG7 as output *
#define cfg_MEM_ALE TRISGCLR = (1 << 7)
#define set_MEM_ALE  LATGSET = (1 << 7)
#define clr_MEM_ALE  LATGCLR = (1 << 7)
#define tgl_MEM_ALE  LATGINV = (1 << 7)

// WP# @ RG8 as output *
#define cfg_MEM_WP TRISGCLR = (1 << 8)
#define set_MEM_WP  LATGSET = (1 << 8)
#define clr_MEM_WP  LATGCLR = (1 << 8)
#define tgl_MEM_WP  LATGINV = (1 << 8)

// R/B# @ RG9 as INPUT *
#define cfg_MEM_BUSY TRISGSET = (1 << 9); ANSELG  &= ~(1 << 9)
#define read_MEM_BUSY (PORTG & (1 << 9))

// CE# @ RD12 as output
#define cfg_MEM_CE TRISDCLR = (1 << 12)
#define set_MEM_CE  LATDSET = (1 << 12)
#define clr_MEM_CE  LATDCLR = (1 << 12)
#define tgl_MEM_CE  LATDINV = (1 << 12)

// WE# @ RC3 as output *
#define cfg_MEM_WE TRISCCLR = (1 << 3)
#define set_MEM_WE  LATCSET = (1 << 3)
#define clr_MEM_WE  LATCCLR = (1 << 3)
#define tgl_MEM_WE  LATCINV = (1 << 3)

// RE# @ RC4 as output *
#define cfg_MEM_RE TRISCCLR = (1 << 4)
#define set_MEM_RE  LATCSET = (1 << 4)
#define clr_MEM_RE  LATCCLR = (1 << 4)
#define tgl_MEM_RE  LATCINV = (1 << 4)

// IO0-7 @ RE0-7 as inputs
#define cfg_MEM_DATA TRISESET = 0x000000FF; ANSELE  &= ~0x000000FF
#define to_output_MEM_DATA TRISECLR = 0xFF
#define to_input_MEM_DATA TRISESET = 0xFF
#define write_MEM_DATA(value) LATE = value
#define read_MEM_DATA PORTE



// Commands list
#define MEM_REG_PAGE_READ 0x00
#define MEM_REG_BLOCK_ERASE 0x60
#define MEM_REG_READ_STATUS_REG 0x70
#define MEM_REG_PAGE_PROGRAM 0x80
#define MEM_REG_READ_ID 0x90

// Prototypes
void initialize_memory_ios (void);
int check_memory_connection (void);
int test_read_routines (void);
int read_memory_size (void);
unsigned char block_erase (int block_index);
void block_erase_start (int block_index);
bool block_erase_check (void);
unsigned char block_erase_finish (void);
unsigned char program_memory (int page_address, unsigned char *page, unsigned char *spare);
unsigned char program_memory_without_spare (int page_address, unsigned char *page);
void read_memory (int page_address, unsigned char *page, unsigned char *spare);
void read_memory_without_spare (int page_address, unsigned char *page);

#endif	/* MEMORY_H */