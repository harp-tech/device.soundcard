#ifndef _HWBP_CORE_COM_H_
#define _HWBP_CORE_COM_H_

#include <string.h>
#include "cpu.h"
#include "hwbp_core_types.h"

// com_mode
#define COM_MODE_USB		0
#define COM_MODE_UART	1

// rx_timeout
#define RX_TIMEOUT_MS 50


/************************************************************************/
/* UART definitions                                                     */
/************************************************************************/
#define HWBP_UART_RX_INT_LEVEL	INT_LEVEL_HIGH
#define HWBP_UART_TX_INT_LEVEL	INT_LEVEL_HIGH
#define HWBP_UART_CTS_INT_LEVEL	INT_LEVEL_HIGH

#define HWBP_UART_RXBUFSIZ			MAX_PACKET_SIZE
#define HWBP_UART_TEMP_RXBUFSIZ		32

#if defined(__AVR_ATxmega16A4U__)
    #define HWBP_UART_UART			USARTE0
    #define HWBP_UART_PORT			PORTE
    #define HWBP_UART_RX_pin		2
    #define HWBP_UART_TX_pin		3

    #define HWBP_UART_RX_ROUTINE_	ISR(USARTE0_RXC_vect/*, ISR_NAKED*/)
    #define HWBP_UART_TX_ROUTINE_	ISR(USARTE0_DRE_vect/*, ISR_NAKED*/)


    #define HWBP_UART_USE_FLOW_CONTROL	// comment this line if don't use
    #define HWBP_UART_RTS_PORT		PORTE
    #define HWBP_UART_RTS_pin		1
    #define HWBP_UART_CTS_PORT		PORTE
    #define HWBP_UART_CTS_pin		0

    #define HWBP_UART_CTS_ROUTINE_		ISR(PORTE_INT0_vect/*, ISR_NAKED*/)
    #define HWBP_UART_CTS_INT_N			0

    #define hwbp_uart_leave_interrupt return/*reti();*/

    /* Use as much as possible */
    #define HWBP_UART_TXBUFSIZ		512
#endif

#if defined(__AVR_ATxmega32A4U__)
	#define HWBP_UART_UART			USARTE0
	#define HWBP_UART_PORT			PORTE
	#define HWBP_UART_RX_pin		2
	#define HWBP_UART_TX_pin		3

	#define HWBP_UART_RX_ROUTINE_	ISR(USARTE0_RXC_vect/*, ISR_NAKED*/)
	#define HWBP_UART_TX_ROUTINE_	ISR(USARTE0_DRE_vect/*, ISR_NAKED*/)


	#define HWBP_UART_USE_FLOW_CONTROL	// comment this line if don't use
	#define HWBP_UART_RTS_PORT		PORTE
	#define HWBP_UART_RTS_pin		1
	#define HWBP_UART_CTS_PORT		PORTE
	#define HWBP_UART_CTS_pin		0

	#define HWBP_UART_CTS_ROUTINE_		ISR(PORTE_INT0_vect/*, ISR_NAKED*/)
	#define HWBP_UART_CTS_INT_N			0

	#define hwbp_uart_leave_interrupt return/*reti();*/

	/* Use as much as possible */
	#define HWBP_UART_TXBUFSIZ		2048
#endif

#if defined(__AVR_ATxmega64A4U__)
	#define HWBP_UART_UART			USARTE0
	#define HWBP_UART_PORT			PORTE
	#define HWBP_UART_RX_pin		2
	#define HWBP_UART_TX_pin		3

	#define HWBP_UART_RX_ROUTINE_	ISR(USARTE0_RXC_vect/*, ISR_NAKED*/)
	#define HWBP_UART_TX_ROUTINE_	ISR(USARTE0_DRE_vect/*, ISR_NAKED*/)


	#define HWBP_UART_USE_FLOW_CONTROL	// comment this line if don't use
	#define HWBP_UART_RTS_PORT		PORTE
	#define HWBP_UART_RTS_pin		1
	#define HWBP_UART_CTS_PORT		PORTE
	#define HWBP_UART_CTS_pin		0

	#define HWBP_UART_CTS_ROUTINE_		ISR(PORTE_INT0_vect/*, ISR_NAKED*/)
	#define HWBP_UART_CTS_INT_N			0

	#define hwbp_uart_leave_interrupt return/*reti();*/

	/* Use as much as possible */
	#define HWBP_UART_TXBUFSIZ		2048
#endif

#if defined(__AVR_ATxmega128A4U__)
	#define HWBP_UART_UART			USARTE0
	#define HWBP_UART_PORT			PORTE
	#define HWBP_UART_RX_pin		2
	#define HWBP_UART_TX_pin		3

	#define HWBP_UART_RX_ROUTINE_	ISR(USARTE0_RXC_vect/*, ISR_NAKED*/)
	#define HWBP_UART_TX_ROUTINE_	ISR(USARTE0_DRE_vect/*, ISR_NAKED*/)


	#define HWBP_UART_USE_FLOW_CONTROL	// comment this line if don't use
	#define HWBP_UART_RTS_PORT		PORTE
	#define HWBP_UART_RTS_pin		1
	#define HWBP_UART_CTS_PORT		PORTE
	#define HWBP_UART_CTS_pin		0

	#define HWBP_UART_CTS_ROUTINE_		ISR(PORTE_INT0_vect/*, ISR_NAKED*/)
	#define HWBP_UART_CTS_INT_N			0

	#define hwbp_uart_leave_interrupt return/*reti();*/

	/* Use as much as possible */
	#define HWBP_UART_TXBUFSIZ		5120
#endif

#if defined(__AVR_ATxmega128A1U__)
	#define HWBP_UART_UART			USARTF0
	#define HWBP_UART_PORT			PORTF
	#define HWBP_UART_RX_pin		2
	#define HWBP_UART_TX_pin		3

	#define HWBP_UART_RX_ROUTINE_	ISR(USARTF0_RXC_vect/*, ISR_NAKED*/)
	#define HWBP_UART_TX_ROUTINE_	ISR(USARTF0_DRE_vect/*, ISR_NAKED*/)


	#define HWBP_UART_USE_FLOW_CONTROL	// comment this line if don't use
	#define HWBP_UART_RTS_PORT		PORTK
	#define HWBP_UART_RTS_pin		0
	#define HWBP_UART_CTS_PORT		PORTJ
	#define HWBP_UART_CTS_pin		6

	#define HWBP_UART_CTS_ROUTINE_		ISR(PORTJ_INT0_vect/*, ISR_NAKED*/)
	#define HWBP_UART_CTS_INT_N			0

	#define hwbp_uart_leave_interrupt return/*reti();*/

	/* Use as much as possible */
	#define HWBP_UART_TXBUFSIZ		5120
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
#define disable_hwbp_uart_rx set_io(HWBP_UART_RTS_PORT, HWBP_UART_RTS_pin);
#define enable_hwbp_uart_rx	 clear_io(HWBP_UART_RTS_PORT, HWBP_UART_RTS_pin);

void hwbp_com_uart_init(uint16_t BSEL, int8_t BSCALE, bool use_clk2x);
void hwbp_com_uart_enable(void);
void hwbp_com_uart_disable();												// Not used

void hwbp_uart_xmit_now(const uint8_t *dataIn0, uint8_t siz);	// Not used
void hwbp_uart_xmit_now_byte(const uint8_t byte);					// Not used
void hwbp_uart_xmit(const uint8_t *dataIn0, uint8_t siz);

bool hwbp_uart_rcv_now(uint8_t * byte);								// Not used


/// Called before execute the uart rRX interrupt
void core_callback_uart_rx_before_exec(void);
/// Called after execute the uart RX interrupt
void core_callback_uart_rx_after_exec(void);
/// Called before execute the uart Tx interrupt
void core_callback_uart_tx_before_exec(void);
/// Called after execute the uart TX interrupt
void core_callback_uart_tx_after_exec(void);
/// Called before execute the uart CTS interrupt
void core_callback_uart_cts_before_exec(void);
/// Called after execute the uart CTS interrupt
void core_callback_uart_cts_after_exec(void);


#endif /* _HWBP_CORE_COM_H_ */
