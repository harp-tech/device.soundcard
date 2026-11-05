#ifndef _UART1_H_
#define _UART1_H_

#include "cpu.h"


/************************************************************************/
/* UART definitions                                                     */
/************************************************************************/
#define UART1_RX_INT_LEVEL		INT_LEVEL_LOW
#define UART1_TX_INT_LEVEL		INT_LEVEL_LOW
#define UART1_CTS_INT_LEVEL	INT_LEVEL_LOW

#define UART1_RXBUFSIZ			256
#define UART1_TXBUFSIZ			256

#define UART1_UART				USARTD0
#define UART1_PORT				PORTD
#define UART1_DATA				USARTD0_DATA
#define UART1_RX_pin				2
#define UART1_TX_pin				3

#define UART1_RX_ROUTINE_		ISR(USARTD0_RXC_vect, ISR_NAKED)
#define UART1_TX_ROUTINE_		ISR(USARTD0_DRE_vect, ISR_NAKED)
//#define UART1_RX_ROUTINE_		ISR(USARTC1_RXC_vect)
//#define UART1_TX_ROUTINE_		ISR(USARTC1_DRE_vect)

//#define UART1_USE_FLOW_CONTROL	// comment this line if don't use
#define UART1_RTS_PORT			PORTA
#define UART1_RTS_pin			1
#define UART1_CTS_PORT			PORTD
#define UART1_CTS_pin			4

#define UART1_CTS_ROUTINE_		ISR(PORTD_INT0_vect, ISR_NAKED)
//#define UART1_CTS_ROUTINE_		ISR(PORTD_INT0_vect)
#define UART1_CTS_INT_N			0

#define uart1_leave_interrupt /*return*/reti()

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
#define disable_uart1_rx set_io(UART1_RTS_PORT, UART1_RTS_pin);
#define enable_uart1_rx	 clear_io(UART1_RTS_PORT, UART1_RTS_pin);

void uart1_init(uint16_t BSEL, int8_t BSCALE, bool use_clk2x);
void uart1_enable(void);
void uart1_disable();

void uart1_xmit_now(const uint8_t *dataIn0, uint8_t siz);
void uart1_xmit_now_byte(const uint8_t byte);
void uart1_xmit(const uint8_t *dataIn0, uint8_t siz);

void uart1_rcv_byte_callback(uint8_t byte);
bool uart1_rcv_now(uint8_t * byte);

#endif /* _UART1_H_ */
