#ifndef PARALLEL_BUS_H
#define	PARALLEL_BUS_H

#include <xc.h>

// PAR bus @ RA0-7 as input
#define cfg_PAR_BUS TRISASET = 0xFF; ANSELACLR = 0xFF
#define read_PAR_BUS (PORTA & 0xFF)

// CMD_WRITE @ RB5 as INPUT
#define cfg_PAR_CMD_WRITE TRISBSET = (1 << 5); ANSELBCLR = (1 << 5)
#define read_PAR_CMD_WRITE (PORTB & (1 << 5))

// CMD_LATCH @ RB4 as output
#define cfg_PAR_CMD_LATCH TRISBCLR = (1 << 4)
#define set_PAR_CMD_LATCH  LATBSET = (1 << 4)
#define clr_PAR_CMD_LATCH  LATBCLR = (1 << 4)
#define tgl_PAR_CMD_LATCH  LATBINV = (1 << 4)

// CMD_NOT_EXEC @ RB2 as output
#define cfg_PAR_CMD_ERROR TRISBCLR = (1 << 2)
#define set_PAR_CMD_ERROR  LATBSET = (1 << 2)
#define clr_PAR_CMD_ERROR  LATBCLR = (1 << 2)
#define tgl_PAR_CMD_ERROR  LATBINV = (1 << 2)

/************************************************************************/
/* Protocol                                                             */
/************************************************************************/
/* STOP                 11110000                                             checksum(1)
 * START                11110001  index(1)   A_left(2)  A_right(2)           checksum(1)
 * START W/ FREQUENCY   11110010             A_left(2)  A_right(2)  Freq(2)  checksum(1)
 * DELETE_SOUND         11110100  index(1)                                   checksum(1)
 * INDEX                11110011  index(1)                                   checksum(1)  
 * UPDATE AMP           11111001             A_left(2)  A_right(2)           checksum(1)
 * UPDATE AMP. & FREQ.  11111010             A_left(2)  A_right(2)  Freq(2)  checksum(1)
 * UPDATE FREQUENCY     11111011                                    Freq(2)  checksum(1)
 */
#define CMD_STOP 0xF0
#define CMD_START 0xF1
#define CMD_START_W_FREQUENCY 0xF2
#define CMD_INDEX 0xF3    
#define CMD_DELETE_SOUND 0xF7
#define CMD_UPDATE_AMPLITUDE 0xF9
#define CMD_UPDATE_AMPLITUDE_AND_FREQUENCY 0xFA
#define CMD_UPDATE_FREQUENCY 0xFB

#define CMD_STOP_LEN 2
#define CMD_DELETE_SOUND_LEN 3
#define CMD_START_LEN 7

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void initialize_par_ios(void);
int par_bus_check_if_command_is_available(void);

int par_bus_process_command_start(void);
void par_bus_process_command_stop(void);

#endif	/* PARALLEL_BUS_H */


