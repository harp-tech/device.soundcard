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
	&app_read_REG_DI0_SOUND_INDEX,
	&app_read_REG_DI1_SOUND_INDEX,
	&app_read_REG_DI0_ATTNUATION_LEFT,
	&app_read_REG_DI1_ATTNUATION_LEFT,
	&app_read_REG_DI0_ATTENUATION_RIGHT,
	&app_read_REG_DI1_ATTENUATION_RIGHT,
	&app_read_REG_RESERVED2,
	&app_read_REG_RESERVED3,
	&app_read_REG_RESERVED4,
	&app_read_REG_DO0_CONF,
	&app_read_REG_DO1_CONF,
	&app_read_REG_DO2_CONF,
	&app_read_REG_RESERVED5,
	&app_read_REG_RESERVED6,
	&app_read_REG_RESERVED7,
	&app_read_REG_DO_SET,
	&app_read_REG_DO_CLEAR,
	&app_read_REG_DO_TOGGLE,
	&app_read_REG_DO_OUT,
	&app_read_REG_RESERVED8,
	&app_read_REG_RESERVED9,
	&app_read_REG_DATA_STREAM_CONF,
	&app_read_REG_DATA_STREAM,
	&app_read_REG_ADC0_CONF,
	&app_read_REG_ADC1_CONF,
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
	&app_read_REG_COMMANDS
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
	&app_write_REG_DI0_SOUND_INDEX,
	&app_write_REG_DI1_SOUND_INDEX,
	&app_write_REG_DI0_ATTNUATION_LEFT,
	&app_write_REG_DI1_ATTNUATION_LEFT,
	&app_write_REG_DI0_ATTENUATION_RIGHT,
	&app_write_REG_DI1_ATTENUATION_RIGHT,
	&app_write_REG_RESERVED2,
	&app_write_REG_RESERVED3,
	&app_write_REG_RESERVED4,
	&app_write_REG_DO0_CONF,
	&app_write_REG_DO1_CONF,
	&app_write_REG_DO2_CONF,
	&app_write_REG_RESERVED5,
	&app_write_REG_RESERVED6,
	&app_write_REG_RESERVED7,
	&app_write_REG_DO_SET,
	&app_write_REG_DO_CLEAR,
	&app_write_REG_DO_TOGGLE,
	&app_write_REG_DO_OUT,
	&app_write_REG_RESERVED8,
	&app_write_REG_RESERVED9,
	&app_write_REG_DATA_STREAM_CONF,
	&app_write_REG_DATA_STREAM,
	&app_write_REG_ADC0_CONF,
	&app_write_REG_ADC1_CONF,
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
	&app_write_REG_COMMANDS
};


/************************************************************************/
/* REG_PLAY_SOUND_OR_FREQ                                               */
/************************************************************************/
uint16_t last_sound_triggered = 0;	// 0 means it's stopped

void app_read_REG_PLAY_SOUND_OR_FREQ(void) {}
bool app_write_REG_PLAY_SOUND_OR_FREQ(void *a)
{
	uint16_t reg = *((uint16_t*)a);
   
   if (reg <= 1 || reg > 40000)
      return false;

   if (last_sound_triggered != 0)
      /* Previous event was not sent yet */ 
      return false;

	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[0] = reg;
	
	app_regs.REG_PLAY_SOUND_OR_FREQ = reg;
		
	/* Save the sound being played */
	last_sound_triggered = reg;
	
	//par_cmd_start_sound(reg, app_regs.REG_ATTNUATION_LEFT, app_regs.REG_ATTENUATION_RIGHT);
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
/* REG_ATTNUATION_LEFT                                                  */
/************************************************************************/
void app_read_REG_ATTNUATION_LEFT(void) {}
bool app_write_REG_ATTNUATION_LEFT(void *a)
{
	uint16_t reg = *((uint16_t*)a);

	app_regs.REG_ATTENUATION_BOTH[0] = reg;
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[1] = reg;

	app_regs.REG_ATTNUATION_LEFT = reg;
	
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
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[2] = reg;

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
		
	app_regs.REG_ATTNUATION_LEFT    = reg[0];
	app_regs.REG_ATTENUATION_RIGHT  = reg[1];
	
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[1] = reg[0];
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[2] = reg[1];
	
	app_regs.REG_ATTENUATION_BOTH[0] = reg[0];
	app_regs.REG_ATTENUATION_BOTH[1] = reg[1];
	
	par_cmd_update_amplitude(reg[0], reg[1]);
	
	return true;
}


/************************************************************************/
/* REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ                           */
/************************************************************************/
void app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void) {}
bool app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void *a)
{
	uint16_t *reg = ((uint16_t*)a);
	
	if (reg[0] <= 1 || reg[0] > 40000)
		return false;

	if (last_sound_triggered != 0)
      /* Previous event was not sent yet */ 
      return false;
	
	app_regs.REG_PLAY_SOUND_OR_FREQ = reg[0];
	app_regs.REG_ATTNUATION_LEFT    = reg[1];
	app_regs.REG_ATTENUATION_RIGHT  = reg[2];
	app_regs.REG_ATTENUATION_BOTH[0] = reg[1];
	app_regs.REG_ATTENUATION_BOTH[1] = reg[2];
	
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[0] = reg[0];
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[1] = reg[1];
	app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[2] = reg[2];
	
	/* Save the sound being played */
	last_sound_triggered = reg[0];
	
	par_cmd_start_sound(reg[0], reg[1], reg[2]);
	
	return true;
}


/************************************************************************/
/* REG_DIGITAL_INPUTS                                                   */
/************************************************************************/
void app_read_REG_DIGITAL_INPUTS(void)
{
	app_regs.REG_DIGITAL_INPUTS = 0;
	
	app_regs.REG_DIGITAL_INPUTS |= (read_DIN0) ? B_DI0 : 0;
	app_regs.REG_DIGITAL_INPUTS |= (read_DIN1) ? B_DI1 : 0;
	app_regs.REG_DIGITAL_INPUTS |= (read_DIN2) ? B_DI2 : 0;
}

bool app_write_REG_DIGITAL_INPUTS(void *a)
{
	return false;
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
	
	if (reg & (~MSK_DI_SEL)) return false;

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
	
	if (reg & (~MSK_DI_SEL)) return false;

	app_regs.REG_DI1_CONF = reg;
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
	uint16_t reg = *((uint16_t*)a);
	
	if (reg <= 1 || reg > 40000)
		return false;

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
	uint16_t reg = *((uint16_t*)a);
	
	if (reg <= 1 || reg > 40000)
		return false;

	app_regs.REG_DI1_SOUND_INDEX = reg;
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
/* REG_DO0_CONF                                                         */
/************************************************************************/
void app_read_REG_DO0_CONF(void)
{
	//app_regs.REG_DO0_CONF = 0;

}

bool app_write_REG_DO0_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DO_SEL)) return false;

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
	
	if (reg & (~MSK_DO_SEL)) return false;

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
	
	if (reg & (~MSK_DO_SEL)) return false;

	app_regs.REG_DO2_CONF = reg;
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
   if (reg & B_DO2) set_DOUT2;

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
/* REG_DATA_STREAM_CONF                                                 */
/************************************************************************/
void app_read_REG_DATA_STREAM_CONF(void)
{
	//app_regs.REG_DATA_STREAM_CONF = 0;

}

bool app_write_REG_DATA_STREAM_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_DATA_STREAM_SEL))
	{
		return false;
	}

	app_regs.REG_DATA_STREAM_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_DATA_STREAM                                                      */
/************************************************************************/
// This register is an array with 5 positions
void app_read_REG_DATA_STREAM(void)
{
	//app_regs.REG_DATA_STREAM[0] = 0;

}

bool app_write_REG_DATA_STREAM(void *a)
{
	return false;
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
	
	if (reg & (~MSK_ADC0_SEL))
	{
		return false;
	}

	app_regs.REG_ADC0_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_ADC1_CONF                                                        */
/************************************************************************/
void app_read_REG_ADC1_CONF(void)
{
	//app_regs.REG_ADC1_CONF = 0;

}

bool app_write_REG_ADC1_CONF(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & (~MSK_ADC1_SEL))
	{
		return false;
	}

	app_regs.REG_ADC1_CONF = reg;
	return true;
}


/************************************************************************/
/* REG_COMMANDS                                                         */
/************************************************************************/
void app_read_REG_COMMANDS(void) {}
bool app_write_REG_COMMANDS(void *a)
{
   uint8_t reg = *((uint8_t*)a);   
   
   if (reg == GM_DIS_BOOTLOADER)
      clr_BOOTLOADER_EN;
      
   if (reg == GM_EN_BOOTLOADER)
      set_BOOTLOADER_EN;
   
   if (reg == GM_DEL_ALL_SOUNDS)
       par_cmd_delete_sound(0, true);
   
   if (reg >= 0+2+2 && reg <= 32+2)
       par_cmd_delete_sound(reg-2, false);   
      
   return true;
}


/************************************************************************/
/* All REG_RESERVEDx                                                    */
/************************************************************************/
void 	app_read_REG_RESERVED0	(void) {}
void 	app_read_REG_RESERVED1	(void) {}
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

bool app_write_REG_RESERVED0	(void *a) { app_regs.REG_RESERVED0	 = *((uint8_t*)a); return true; }
bool app_write_REG_RESERVED1	(void *a) { app_regs.REG_RESERVED1	 = *((uint8_t*)a); return true; }
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
