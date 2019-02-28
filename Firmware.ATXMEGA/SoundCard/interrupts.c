#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
//
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
//
// ISR(TCD1_OVF_vect, ISR_NAKED)
//
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/
/* DIN0                                                                 */
/************************************************************************/
ISR(PORTB_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* DIN1                                                                 */
/************************************************************************/
ISR(PORTD_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* DIN2                                                                 */
/************************************************************************/
ISR(PORTC_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* PIC32_READY                                                          */
/************************************************************************/
ISR(PORTD_INT1_vect, ISR_NAKED)
{   
   reti();
}