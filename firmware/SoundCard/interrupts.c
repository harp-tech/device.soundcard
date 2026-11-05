#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"
#include "parallel_bus.h"

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
bool din0_previous_state = false;

extern uint16_t last_sound_triggered;

ISR(PORTB_INT0_vect, ISR_NAKED)
{
	bool din0 = read_DIN0 ? true : false;
	
	if (din0 != din0_previous_state)
	{
		din0_previous_state = din0;
		
		if (din0)
		{
			switch (app_regs.REG_CONFIGURE_DI0)
			{
				case GM_DI_MODE_DIGITAL:
					app_regs.REG_INPUT_STATE |= B_DI0;
					core_func_send_event(ADD_REG_INPUT_STATE, true);
					break;
				
				case GM_DI_MODE_START_AND_STOP:
				case GM_DI_MODE_START:
					par_cmd_start_sound(app_regs.REG_START_ON_DI0, app_regs.REG_ATTENUATION_LEFT_DI0, app_regs.REG_ATTENUATION_RIGHT_DI0);
					/* Save the sound being played */
					last_sound_triggered = app_regs.REG_START_ON_DI0;
					break;
				
				case GM_DI_MODE_STOP:
					par_cmd_stop();
					break;
			}
		}
		else
		{
			switch (app_regs.REG_CONFIGURE_DI0)
			{
				case GM_DI_MODE_DIGITAL:
					app_regs.REG_INPUT_STATE &= ~B_DI0;				
					core_func_send_event(ADD_REG_INPUT_STATE, true);
					break;
				
				case GM_DI_MODE_START_AND_STOP:
					par_cmd_stop();
					break;
			}
		}
	}
	
	reti();
}

/************************************************************************/
/* DIN1                                                                 */
/************************************************************************/
bool din1_previous_state = false;

ISR(PORTD_INT0_vect, ISR_NAKED)
{
	bool din1 = read_DIN1 ? true : false;
	
	if (din1 != din1_previous_state)
	{
		din1_previous_state = din1;
		
		if (din1)
		{
			switch (app_regs.REG_CONFIGURE_DI1)
			{
				case GM_DI_MODE_DIGITAL:
					app_regs.REG_INPUT_STATE |= B_DI1;
					core_func_send_event(ADD_REG_INPUT_STATE, true);
					break;
				
				case GM_DI_MODE_START_AND_STOP:
				case GM_DI_MODE_START:
					par_cmd_start_sound(app_regs.REG_START_ON_DI1, app_regs.REG_ATTENUATION_LEFT_DI1, app_regs.REG_ATTENUATION_RIGHT_DI1);
					/* Save the sound being played */
					last_sound_triggered = app_regs.REG_START_ON_DI1;
					break;
				
				case GM_DI_MODE_STOP:
					par_cmd_stop();
					break;
			}
		}
		else
		{
			switch (app_regs.REG_CONFIGURE_DI1)
			{
				case GM_DI_MODE_DIGITAL:
					app_regs.REG_INPUT_STATE &= ~B_DI1;				
					core_func_send_event(ADD_REG_INPUT_STATE, true);
					break;
				
				case GM_DI_MODE_START_AND_STOP:
					par_cmd_stop();
					break;
			}
		}
	}
	
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


/************************************************************************/
/* ADC                                                                  */
/************************************************************************/
extern int16_t AdcOffset;

extern bool first_adc_channel;

ISR(ADCA_CH0_vect, ISR_NAKED)
{	
	if (first_adc_channel)
	{
		first_adc_channel = false;
		
		/* Read ADC */
		if (ADCA_CH0_RES > AdcOffset)
			app_regs.REG_ADC_CONTROL_STATE[0] = (ADCA_CH0_RES & 0x0FFF) - AdcOffset;
		else
			app_regs.REG_ADC_CONTROL_STATE[0] = 0;
		
		/* Start conversation on ADCA Channel 9 */
		ADCA_CH0_MUXCTRL = 9 << 3;
		ADCA_CH0_CTRL |= ADC_CH_START_bm;
	}
	else
	{	
		/* Read ADC */
		if (ADCA_CH0_RES > AdcOffset)
			app_regs.REG_ADC_CONTROL_STATE[1] = (ADCA_CH0_RES & 0x0FFF) - AdcOffset;
		else
			app_regs.REG_ADC_CONTROL_STATE[1] = 0;

		core_func_send_event(ADD_REG_ADC_CONTROL_STATE, false);
	}
	
	reti();
}

/************************************************************************/
/* STOP CMD SENT                                                        */
/************************************************************************/
ISR(TCC0_OVF_vect, ISR_NAKED)
{
	core_func_send_event(ADD_REG_STOP, true);
	timer_type0_stop(&TCC0);
	
	reti();
}

/************************************************************************/
/* CLEAR DIGITAL OUTPUT                                                 */
/************************************************************************/
ISR(TCE0_OVF_vect, ISR_NAKED)
{
	if (app_regs.REG_CONFIGURE_DO0 == GM_DO_MODE_PULSE_ON_START) {clr_DOUT0;}
	if (app_regs.REG_CONFIGURE_DO1 == GM_DO_MODE_PULSE_ON_START) {clr_DOUT1;}
	if (app_regs.REG_CONFIGURE_DO2 == GM_DO_MODE_PULSE_ON_START) {clr_DOUT2;}
	
	timer_type0_stop(&TCE0);
	
	reti();
}