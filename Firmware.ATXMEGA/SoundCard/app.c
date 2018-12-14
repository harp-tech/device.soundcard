#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "parallel_bus.h"
#include "uart1.h"

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

#define MAJOR_FW_VERSION 0

void hwbp_app_initialize(void)
{
   /* Define versions */
   uint8_t hwH = 1;
   uint8_t hwL = 0;
   uint8_t fwH = MAJOR_FW_VERSION;
   uint8_t fwL = 1;
   uint8_t ass = 1;
   
   /* Start core */
   core_func_start_core(
   1280,
   hwH, hwL,
   fwH, fwL,
   ass,
   (uint8_t*)(&app_regs),
   APP_NBYTES_OF_REG_BANK,
   APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
   default_device_name
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

#define BPOD_MODULE_INFO_LENGTH 1+4+1+9+1

void uart1_rcv_byte_callback(uint8_t byte)
{
   if (byte == 255)
   {
      uint8_t reply[BPOD_MODULE_INFO_LENGTH];
      reply[0] = 65;                               // Acknowledge
      
      *((int32_t*)(&reply[1])) = MAJOR_FW_VERSION; // 4-byte firwmare version as 32-bit unsigned
      reply[5] = 10;                               // Length of module's name
      for (uint8_t i = 0; i < 9; i++)
         reply[i+6] = default_device_name[i];      // Module's name
         
      reply[BPOD_MODULE_INFO_LENGTH-1] = 0;        // 1 if more info follows. 0 if not
      
      uart1_xmit(reply, (uint8_t) BPOD_MODULE_INFO_LENGTH);
   }
   
   if (byte < 31)
   {
      par_cmd_start_sound(byte, 0, 0);
   }
   
   if (byte == 0x88)
   {
      par_cmd_stop();
   }
}

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_1st_config_hw_after_boot(void)
{
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	/* SYNC OUTx */
	//io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_DIS);

	/* TRIG_IN0 */
	//io_pin2in(&PORTF, 5, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
	//io_set_int(&PORTF, INT_LEVEL_LOW, 0, (1<<5), true);
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	/*
	app_regs.REG_CH0_FREQ = 10.0;
	app_regs.REG_CH0_DUTYCYCLE = 50;
	
	if ((app_regs.REG_MODE0 & B_M0) == GM_USB_MODE)
	{
		app_regs.REG_OUT0 = 0;
	}

	if ((app_regs.REG_MODE1AND2 & B_M1AND2) == GM_USB_MODE)
	{
		app_regs.REG_OUT1 = 0;
		app_regs.REG_OUT2 = 0;
	}
	*/
}

void core_callback_registers_were_reinitialized(void)
{
	/* Check if the user indication is valid */
	//update_enabled_pwmx();
	
	/* Update state register */
	//app_regs.REG_TRIG_STATE = (read_TRIG_IN0) ? B_LTRG0 : 0;
	//app_regs.REG_TRIG_STATE |= (read_TRIG_IN1) ? B_LTRG1 : 0;

	/* Reset start bits */
	//app_regs.REG_TRG0_START = 0;
	//app_regs.REG_TRG1_START = 0;

	/*
	if ((app_regs.REG_MODE0 & B_M0) == GM_BNC_MODE)
	{
		app_regs.REG_OUT0 = app_regs.REG_CTRL0;
		set_OUT0(app_regs.REG_OUT0);
	}
	else
	{
		set_OUT0(app_regs.REG_OUT0);
	}
	*/
}

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
void core_callback_t_1ms(void) {}

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