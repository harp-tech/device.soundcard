#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "parallel_bus.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "SoundCard";

#define MAJOR_FW_VERSION 3

void hwbp_app_initialize(void)
{
   /* Define versions */
   uint8_t hwH = 2;
   uint8_t hwL = 2;
   uint8_t fwH = MAJOR_FW_VERSION;
   uint8_t fwL = 0;
   uint8_t ass = 0;
   
		/* Start core */
	core_func_start_core(
		1280,
		hwH, hwL,
		fwH, fwL,
		ass,
		(uint8_t*)(&app_regs),
		APP_NBYTES_OF_REG_BANK,
		APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
		default_device_name,
		false,	// The device is _not_ able to repeat the harp timestamp clock
		false,	// The device is _not_ able to generate the harp timestamp clock
		0	// Default timestamp offset
	);
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	
}

/************************************************************************/
/* General definitions                                                  */
/************************************************************************/
// #define NBYTES 23

/************************************************************************/
/* General used functions                                               */
/************************************************************************/
/* Load external functions if needed */
//#include "hwbp_app_pwm_gen_funcs.c"

/* Initializes an ADC for reading the temperature sensor.
 * It uses the channel 0 of the selected ADC.
 */
void temperature_initialize_ADC (ADC_t* adc)
{
   /* Remove power reduction */
   PR_PRPA &= ~(PR_ADC_bm);
   
   /* Enable ADC */
   adc->CTRLA = ADC_ENABLE_bm;
   
   /* Signed mode, 12 bits right adjusted. */
   adc->CTRLB = ADC_CONMODE_bm | ADC_RESOLUTION_12BIT_gc;
   
   /* Internal reference 1V, enable band gap, enable temperature sensor. */
   adc->REFCTRL = ADC_REFSEL_INT1V_gc | ADC_BANDGAP_bm | ADC_TEMPREF_bm;
   
   /* Set prescaler to 256 relative to the peripheral clock. */
   adc->PRESCALER = ADC_PRESCALER_DIV256_gc;
   
   /* On channel 0, select an internal source. */
   adc->CH0.CTRL = ADC_CH_INPUTMODE_INTERNAL_gc;
   
   /* On channel 0, select the temperature sensor. */
   adc->CH0.MUXCTRL = ADC_CH_MUXINT_TEMP_gc;
   //adc->CH0.MUXCTRL = ADC_CH_MUXINT_BANDGAP_gc;
   
   /* On channel 0, configure interrupt for COMPLETE. */
   adc->CH0.INTCTRL = ADC_CH_INTMODE_COMPLETE_gc;
}

/* Reads the temperature.
 * The temperature returned is in the format TBD.
 */
uint8_t temperature_read(ADC_t* adc)
{  
   /* Start conversion on channel 0 and wait for completion. */
   adc->CH0.CTRL |= ADC_CH_START_bm;
   while(!(adc->CH0.INTFLAGS & ADC_CH_CHIF_bm));
   adc->CH0.INTFLAGS = ADC_CH_CHIF_bm;

   /* Read conversion */
   uint16_t temperature = adc->CH0.RES;
   uint16_t tempsense = 0x089B;           // TEMPSENSE1 << 8 | TEMPESENSE0
   
   /* Return conversion */
   return (uint8_t) (( ((uint32_t) temperature) * 85) / tempsense);
}



/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
uint16_t AdcOffset;

void core_callback_define_clock_default(void) {}
	
void core_callback_initialize_hardware(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
   
	/* Initialize ADC */
	PR_PRPA &= ~(PR_ADC_bm);									// Remove power reduction
	ADCA_CTRLA = ADC_ENABLE_bm;								// Enable ADCA
	ADCA_CTRLB = ADC_CURRLIMIT_HIGH_gc;						// High current limit, max. sampling rate 0.5MSPS
	ADCA_CTRLB  |= ADC_RESOLUTION_12BIT_gc;				// 12-bit result, right adjusted
	ADCA_REFCTRL = ADC_REFSEL_INTVCC_gc;					// VCC/1.6 = 3.3/1.6 = 2.0625 V
	ADCA_PRESCALER = ADC_PRESCALER_DIV128_gc;				// 250 ksps
	// Propagation Delay = (1 + 12[bits]/2 + 1[gain]) / fADC[125k] = 32 us
	// Note: For single measurements, Propagation Delay is equal to Conversion Time
	
	ADCA_CH0_CTRL = ADC_CH_INPUTMODE_SINGLEENDED_gc;	// Single-ended positive input signal
	ADCA_CH0_INTCTRL = ADC_CH_INTMODE_COMPLETE_gc;		// Rise interrupt flag when conversion is complete
	ADCA_CH0_INTCTRL |= ADC_CH_INTLVL_OFF_gc;				// Interrupt is not used
	
	/* Wait 100 us to stabilization before measure the first time */
	timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV2, 1600, INT_LEVEL_OFF);
	while(!timer_type0_get_flag(&TCD0));
	timer_type0_stop(&TCD0);
	
	AdcOffset = 180;

	/* Point ADC to the ADC0 analog input */
	ADCA_CH0_MUXCTRL = 10 << 3;							   // Select pin for further conversions
	ADCA_CH0_CTRL |= ADC_CH_START_bm;						// Force the first conversion
	while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));		// Wait for conversion to finish
	ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;						// Clear interrupt bit
}

void core_callback_reset_registers(void) {}
   
void core_callback_registers_were_reinitialized(void) {}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update channels enable indicators */
	//update_enabled_pwmx();
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void) {}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
void core_callback_t_before_exec(void) {}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void) {}
void core_callback_t_500us(void) {}
void core_callback_t_1ms(void)
{
   /* Read ADC0 */
   ADCA_CH0_MUXCTRL = 10 << 3;							   // Select pin
   ADCA_CH0_CTRL |= ADC_CH_START_bm;						// Start conversion
   while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));		// Wait for conversion to finish
   ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;						// Clear interrupt bit
   if (ADCA_CH0_RES > AdcOffset)
      app_regs.REG_DATA_STREAM[0] = (ADCA_CH0_RES & 0x0FFF) - AdcOffset;
   else
      app_regs.REG_DATA_STREAM[0] = 0;
      
   /* Read ADC1 */
   ADCA_CH0_MUXCTRL = 9 << 3;							      // Select pin
   ADCA_CH0_CTRL |= ADC_CH_START_bm;						// Start conversion
   while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));		// Wait for conversion to finish
   ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;						// Clear interrupt bit
   if (ADCA_CH0_RES > AdcOffset)
      app_regs.REG_DATA_STREAM[1] = (ADCA_CH0_RES & 0x0FFF) - AdcOffset;
   else
      app_regs.REG_DATA_STREAM[1] = 0;
      
   if (app_regs.REG_DATA_STREAM_CONF == GM_DATA_STREAM_1KHz)
	{
		app_regs.REG_DATA_STREAM[2] = app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[1];
		app_regs.REG_DATA_STREAM[3] = app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[2];
		app_regs.REG_DATA_STREAM[4] = app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[0];
		
		core_func_send_event(ADD_REG_DATA_STREAM, true);
	}
}

/************************************************************************/
/* Callbacks: clock control                                              */
/************************************************************************/
void core_callback_clock_to_repeater(void) {}
void core_callback_clock_to_generator(void) {}
void core_callback_clock_to_unlock(void) {}
void core_callback_clock_to_lock(void) {}
	
/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}