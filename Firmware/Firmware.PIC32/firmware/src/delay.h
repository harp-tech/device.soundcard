#ifndef DELAY_H
#define	DELAY_H

#define CPU_CLOCK_HZ    (200000000UL)           // CPU Clock Speed in Hz
#define CPU_CT_HZ       (CPU_CLOCK_HZ/2)        // CPU CoreTimer in Hz
#define TICKS_FOR_1US   (CPU_CT_HZ/1000000UL)   // Ticks to produce 1 uS
#define TICKS_FOR_1MS   (CPU_CT_HZ/1000UL)      // Ticks to produce 1 mS

void _tick_delay(int ticks);
void _us_delay(int us);
void _ms_delay(int ms);

void config_timer_for_pin_sound_is_on(void);
void config_timer_for_pin_sound_is_off(void);
void trigger_pin_sound_is_on(int sample_freq);
void trigger_pin_sinewave_is_on(int sample_freq, int samples);
void trigger_pin_sound_is_off(int sample_freq, int num_samples);

#endif	/* DELAY_H */

