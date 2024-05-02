#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_PLAY_SOUND_OR_FREQ(void);
void app_read_REG_STOP(void);
void app_read_REG_ATTNUATION_LEFT(void);
void app_read_REG_ATTENUATION_RIGHT(void);
void app_read_REG_ATTENUATION_BOTH(void);
void app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void);
void app_read_REG_RESERVED0(void);
void app_read_REG_RESERVED1(void);
void app_read_REG_DIGITAL_INPUTS(void);
void app_read_REG_DI0_CONF(void);
void app_read_REG_DI1_CONF(void);
void app_read_REG_DI0_SOUND_INDEX(void);
void app_read_REG_DI1_SOUND_INDEX(void);
void app_read_REG_DI0_FREQ(void);
void app_read_REG_DI1_FREQ(void);
void app_read_REG_DI0_ATTNUATION_LEFT(void);
void app_read_REG_DI1_ATTNUATION_LEFT(void);
void app_read_REG_DI0_ATTENUATION_RIGHT(void);
void app_read_REG_DI1_ATTENUATION_RIGHT(void);
void app_read_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void);
void app_read_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void);
void app_read_REG_DI0_ATTENUATION_AND_FREQUENCY(void);
void app_read_REG_DI1_ATTENUATION_AND_FREQUENCY(void);
void app_read_REG_RESERVED2(void);
void app_read_REG_RESERVED3(void);
void app_read_REG_RESERVED4(void);
void app_read_REG_DO0_CONF(void);
void app_read_REG_DO1_CONF(void);
void app_read_REG_DO2_CONF(void);
void app_read_REG_RESERVED5(void);
void app_read_REG_RESERVED6(void);
void app_read_REG_RESERVED7(void);
void app_read_REG_DO_SET(void);
void app_read_REG_DO_CLEAR(void);
void app_read_REG_DO_TOGGLE(void);
void app_read_REG_DO_OUT(void);
void app_read_REG_RESERVED8(void);
void app_read_REG_RESERVED9(void);
void app_read_REG_ADC_CONF(void);
void app_read_REG_ADC_VALUES(void);
void app_read_REG_COMMANDS(void);
void app_read_REG_RESERVED10(void);
void app_read_REG_RESERVED11(void);
void app_read_REG_RESERVED12(void);

bool app_write_REG_PLAY_SOUND_OR_FREQ(void *a);
bool app_write_REG_STOP(void *a);
bool app_write_REG_ATTNUATION_LEFT(void *a);
bool app_write_REG_ATTENUATION_RIGHT(void *a);
bool app_write_REG_ATTENUATION_BOTH(void *a);
bool app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ(void *a);
bool app_write_REG_RESERVED0(void *a);
bool app_write_REG_RESERVED1(void *a);
bool app_write_REG_DIGITAL_INPUTS(void *a);
bool app_write_REG_DI0_CONF(void *a);
bool app_write_REG_DI1_CONF(void *a);
bool app_write_REG_DI0_SOUND_INDEX(void *a);
bool app_write_REG_DI1_SOUND_INDEX(void *a);
bool app_write_REG_DI0_FREQ(void *a);
bool app_write_REG_DI1_FREQ(void *a);
bool app_write_REG_DI0_ATTNUATION_LEFT(void *a);
bool app_write_REG_DI1_ATTNUATION_LEFT(void *a);
bool app_write_REG_DI0_ATTENUATION_RIGHT(void *a);
bool app_write_REG_DI1_ATTENUATION_RIGHT(void *a);
bool app_write_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void *a);
bool app_write_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void *a);
bool app_write_REG_DI0_ATTENUATION_AND_FREQUENCY(void *a);
bool app_write_REG_DI1_ATTENUATION_AND_FREQUENCY(void *a);
bool app_write_REG_RESERVED2(void *a);
bool app_write_REG_RESERVED3(void *a);
bool app_write_REG_RESERVED4(void *a);
bool app_write_REG_DO0_CONF(void *a);
bool app_write_REG_DO1_CONF(void *a);
bool app_write_REG_DO2_CONF(void *a);
bool app_write_REG_RESERVED5(void *a);
bool app_write_REG_RESERVED6(void *a);
bool app_write_REG_RESERVED7(void *a);
bool app_write_REG_DO_SET(void *a);
bool app_write_REG_DO_CLEAR(void *a);
bool app_write_REG_DO_TOGGLE(void *a);
bool app_write_REG_DO_OUT(void *a);
bool app_write_REG_RESERVED8(void *a);
bool app_write_REG_RESERVED9(void *a);
bool app_write_REG_ADC_CONF(void *a);
bool app_write_REG_ADC_VALUES(void *a);
bool app_write_REG_COMMANDS(void *a);
bool app_write_REG_RESERVED10(void *a);
bool app_write_REG_RESERVED11(void *a);
bool app_write_REG_RESERVED12(void *a);


#endif /* _APP_FUNCTIONS_H_ */