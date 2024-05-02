#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"
//#include "parallel_bus.h"

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
	
	//app_regs.REG_PLAY_SOUND_OR_FREQ = 3; //default plays index 3
	//app_write_REG_PLAY_SOUND_OR_FREQ(&app_regs.REG_PLAY_SOUND_OR_FREQ);
	
	uint8_t aux = read_DIN0;

	app_regs.REG_DIGITAL_INPUTS = aux;
	app_write_REG_DIGITAL_INPUTS(&app_regs.REG_DIGITAL_INPUTS);
	core_func_send_event(ADD_REG_DIGITAL_INPUTS, true);
	
	par_cmd_start_sound(app_regs.REG_DI0_SOUND_INDEX, app_regs.REG_ATTNUATION_LEFT, app_regs.REG_ATTENUATION_RIGHT);
	
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
/*
ISR(PORTC_INT0_vect, ISR_NAKED)
{
	Digital IN2 don't have interrupt because the available interrupt on
	PortC is being used by the communication between ATXMEGA and PIC32.
   
	reti();
}
*/

/************************************************************************/
/* PIC32_READY                                                          */
/************************************************************************/
ISR(PORTD_INT1_vect, ISR_NAKED)
{   
   reti();
}