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
// CMD_LATCHED            Description: Command read from the PAR bus
// CMD_NOT_EXEC           Description: Command on the PARbus can't be executed
// SOUND_IS_ON            Description: Line is high when sound is being played

#define read_DIN0 read_io(PORTB, 0)             // DIN0
#define read_DIN1 read_io(PORTD, 0)             // DIN1
#define read_DIN2 read_io(PORTC, 0)             // DIN2
#define read_CMD_LATCHED read_io(PORTC, 5)      // CMD_LATCHED
#define read_CMD_NOT_EXEC read_io(PORTD, 5)     // CMD_NOT_EXEC
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
	uint16_t REG_PLAY_SOUND_OR_FREQ;
	uint8_t REG_STOP;
	uint16_t REG_ATTNUATION_LEFT;
	uint16_t REG_ATTENUATION_RIGHT;
	uint16_t REG_ATTENUATION_BOTH[2];
	uint16_t REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ[3];
	uint8_t REG_RESERVED0;
	uint8_t REG_RESERVED1;
	uint8_t REG_DIGITAL_INPUTS;
	uint8_t REG_DI0_CONF;
	uint8_t REG_DI1_CONF;
	uint8_t REG_DI0_SOUND_INDEX;
	uint8_t REG_DI1_SOUND_INDEX;
	uint16_t REG_DI0_ATTNUATION_LEFT;
	uint16_t REG_DI1_ATTNUATION_LEFT;
	uint16_t REG_DI0_ATTENUATION_RIGHT;
	uint16_t REG_DI1_ATTENUATION_RIGHT;
	uint8_t REG_RESERVED2;
	uint8_t REG_RESERVED3;
	uint8_t REG_RESERVED4;
	uint8_t REG_DO0_CONF;
	uint8_t REG_DO1_CONF;
	uint8_t REG_DO2_CONF;
	uint8_t REG_RESERVED5;
	uint8_t REG_RESERVED6;
	uint8_t REG_RESERVED7;
	uint8_t REG_DO_SET;
	uint8_t REG_DO_CLEAR;
	uint8_t REG_DO_TOGGLE;
	uint8_t REG_DO_OUT;
	uint8_t REG_RESERVED8;
	uint8_t REG_RESERVED9;
	uint8_t REG_DATA_STREAM_CONF;
	uint16_t REG_DATA_STREAM[5];
	uint8_t REG_ADC0_CONF;
	uint8_t REG_ADC1_CONF;
	uint8_t REG_RESERVED10;
	uint8_t REG_RESERVED11;
	uint8_t REG_RESERVED12;
	uint8_t REG_RESERVED13;
	uint8_t REG_RESERVED14;
	uint8_t REG_RESERVED15;
	uint8_t REG_RESERVED16;
	uint8_t REG_RESERVED17;
	uint8_t REG_RESERVED18;
	uint8_t REG_RESERVED19;
	uint8_t REG_RESERVED20;
	uint8_t REG_RESERVED21;
	uint8_t REG_RESERVED22;
	uint8_t REG_RESERVED23;
	uint8_t REG_COMMANDS;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_PLAY_SOUND_OR_FREQ          32 // U16    Starts the sound index (if < 32) or frequency (if >= 32)
#define ADD_REG_STOP                        33 // U8     Any value will stops the current sound
#define ADD_REG_ATTNUATION_LEFT             34 // U16    Configure left channel's attenuation (1 LSB is 0.1dB)
#define ADD_REG_ATTENUATION_RIGHT           35 // U16    Configure right channel's attenuation (1 LSB is 0.1dB)
#define ADD_REG_ATTENUATION_BOTH            36 // U16    Configures both attenuation on right and left channels [Att R] [Att L]
#define ADD_REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ 37 // U16    Configures attenuation and plays sound index [Att R] [Att L] [Index]
#define ADD_REG_RESERVED0                   38 // U8     Reserved for future purposes
#define ADD_REG_RESERVED1                   39 // U8     Reserved for future purposes
#define ADD_REG_DIGITAL_INPUTS              40 // U8     State of the digital inputs
#define ADD_REG_DI0_CONF                    41 // U8     Configuration of the digital input 0 (DI0)
#define ADD_REG_DI1_CONF                    42 // U8     Configuration of the digital input 1 (DI1)
#define ADD_REG_DI0_SOUND_INDEX             43 // U8     Sound index to be played when triggering DI0
#define ADD_REG_DI1_SOUND_INDEX             44 // U8     Sound index to be played when triggering DI1
#define ADD_REG_DI0_ATTNUATION_LEFT         45 // U16    Left channel's attenuation (1 LSB is 0.1dB) when triggering DI0
#define ADD_REG_DI1_ATTNUATION_LEFT         46 // U16    Left channel's attenuation (1 LSB is 0.1dB) when triggering DI1
#define ADD_REG_DI0_ATTENUATION_RIGHT       47 // U16    Right channel's attenuation (1 LSB is 0.1dB) when triggering DI0
#define ADD_REG_DI1_ATTENUATION_RIGHT       48 // U16    Right channel's attenuation (1 LSB is 0.1dB) when triggering DI1
#define ADD_REG_RESERVED2                   49 // U8     Reserved for future purposes
#define ADD_REG_RESERVED3                   50 // U8     Reserved for future purposes
#define ADD_REG_RESERVED4                   51 // U8     Reserved for future purposes
#define ADD_REG_DO0_CONF                    52 // U8     Configuration of the digital output 0 (DO0)
#define ADD_REG_DO1_CONF                    53 // U8     Configuration of the digital output 1 (DO1)
#define ADD_REG_DO2_CONF                    54 // U8     Configuration of the digital output 1 (DO1)
#define ADD_REG_RESERVED5                   55 // U8     Reserved for future purposes
#define ADD_REG_RESERVED6                   56 // U8     Reserved for future purposes
#define ADD_REG_RESERVED7                   57 // U8     Reserved for future purposes
#define ADD_REG_DO_SET                      58 // U8     Set the digital outputs
#define ADD_REG_DO_CLEAR                    59 // U8     Clear the digital outputs
#define ADD_REG_DO_TOGGLE                   60 // U8     Toggle the digital outputs
#define ADD_REG_DO_OUT                      61 // U8     Writes to the digital output
#define ADD_REG_RESERVED8                   62 // U8     Reserved for future purposes
#define ADD_REG_RESERVED9                   63 // U8     Reserved for future purposes
#define ADD_REG_DATA_STREAM_CONF            64 // U8     Configuration of data stream
#define ADD_REG_DATA_STREAM                 65 // U16    [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used
#define ADD_REG_ADC0_CONF                   66 // U8     
#define ADD_REG_ADC1_CONF                   67 // U8     
#define ADD_REG_RESERVED10                  68 // U8     Reserved for future purposes
#define ADD_REG_RESERVED11                  69 // U8     Reserved for future purposes
#define ADD_REG_RESERVED12                  70 // U8     Reserved for future purposes
#define ADD_REG_RESERVED13                  71 // U8     Reserved for future purposes
#define ADD_REG_RESERVED14                  72 // U8     Reserved for future purposes
#define ADD_REG_RESERVED15                  73 // U8     Reserved for future purposes
#define ADD_REG_RESERVED16                  74 // U8     Reserved for future purposes
#define ADD_REG_RESERVED17                  75 // U8     Reserved for future purposes
#define ADD_REG_RESERVED18                  76 // U8     Reserved for future purposes
#define ADD_REG_RESERVED19                  77 // U8     Reserved for future purposes
#define ADD_REG_RESERVED20                  78 // U8     Reserved for future purposes
#define ADD_REG_RESERVED21                  79 // U8     Reserved for future purposes
#define ADD_REG_RESERVED22                  80 // U8     Reserved for future purposes
#define ADD_REG_RESERVED23                  81 // U8     Reserved for future purposes
#define ADD_REG_COMMANDS                    82 // U8     Send commands to PIC32 ucontroller

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x52
#define APP_NBYTES_OF_REG_BANK              75

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_DI0                              (1<<0)       // Digital input 0
#define B_DI1                              (1<<1)       // Digital input 1
#define B_DI2                              (1<<2)       // Digital input 2
#define MSK_DI_SEL                         (7<<0)       // 
#define GM_DI_SYNC                         (0<<0)       // Used as a pure digital input
#define GM_DI_START_AND_STOP_SOUND         (1<<0)       // Starts sound when rising edge and stop when falling edge
#define GM_DI_START_SOUND                  (2<<0)       // Starts sound when rising edge
#define GM_DI_STOP                         (3<<0)       // Stops sound or frequency when rising edge
#define MSK_DO_SEL                         (1<<0)       // 
#define GM_DO_DIG                          (0<<0)       // Used as a pure digital output
#define GM_DO_HIGH_WHEN_SOUND              (1<<0)       // High when the sound is being played
#define B_DO0                              (1<<0)       // Digital output 0
#define B_DO1                              (1<<1)       // Digital output 1
#define B_DO2                              (1<<2)       // Digital output 2
#define MSK_DATA_STREAM_SEL                (1<<0)       // 
#define GM_DATA_STREAM_OFF                 (0<<0)       // Not used
#define GM_DATA_STREAM_1KHz                (1<<0)       // Use both as a pure analog inputs acquired at 1000 Hz
#define MSK_ADC0_SEL                       (3<<0)       // 
#define GM_ADC0_PURE_ANALOG_INPUT          (0<<0)       // 
#define GM_ADC0_CONTROL_LEFT_AMPLITUDE     (1<<0)       // 
#define GM_ADC0_CONTROL_BOTH_AMPLITUDE     (2<<0)       // 
#define MSK_ADC1_SEL                       (3<<0)       // ADC0 controls amplitude of both channels and ADC1 is an analog input
#define GM_ADC1_PURE_ANALOG_INPUT          (0<<0)       // ADC0 controls left amplitude and ADC1 is an analog input
#define GM_ADC1_CONTROL_RIGHT_AMPLITUDE    (1<<0)       // ADC0 controls right amplitude and ADC1 is an analog input
#define GM_ADC1_CONTROL_BOTH_AMPLITUDE     (2<<0)       // ADC0 controls left amplitude and ADC1 controls right amplitude
#define GM_ADC1_CONTROL_FREQUENCY          (3<<0)       // ADC0 controls both amplitude and ADC1 controls the output frequency
#define GM_DIS_BOOTLOADER                  0            // Disables bootloader buffers
#define GM_EN_BOOTLOADER                   1            // Enables bootloader buffers
#define GM_DEL_ALL_SOUNDS                  255          // Delete all sounds in the sound card

#endif /* _APP_REGS_H_ */