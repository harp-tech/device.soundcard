/*******************************************************************************
 Data Stream USART Source File

  File Name:
    usart.c

  Summary:
 Data Stream USART source

  Description:
    This file contains source code necessary for the data stream interface.
 *******************************************************************************/

// DOM-IGNORE-BEGIN
/*******************************************************************************
Copyright (c) 2013 released Microchip Technology Inc.  All rights reserved.

Microchip licenses to you the right to use, modify, copy and distribute
Software only when embedded on a Microchip microcontroller or digital signal
controller that is integrated into your product or third party product
(pursuant to the sublicense terms in the accompanying license agreement).

You should refer to the license agreement accompanying this Software for
additional information regarding your rights and obligations.

SOFTWARE AND DOCUMENTATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF
MERCHANTABILITY, TITLE, NON-INFRINGEMENT AND FITNESS FOR A PARTICULAR PURPOSE.
IN NO EVENT SHALL MICROCHIP OR ITS LICENSORS BE LIABLE OR OBLIGATED UNDER
CONTRACT, NEGLIGENCE, STRICT LIABILITY, CONTRIBUTION, BREACH OF WARRANTY, OR
OTHER LEGAL EQUITABLE THEORY ANY DIRECT OR INDIRECT DAMAGES OR EXPENSES
INCLUDING BUT NOT LIMITED TO ANY INCIDENTAL, SPECIAL, INDIRECT, PUNITIVE OR
CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, COST OF PROCUREMENT OF
SUBSTITUTE GOODS, TECHNOLOGY, SERVICES, OR ANY CLAIMS BY THIRD PARTIES
(INCLUDING BUT NOT LIMITED TO ANY DEFENSE THEREOF), OR OTHER SIMILAR COSTS.
 *******************************************************************************/
// DOM-IGNORE-END
#include "system/common/sys_module.h"
#include "bootloader/src/datastream.h"
#include "peripheral/ports/plib_ports.h"
#include "peripheral/usart/plib_usart.h"
#include "peripheral/dma/plib_dma.h"
#include "bootloader/src/bootloader.h"
#include "system/clk/sys_clk.h"
#include "driver/usart/drv_usart_static.h"

uint8_t DRV_USART0_ReadByte(void);
bool DRV_USART0_ReceiverBufferIsEmpty(void);
void DRV_USART0_WriteByte(const uint8_t byte);

extern DATASTREAM_HandlerType* handler;
extern uintptr_t _context;

void DATASTREAM_Tasks(void)
{
    if (handler == (DATASTREAM_HandlerType*)NULL)
    {
        return;
    }
    
    if (RX == currDir)
    {
        if (!DRV_USART0_ReceiverBufferIsEmpty())
        {
             
            // Copy the data to the buffer
            _rxBuffer[_rxCurSize] = DRV_USART0_ReadByte();
            
            if((_rxBuffer[_rxCurSize++] == EOT) || (_rxCurSize >= _rxMaxSize))
            {
                currDir = IDLE;
                handler(DATASTREAM_BUFFER_EVENT_COMPLETE, (DATASTREAM_BUFFER_HANDLE)_bufferHandle, _rxCurSize);
                _rxCurSize = 0;
            }
        }
    }
    else if (TX == currDir)
    {
        if (_txCurPos < _txMaxSize)
        {
            if (!DRV_USART0_TransmitBufferIsFull())
            {
                DRV_USART0_WriteByte(_txBuffer[_txCurPos++]);

                if (_txCurPos == _txMaxSize) // All data has been sent or is in the buffer
                {
                    currDir = IDLE;
                    handler(DATASTREAM_BUFFER_EVENT_COMPLETE, (DATASTREAM_BUFFER_HANDLE)_bufferHandle, _context);
                }
            }
        }
    }
}
