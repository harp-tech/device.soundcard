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

// CMD_ERROR @ RG6 as output
#define cfg_PAR_CMD_ERROR TRISGCLR = (1 << 6)
#define set_PAR_CMD_ERROR  LATGSET = (1 << 6)
#define clr_PAR_CMD_ERROR  LATGCLR = (1 << 6)
#define tgl_PAR_CMD_ERROR  LATGINV = (1 << 6)

void initialize_par_ios(void);
int par_bus_check_for_command(void);

#endif	/* PARALLEL_BUS_H */


