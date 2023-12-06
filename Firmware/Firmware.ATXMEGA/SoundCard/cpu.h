// Author: Filipe Carvalho
#ifndef _CPU_1V1_H_
#define _CPU_1V1_H_

/************************************************************************/
/* Include external libraries                                           */
/************************************************************************/
#include <avr/io.h>
#include <avr/interrupt.h>

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
/* Common defines                                                       */
/************************************************************************/
#define INT_LEVEL_OFF	0
#define INT_LEVEL_LOW	1
#define INT_LEVEL_MED	2
#define INT_LEVEL_HIGH	3

/************************************************************************/
/* CPU                                                                  */
/************************************************************************/
#define PRESCALER_CPU_1		CLK_PSADIV_1_gc
#define PRESCALER_CPU_2		CLK_PSADIV_2_gc
#define PRESCALER_CPU_4		CLK_PSADIV_4_gc
#define PRESCALER_CPU_8		CLK_PSADIV_8_gc
#define PRESCALER_CPU_16	CLK_PSADIV_16_gc
#define PRESCALER_CPU_32	CLK_PSADIV_32_gc
#define PRESCALER_CPU_64	CLK_PSADIV_64_gc
#define PRESCALER_CPU_128	CLK_PSADIV_128_gc
#define PRESCALER_CPU_256	CLK_PSADIV_256_gc
#define PRESCALER_CPU_512	CLK_PSADIV_512_gc

#define OUT_PORT_CLK_PORTC	0
#define OUT_PORT_CLK_PORTD	1

bool cpu_config_clock(uint32_t cpu_freq, bool lock_clock, bool external_clock);
void cpu_config_clock_32Khz(uint8_t prescaler, bool lock_clock);
bool cpu_change_clock(uint32_t cpu_freq, bool lock_clock);

void cpu_clk_output(bool stay_on_loop, bool pin4_instead_of_7, uint8_t out_port);

void cpu_enable_int_level(uint8_t int_level);
void cpu_disable_int_level(uint8_t int_level);

/************************************************************************/
/* Watchdog                                                             */
/************************************************************************/
#define PER_WDT_8ms		WDT_PER_8CLK_gc
#define PER_WDT_16ms		WDT_PER_16CLK_gc
#define PER_WDT_32ms		WDT_PER_32CLK_gc
#define PER_WDT_64ms		WDT_PER_64CLK_gc
#define PER_WDT_125ms	WDT_PER_125CLK_gc
#define PER_WDT_250ms	WDT_PER_250CLK_gc
#define PER_WDT_500ms	WDT_PER_500CLK_gc
#define PER_WDT_1s		WDT_PER_1KCLK_gc
#define PER_WDT_2s		WDT_PER_2KCLK_gc
#define PER_WDT_4s		WDT_PER_4KCLK_gc
#define PER_WDT_8s		WDT_PER_8KCLK_gc

void wdt_enable(uint8_t per);
void wdt_disable(void);
void wdt_reset_device(void);

/************************************************************************/
/* IO                                                                   */
/************************************************************************/
#define PULL_IO_TRISTATE	(0x0 << 3)
#define PULL_IO_BUSHOLDER	(0x1 << 3)
#define PULL_IO_DOWN			(0x2 << 3)
#define PULL_IO_UP			(0x3 << 3)

#define SENSE_IO_EDGES_BOTH	0
#define SENSE_IO_EDGE_RISING	1
#define SENSE_IO_EDGE_FALLING	2
#define SENSE_IO_LOW_LEVEL		3
#define SENSE_IO_NO_INT_USED	0

#define OUT_IO_DIGITAL		(0x0 << 3)
#define OUT_IO_WIREDOR		(0x4 << 3)
#define OUT_IO_WIREDAND		(0x5 << 3)
#define OUT_IO_WIREDORPULL	(0x6 << 3)
#define OUT_IO_WIREDANDPULL	(0x7 << 3)

#define IN_EN_IO_EN 1
#define IN_EN_IO_DIS 0

#define set_io(port, pin)				port.OUTSET = 1 << pin
#define clear_io(port, pin)			port.OUTCLR = 1 << pin
#define toggle_io(port, pin)			port.OUTTGL = 1 << pin
#define read_io(port, pin)				(*(&port.IN) & (1 << pin))
#define set_io_mask(port, mask)		port.OUTSET = mask
#define clear_io_mask(port, mask)	port.OUTCLR = mask
#define toggle_io_mask(port, mask)	port.OUTTGL = mask

void io_pin2in(PORT_t* port, uint8_t pin, uint8_t pull, uint8_t sense);
void io_pin2out(PORT_t* port, uint8_t pin, uint8_t out, bool input_en);
void io_pin2out_with_interrupt(PORT_t* port, uint8_t pin, uint8_t out, uint8_t sense);
void io_set_int(PORT_t* port, uint8_t int_level, uint8_t int_n, uint8_t mask, bool reset_mask);

/************************************************************************/
/* Timer                                                                */
/************************************************************************/
#define TIMER_PRESCALER_DIV1		1
#define TIMER_PRESCALER_DIV2		2
#define TIMER_PRESCALER_DIV4		3
#define TIMER_PRESCALER_DIV8		4
#define TIMER_PRESCALER_DIV64		5
#define TIMER_PRESCALER_DIV256	6
#define TIMER_PRESCALER_DIV1024	7

void timer_type0_enable(TC0_t* timer, uint8_t prescaler, uint16_t target_count, uint8_t int_level);
void timer_type0_pwm(TC0_t* timer, uint8_t prescaler, uint16_t target_count, uint16_t duty_cycle_count, uint8_t int_level_ovf, uint8_t int_level_cca);
void timer_type0_set_target(TC0_t* timer, uint16_t target_count);
bool timer_type0_get_flag(TC0_t* timer);
void timer_type0_reset_flag(TC0_t* timer);
void timer_type0_stop(TC0_t* timer);
void timer_type0_set_counter(TC0_t* timer, uint16_t counter);
void timer_type0_wait(TC0_t* timer, uint8_t prescaler, uint16_t target_count);
bool calculate_timer_16bits(uint32_t f_cpu, float freq, uint8_t * timer_prescaler, uint16_t * timer_target_count);

void timer_type1_enable(TC1_t* timer, uint8_t prescaler, uint16_t target_count, uint8_t int_level);
void timer_type1_set_target(TC1_t* timer, uint16_t target_count);
bool timer_type1_get_flag(TC1_t* timer);
void timer_type1_reset_flag(TC1_t* timer);
void timer_type1_stop(TC1_t* timer);
void timer_type1_set_counter(TC1_t* timer, uint16_t counter);
void timer_type1_wait(TC1_t* timer, uint8_t prescaler, uint16_t target_count);

/************************************************************************/
/* EEPROM                                                               */
/************************************************************************/
bool eeprom_is_busy(void);

uint8_t eeprom_rd_byte(uint16_t addr);
void eeprom_wr_byte(uint16_t addr, uint8_t byte);

void eeprom_wr_i16(uint16_t addr, int16_t _16b);
void eeprom_wr_i32(uint16_t addr, int32_t _32b);
int16_t eeprom_rd_i16(uint16_t addr);
int32_t eeprom_rd_i32(uint16_t addr);

/************************************************************************/
/* ADC                                                                  */
/************************************************************************/
#define RES_ADC_8BITS	ADC_RESOLUTION_8BIT_gc
#define RES_ADC_12BITS	ADC_RESOLUTION_12BIT_gc

#define REF_ADC_1V		ADC_REFSEL_INT1V_gc | ADC_BANDGAP_bm	// 10/11
#define REF_ADC_VCC		ADC_REFSEL_INTVCC_gc							// VCC/1.6
#define REF_ADC_PORTA	ADC_REFSEL_AREFA_gc							//
#define REF_ADC_PORTB	ADC_REFSEL_AREFB_gc							//
#define REF_ADC_VCCDIV2	ADC_REFSEL_INTVCC2_gc						// VCC/2

#define PRESCALER_ADC_DIV4		ADC_PRESCALER_DIV4_gc
#define PRESCALER_ADC_DIV8		ADC_PRESCALER_DIV8_gc
#define PRESCALER_ADC_DIV16	ADC_PRESCALER_DIV16_gc
#define PRESCALER_ADC_DIV32	ADC_PRESCALER_DIV32_gc
#define PRESCALER_ADC_DIV64	ADC_PRESCALER_DIV64_gc
#define PRESCALER_ADC_DIV128	ADC_PRESCALER_DIV128_gc
#define PRESCALER_ADC_DIV256	ADC_PRESCALER_DIV256_gc
#define PRESCALER_ADC_DIV512	ADC_PRESCALER_DIV512_gc

void adc_A_initialize_single_ended(uint8_t analog_reference);
int16_t adc_A_read_channel(uint8_t index);
void adc_A_calibrate_offset(uint8_t index);

uint16_t adcA_unsigned_single_ended(ADC_t* adc, uint8_t res, uint8_t ref , uint8_t prescaler, uint8_t adc_pin, TC0_t* timer);

#endif /* _CPU_1V1_H_ */