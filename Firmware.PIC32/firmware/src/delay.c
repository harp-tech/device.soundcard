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

/*
void trigger_delayed_stop(int sample_freq, int num_of_samples)
{
    set_TP0;
    T2CON   = 0x0;              // Disable timer 2 when setting it up
    TMR2    = 0;                // Set timer 2 counter to 0
    IEC0bits.T2IE = 1;          // Disable Timer 2 Interrupt

    if (sample_freq == 96000)
        PR2 = num_of_samples * 16 * (200000000/256/96000.0);
    else
        PR2 = num_of_samples * 16 * (200000000/256/192000.0);

    T2CONbits.TCKPS = 0b11;    // Pre-scale of 256

    IFS0bits.T2IF = 0;          // Clear interrupt flag for timer 2
    IPC2bits.T2IP = 1;          // Interrupt priority 1
    IPC2bits.T2IS = 1;          // Sub-priority 1
    IEC0bits.T2IE = 1;          // Enable Timer 2 Interrupt
    
    T2CONbits.TON   = 1;        // Turn on timer 2
}

void __attribute__((vector(_TIMER_2_VECTOR), interrupt(ipl3soft), nomips16)) timer2_handler()
{
    clr_TP0;
    clr_SOUND_IS_ON;
    IFS0bits.T2IF = 0;          // Clear interrupt flag for timer 2
    T2CON   = 0x0;              // Disable timer 2    
}
*/

//http://www.aidanmocke.com/blog/2018/11/15/timers/

#define INT_T2_PRIORITY_N 1
#define INT_T2_PRIORITY_TLDR_SOFT ipl1soft  // Forces it to use software context switching
#define INT_T2_PRIORITY_TLDR_SRS ipl1srs    // Forces it to use the shadow register set, which is much faster.
#define INT_T3_PRIORITY_N 1
#define INT_T3_PRIORITY_TLDR_SOFT ipl1soft
#define INT_T3_PRIORITY_TLDR_SRS ipl1srs

void config_timer_for_pin_sound_is_on(void)
{
    T2CON   = 0x0;                      // Disable timer
    //IEC0bits.T2IE = 1;                // Disable interrupt
    T2CONbits.TCKPS = 0;                // Pre-scale of 1
    IFS0bits.T2IF = 0;                  // Clear interrupt flag
    IPC2bits.T2IP = INT_T2_PRIORITY_N;  // Interrupt priority
    IPC2bits.T2IS = 1;                  // Sub-priority 1
    IEC0bits.T2IE = 1;                  // Enable interrupt
}

void config_timer_for_pin_sound_is_off(void)
{
    T3CON   = 0x0;                      // Disable timer
    //IEC0bits.T2IE = 1;                // Disable interrupt
    T3CONbits.TCKPS = 0;                // Pre-scale of 1
    IFS0bits.T3IF = 0;                  // Clear interrupt flag
    IPC3bits.T3IP = INT_T3_PRIORITY_N;  // Interrupt priority
    IPC3bits.T3IS = 1;                  // Sub-priority 1
    IEC0bits.T3IE = 1;                  // Enable interrupt
}


void trigger_pin_sound_is_on(int sample_freq)
{
    TMR2    = 0;                        // Set counter to 0
    
    if (sample_freq == 96000)
        PR2 = 818;                      // 266us
    else
        PR2 = 274;                      // 89us
    
    T2CONbits.TON   = 1;                // Turn on
}

void __attribute__((vector(_TIMER_2_VECTOR), interrupt(INT_T2_PRIORITY_TLDR_SOFT), nomips16)) timer2_handler()
{
    set_SOUND_IS_ON;
    IFS0bits.T2IF = 0;                  // Clear interrupt flag
    //T2CON   = 0x0;                      // Disable timer
    T2CONbits.TON = 0;                  // Turn off
}

void trigger_pin_sound_is_off(int sample_freq, int num_samples)
{
    TMR3    = 0;                        // Set counter to 0
    
    if (sample_freq == 96000)
        PR3 = num_samples/2 * 32 + 818 + 40;
    else
        PR3 = num_samples/2 * 16 + 274 + 15;
    
    T3CONbits.TON   = 1;                // Turn on
}

void __attribute__((vector(_TIMER_3_VECTOR), interrupt(INT_T3_PRIORITY_TLDR_SOFT), nomips16)) timer3_handler()
{
    clr_SOUND_IS_ON;
    IFS0bits.T3IF = 0;                  // Clear interrupt flag
    //T3CON   = 0x0;                      // Disable timer
    T3CONbits.TON = 0;                  // Turn off
}