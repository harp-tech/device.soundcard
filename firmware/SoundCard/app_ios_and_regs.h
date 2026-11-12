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
// CMD_NOT_EXEC           Description: Command on the PAR bus can't be executed
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
    uint16_t REG_START;
    uint8_t REG_STOP;
    uint16_t REG_ATTENUATION_LEFT;
    uint16_t REG_ATTENUATION_RIGHT;
    uint16_t REG_ATTENUATION_BOTH[2];
    uint16_t REG_ATTENUATION_START[3];
    uint8_t REG_RESERVED0;
    uint8_t REG_RESERVED1;
    uint8_t REG_INPUT_STATE;
    uint8_t REG_CONFIGURE_DI0;
    uint8_t REG_CONFIGURE_DI1;
    uint16_t REG_START_ON_DI0;
    uint16_t REG_START_ON_DI1;
    uint16_t REG_ATTENUATION_LEFT_DI0;
    uint16_t REG_ATTENUATION_LEFT_DI1;
    uint16_t REG_ATTENUATION_RIGHT_DI0;
    uint16_t REG_ATTENUATION_RIGHT_DI1;
    uint8_t REG_RESERVED2;
    uint8_t REG_RESERVED3;
    uint8_t REG_RESERVED4;
    uint8_t REG_CONFIGURE_DO0;
    uint8_t REG_CONFIGURE_DO1;
    uint8_t REG_CONFIGURE_DO2;
    uint8_t REG_RESERVED5;
    uint8_t REG_RESERVED6;
    uint8_t REG_RESERVED7;
    uint8_t REG_OUTPUT_SET;
    uint8_t REG_OUTPUT_CLEAR;
    uint8_t REG_OUTPUT_TOGGLE;
    uint8_t REG_OUTPUT_STATE;
    uint8_t REG_RESERVED8;
    uint8_t REG_RESERVED9;
    uint8_t REG_ENABLE_ADC_CONTROL_STATE;
    uint16_t REG_ADC_CONTROL_STATE[5];
    uint8_t REG_CONFIGURE_ADC0;
    uint8_t REG_CONFIGURE_ADC1;
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
    uint8_t REG_PIC32_COMMANDS;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_START                       32 // U16    Starts the sound index if the value is less than 32, or otherwise the waveform generator at the specified frequency in Hz.
#define ADD_REG_STOP                        33 // U8     Stops the sound or frequency being played if the value is non-zero.
#define ADD_REG_ATTENUATION_LEFT            34 // U16    Specifies the attenuation for the left channel (1 LSB is 0.1dB).
#define ADD_REG_ATTENUATION_RIGHT           35 // U16    Specifies the attenuation for the right channel (1 LSB is 0.1dB).
#define ADD_REG_ATTENUATION_BOTH            36 // U16    Specifies the attenuation for the left and right channels simultaneously [Att L] [Att R] (1 LSB is 0.1dB).
#define ADD_REG_ATTENUATION_START           37 // U16    Specifies attenuation and simultaneously starts the sound index or frequency [Index] [Att L] [Att R].
#define ADD_REG_RESERVED0                   38 // U8     Reserved for future use.
#define ADD_REG_RESERVED1                   39 // U8     Reserved for future use.
#define ADD_REG_INPUT_STATE                 40 // U8     Reports the state of the digital inputs. Events are sent only for inputs configured in Digital mode.
#define ADD_REG_CONFIGURE_DI0               41 // U8     Specifies the operation mode of the digital input 0 (DI0).
#define ADD_REG_CONFIGURE_DI1               42 // U8     Specifies the operation mode of the digital input 1 (DI1).
#define ADD_REG_START_ON_DI0                43 // U16    Specifies the sound index or frequency to start when triggering DI0, following the same rules as Start.
#define ADD_REG_START_ON_DI1                44 // U16    Specifies the sound index or frequency to start when triggering DI1, following the same rules as Start.
#define ADD_REG_ATTENUATION_LEFT_DI0        45 // U16    Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
#define ADD_REG_ATTENUATION_LEFT_DI1        46 // U16    Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
#define ADD_REG_ATTENUATION_RIGHT_DI0       47 // U16    Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
#define ADD_REG_ATTENUATION_RIGHT_DI1       48 // U16    Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
#define ADD_REG_RESERVED2                   49 // U8     Reserved for future use.
#define ADD_REG_RESERVED3                   50 // U8     Reserved for future use.
#define ADD_REG_RESERVED4                   51 // U8     Reserved for future use.
#define ADD_REG_CONFIGURE_DO0               52 // U8     Specifies the operation mode of the digital output 0 (DO0).
#define ADD_REG_CONFIGURE_DO1               53 // U8     Specifies the operation mode of the digital output 1 (DO1).
#define ADD_REG_CONFIGURE_DO2               54 // U8     Specifies the operation mode of the digital output 2 (DO2).
#define ADD_REG_RESERVED5                   55 // U8     Reserved for future use.
#define ADD_REG_RESERVED6                   56 // U8     Reserved for future use.
#define ADD_REG_RESERVED7                   57 // U8     Reserved for future use.
#define ADD_REG_OUTPUT_SET                  58 // U8     Sets the specified digital output lines.
#define ADD_REG_OUTPUT_CLEAR                59 // U8     Clears the specified digital output lines.
#define ADD_REG_OUTPUT_TOGGLE               60 // U8     Toggles the specified digital output lines.
#define ADD_REG_OUTPUT_STATE                61 // U8     Writes the state of all digital output lines.
#define ADD_REG_RESERVED8                   62 // U8     Reserved for future use.
#define ADD_REG_RESERVED9                   63 // U8     Reserved for future use.
#define ADD_REG_ENABLE_ADC_CONTROL_STATE    64 // U8     Specifies whether the ADC control state is reported, sampled at 1 kHz.
#define ADD_REG_ADC_CONTROL_STATE           65 // U16    Reports sampled analog input data and current sound parameters.
#define ADD_REG_CONFIGURE_ADC0              66 // U8     Specifies the operation mode of analog input ADC0 (not implemented yet).
#define ADD_REG_CONFIGURE_ADC1              67 // U8     Specifies the operation mode of analog input ADC1 (not implemented yet).
#define ADD_REG_RESERVED10                  68 // U8     Reserved for future use.
#define ADD_REG_RESERVED11                  69 // U8     Reserved for future use.
#define ADD_REG_RESERVED12                  70 // U8     Reserved for future use.
#define ADD_REG_RESERVED13                  71 // U8     Reserved for future use.
#define ADD_REG_RESERVED14                  72 // U8     Reserved for future use.
#define ADD_REG_RESERVED15                  73 // U8     Reserved for future use.
#define ADD_REG_RESERVED16                  74 // U8     Reserved for future use.
#define ADD_REG_RESERVED17                  75 // U8     Reserved for future use.
#define ADD_REG_RESERVED18                  76 // U8     Reserved for future use.
#define ADD_REG_RESERVED19                  77 // U8     Reserved for future use.
#define ADD_REG_RESERVED20                  78 // U8     Reserved for future use.
#define ADD_REG_RESERVED21                  79 // U8     Reserved for future use.
#define ADD_REG_RESERVED22                  80 // U8     Reserved for future use.
#define ADD_REG_RESERVED23                  81 // U8     Reserved for future use.
#define ADD_REG_PIC32_COMMANDS              82 // U8     Sends commands to the PIC32 micro-controller

/************************************************************************/
/* SoundCard registers' memory limits                                   */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x52
#define APP_NBYTES_OF_REG_BANK              77

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_DI0                                  (1<<0)       // Digital input 0.
#define B_DI1                                  (1<<1)       // Digital input 1.
#define B_DI2                                  (1<<2)       // Digital input 2 (no associated event).
#define B_DO0                                  (1<<0)       // Digital output 0.
#define B_DO1                                  (1<<1)       // Digital output 1.
#define B_DO2                                  (1<<2)       // Digital output 2.
#define MSK_DI_MODE                            0x03         //
#define GM_DI_MODE_DIGITAL                     0x00         // Used as a pure digital input.
#define GM_DI_MODE_START_AND_STOP              0x01         // Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge and stop when falling edge (frequency only).
#define GM_DI_MODE_START                       0x02         // Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge.
#define GM_DI_MODE_STOP                        0x03         // Stops frequency when rising edge.
#define MSK_DO_MODE                            0x01         //
#define GM_DO_MODE_DIGITAL                     0x00         // Used as a pure digital output.
#define GM_DO_MODE_PULSE_ON_START              0x01         // Positive 400us pulse when a sound or frequency starts.
#define MSK_ADC_MODE                           0x03         //
#define GM_ADC_MODE_ANALOG_INPUT               0x00         // Used as a pure analog input.
#define GM_ADC_MODE_ATTENUATION                0x01         // Controls the attenuation of the left channel from ADC0 and of the right channel from ADC1.
#define GM_ADC_MODE_ATTENUATION_BOTH           0x02         // Controls the attenuation of both channels.
#define GM_ADC_MODE_FREQUENCY                  0x03         // Controls the frequency of the waveform generator from ADC1 only.
#define MSK_PIC32_COMMAND                      0xFF         //
#define GM_PIC32_COMMAND_DISABLE_BOOTLOADER    0x00         // Disable the PIC32 bootloader mode.
#define GM_PIC32_COMMAND_ENABLE_BOOTLOADER     0x01         // Enable the PIC32 bootloader mode.
#define GM_PIC32_COMMAND_DELETE_ALL_SOUNDS     0xFF         // Delete all sounds stored in the PIC32 micro-controller.

#endif /* _APP_REGS_H_ */
