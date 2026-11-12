#ifndef _APP_H_
#define _APP_H_
#include <avr/io.h>


/************************************************************************/
/* Define identifiers                                                   */
/************************************************************************/
#define DEVICE_NAME      "SoundCard"
#define WHO_AM_I         1280


/************************************************************************/
/* Define versions                                                      */
/************************************************************************/
#ifndef MAJOR_HW_VERSION
#define MAJOR_HW_VERSION 2
#endif
#ifndef MINOR_HW_VERSION
#define MINOR_HW_VERSION 2
#endif
#ifndef PATCH_HW_VERSION
#define PATCH_HW_VERSION 0
#endif
#ifndef MAJOR_FW_VERSION
#define MAJOR_FW_VERSION 3
#endif
#ifndef MINOR_FW_VERSION
#define MINOR_FW_VERSION 0
#endif
#ifndef PATCH_FW_VERSION
#define PATCH_FW_VERSION 0
#endif


/************************************************************************/
/* Enable the interrupts                                                */
/************************************************************************/
#define hwbp_app_enable_interrupts  PMIC_CTRL = PMIC_CTRL | PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm; __asm volatile("sei");


/************************************************************************/
/* Initialize the application                                           */
/************************************************************************/
void hwbp_app_initialize(void);


#endif /* _APP_H_ */
