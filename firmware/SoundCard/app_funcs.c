#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"
#include "parallel_bus.h"


/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_START,
	&app_read_REG_STOP,
	&app_read_REG_ATTENUATION_LEFT,
	&app_read_REG_ATTENUATION_RIGHT,
	&app_read_REG_ATTENUATION_BOTH,
	&app_read_REG_ATTENUATION_START,
	&app_read_REG_DELETE_SOUND,
	&app_read_REG_DELETE_ALL_SOUNDS,
	&app_read_REG_INPUT_STATE,
	&app_read_REG_CONFIGURE_DI0,
	&app_read_REG_CONFIGURE_DI1,
	&app_read_REG_START_ON_DI0,
	&app_read_REG_START_ON_DI1,
	&app_read_REG_ATTENUATION_LEFT_DI0,
	&app_read_REG_ATTENUATION_LEFT_DI1,
	&app_read_REG_ATTENUATION_RIGHT_DI0,
	&app_read_REG_ATTENUATION_RIGHT_DI1,
	&app_read_REG_RESERVED2,
	&app_read_REG_RESERVED3,
	&app_read_REG_RESERVED4,
	&app_read_REG_CONFIGURE_DO0,
	&app_read_REG_CONFIGURE_DO1,
	&app_read_REG_CONFIGURE_DO2,
	&app_read_REG_RESERVED5,
	&app_read_REG_RESERVED6,
	&app_read_REG_RESERVED7,
	&app_read_REG_OUTPUT_SET,
	&app_read_REG_OUTPUT_CLEAR,
	&app_read_REG_OUTPUT_TOGGLE,
	&app_read_REG_OUTPUT_STATE,
	&app_read_REG_RESERVED8,
	&app_read_REG_RESERVED9,
	&app_read_REG_ENABLE_ADC_CONTROL_STATE,
	&app_read_REG_ADC_CONTROL_STATE,
	&app_read_REG_CONFIGURE_ADC0,
	&app_read_REG_CONFIGURE_ADC1,
	&app_read_REG_RESERVED10,
	&app_read_REG_RESERVED11,
	&app_read_REG_RESERVED12,
	&app_read_REG_RESERVED13,
	&app_read_REG_RESERVED14,
	&app_read_REG_RESERVED15,
	&app_read_REG_RESERVED16,
	&app_read_REG_RESERVED17,
	&app_read_REG_RESERVED18,
	&app_read_REG_RESERVED19,
	&app_read_REG_RESERVED20,
	&app_read_REG_RESERVED21,
	&app_read_REG_RESERVED22,
	&app_read_REG_RESERVED23,
	&app_read_REG_PIC32_BOOTLOADER
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_START,
	&app_write_REG_STOP,
	&app_write_REG_ATTENUATION_LEFT,
	&app_write_REG_ATTENUATION_RIGHT,
	&app_write_REG_ATTENUATION_BOTH,
	&app_write_REG_ATTENUATION_START,
	&app_write_REG_DELETE_SOUND,
	&app_write_REG_DELETE_ALL_SOUNDS,
	&app_write_REG_INPUT_STATE,
	&app_write_REG_CONFIGURE_DI0,
	&app_write_REG_CONFIGURE_DI1,
	&app_write_REG_START_ON_DI0,
	&app_write_REG_START_ON_DI1,
	&app_write_REG_ATTENUATION_LEFT_DI0,
	&app_write_REG_ATTENUATION_LEFT_DI1,
	&app_write_REG_ATTENUATION_RIGHT_DI0,
	&app_write_REG_ATTENUATION_RIGHT_DI1,
	&app_write_REG_RESERVED2,
	&app_write_REG_RESERVED3,
	&app_write_REG_RESERVED4,
	&app_write_REG_CONFIGURE_DO0,
	&app_write_REG_CONFIGURE_DO1,
	&app_write_REG_CONFIGURE_DO2,
	&app_write_REG_RESERVED5,
	&app_write_REG_RESERVED6,
	&app_write_REG_RESERVED7,
	&app_write_REG_OUTPUT_SET,
	&app_write_REG_OUTPUT_CLEAR,
	&app_write_REG_OUTPUT_TOGGLE,
	&app_write_REG_OUTPUT_STATE,
	&app_write_REG_RESERVED8,
	&app_write_REG_RESERVED9,
	&app_write_REG_ENABLE_ADC_CONTROL_STATE,
	&app_write_REG_ADC_CONTROL_STATE,
	&app_write_REG_CONFIGURE_ADC0,
	&app_write_REG_CONFIGURE_ADC1,
	&app_write_REG_RESERVED10,
	&app_write_REG_RESERVED11,
	&app_write_REG_RESERVED12,
	&app_write_REG_RESERVED13,
	&app_write_REG_RESERVED14,
	&app_write_REG_RESERVED15,
	&app_write_REG_RESERVED16,
	&app_write_REG_RESERVED17,
	&app_write_REG_RESERVED18,
	&app_write_REG_RESERVED19,
	&app_write_REG_RESERVED20,
	&app_write_REG_RESERVED21,
	&app_write_REG_RESERVED22,
	&app_write_REG_RESERVED23,
	&app_write_REG_PIC32_BOOTLOADER
};


/************************************************************************/
/* REG_START                                                            */
/************************************************************************/
uint16_t last_sound_triggered = 0;	// 0 means it's stopped

void app_read_REG_START(void) {}
bool app_write_REG_START(void *a)
{
	uint16_t reg = *((uint16_t*)a);
   
   if (reg <= 1 || reg > 40000)
      return false;

   if (last_sound_triggered != 0)
      /* Previous event was not sent yet */ 
      return false;

	app_regs.REG_ATTENUATION_START[0] = reg;
	
	app_regs.REG_START = reg;
		
	/* Save the sound being played */
	last_sound_triggered = reg;
	
	//par_cmd_start_sound(reg, app_regs.REG_ATTENUATION_LEFT, app_regs.REG_ATTENUATION_RIGHT);
	par_cmd_update_frequency(reg);
	
	return true;
}


/************************************************************************/
/* REG_STOP                                                             */
/************************************************************************/
void app_read_REG_STOP(void) {}
bool app_write_REG_STOP(void *a)
{
	uint8_t reg = *((uint8_t*)a);
    
    if (reg)
	{
        par_cmd_stop();
		last_sound_triggered = 0;
	}

	app_regs.REG_STOP = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_LEFT                                                  */
/************************************************************************/
void app_read_REG_ATTENUATION_LEFT(void) {}
bool app_write_REG_ATTENUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_BOTH[0] = reg;
	app_regs.REG_ATTENUATION_START[1] = reg;

	app_regs.REG_ATTENUATION_LEFT = reg;
	
	par_cmd_update_amplitude_left(reg);
	
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_RIGHT                                                */
/************************************************************************/
void app_read_REG_ATTENUATION_RIGHT(void) {}
bool app_write_REG_ATTENUATION_RIGHT(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	app_regs.REG_ATTENUATION_BOTH[1] = reg;
	app_regs.REG_ATTENUATION_START[2] = reg;

	app_regs.REG_ATTENUATION_RIGHT = reg;
	
	par_cmd_update_amplitude_right(reg);
	
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_BOTH                                                 */
/************************************************************************/
void app_read_REG_ATTENUATION_BOTH(void) {}
bool app_write_REG_ATTENUATION_BOTH(void *a)
{
	uint16_t *reg = ((uint16_t*)a);
		
	app_regs.REG_ATTENUATION_LEFT    = reg[0];
	app_regs.REG_ATTENUATION_RIGHT  = reg[1];
	
	app_regs.REG_ATTENUATION_START[1] = reg[0];
	app_regs.REG_ATTENUATION_START[2] = reg[1];
	
	app_regs.REG_ATTENUATION_BOTH[0] = reg[0];
	app_regs.REG_ATTENUATION_BOTH[1] = reg[1];
	
	par_cmd_update_amplitude(reg[0], reg[1]);
	
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_START                                                */
/************************************************************************/
void app_read_REG_ATTENUATION_START(void) {}
bool app_write_REG_ATTENUATION_START(void *a)
{
	uint16_t *reg = ((uint16_t*)a);
	
	if (reg[0] <= 1 || reg[0] > 40000)
		return false;

	if (last_sound_triggered != 0)
      /* Previous event was not sent yet */ 
      return false;
	
	app_regs.REG_START = reg[0];
	app_regs.REG_ATTENUATION_LEFT    = reg[1];
	app_regs.REG_ATTENUATION_RIGHT  = reg[2];
	app_regs.REG_ATTENUATION_BOTH[0] = reg[1];
	app_regs.REG_ATTENUATION_BOTH[1] = reg[2];
	
	app_regs.REG_ATTENUATION_START[0] = reg[0];
	app_regs.REG_ATTENUATION_START[1] = reg[1];
	app_regs.REG_ATTENUATION_START[2] = reg[2];
	
	/* Save the sound being played */
	last_sound_triggered = reg[0];
	
	par_cmd_start_sound(reg[0], reg[1], reg[2]);

	return true;
}


/************************************************************************/
/* REG_DELETE_SOUND                                                     */
/************************************************************************/
void app_read_REG_DELETE_SOUND(void) {}
bool app_write_REG_DELETE_SOUND(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if (reg < 2 || reg > 31)
		return false;

	app_regs.REG_DELETE_SOUND = reg;
	par_cmd_delete_sound(reg, false);
	return true;
}


/************************************************************************/
/* REG_DELETE_ALL_SOUNDS                                                */
/************************************************************************/
void app_read_REG_DELETE_ALL_SOUNDS(void) {}
bool app_write_REG_DELETE_ALL_SOUNDS(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	app_regs.REG_DELETE_ALL_SOUNDS = reg;
	if (reg)
		par_cmd_delete_sound(0, true);

	return true;
}


/************************************************************************/
/* REG_INPUT_STATE                                                      */
/************************************************************************/
void app_read_REG_INPUT_STATE(void)
{
	app_regs.REG_INPUT_STATE = 0;
	
	app_regs.REG_INPUT_STATE |= (read_DIN0) ? B_DI0 : 0;
	app_regs.REG_INPUT_STATE |= (read_DIN1) ? B_DI1 : 0;
	app_regs.REG_INPUT_STATE |= (read_DIN2) ? B_DI2 : 0;
}

bool app_write_REG_INPUT_STATE(void *a)
{
	return false;
}


/************************************************************************/
/* REG_CONFIGURE_DI0                                                    */
/************************************************************************/
void app_read_REG_CONFIGURE_DI0(void)
{
	//app_regs.REG_CONFIGURE_DI0 = 0;

}

bool app_write_REG_CONFIGURE_DI0(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DI_MODE)) return false;

	app_regs.REG_CONFIGURE_DI0 = reg;
	return true;
}


/************************************************************************/
/* REG_CONFIGURE_DI1                                                    */
/************************************************************************/
void app_read_REG_CONFIGURE_DI1(void)
{
	//app_regs.REG_CONFIGURE_DI1 = 0;

}

bool app_write_REG_CONFIGURE_DI1(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DI_MODE)) return false;

	app_regs.REG_CONFIGURE_DI1 = reg;
	return true;
}


/************************************************************************/
/* REG_START_ON_DI0                                                     */
/************************************************************************/
void app_read_REG_START_ON_DI0(void)
{
	//app_regs.REG_START_ON_DI0 = 0;

}

bool app_write_REG_START_ON_DI0(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg <= 1 || reg > 40000)
		return false;

	app_regs.REG_START_ON_DI0 = reg;
	return true;
}


/************************************************************************/
/* REG_START_ON_DI1                                                     */
/************************************************************************/
void app_read_REG_START_ON_DI1(void)
{
	//app_regs.REG_START_ON_DI1 = 0;

}

bool app_write_REG_START_ON_DI1(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg <= 1 || reg > 40000)
		return false;

	app_regs.REG_START_ON_DI1 = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_LEFT_DI0                                             */
/************************************************************************/
void app_read_REG_ATTENUATION_LEFT_DI0(void)
{
	//app_regs.REG_ATTENUATION_LEFT_DI0 = 0;

}

bool app_write_REG_ATTENUATION_LEFT_DI0(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_LEFT_DI0 = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_LEFT_DI1                                             */
/************************************************************************/
void app_read_REG_ATTENUATION_LEFT_DI1(void)
{
	//app_regs.REG_ATTENUATION_LEFT_DI1 = 0;

}

bool app_write_REG_ATTENUATION_LEFT_DI1(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_LEFT_DI1 = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_RIGHT_DI0                                            */
/************************************************************************/
void app_read_REG_ATTENUATION_RIGHT_DI0(void)
{
	//app_regs.REG_ATTENUATION_RIGHT_DI0 = 0;

}

bool app_write_REG_ATTENUATION_RIGHT_DI0(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_RIGHT_DI0 = reg;
	return true;
}


/************************************************************************/
/* REG_ATTENUATION_RIGHT_DI1                                            */
/************************************************************************/
void app_read_REG_ATTENUATION_RIGHT_DI1(void)
{
	//app_regs.REG_ATTENUATION_RIGHT_DI1 = 0;

}

bool app_write_REG_ATTENUATION_RIGHT_DI1(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_RIGHT_DI1 = reg;
	return true;
}


/************************************************************************/
/* REG_CONFIGURE_DO0                                                    */
/************************************************************************/
void app_read_REG_CONFIGURE_DO0(void)
{
	//app_regs.REG_CONFIGURE_DO0 = 0;

}

bool app_write_REG_CONFIGURE_DO0(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DO_MODE)) return false;

	app_regs.REG_CONFIGURE_DO0 = reg;
	return true;
}


/************************************************************************/
/* REG_CONFIGURE_DO1                                                    */
/************************************************************************/
void app_read_REG_CONFIGURE_DO1(void)
{
	//app_regs.REG_CONFIGURE_DO1 = 0;

}

bool app_write_REG_CONFIGURE_DO1(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DO_MODE)) return false;

	app_regs.REG_CONFIGURE_DO1 = reg;
	return true;
}


/************************************************************************/
/* REG_CONFIGURE_DO2                                                    */
/************************************************************************/
void app_read_REG_CONFIGURE_DO2(void)
{
	//app_regs.REG_CONFIGURE_DO2 = 0;

}

bool app_write_REG_CONFIGURE_DO2(void *a)
{
	uint8_t reg = *((uint8_t*)a);	
	
	if (reg & (~MSK_DO_MODE)) return false;

	app_regs.REG_CONFIGURE_DO2 = reg;
	return true;
}


/************************************************************************/
/* REG_OUTPUT_SET                                                       */
/************************************************************************/
void app_read_REG_OUTPUT_SET(void)
{
   app_regs.REG_OUTPUT_SET = 0;
}
bool app_write_REG_OUTPUT_SET(void *a)
{
	uint8_t reg = *((uint8_t*)a);
   
	if (reg & B_DO0) set_DOUT0;
	if (reg & B_DO1) set_DOUT1;
	if (reg & B_DO2) set_DOUT2;

	app_regs.REG_OUTPUT_SET = reg;
	return true;
}


/************************************************************************/
/* REG_OUTPUT_CLEAR                                                     */
/************************************************************************/
void app_read_REG_OUTPUT_CLEAR(void)
{
   app_regs.REG_OUTPUT_CLEAR = 0;
}
bool app_write_REG_OUTPUT_CLEAR(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_DO0) clr_DOUT0;
	if (reg & B_DO1) clr_DOUT1;
	if (reg & B_DO2) clr_DOUT2;

	app_regs.REG_OUTPUT_CLEAR = reg;
	return true;
}


/************************************************************************/
/* REG_OUTPUT_TOGGLE                                                    */
/************************************************************************/
void app_read_REG_OUTPUT_TOGGLE(void)
{
   app_regs.REG_OUTPUT_TOGGLE = 0;
}
bool app_write_REG_OUTPUT_TOGGLE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_DO0) tgl_DOUT0;
	if (reg & B_DO1) tgl_DOUT1;
	if (reg & B_DO2) tgl_DOUT2;

	app_regs.REG_OUTPUT_TOGGLE = reg;
	return true;
}


/************************************************************************/
/* REG_OUTPUT_STATE                                                     */
/************************************************************************/
void app_read_REG_OUTPUT_STATE(void)
{
	app_regs.REG_OUTPUT_STATE  = read_DOUT0 ? B_DO0 : 0;
	app_regs.REG_OUTPUT_STATE |= read_DOUT1 ? B_DO1 : 0;
	app_regs.REG_OUTPUT_STATE |= read_DOUT2 ? B_DO2 : 0;
}

bool app_write_REG_OUTPUT_STATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
   
	if (reg & B_DO0) set_DOUT0; else clr_DOUT0;
	if (reg & B_DO1) set_DOUT1; else clr_DOUT1;
	if (reg & B_DO2) set_DOUT2; else clr_DOUT2;

	app_regs.REG_OUTPUT_STATE = reg;
	return true;
}


/************************************************************************/
/* REG_ENABLE_ADC_CONTROL_STATE                                         */
/************************************************************************/
void app_read_REG_ENABLE_ADC_CONTROL_STATE(void)
{
	//app_regs.REG_ENABLE_ADC_CONTROL_STATE = 0;

}

bool app_write_REG_ENABLE_ADC_CONTROL_STATE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~0x01))
	{
		return false;
	}

	app_regs.REG_ENABLE_ADC_CONTROL_STATE = reg;
	return true;
}


/************************************************************************/
/* REG_ADC_CONTROL_STATE                                                */
/************************************************************************/
// This register is an array with 5 positions
void app_read_REG_ADC_CONTROL_STATE(void)
{
	//app_regs.REG_ADC_CONTROL_STATE[0] = 0;

}

bool app_write_REG_ADC_CONTROL_STATE(void *a)
{
	return false;
}


/************************************************************************/
/* REG_CONFIGURE_ADC0                                                   */
/************************************************************************/
void app_read_REG_CONFIGURE_ADC0(void)
{
	//app_regs.REG_CONFIGURE_ADC0 = 0;

}

bool app_write_REG_CONFIGURE_ADC0(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_ADC_MODE))
	{
		return false;
	}

	app_regs.REG_CONFIGURE_ADC0 = reg;
	return true;
}


/************************************************************************/
/* REG_CONFIGURE_ADC1                                                   */
/************************************************************************/
void app_read_REG_CONFIGURE_ADC1(void)
{
	//app_regs.REG_CONFIGURE_ADC1 = 0;

}

bool app_write_REG_CONFIGURE_ADC1(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_ADC_MODE))
	{
		return false;
	}

	app_regs.REG_CONFIGURE_ADC1 = reg;
	return true;
}


/************************************************************************/
/* REG_PIC32_BOOTLOADER                                                 */
/************************************************************************/
void app_read_REG_PIC32_BOOTLOADER(void) {}
bool app_write_REG_PIC32_BOOTLOADER(void *a)
{
	uint8_t reg = *((uint8_t*)a);

	if (reg & (~0x01))
		return false;

	if (reg)
		set_BOOTLOADER_EN;
	else
		clr_BOOTLOADER_EN;

	app_regs.REG_PIC32_BOOTLOADER = reg;
	return true;
}


/************************************************************************/
/* All REG_RESERVEDx                                                    */
/************************************************************************/
void 	app_read_REG_RESERVED2	(void) {}
void 	app_read_REG_RESERVED3	(void) {}
void 	app_read_REG_RESERVED4	(void) {}
void 	app_read_REG_RESERVED5	(void) {}
void 	app_read_REG_RESERVED6	(void) {}
void 	app_read_REG_RESERVED7	(void) {}
void 	app_read_REG_RESERVED8	(void) {}
void 	app_read_REG_RESERVED9	(void) {}
void 	app_read_REG_RESERVED10	(void) {}
void 	app_read_REG_RESERVED11	(void) {}
void 	app_read_REG_RESERVED12	(void) {}
void 	app_read_REG_RESERVED13	(void) {}
void 	app_read_REG_RESERVED14	(void) {}
void 	app_read_REG_RESERVED15	(void) {}
void 	app_read_REG_RESERVED16	(void) {}
void 	app_read_REG_RESERVED17	(void) {}
void 	app_read_REG_RESERVED18	(void) {}
void 	app_read_REG_RESERVED19	(void) {}
void 	app_read_REG_RESERVED20	(void) {}
void 	app_read_REG_RESERVED21	(void) {}
void 	app_read_REG_RESERVED22	(void) {}
void 	app_read_REG_RESERVED23	(void) {}

bool app_write_REG_RESERVED2	(void *a) { app_regs.REG_RESERVED2	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED3	(void *a) { app_regs.REG_RESERVED3	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED4	(void *a) { app_regs.REG_RESERVED4	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED5	(void *a) { app_regs.REG_RESERVED5	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED6	(void *a) { app_regs.REG_RESERVED6	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED7	(void *a) { app_regs.REG_RESERVED7	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED8	(void *a) { app_regs.REG_RESERVED8	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED9	(void *a) { app_regs.REG_RESERVED9	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED10	(void *a) { app_regs.REG_RESERVED10	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED11	(void *a) { app_regs.REG_RESERVED11	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED12	(void *a) { app_regs.REG_RESERVED12	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED13	(void *a) { app_regs.REG_RESERVED13	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED14	(void *a) { app_regs.REG_RESERVED14	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED15	(void *a) { app_regs.REG_RESERVED15	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED16	(void *a) { app_regs.REG_RESERVED16	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED17	(void *a) { app_regs.REG_RESERVED17	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED18	(void *a) { app_regs.REG_RESERVED18	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED19	(void *a) { app_regs.REG_RESERVED19	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED20	(void *a) { app_regs.REG_RESERVED20	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED21	(void *a) { app_regs.REG_RESERVED21	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED22	(void *a) { app_regs.REG_RESERVED22	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED23	(void *a) { app_regs.REG_RESERVED23	 = *((uint8_t*)a); return true; }
