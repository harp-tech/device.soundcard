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
void app_read_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX(void);
void app_read_REG_STOP(void);
void app_read_REG_ATTENUATION_BOTH(void);
void app_read_REG_PLAY_SOUND_INDEX(void);
void app_read_REG_ATTENUATION_RIGHT(void);
void app_read_REG_ATTNUATION_LEFT(void);
void app_read_REG_DIGITAL_INPUTS(void);
void app_read_REG_DI0_CONF(void);
void app_read_REG_DI1_CONF(void);
void app_read_REG_DI2_CONF(void);
void app_read_REG_DI0_SOUND_INDEX(void);
void app_read_REG_DI1_SOUND_INDEX(void);
void app_read_REG_DI2_SOUND_INDEX(void);
void app_read_REG_DI0_ATTENUATION_RIGHT(void);
void app_read_REG_DI1_ATTENUATION_RIGHT(void);
void app_read_REG_DI2_ATTENUATION_RIGHT(void);
void app_read_REG_DI0_ATTNUATION_LEFT(void);
void app_read_REG_DI1_ATTNUATION_LEFT(void);
void app_read_REG_DI2_ATTNUATION_LEFT(void);
void app_read_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void);
void app_read_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void);
void app_read_REG_DI2_ATTENUATION_AND_SOUND_INDEX(void);
void app_read_REG_DO0_CONF(void);
void app_read_REG_DO1_CONF(void);
void app_read_REG_DO2_CONF(void);
void app_read_REG_DO0_PULSE(void);
void app_read_REG_DO1_PULSE(void);
void app_read_REG_DO2_PULSE(void);
void app_read_REG_DO_SET(void);
void app_read_REG_DO_CLEAR(void);
void app_read_REG_DO_TOGGLE(void);
void app_read_REG_DO_OUT(void);
void app_read_REG_ADC0_CONF(void);
void app_read_REG_BOOTLOADER(void);
void app_read_REG_EVNT_ENABLE(void);

bool app_write_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX(void *a);
bool app_write_REG_STOP(void *a);
bool app_write_REG_ATTENUATION_BOTH(void *a);
bool app_write_REG_PLAY_SOUND_INDEX(void *a);
bool app_write_REG_ATTENUATION_RIGHT(void *a);
bool app_write_REG_ATTNUATION_LEFT(void *a);
bool app_write_REG_DIGITAL_INPUTS(void *a);
bool app_write_REG_DI0_CONF(void *a);
bool app_write_REG_DI1_CONF(void *a);
bool app_write_REG_DI2_CONF(void *a);
bool app_write_REG_DI0_SOUND_INDEX(void *a);
bool app_write_REG_DI1_SOUND_INDEX(void *a);
bool app_write_REG_DI2_SOUND_INDEX(void *a);
bool app_write_REG_DI0_ATTENUATION_RIGHT(void *a);
bool app_write_REG_DI1_ATTENUATION_RIGHT(void *a);
bool app_write_REG_DI2_ATTENUATION_RIGHT(void *a);
bool app_write_REG_DI0_ATTNUATION_LEFT(void *a);
bool app_write_REG_DI1_ATTNUATION_LEFT(void *a);
bool app_write_REG_DI2_ATTNUATION_LEFT(void *a);
bool app_write_REG_DI0_ATTENUATION_AND_SOUND_INDEX(void *a);
bool app_write_REG_DI1_ATTENUATION_AND_SOUND_INDEX(void *a);
bool app_write_REG_DI2_ATTENUATION_AND_SOUND_INDEX(void *a);
bool app_write_REG_DO0_CONF(void *a);
bool app_write_REG_DO1_CONF(void *a);
bool app_write_REG_DO2_CONF(void *a);
bool app_write_REG_DO0_PULSE(void *a);
bool app_write_REG_DO1_PULSE(void *a);
bool app_write_REG_DO2_PULSE(void *a);
bool app_write_REG_DO_SET(void *a);
bool app_write_REG_DO_CLEAR(void *a);
bool app_write_REG_DO_TOGGLE(void *a);
bool app_write_REG_DO_OUT(void *a);
bool app_write_REG_ADC0_CONF(void *a);
bool app_write_REG_BOOTLOADER(void *a);
bool app_write_REG_EVNT_ENABLE(void *a);


#endif /* _APP_FUNCTIONS_H_ */