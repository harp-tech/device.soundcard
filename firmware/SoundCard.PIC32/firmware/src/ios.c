#include <xc.h>
#include "ios.h"
#include "delay.h"

#define RESET_FROM_SOFTWARE 64

void initialize_ios(int reset_reason_type)
{
    cfg_SOUND_IS_ON;            
    cfg_LED_USB;
    cfg_LED_AUDIO;
    cfg_TP0;
    cfg_TP1;
    cfg_LED_MEMORY;
    
    clr_SOUND_IS_ON;        // Turn sound indication off
    set_LED_USB;            // Turn USB LED on
    set_LED_AUDIO;          // Turn AUDIO LED on
    set_LED_MEMORY;         // Turn MEMORY LED on
    clr_TP0;                // Disable test point 0
    clr_TP1;                // Disable test point 1
    
    cfg_EN_P12V;
    cfg_EN_N12V;
    
    if (reset_reason_type != RESET_FROM_SOFTWARE)
    {
        clr_EN_P12V;            // Disable +12V
        clr_EN_N12V;            // Disable -12V

        //_ms_delay(200);         // Wait 200 ms
        set_EN_P12V;            // Enable +12V
        _ms_delay(1000);        // Wait 1 s
        set_EN_N12V;            // Enable -12V
        _ms_delay(1000);        // Wait 1 s
    }
    else
    {
        set_EN_P12V;            // Enable +12V
        set_EN_N12V;            // Enable -12V
    }
}