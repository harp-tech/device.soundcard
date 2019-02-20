#include <xc.h>
#include "delay.h"
#include "ios.h"

void _tick_delay(int ticks)
{ 
    int StartTime = _CP0_GET_COUNT();
    while((_CP0_GET_COUNT() - StartTime) < ticks); 
}

void _us_delay(int us)
{
    int StartTime = _CP0_GET_COUNT();
    while((_CP0_GET_COUNT() - StartTime) < (us * TICKS_FOR_1US)); 
}

void _ms_delay(int ms)
{
    int StartTime = _CP0_GET_COUNT();
    while((_CP0_GET_COUNT() - StartTime) < (ms * TICKS_FOR_1MS)); 
}

void trigger_delayed_stop(int sample_freq, int num_of_samples)
{
    T2CON   = 0x0;              // Disable timer 2 when setting it up
    TMR2    = 0;                // Set timer 2 counter to 0
    IEC0bits.T2IE = 1;          // Disable Timer 2 Interrupt

    if (sample_freq == 96000)
        PR2 = num_of_samples * (12500000/96000);
    else
        PR2 = num_of_samples * (12500000/192000);   

    T2CONbits.TCKPS = 0b011;    // Pre-scale of 8

    IFS0bits.T2IF = 0;          // Clear interrupt flag for timer 2
    IPC2bits.T2IP = 1;          // Interrupt priority 1
    IPC2bits.T2IS = 1;          // Sub-priority 1
    IEC0bits.T2IE = 1;          // Enable Timer 2 Interrupt
    
    T2CONbits.TON   = 1;        // Turn on timer 2
}

void __attribute__((vector(_TIMER_2_VECTOR), interrupt(ipl3soft), nomips16)) timer2_handler()
{
    IFS0bits.T2IF = 0;          // Clear interrupt flag for timer 2
    T2CON   = 0x0;              // Disable timer 2
    clr_SOUND_IS_ON;
    
}