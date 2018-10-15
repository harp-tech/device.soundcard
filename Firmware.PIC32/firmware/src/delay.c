#include <xc.h>
#include "delay.h"

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
