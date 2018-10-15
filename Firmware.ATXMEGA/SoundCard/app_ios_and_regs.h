#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// DIN0                   Description: Digital input 0
// DIN1                   Description: Digital input 1
// DIN2                   Description: Digital input 2
// PIC32_READY            Description: PIC32 has started (left bootloader or just boot)
// CMD_LATCHED            Description: Command read from the PAR bus
// SOUND_IS_ON            Description: Command received had an error

#define read_DIN0 read_io(PORTB, 0)             // DIN0
#define read_DIN1 read_io(PORTD, 0)             // DIN1
#define read_DIN2 read_io(PORTC, 0)             // DIN2
#define read_PIC32_READY read_io(PORTD, 5)      // PIC32_READY
#define read_CMD_LATCHED read_io(PORTC, 5)      // CMD_LATCHED
#define read_SOUND_IS_ON read_io(PORTC, 6)      // SOUND_IS_ON

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// PAR0                   Description: Digital bus pin 0
// PAR1                   Description: Digital bus pin 1
// PAR2                   Description: Digital bus pin 2
// PAR3                   Description: Digital bus pin 3
// PAR4                   Description: Digital bus pin 4
// PAR5                   Description: Digital bus pin 5
// PAR6                   Description: Digital bus pin 6
// PAR7                   Description: Digital bus pin 7
// DOUT0                  Description: Digital output 0
// DOUT1                  Description: Digital output 1
// DOUT2                  Description: Digital output 2
// BOOTLOADER_EN          Description: Enables bootloader on PIC32 and buffers
// CMD_WRITE              Description: Command available on the PAR bus

/* PAR0 */
#define set_PAR0 set_io(PORTA, 0)
#define clr_PAR0 clear_io(PORTA, 0)
#define tgl_PAR0 toggle_io(PORTA, 0)
#define read_PAR0 read_io(PORTA, 0)

/* PAR1 */
#define set_PAR1 set_io(PORTA, 1)
#define clr_PAR1 clear_io(PORTA, 1)
#define tgl_PAR1 toggle_io(PORTA, 1)
#define read_PAR1 read_io(PORTA, 1)

/* PAR2 */
#define set_PAR2 set_io(PORTA, 2)
#define clr_PAR2 clear_io(PORTA, 2)
#define tgl_PAR2 toggle_io(PORTA, 2)
#define read_PAR2 read_io(PORTA, 2)

/* PAR3 */
#define set_PAR3 set_io(PORTA, 3)
#define clr_PAR3 clear_io(PORTA, 3)
#define tgl_PAR3 toggle_io(PORTA, 3)
#define read_PAR3 read_io(PORTA, 3)

/* PAR4 */
#define set_PAR4 set_io(PORTA, 4)
#define clr_PAR4 clear_io(PORTA, 4)
#define tgl_PAR4 toggle_io(PORTA, 4)
#define read_PAR4 read_io(PORTA, 4)

/* PAR5 */
#define set_PAR5 set_io(PORTA, 5)
#define clr_PAR5 clear_io(PORTA, 5)
#define tgl_PAR5 toggle_io(PORTA, 5)
#define read_PAR5 read_io(PORTA, 5)

/* PAR6 */
#define set_PAR6 set_io(PORTA, 6)
#define clr_PAR6 clear_io(PORTA, 6)
#define tgl_PAR6 toggle_io(PORTA, 6)
#define read_PAR6 read_io(PORTA, 6)

/* PAR7 */
#define set_PAR7 set_io(PORTA, 7)
#define clr_PAR7 clear_io(PORTA, 7)
#define tgl_PAR7 toggle_io(PORTA, 7)
#define read_PAR7 read_io(PORTA, 7)

/* DOUT0 */
#define set_DOUT0 set_io(PORTD, 1)
#define clr_DOUT0 clear_io(PORTD, 1)
#define tgl_DOUT0 toggle_io(PORTD, 1)
#define read_DOUT0 read_io(PORTD, 1)

/* DOUT1 */
#define set_DOUT1 set_io(PORTC, 3)
#define clr_DOUT1 clear_io(PORTC, 3)
#define tgl_DOUT1 toggle_io(PORTC, 3)
#define read_DOUT1 read_io(PORTC, 3)

/* DOUT2 */
#define set_DOUT2 set_io(PORTC, 4)
#define clr_DOUT2 clear_io(PORTC, 4)
#define tgl_DOUT2 toggle_io(PORTC, 4)
#define read_DOUT2 read_io(PORTC, 4)

/* BOOTLOADER_EN */
#define set_BOOTLOADER_EN set_io(PORTC, 7)
#define clr_BOOTLOADER_EN clear_io(PORTC, 7)
#define tgl_BOOTLOADER_EN toggle_io(PORTC, 7)
#define read_BOOTLOADER_EN read_io(PORTC, 7)

/* CMD_WRITE */
#define set_CMD_WRITE set_io(PORTC, 1)
#define clr_CMD_WRITE clear_io(PORTC, 1)
#define tgl_CMD_WRITE toggle_io(PORTC, 1)
#define read_CMD_WRITE read_io(PORTC, 1)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint16_t REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX[3];
	uint8_t REG_STOP;
	uint16_t REG_ATTENUATION_BOTH[2];
	uint8_t REG_PLAY_SOUND_INDEX;
	uint16_t REG_ATTENUATION_RIGHT;
	uint16_t REG_ATTNUATION_LEFT;
	uint8_t REG_DIGITAL_INPUTS;
	uint8_t REG_DI0_CONF;
	uint8_t REG_DI1_CONF;
	uint8_t REG_DI2_CONF;
	uint8_t REG_DI0_SOUND_INDEX;
	uint8_t REG_DI1_SOUND_INDEX;
	uint8_t REG_DI2_SOUND_INDEX;
	uint16_t REG_DI0_ATTENUATION_RIGHT;
	uint16_t REG_DI1_ATTENUATION_RIGHT;
	uint16_t REG_DI2_ATTENUATION_RIGHT;
	uint16_t REG_DI0_ATTNUATION_LEFT;
	uint16_t REG_DI1_ATTNUATION_LEFT;
	uint16_t REG_DI2_ATTNUATION_LEFT;
	uint16_t REG_DI0_ATTENUATION_AND_SOUND_INDEX[3];
	uint16_t REG_DI1_ATTENUATION_AND_SOUND_INDEX[3];
	uint16_t REG_DI2_ATTENUATION_AND_SOUND_INDEX[3];
	uint8_t REG_DO0_CONF;
	uint8_t REG_DO1_CONF;
	uint8_t REG_DO2_CONF;
	uint8_t REG_DO0_PULSE;
	uint8_t REG_DO1_PULSE;
	uint8_t REG_DO2_PULSE;
	uint8_t REG_DO_SET;
	uint8_t REG_DO_CLEAR;
	uint8_t REG_DO_TOGGLE;
	uint8_t REG_DO_OUT;
	uint8_t REG_ADC0_CONF;
	uint8_t REG_BOOTLOADER;
	uint8_t REG_EVNT_ENABLE;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_SET_ATTENUATION_AND_PLAY_SOUND_INDEX 32 // U16    Configures attenuation and plays sound index [Att R] [Att L] [Index]
#define ADD_REG_STOP                        33 // U8     Any value will stops the current sound
#define ADD_REG_ATTENUATION_BOTH            34 // U16    Configures both attenuation on right and left channels [Att R] [Att L]
#define ADD_REG_PLAY_SOUND_INDEX            35 // U8     Starts the sound index
#define ADD_REG_ATTENUATION_RIGHT           36 // U16    Configure right channel's attenuation (1 LSB is 0.1dB)
#define ADD_REG_ATTNUATION_LEFT             37 // U16    Configure left channel's attenuation (1 LSB is 0.1dB)
#define ADD_REG_DIGITAL_INPUTS              38 // U8     State of the digital inputs
#define ADD_REG_DI0_CONF                    39 // U8     Configuration of the digital input 0 (DI0)
#define ADD_REG_DI1_CONF                    40 // U8     Configuration of the digital input 1 (DI1)
#define ADD_REG_DI2_CONF                    41 // U8     Configuration of the digital input 2 (DI2)
#define ADD_REG_DI0_SOUND_INDEX             42 // U8     Sound index to be played when triggering DI0
#define ADD_REG_DI1_SOUND_INDEX             43 // U8     Sound index to be played when triggering DI1
#define ADD_REG_DI2_SOUND_INDEX             44 // U8     Sound index to be played when triggering DI2
#define ADD_REG_DI0_ATTENUATION_RIGHT       45 // U16    Right channel's attenuation (1 LSB is 0.1dB) when triggering DI0
#define ADD_REG_DI1_ATTENUATION_RIGHT       46 // U16    Right channel's attenuation (1 LSB is 0.1dB) when triggering DI1
#define ADD_REG_DI2_ATTENUATION_RIGHT       47 // U16    Right channel's attenuation (1 LSB is 0.1dB) when triggering DI2
#define ADD_REG_DI0_ATTNUATION_LEFT         48 // U16    Left channel's attenuation (1 LSB is 0.1dB) when triggering DI0
#define ADD_REG_DI1_ATTNUATION_LEFT         49 // U16    Left channel's attenuation (1 LSB is 0.1dB) when triggering DI1
#define ADD_REG_DI2_ATTNUATION_LEFT         50 // U16    Left channel's attenuation (1 LSB is 0.1dB) when triggering DI2
#define ADD_REG_DI0_ATTENUATION_AND_SOUND_INDEX 51 // U16    Sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index]
#define ADD_REG_DI1_ATTENUATION_AND_SOUND_INDEX 52 // U16    Sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index]
#define ADD_REG_DI2_ATTENUATION_AND_SOUND_INDEX 53 // U16    Sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index]
#define ADD_REG_DO0_CONF                    54 // U8     Configuration of the digital output 0 (DO0)
#define ADD_REG_DO1_CONF                    55 // U8     Configuration of the digital output 1 (DO1)
#define ADD_REG_DO2_CONF                    56 // U8     Configuration of the digital output 1 (DO1)
#define ADD_REG_DO0_PULSE                   57 // U8     Pulse for the digital output 0 (DO0) [1:255]
#define ADD_REG_DO1_PULSE                   58 // U8     Pulse for the digital output 1 (DO1) [1:255]
#define ADD_REG_DO2_PULSE                   59 // U8     Pulse for the digital output 2 (DO2) [1:255]
#define ADD_REG_DO_SET                      60 // U8     Set the digital outputs
#define ADD_REG_DO_CLEAR                    61 // U8     Clear the digital outputs
#define ADD_REG_DO_TOGGLE                   62 // U8     Toggle the digital outputs
#define ADD_REG_DO_OUT                      63 // U8     Writes to the digital output
#define ADD_REG_ADC0_CONF                   64 // U8     Configuration of Analog Input 0
#define ADD_REG_BOOTLOADER                  65 // U8     Enable board's buffers to send a new image to the PIC32
#define ADD_REG_EVNT_ENABLE                 66 // U8     Enable the Events

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x42
#define APP_NBYTES_OF_REG_BANK              66

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_DI0                              (1<<0)       // Digital input 0
#define B_DI1                              (1<<1)       // Digital input 1
#define B_DI2                              (1<<2)       // Digital input 2
#define MSK_DI_SEL                         (7<<0)       // 
#define GM_DI_SYNC                         (0<<0)       // Used as a pure digital input
#define GM_DI_RISE_START_FALL_STOP         (1<<0)       // Starts sound when rising edge and stop when falling edge
#define GM_DI_RISE_START                   (2<<0)       // Starts sound when rising edge
#define GM_DI_RISE_STOP                    (3<<0)       // Starts sound when falling edge
#define GM_DI_FALL_START                   (4<<0)       // Stops sound when rising edge
#define GM_DI_FALL_STOP                    (5<<0)       // Stops sound when falling edge
#define MSK_DO_SEL                         (15<<0)      // 
#define GM_DO_DIG                          (0<<0)       // Used as a pure digital output
#define GM_DO_HIGH_WHEN_SOUND              (1<<0)       // High when the sound is being played
#define GM_DO_PULSE_1MS_WHEN_START         (2<<0)       // High when sound starts during 1 ms
#define GM_DO_PULSE_10MS_WHEN_START        (3<<0)       // High when sound starts during 1 0ms
#define GM_DO_PULSE_100MS_WHEN_START       (4<<0)       // High when sound starts during 100 ms
#define GM_DO_PULSE_1MS_WHEN_STOP          (5<<0)       // High when sound stops during 1 ms
#define GM_DO_PULSE_10MS_WHEN_STOP         (6<<0)       // High when sound stops during 10 ms
#define GM_DO_PULSE_100MS_WHEN_STOP        (7<<0)       // High when sound stops during 100 ms
#define GM_DO_PULSE                        (8<<0)       // The digital output will be high during a period specified by register DOx_PULSE
#define B_DO0                              (1<<0)       // Digital output 0
#define B_DO1                              (1<<1)       // Digital output 1
#define B_DO2                              (1<<2)       // Digital output 2
#define MSK_ADC_SEL                        (3<<0)       // 
#define GM_ADC_ANALOG_500                  (0<<0)       // Used as a pure analog input acquired at 500 Hz
#define GM_ADC_ANALOG_1000                 (1<<0)       // Used as a pure analog input acquired at 1000 Hz
#define GM_ADC_ATTENUATION_R_L_CH          (2<<0)       // Used as attenuation control of channel (ADC0 -> Right) (ADC1 -> Left)
#define GM_ADC_ATTENUATION_BOTH_CHS        (3<<0)       // Used as attenuation control of both channels (ADC0 controls both)
#define B_EN_BOOT                          (1<<0)       // Enables buffers if equal to ONE
#define B_EVT_REG32                        (1<<0)       // Event of register SET_ATTENUATION_AND_PLAY_SOUND_INDEX
#define B_EVT_REG33                        (1<<1)       // Event of register STOP
#define B_EVT_REG34                        (1<<2)       // Event of register ATTENUATION_BOTH
#define B_EVT_REG38                        (1<<3)       // Event of register DIGITAL_INPUTS

#endif /* _APP_REGS_H_ */