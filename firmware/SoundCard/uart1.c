#include "uart1.h"
#include <string.h>


/************************************************************************/
/* Buffers and pointers                                                 */
/************************************************************************/
uint8_t txbuff_uart1[UART1_TXBUFSIZ];
uint8_t rxbuff_uart1[UART1_RXBUFSIZ];

#if UART1_TXBUFSIZ > 256
	uint16_t uart1_tail = 0;
	uint16_t uart1_head = 0;
#else
	uint8_t uart1_tail = 0;
	uint8_t uart1_head = 0;
#endif

#if UART1_RXBUFSIZ > 256
	uint16_t uart1_rx_pointer = 0;
#else
	uint8_t uart1_rx_pointer = 0;
#endif
	

	
/************************************************************************/
/* Initialization and ON/OFF                                            */
/************************************************************************/
void uart1_init(uint16_t BSEL, int8_t BSCALE, bool use_clk2x)
{
	#ifdef UART1_USE_FLOW_CONTROL
		io_pin2out(&UART1_RTS_PORT, UART1_RTS_pin, OUT_IO_DIGITAL, IN_EN_IO_DIS);
		io_pin2in(&UART1_CTS_PORT, UART1_CTS_pin, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);
		io_set_int(&UART1_CTS_PORT, UART1_CTS_INT_LEVEL, UART1_CTS_INT_N, (1<<UART1_CTS_pin), true);
		enable_uart1_rx;
	#endif
	
	UART1_UART.CTRLC = USART_CMODE_ASYNCHRONOUS_gc | USART_PMODE_DISABLED_gc | USART_CHSIZE_8BIT_gc;
	UART1_UART.BAUDCTRLA = *((uint8_t*)&BSEL);
	UART1_UART.BAUDCTRLB = (*(1+(uint8_t*)&BSEL) & 0x0F) | ((BSCALE<<4) & 0xF0);

	if (use_clk2x)
		UART1_UART.CTRLB |= USART_CLK2X_bm;
	
	set_io(UART1_PORT, UART1_TX_pin);
	io_pin2out(&UART1_PORT, UART1_TX_pin, OUT_IO_DIGITAL, IN_EN_IO_DIS);
	io_pin2in(&UART1_PORT, UART1_RX_pin, PULL_IO_TRISTATE, SENSE_IO_NO_INT_USED);
	
	#ifdef UART1_USE_FLOW_CONTROL
		if (UART1_CTS_PORT.IN & (1 << UART1_CTS_pin) )
			/* Disable uart interrupt until RTS is logic low */
			UART1_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);
	#endif
}

void uart1_enable(void)
{
	UART1_UART.CTRLB |= (USART_RXEN_bm | USART_TXEN_bm);
	UART1_UART.STATUS = USART_RXCIF_bm | USART_TXCIF_bm | USART_DREIF_bm;
	UART1_UART.CTRLA |= (UART1_RX_INT_LEVEL<< 4);
}

void uart1_disable(void)
{
	UART1_UART.CTRLB &= (USART_RXEN_bm | USART_TXEN_bm);
}

/************************************************************************/
/* Interrupt TX                                                         */
/************************************************************************/
UART1_TX_ROUTINE_
{
	UART1_UART.DATA = txbuff_uart1[uart1_tail++];
	if (uart1_tail == UART1_TXBUFSIZ)
		uart1_tail = 0;
	
	/* disable this interrupt until new data arrive to buffer */
	if (uart1_head == uart1_tail)
		UART1_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);

	uart1_leave_interrupt;
}

/************************************************************************/
/* Interrupt CTS                                                        */
/************************************************************************/
#ifdef UART1_USE_FLOW_CONTROL
   UART1_CTS_ROUTINE_
   {
	   if (UART1_CTS_PORT.IN & (1 << UART1_CTS_pin) )
		   /* Disable uart interrupt until RTS is logic low */
		   UART1_UART.CTRLA &= ~(USART_DREINTLVL_OFF_gc | USART_DREINTLVL_gm);
	   else
		   if (uart1_tail != uart1_head)
			   /* If the buffer is not empty, enable Tx interrupt */
			   UART1_UART.CTRLA |= UART1_TX_INT_LEVEL;

	   uart1_leave_interrupt;
   }
#endif

/************************************************************************/
/* Send data                                                            */
/************************************************************************/
void uart1_xmit_now(const uint8_t *dataIn0, uint8_t siz)
{
	for (uint8_t i = 0; i < siz; i++) {
		loop_until_bit_is_set(UART1_UART.STATUS, USART_DREIF_bp);
		UART1_UART.DATA = dataIn0[i];
	}
}

void uart1_xmit_now_byte(const uint8_t byte)
{
	loop_until_bit_is_set(UART1_UART.STATUS, USART_DREIF_bp);
	UART1_UART.DATA = byte;
}

void uart1_xmit(const uint8_t *dataIn0, uint8_t siz)
{
	#ifdef UART1_USE_FLOW_CONTROL
		if (!(UART1_CTS_PORT.IN & (1 << UART1_CTS_pin)))
	#endif
			UART1_UART.CTRLA |= UART1_TX_INT_LEVEL;	// Re-enable TX interrupt
	
	
	uint16_t space = UART1_TXBUFSIZ - uart1_head;
	if (space >= siz)
	{
		memcpy(txbuff_uart1+uart1_head, dataIn0, siz);
		
		bool tail_ahead = (uart1_tail > uart1_head);
		uart1_head += siz;
		if (uart1_head == UART1_TXBUFSIZ)
		{
			uart1_head = 0;
			if (uart1_tail == 0)    uart1_tail = 1;			// lose oldest byte in buffer
		}
		else if (tail_ahead && uart1_tail <= uart1_head)	// if buffer overflow
		{
			uart1_tail = uart1_head+1;								// lose oldest bytes in buffer
			if (uart1_tail == UART1_TXBUFSIZ)    uart1_tail = 0;
			
		}
	}
	else
	{
		memcpy(txbuff_uart1+uart1_head, dataIn0, space);
		siz -= space;
		memcpy(txbuff_uart1, dataIn0+space, siz);
		bool tail_ahead = (uart1_tail > uart1_head);
		uart1_head = siz;
		if (tail_ahead || uart1_tail <= uart1_head)			// if buffer overflow
		{
			uart1_tail = uart1_head+1;								// lose oldest bytes in buffer
			if (uart1_tail == UART1_TXBUFSIZ)    uart1_tail = 0;
		}
	}
}

/************************************************************************/
/* Receive data                                                         */
/************************************************************************/
bool uart1_rcv_now(uint8_t * byte)
{
	if (UART1_UART.STATUS & USART_RXCIF_bm)
	{
		*byte = UART1_UART.DATA;
		return true;
	}
	
	return false;
}

UART1_RX_ROUTINE_
{
	uart1_rcv_byte_callback(UART1_DATA);
	uart1_leave_interrupt;
}