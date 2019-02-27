#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"
#include "parallel_bus.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_PLAY_SOUND_OR_FREQ,
	&app_read_REG_STOP,
	&app_read_REG_ATTNUATION_LEFT,
	&app_read_REG_ATTENUATION_RIGHT,
	&app_read_REG_ATTENUATION_BOTH,
	&app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ,
	&app_read_REG_RESERVED0,
	&app_read_REG_RESERVED1,
	&app_read_REG_DIGITAL_INPUTS,
	&app_read_REG_DI0_CONF,
	&app_read_REG_DI1_CONF,
	&app_read_REG_DI2_CONF,
	&app_read_REG_DI0_SOUND_INDEX,
	&app_read_REG_DI1_SOUND_INDEX,
	&app_read_REG_DI2_SOUND_INDEX,
	&app_read_REG_DI0_FREQ,
	&app_read_REG_DI1_FREQ,
	&app_read_REG_DI2_FREQ,
	&app_read_REG_DI0_ATTNUATION_LEFT,
	&app_read_REG_DI1_ATTNUATION_LEFT,
	&app_read_REG_DI2_ATTNUATION_LEFT,
	&app_read_REG_DI0_ATTENUATION_RIGHT,
	&app_read_REG_DI1_ATTENUATION_RIGHT,
	&app_read_REG_DI2_ATTENUATION_RIGHT,
	&app_read_REG_DI0_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DI1_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DI2_ATTENUATION_AND_SOUND_INDEX,
	&app_read_REG_DI0_ATTENUATION_AND_FREQUENCY,
	&app_read_REG_DI1_ATTENUATION_AND_FREQUENCY,
	&app_read_REG_DI2_ATTENUATION_AND_FReQUENCY,
	&app_read_REG_RESERVED2,
	&app_read_REG_RESERVED3,
	&app_read_REG_RESERVED4,
	&app_read_REG_DO0_CONF,
	&app_read_REG_DO1_CONF,
	&app_read_REG_DO2_CONF,
	&app_read_REG_DO0_PULSE,
	&app_read_REG_DO1_PULSE,
	&app_read_REG_DO2_PULSE,
	&app_read_REG_RESERVED5,
	&app_read_REG_RESERVED6,
	&app_read_REG_RESERVED7,
	&app_read_REG_DO_SET,
	&app_read_REG_DO_CLEAR,
	&app_read_REG_DO_TOGGLE,
	&app_read_REG_DO_OUT,
	&app_read_REG_RESERVED8,
	&app_read_REG_RESERVED9,
	&app_read_REG_ADC_CONF,
	&app_read_REG_ADC_VALUES,
	&app_read_REG_BOOTLOADER,
	&app_read_REG_RESERVED10,
	&app_read_REG_RESERVED11,
	&app_read_REG_RESERVED12,
	&app_read_REG_EVNT_ENABLE
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_PLAY_SOUND_OR_FREQ,
	&app_write_REG_STOP,
	&app_write_REG_ATTNUATION_LEFT,
	&app_write_REG_ATTENUATION_RIGHT,
	&app_write_REG_ATTENUATION_BOTH,
	&app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ,
	&app_write_REG_RESERVED0,
	&app_write_REG_RESERVED1,
	&app_write_REG_DIGITAL_INPUTS,
	&app_write_REG_DI0_CONF,
	&app_write_REG_DI1_CONF,
	&app_write_REG_DI2_CONF,
	&app_write_REG_DI0_SOUND_INDEX,
	&app_write_REG_DI1_SOUND_INDEX,
	&app_write_REG_DI2_SOUND_INDEX,
	&app_write_REG_DI0_FREQ,
	&app_write_REG_DI1_FREQ,
	&app_write_REG_DI2_FREQ,
	&app_write_REG_DI0_ATTNUATION_LEFT,
	&app_write_REG_DI1_ATTNUATION_LEFT,
	&app_write_REG_DI2_ATTNUATION_LEFT,
	&app_write_REG_DI0_ATTENUATION_RIGHT,
	&app_write_REG_DI1_ATTENUATION_RIGHT,
	&app_write_REG_DI2_ATTENUATION_RIGHT,
	&app_write_REG_DI0_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DI1_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DI2_ATTENUATION_AND_SOUND_INDEX,
	&app_write_REG_DI0_ATTENUATION_AND_FREQUENCY,
	&app_write_REG_DI1_ATTENUATION_AND_FREQUENCY,
	&app_write_REG_DI2_ATTENUATION_AND_FReQUENCY,
	&app_write_REG_RESERVED2,
	&app_write_REG_RESERVED3,
	&app_write_REG_RESERVED4,
	&app_write_REG_DO0_CONF,
	&app_write_REG_DO1_CONF,
	&app_write_REG_DO2_CONF,
	&app_write_REG_DO0_PULSE,
	&app_write_REG_DO1_PULSE,
	&app_write_REG_DO2_PULSE,
	&app_write_REG_RESERVED5,
	&app_write_REG_RESERVED6,
	&app_write_REG_RESERVED7,
	&app_write_REG_DO_SET,
	&app_write_REG_DO_CLEAR,
	&app_write_REG_DO_TOGGLE,
	&app_write_REG_DO_OUT,
	&app_write_REG_RESERVED8,
	&app_write_REG_RESERVED9,
	&app_write_REG_ADC_CONF,
	&app_write_REG_ADC_VALUES,
	&app_write_REG_BOOTLOADER,
	&app_write_REG_RESERVED10,
	&app_write_REG_RESERVED11,
	&app_write_REG_RESERVED12,
	&app_write_REG_EVNT_ENABLE
};


/************************************************************************/
/* REG_PLAY_SOUND_OR_FREQ                                               */
/************************************************************************/
void app_read_REG_PLAY_SOUND_OR_FREQ(void) {}
bool app_write_REG_PLAY_SOUND_OR_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);
   
   if (reg < 32)
      par_cmd_start_sound(reg, app_regs.REG_ATTNUATION_LEFT, app_regs.REG_ATTENUATION_RIGHT);

	app_regs.REG_PLAY_SOUND_OR_FREQ = reg;
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
/* REG_ATTENUATION_BOTH                                                 */
/************************************************************************/
void app_read_REG_ATTENUATION_BOTH(void)
{
	//app_regs.REG_ATTENUATION_BOTH = 0;

}

bool app_write_REG_ATTENUATION_BOTH(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_BOTH[0] = reg;
	return true;
}


/************************************************************************/
/* REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ                           */
/************************************************************************/
void app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void)
{
	//app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ = 0;

}

bool app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[0] = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED0                                                        */
/************************************************************************/
void app_read_REG_RESERVED0(void)
{
	//app_regs.REG_RESERVED0 = 0;

}

bool app_write_REG_RESERVED0(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED0 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED1                                                        */
/************************************************************************/
void app_read_REG_RESERVED1(void)
{
	//app_regs.REG_RESERVED1 = 0;

}

bool app_write_REG_RESERVED1(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED1 = reg;
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
/* REG_DI0_FREQ                                                         */
/************************************************************************/
void app_read_REG_DI0_FREQ(void)
{
	//app_regs.REG_DI0_FREQ = 0;

}

bool app_write_REG_DI0_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI0_FREQ = reg;
	return true;
}


/************************************************************************/
/* REG_DI1_FREQ                                                         */
/************************************************************************/
void app_read_REG_DI1_FREQ(void)
{
	//app_regs.REG_DI1_FREQ = 0;

}

bool app_write_REG_DI1_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI1_FREQ = reg;
	return true;
}


/************************************************************************/
/* REG_DI2_FREQ                                                         */
/************************************************************************/
void app_read_REG_DI2_FREQ(void)
{
	//app_regs.REG_DI2_FREQ = 0;

}

bool app_write_REG_DI2_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_DI2_FREQ = reg;
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
/* REG_DI0_ATTENUATION_AND_FREQUENCY                                    */
/************************************************************************/
// This register is an array with 2 positions
void app_read_REG_DI0_ATTENUATION_AND_FREQUENCY(void)
{
	//app_regs.REG_DI0_ATTENUATION_AND_FREQUENCY[0] = 0;

}

bool app_write_REG_DI0_ATTENUATION_AND_FREQUENCY(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI0_ATTENUATION_AND_FREQUENCY[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DI1_ATTENUATION_AND_FREQUENCY                                    */
/************************************************************************/
// This register is an array with 2 positions
void app_read_REG_DI1_ATTENUATION_AND_FREQUENCY(void)
{
	//app_regs.REG_DI1_ATTENUATION_AND_FREQUENCY[0] = 0;

}

bool app_write_REG_DI1_ATTENUATION_AND_FREQUENCY(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI1_ATTENUATION_AND_FREQUENCY[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_DI2_ATTENUATION_AND_FReQUENCY                                    */
/************************************************************************/
// This register is an array with 2 positions
void app_read_REG_DI2_ATTENUATION_AND_FReQUENCY(void)
{
	//app_regs.REG_DI2_ATTENUATION_AND_FReQUENCY[0] = 0;

}

bool app_write_REG_DI2_ATTENUATION_AND_FReQUENCY(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_DI2_ATTENUATION_AND_FReQUENCY[0] = reg[0];
	return true;
}


/************************************************************************/
/* REG_RESERVED2                                                        */
/************************************************************************/
void app_read_REG_RESERVED2(void)
{
	//app_regs.REG_RESERVED2 = 0;

}

bool app_write_REG_RESERVED2(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED2 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED3                                                        */
/************************************************************************/
void app_read_REG_RESERVED3(void)
{
	//app_regs.REG_RESERVED3 = 0;

}

bool app_write_REG_RESERVED3(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED3 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED4                                                        */
/************************************************************************/
void app_read_REG_RESERVED4(void)
{
	//app_regs.REG_RESERVED4 = 0;

}

bool app_write_REG_RESERVED4(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED4 = reg;
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
/* REG_RESERVED5                                                        */
/************************************************************************/
void app_read_REG_RESERVED5(void)
{
	//app_regs.REG_RESERVED5 = 0;

}

bool app_write_REG_RESERVED5(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED5 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED6                                                        */
/************************************************************************/
void app_read_REG_RESERVED6(void)
{
	//app_regs.REG_RESERVED6 = 0;

}

bool app_write_REG_RESERVED6(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED6 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED7                                                        */
/************************************************************************/
void app_read_REG_RESERVED7(void)
{
	//app_regs.REG_RESERVED7 = 0;

}

bool app_write_REG_RESERVED7(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED7 = reg;
	return true;
}


/************************************************************************/
/* REG_DO_SET                                                           */
/************************************************************************/
void app_read_REG_DO_SET(void)
{
   app_regs.REG_DO_SET = 0;
}
bool app_write_REG_DO_SET(void *a)
{
	uint8_t reg = *((uint8_t*)a);
   
   if (reg & B_DO0) set_DOUT0;
   if (reg & B_DO1) set_DOUT1;
   if (reg & B_DO1) set_DOUT1;

	app_regs.REG_DO_SET = reg;
	return true;
}


/************************************************************************/
/* REG_DO_CLEAR                                                         */
/************************************************************************/
void app_read_REG_DO_CLEAR(void)
{
   app_regs.REG_DO_CLEAR = 0;
}
bool app_write_REG_DO_CLEAR(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_DO0) clr_DOUT0;
	if (reg & B_DO1) clr_DOUT1;
	if (reg & B_DO1) clr_DOUT1;

	app_regs.REG_DO_CLEAR = reg;
	return true;
}


/************************************************************************/
/* REG_DO_TOGGLE                                                        */
/************************************************************************/
void app_read_REG_DO_TOGGLE(void)
{
   app_regs.REG_DO_TOGGLE = 0;
}
bool app_write_REG_DO_TOGGLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_DO0) tgl_DOUT0;
	if (reg & B_DO1) tgl_DOUT1;
	if (reg & B_DO1) tgl_DOUT1;

	app_regs.REG_DO_TOGGLE = reg;
	return true;
}


/************************************************************************/
/* REG_DO_OUT                                                           */
/************************************************************************/
void app_read_REG_DO_OUT(void)
{
	app_regs.REG_DO_OUT  = read_DOUT0 ? B_DO0 : 0;
   app_regs.REG_DO_OUT |= read_DOUT1 ? B_DO1 : 0;
   app_regs.REG_DO_OUT |= read_DOUT2 ? B_DO2 : 0;
}

bool app_write_REG_DO_OUT(void *a)
{
	uint8_t reg = *((uint8_t*)a);
   
   if (reg & B_DO0) set_DOUT0; else clr_DOUT0;
   if (reg & B_DO1) set_DOUT1; else clr_DOUT1;
   if (reg & B_DO2) set_DOUT2; else clr_DOUT2;

	app_regs.REG_DO_OUT = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED8                                                        */
/************************************************************************/
void app_read_REG_RESERVED8(void)
{
	//app_regs.REG_RESERVED8 = 0;

}

bool app_write_REG_RESERVED8(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED8 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED9                                                        */
/************************************************************************/
void app_read_REG_RESERVED9(void)
{
	//app_regs.REG_RESERVED9 = 0;

}

bool app_write_REG_RESERVED9(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED9 = reg;
	return true;
}


/************************************************************************/
/* REG_ADC_CONF                                                         */
/************************************************************************/
void app_read_REG_ADC_CONF(void)
{
	//app_regs.REG_ADC_CONF = 0;

}

bool app_write_REG_ADC_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_ADC_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_ADC_VALUES                                                       */
/************************************************************************/
// This register is an array with 5 positions
void app_read_REG_ADC_VALUES(void)
{
	//app_regs.REG_ADC_VALUES[0] = 0;

}

bool app_write_REG_ADC_VALUES(void *a)
{
	uint16_t *reg = ((uint16_t*)a);

	app_regs.REG_ADC_VALUES[0] = reg[0];
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
   uint8_t reg = *((uint8_t*)a);
    
   if (reg)
      set_BOOTLOADER_EN;
   else
      clr_BOOTLOADER_EN;
      
   return true;
    
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
/* REG_RESERVED10                                                       */
/************************************************************************/
void app_read_REG_RESERVED10(void)
{
	//app_regs.REG_RESERVED10 = 0;

}

bool app_write_REG_RESERVED10(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED10 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED11                                                       */
/************************************************************************/
void app_read_REG_RESERVED11(void)
{
	//app_regs.REG_RESERVED11 = 0;

}

bool app_write_REG_RESERVED11(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED11 = reg;
	return true;
}


/************************************************************************/
/* REG_RESERVED12                                                       */
/************************************************************************/
void app_read_REG_RESERVED12(void)
{
	//app_regs.REG_RESERVED12 = 0;

}

bool app_write_REG_RESERVED12(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_RESERVED12 = reg;
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