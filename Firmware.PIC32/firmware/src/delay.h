#ifndef DELAY_H
#define	DELAY_H

#define CPU_CLOCK_HZ    (200000000UL)           // CPU Clock Speed in Hz
#define CPU_CT_HZ       (CPU_CLOCK_HZ/2)        // CPU CoreTimer in Hz
#define TICKS_FOR_1US   (CPU_CT_HZ/1000000UL)   // Ticks to produce 1 uS
#define TICKS_FOR_1MS   (CPU_CT_HZ/1000UL)      // Ticks to produce 1 mS

void _tick_delay(int ticks);
void _us_delay(int us);
void _ms_delay(int ms);

#endif	/* DELAY_H */

