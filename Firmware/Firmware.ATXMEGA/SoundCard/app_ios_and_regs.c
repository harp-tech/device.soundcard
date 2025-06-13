#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // DIN0
	io_pin2in(&PORTD, 0, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // DIN1
	io_pin2in(&PORTC, 0, PULL_IO_UP, SENSE_IO_NO_INT_USED);              // DIN2
	io_pin2in(&PORTC, 5, PULL_IO_UP, SENSE_IO_EDGE_RISING);              // CMD_LATCHED
	io_pin2in(&PORTD, 5, PULL_IO_UP, SENSE_IO_EDGE_RISING);              // CMD_NOT_EXEC
	io_pin2in(&PORTC, 6, PULL_IO_UP, SENSE_IO_EDGES_BOTH);               // SOUND_IS_ON

	/* Configure input interrupts */
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // DIN0
	io_set_int(&PORTD, INT_LEVEL_LOW, 0, (1<<0), false);                 // DIN1
	io_set_int(&PORTC, INT_LEVEL_LOW, 1, (1<<5), false);                 // CMD_LATCHED
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<6), false);                 // SOUND_IS_ON

	/* Configure output pins */
	io_pin2out(&PORTA, 0, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR0
	io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR1
	io_pin2out(&PORTA, 2, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR2
	io_pin2out(&PORTA, 3, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR3
	io_pin2out(&PORTA, 4, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR4
	io_pin2out(&PORTA, 5, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR5
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR6
	io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // PAR7
	io_pin2out(&PORTD, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DOUT0
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DOUT1
	io_pin2out(&PORTC, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DOUT2
	io_pin2out(&PORTC, 7, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // BOOTLOADER_EN
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_DIS);                 // CMD_WRITE

	/* Initialize output pins */
	clr_PAR0;
	clr_PAR1;
	clr_PAR2;
	clr_PAR3;
	clr_PAR4;
	clr_PAR5;
	clr_PAR6;
	clr_PAR7;
	clr_DOUT0;
	clr_DOUT1;
	clr_DOUT2;
	clr_BOOTLOADER_EN;
	clr_CMD_WRITE;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U16,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	1,
	1,
	2,
	3,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	5,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_PLAY_SOUND_OR_FREQ),
	(uint8_t*)(&app_regs.REG_STOP),
	(uint8_t*)(&app_regs.REG_ATTENUATION_LEFT),
	(uint8_t*)(&app_regs.REG_ATTENUATION_RIGHT),
	(uint8_t*)(app_regs.REG_ATTENUATION_BOTH),
	(uint8_t*)(app_regs.REG_SET_ATTENUATION_AND_PLAY_SOUND_OR_FREQ),
	(uint8_t*)(&app_regs.REG_RESERVED0),
	(uint8_t*)(&app_regs.REG_RESERVED1),
	(uint8_t*)(&app_regs.REG_DIGITAL_INPUTS),
	(uint8_t*)(&app_regs.REG_DI0_CONF),
	(uint8_t*)(&app_regs.REG_DI1_CONF),
	(uint8_t*)(&app_regs.REG_DI0_SOUND_INDEX),
	(uint8_t*)(&app_regs.REG_DI1_SOUND_INDEX),
	(uint8_t*)(&app_regs.REG_DI0_ATTENUATION_LEFT),
	(uint8_t*)(&app_regs.REG_DI1_ATTENUATION_LEFT),
	(uint8_t*)(&app_regs.REG_DI0_ATTENUATION_RIGHT),
	(uint8_t*)(&app_regs.REG_DI1_ATTENUATION_RIGHT),
	(uint8_t*)(&app_regs.REG_RESERVED2),
	(uint8_t*)(&app_regs.REG_RESERVED3),
	(uint8_t*)(&app_regs.REG_RESERVED4),
	(uint8_t*)(&app_regs.REG_DO0_CONF),
	(uint8_t*)(&app_regs.REG_DO1_CONF),
	(uint8_t*)(&app_regs.REG_DO2_CONF),
	(uint8_t*)(&app_regs.REG_RESERVED5),
	(uint8_t*)(&app_regs.REG_RESERVED6),
	(uint8_t*)(&app_regs.REG_RESERVED7),
	(uint8_t*)(&app_regs.REG_DO_SET),
	(uint8_t*)(&app_regs.REG_DO_CLEAR),
	(uint8_t*)(&app_regs.REG_DO_TOGGLE),
	(uint8_t*)(&app_regs.REG_DO_OUT),
	(uint8_t*)(&app_regs.REG_RESERVED8),
	(uint8_t*)(&app_regs.REG_RESERVED9),
	(uint8_t*)(&app_regs.REG_DATA_STREAM_CONF),
	(uint8_t*)(app_regs.REG_DATA_STREAM),
	(uint8_t*)(&app_regs.REG_ADC0_CONF),
	(uint8_t*)(&app_regs.REG_ADC1_CONF),
	(uint8_t*)(&app_regs.REG_RESERVED10),
	(uint8_t*)(&app_regs.REG_RESERVED11),
	(uint8_t*)(&app_regs.REG_RESERVED12),
	(uint8_t*)(&app_regs.REG_RESERVED13),
	(uint8_t*)(&app_regs.REG_RESERVED14),
	(uint8_t*)(&app_regs.REG_RESERVED15),
	(uint8_t*)(&app_regs.REG_RESERVED16),
	(uint8_t*)(&app_regs.REG_RESERVED17),
	(uint8_t*)(&app_regs.REG_RESERVED18),
	(uint8_t*)(&app_regs.REG_RESERVED19),
	(uint8_t*)(&app_regs.REG_RESERVED20),
	(uint8_t*)(&app_regs.REG_RESERVED21),
	(uint8_t*)(&app_regs.REG_RESERVED22),
	(uint8_t*)(&app_regs.REG_RESERVED23),
	(uint8_t*)(&app_regs.REG_COMMANDS)
};