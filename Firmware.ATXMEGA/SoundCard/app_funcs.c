#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"
#include "parallel_bus.h"

/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX,
	&app_read_REG_STOP,
	&app_read_REG_ATTENUATION_BOTH,
	&app_read_REG_PLAY_SOUND_INDEX,
	&app_read_REG_ATTENUATION_RIGHT,
	&app_read_REG_ATTNUATION_LEFT,
	&app_read_REG_DIGITAL_INPUTS,
	&app_read_REG_DI0_CONF,
	&app_read_REG_DI1_CONF,
	&app_read_REG_DI2_CONF,
	&app_read_REG_DI0_SOUND_INDEX,
	&app_read_REG_DI1_SOUND_INDEX,
	&app_read_REG_DI2_SOUND_INDEX,
	&app_read_REG_DI0_ATTENUATION_RIGHT,
	&app_read_REG_DI1_ATTENUATION_RIGHT,
	&app_read_REG_DI2_ATTENUATION_RIGHT,
	&app_read_REG_DI0_ATTNUATION_LEFT,
	&app_read_REG_DI1_ATTNUATION_LEFT,
	&app_read_REG_DI2_ATTNUATION_LEFT,
	&app_read_REG_DI0_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DI1_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DI2_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DO0_CONF,
	&app_read_REG_DO1_CONF,
	&app_read_REG_DO2_CONF,
	&app_read_REG_DO0_PULSE,
	&app_read_REG_DO1_PULSE,
	&app_read_REG_DO2_PULSE,
	&app_read_REG_DO_SET,
	&app_read_REG_DO_CLEAR,
	&app_read_REG_DO_TOGGLE,
	&app_read_REG_DO_OUT,
	&app_read_REG_ADC0_CONF,
	&app_read_REG_BOOTLOADER,
	&app_read_REG_EVNT_ENABLE
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX,
	&app_write_REG_STOP,
	&app_write_REG_ATTENUATION_BOTH,
	&app_write_REG_PLAY_SOUND_INDEX,
	&app_write_REG_ATTENUATION_RIGHT,
	&app_write_REG_ATTNUATION_LEFT,
	&app_write_REG_DIGITAL_INPUTS,
	&app_write_REG_DI0_CONF,
	&app_write_REG_DI1_CONF,
	&app_write_REG_DI2_CONF,
	&app_write_REG_DI0_SOUND_INDEX,
	&app_write_REG_DI1_SOUND_INDEX,
	&app_write_REG_DI2_SOUND_INDEX,
	&app_write_REG_DI0_ATTENUATION_RIGHT,
	&app_write_REG_DI1_ATTENUATION_RIGHT,
	&app_write_REG_DI2_ATTENUATION_RIGHT,
	&app_write_REG_DI0_ATTNUATION_LEFT,
	&app_write_REG_DI1_ATTNUATION_LEFT,
	&app_write_REG_DI2_ATTNUATION_LEFT,
	&app_write_REG_DI0_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DI1_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DI2_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DO0_CONF,
	&app_write_REG_DO1_CONF,
	&app_write_REG_DO2_CONF,
	&app_write_REG_DO0_PULSE,
	&app_write_REG_DO1_PULSE,
	&app_write_REG_DO2_PULSE,
	&app_write_REG_DO_SET,
	&app_write_REG_DO_CLEAR,
	&app_write_REG_DO_TOGGLE,
	&app_write_REG_DO_OUT,
	&app_write_REG_ADC0_CONF,
	&app_write_REG_BOOTLOADER,
	&app_write_REG_EVNT_ENABLE
};


/************************************************************************/
/* REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX                             */
/************************************************************************/
// This register is an array with 3 positions
void app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX(void)
{
	//app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX[0] = 0;

}

bool app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_STOP                                                             */
/************************************************************************/
void app_read_REG_STOP(void)
{
	//app_regs.REG_STOP = 0;

}

bool app_write_REG_STOP(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_STOP = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_BOTH                                                 */
/************************************************************************/
// This register is an array with 2 positions
void app_read_REG_ATTENUATION_BOTH(void)
{
	//app_regs.REG_ATTENUATION_BOTH[0] = 0;

}

bool app_write_REG_ATTENUATION_BOTH(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_ATTENUATION_BOTH[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_PLAY_SOUND_INDEX                                                 */
/************************************************************************/
void app_read_REG_PLAY_SOUND_INDEX(void)
{
	//app_regs.REG_PLAY_SOUND_INDEX = 0;

}

bool app_write_REG_PLAY_SOUND_INDEX(void *a)
{
	uint8_t reg = *((uint8_t*)a);
   
   par_cmd_start_sound(reg);

	app_regs.REG_PLAY_SOUND_INDEX = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_RIGHT                                                */
/************************************************************************/
void app_read_REG_ATTENUATION_RIGHT(void)
{
	//app_regs.REG_ATTENUATION_RIGHT = 0;

}

bool app_write_REG_ATTENUATION_RIGHT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_RIGHT = reg;
	return true;
}


/************************************************************************/
/* REG_ATTNUATION_LEFT                                                  */
/************************************************************************/
void app_read_REG_ATTNUATION_LEFT(void)
{
	//app_regs.REG_ATTNUATION_LEFT = 0;

}

bool app_write_REG_ATTNUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTNUATION_LEFT = reg;
	return true;
}


/************************************************************************/
/* REG_DIGITAL_INPUTS                                                   */
/************************************************************************/
void app_read_REG_DIGITAL_INPUTS(void)
{
	//app_regs.REG_DIGITAL_INPUTS = 0;

}

bool app_write_REG_DIGITAL_INPUTS(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DIGITAL_INPUTS = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_CONF                                                         */
/************************************************************************/
void app_read_REG_DI0_CONF(void)
{
	//app_regs.REG_DI0_CONF = 0;

}

bool app_write_REG_DI0_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI0_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DI1_CONF                                                         */
/************************************************************************/
void app_read_REG_DI1_CONF(void)
{
	//app_regs.REG_DI1_CONF = 0;

}

bool app_write_REG_DI1_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI1_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DI2_CONF                                                         */
/************************************************************************/
void app_read_REG_DI2_CONF(void)
{
	//app_regs.REG_DI2_CONF = 0;

}

bool app_write_REG_DI2_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI2_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_SOUND_INDEX                                                  */
/************************************************************************/
void app_read_REG_DI0_SOUND_INDEX(void)
{
	//app_regs.REG_DI0_SOUND_INDEX = 0;

}

bool app_write_REG_DI0_SOUND_INDEX(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI0_SOUND_INDEX = reg;
	return true;
}


/************************************************************************/
/* REG_DI1_SOUND_INDEX                                                  */
/************************************************************************/
void app_read_REG_DI1_SOUND_INDEX(void)
{
	//app_regs.REG_DI1_SOUND_INDEX = 0;

}

bool app_write_REG_DI1_SOUND_INDEX(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI1_SOUND_INDEX = reg;
	return true;
}


/************************************************************************/
/* REG_DI2_SOUND_INDEX                                                  */
/************************************************************************/
void app_read_REG_DI2_SOUND_INDEX(void)
{
	//app_regs.REG_DI2_SOUND_INDEX = 0;

}

bool app_write_REG_DI2_SOUND_INDEX(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DI2_SOUND_INDEX = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_ATTENUATION_RIGHT                                            */
/************************************************************************/
void app_read_REG_DI0_ATTENUATION_RIGHT(void)
{
	//app_regs.REG_DI0_ATTENUATION_RIGHT = 0;

}

bool app_write_REG_DI0_ATTENUATION_RIGHT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI0_ATTENUATION_RIGHT = reg;
	return true;
}


/************************************************************************/
/* REG_DI1_ATTENUATION_RIGHT                                            */
/************************************************************************/
void app_read_REG_DI1_ATTENUATION_RIGHT(void)
{
	//app_regs.REG_DI1_ATTENUATION_RIGHT = 0;

}

bool app_write_REG_DI1_ATTENUATION_RIGHT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI1_ATTENUATION_RIGHT = reg;
	return true;
}


/************************************************************************/
/* REG_DI2_ATTENUATION_RIGHT                                            */
/************************************************************************/
void app_read_REG_DI2_ATTENUATION_RIGHT(void)
{
	//app_regs.REG_DI2_ATTENUATION_RIGHT = 0;

}

bool app_write_REG_DI2_ATTENUATION_RIGHT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI2_ATTENUATION_RIGHT = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_ATTNUATION_LEFT                                              */
/************************************************************************/
void app_read_REG_DI0_ATTNUATION_LEFT(void)
{
	//app_regs.REG_DI0_ATTNUATION_LEFT = 0;

}

bool app_write_REG_DI0_ATTNUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI0_ATTNUATION_LEFT = reg;
	return true;
}


/************************************************************************/
/* REG_DI1_ATTNUATION_LEFT                                              */
/************************************************************************/
void app_read_REG_DI1_ATTNUATION_LEFT(void)
{
	//app_regs.REG_DI1_ATTNUATION_LEFT = 0;

}

bool app_write_REG_DI1_ATTNUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI1_ATTNUATION_LEFT = reg;
	return true;
}


/************************************************************************/
/* REG_DI2_ATTNUATION_LEFT                                              */
/************************************************************************/
void app_read_REG_DI2_ATTNUATION_LEFT(void)
{
	//app_regs.REG_DI2_ATTNUATION_LEFT = 0;

}

bool app_write_REG_DI2_ATTNUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI2_ATTNUATION_LEFT = reg;
	return true;
}


/************************************************************************/
/* REG_DI0_ATTENUATION_AND_SOUND_INDEX                                  */
/************************************************************************/
// This register is an array with 3 positions
void app_read_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void)
{
	//app_regs.REG_DI0_ATTENUATION_AND_SOUND_INDEX[0] = 0;

}

bool app_write_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI0_ATTENUATION_AND_SOUND_INDEX[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DI1_ATTENUATION_AND_SOUND_INDEX                                  */
/************************************************************************/
// This register is an array with 3 positions
void app_read_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void)
{
	//app_regs.REG_DI1_ATTENUATION_AND_SOUND_INDEX[0] = 0;

}

bool app_write_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI1_ATTENUATION_AND_SOUND_INDEX[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DI2_ATTENUATION_AND_SOUND_INDEX                                  */
/************************************************************************/
// This register is an array with 3 positions
void app_read_REG_DI2_ATTENUATION_AND_SOUND_INDEX(void)
{
	//app_regs.REG_DI2_ATTENUATION_AND_SOUND_INDEX[0] = 0;

}

bool app_write_REG_DI2_ATTENUATION_AND_SOUND_INDEX(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI2_ATTENUATION_AND_SOUND_INDEX[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DO0_CONF                                                         */
/************************************************************************/
void app_read_REG_DO0_CONF(void)
{
	//app_regs.REG_DO0_CONF = 0;

}

bool app_write_REG_DO0_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO0_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DO1_CONF                                                         */
/************************************************************************/
void app_read_REG_DO1_CONF(void)
{
	//app_regs.REG_DO1_CONF = 0;

}

bool app_write_REG_DO1_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO1_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DO2_CONF                                                         */
/************************************************************************/
void app_read_REG_DO2_CONF(void)
{
	//app_regs.REG_DO2_CONF = 0;

}

bool app_write_REG_DO2_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO2_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DO0_PULSE                                                        */
/************************************************************************/
void app_read_REG_DO0_PULSE(void)
{
	//app_regs.REG_DO0_PULSE = 0;

}

bool app_write_REG_DO0_PULSE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO0_PULSE = reg;
	return true;
}


/************************************************************************/
/* REG_DO1_PULSE                                                        */
/************************************************************************/
void app_read_REG_DO1_PULSE(void)
{
	//app_regs.REG_DO1_PULSE = 0;

}

bool app_write_REG_DO1_PULSE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO1_PULSE = reg;
	return true;
}


/************************************************************************/
/* REG_DO2_PULSE                                                        */
/************************************************************************/
void app_read_REG_DO2_PULSE(void)
{
	//app_regs.REG_DO2_PULSE = 0;

}

bool app_write_REG_DO2_PULSE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO2_PULSE = reg;
	return true;
}


/************************************************************************/
/* REG_DO_SET                                                           */
/************************************************************************/
void app_read_REG_DO_SET(void)
{
	//app_regs.REG_DO_SET = 0;

}

bool app_write_REG_DO_SET(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO_SET = reg;
	return true;
}


/************************************************************************/
/* REG_DO_CLEAR                                                         */
/************************************************************************/
void app_read_REG_DO_CLEAR(void)
{
	//app_regs.REG_DO_CLEAR = 0;

}

bool app_write_REG_DO_CLEAR(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO_CLEAR = reg;
	return true;
}


/************************************************************************/
/* REG_DO_TOGGLE                                                        */
/************************************************************************/
void app_read_REG_DO_TOGGLE(void)
{
	//app_regs.REG_DO_TOGGLE = 0;

}

bool app_write_REG_DO_TOGGLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO_TOGGLE = reg;
	return true;
}


/************************************************************************/
/* REG_DO_OUT                                                           */
/************************************************************************/
void app_read_REG_DO_OUT(void)
{
	//app_regs.REG_DO_OUT = 0;

}

bool app_write_REG_DO_OUT(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DO_OUT = reg;
	return true;
}


/************************************************************************/
/* REG_ADC0_CONF                                                        */
/************************************************************************/
void app_read_REG_ADC0_CONF(void)
{
	//app_regs.REG_ADC0_CONF = 0;

}

bool app_write_REG_ADC0_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_ADC0_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_BOOTLOADER                                                       */
/************************************************************************/
#define PIC32_BOOTLOADER_STATE__STANDBY 0
#define PIC32_BOOTLOADER_STATE__START_BOOTLOADER_PROCESS 1

extern uint8_t PIC32_READY_state;

void app_read_REG_BOOTLOADER(void) {}

bool app_write_REG_BOOTLOADER(void *a)
{
   if ((*((uint8_t*)a) & 1) == B_EN_BOOT)
   {
      app_regs.REG_EVNT_ENABLE = B_EN_BOOT;
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__START_BOOTLOADER_PROCESS;
   }
   else
   {
      app_regs.REG_EVNT_ENABLE = 0;
      
      clr_BOOTLOADER_EN;      // Disable buffers
      timer_type1_stop(&TCD1);
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__STANDBY;
   }
   
   return true;
}


/************************************************************************/
/* REG_EVNT_ENABLE                                                      */
/************************************************************************/
void app_read_REG_EVNT_ENABLE(void)
{
	//app_regs.REG_EVNT_ENABLE = 0;

}

bool app_write_REG_EVNT_ENABLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_EVNT_ENABLE = reg;
	return true;
}