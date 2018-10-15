#ifndef IOS_H
#define	IOS_H

#include <xc.h>

// DOUT0 @ RG6 as output
#define cfg_SOUND_IS_ON TRISGCLR = (1 << 6)
#define set_SOUND_IS_ON  LATGSET = (1 << 6)
#define clr_SOUND_IS_ON  LATGCLR = (1 << 6)
#define tgl_SOUND_IS_ON  LATGINV = (1 << 6)

// LED_USB @ RF0 as output
#define cfg_LED_USB TRISFCLR = (1 << 0)
#define set_LED_USB  LATFSET = (1 << 0)
#define clr_LED_USB  LATFCLR = (1 << 0)
#define tgl_LED_USB  LATFINV = (1 << 0)

// LED_AUDIO @ RF1 as output
#define cfg_LED_AUDIO TRISFCLR = (1 << 1)
#define set_LED_AUDIO  LATFSET = (1 << 1)
#define clr_LED_AUDIO  LATFCLR = (1 << 1)
#define tgl_LED_AUDIO  LATFINV = (1 << 1)

// TP0 @ RG12 as output
#define cfg_TP0 TRISGCLR = (1 << 12)
#define set_TP0  LATGSET = (1 << 12)
#define clr_TP0  LATGCLR = (1 << 12)
#define tgl_TP0  LATGINV = (1 << 12)
#define read_TP0 (PORTG & (1 << 12))

// TP1 @ RG13 as output
#define cfg_TP1 TRISGCLR = (1 << 13)
#define set_TP1  LATGSET = (1 << 13)
#define clr_TP1  LATGCLR = (1 << 13)
#define tgl_TP1  LATGINV = (1 << 13)
#define read_TP1 (PORTG & (1 << 13))


// LED_MEMORY @ RG14 as output
#define cfg_LED_MEMORY TRISGCLR = (1 << 14)
#define set_LED_MEMORY  LATGSET = (1 << 14)
#define clr_LED_MEMORY  LATGCLR = (1 << 14)
#define tgl_LED_MEMORY  LATGINV = (1 << 14)

// EN_P12V @ RD1 as output
#define cfg_EN_P12V TRISDCLR = (1 << 1)
#define set_EN_P12V  LATDSET = (1 << 1)
#define clr_EN_P12V  LATDCLR = (1 << 1)
#define tgl_EN_P12V  LATDINV = (1 << 1)

// EN_N12V @ RD2 as output
#define cfg_EN_N12V TRISDCLR = (1 << 2)
#define set_EN_N12V  LATDSET = (1 << 2)
#define clr_EN_N12V  LATDCLR = (1 << 2)
#define tgl_EN_N12V  LATDINV = (1 << 2)

void initialize_ios(int reset_reason_type);

#endif	/* IOS_H */


