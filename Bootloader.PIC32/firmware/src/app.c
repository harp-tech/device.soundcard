/*******************************************************************************
  MPLAB Harmony Application Source File
  
  Company:
    Microchip Technology Inc.
  
  File Name:
    app.c

  Summary:
    This file contains the source code for the MPLAB Harmony application.

  Description:
    This file contains the source code for the MPLAB Harmony application.  It 
    implements the logic of the application's state machine and it may call 
    API routines of other MPLAB Harmony modules in the system, such as drivers,
    system services, and middleware.  However, it does not call any of the
    system interfaces (such as the "Initialize" and "Tasks" functions) of any of
    the modules in the system or make any assumptions about when those functions
    are called.  That is the responsibility of the configuration-specific system
    files.
 *******************************************************************************/

// DOM-IGNORE-BEGIN
/*******************************************************************************
Copyright (c) 2013-2014 released Microchip Technology Inc.  All rights reserved.

Microchip licenses to you the right to use, modify, copy and distribute
Software only when embedded on a Microchip microcontroller or digital signal
controller that is integrated into your product or third party product
(pursuant to the sublicense terms in the accompanying license agreement).

You should refer to the license agreement accompanying this Software for
additional information regarding your rights and obligations.

SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
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


// *****************************************************************************
// *****************************************************************************
// Section: Included Files 
// *****************************************************************************
// *****************************************************************************

#include "app.h"

// *****************************************************************************
// *****************************************************************************
// Section: Global Data Definitions
// *****************************************************************************
// *****************************************************************************

// *****************************************************************************
/* Application Data

  Summary:
    Holds application data

  Description:
    This structure holds the application's data.

  Remarks:
    This structure should be initialized by the APP_Initialize function.
    
    Application strings and buffers are be defined outside this structure.
*/

APP_DATA appData;

// *****************************************************************************
// *****************************************************************************
// Section: Application Callback Functions
// *****************************************************************************
// *****************************************************************************
/******************************************************************************
  Function:
    static void APP_Bootloader_ForceEvent (void)
    
   Remarks:
    Sets a trigger to be passed to force bootloader callback.
    Run bootloader if switch_1 is pressed OR
    if memory location == '0xFFFFFFFF' otherwise jump to user 
    application.
*/ 
int APP_Bootloader_ForceEvent(void)
{
    /* Check the BOOTLOADER_EN @ RB1 "switch" to trigger bootloader */
    if ((PORTB & (1 << 1)))
        return (1);

    /* Check the trigger memory location and return true/false. */
    if (*(uint32_t *)APP_RESET_ADDRESS == 0xFFFFFFFF)
        return (1);
    
    return (0);
}

/* TODO:  Add any necessary callback functions.
*/

// *****************************************************************************
// *****************************************************************************
// Section: Application Local Functions
// *****************************************************************************
// *****************************************************************************


/* TODO:  Add any necessary local functions.
*/


// *****************************************************************************
// *****************************************************************************
// Section: Application Initialization and State Machine Functions
// *****************************************************************************
// *****************************************************************************

/*******************************************************************************
  Function:
    void APP_Initialize ( void )

  Remarks:
    See prototype in app.h.
 */

void APP_Initialize ( void )
{
    /* Place the App state machine in its initial state. */
    appData.state = APP_STATE_INIT;

    // Register the bootloader callbacks
    BOOTLOADER_ForceBootloadRegister(APP_Bootloader_ForceEvent);
    
    /* TODO: Initialize your application's state machine and other
     * parameters.
     */
}


/******************************************************************************
  Function:
    void APP_Tasks ( void )

  Remarks:
    See prototype in app.h.
 */

void APP_Tasks ( void )
{

    /* Check the application's current state. */
    switch ( appData.state )
    {
        /* Application's initial state. */
        case APP_STATE_INIT:
        {
            bool appInitialized = true;
            
            TRISGCLR = (1 << 14);   // config LED MEMORY
            TRISFCLR = (1 << 1);    // config LED AUDIO
            TRISFCLR = (1 << 0);    // config LED USB
        
            if (appInitialized)
            {
            
                appData.state = APP_STATE_SERVICE_TASKS;
            }
            break;
        }

        case APP_STATE_SERVICE_TASKS:
        {
            static uint32_t cntr = 0;
            static uint32_t led_index = 0;
            // Blink the LED
            if (cntr++ == 200000)
            {
                // LED_USB @ RF0 toggle
                //LATFINV = (1 << 0);
                cntr = 0;
                
                switch (led_index)
                {
                    case 0:
                        LATGSET = (1 << 14);    // set MEMORY
                        LATFCLR = (1 << 1);     // clear AUDIO
                        LATFCLR = (1 << 0);     // clear USB
                        break;
                        
                    case 1:
                        LATGCLR = (1 << 14);    // clear MEMORY
                        LATFSET = (1 << 1);     // set AUDIO
                        LATFCLR = (1 << 0);     // clear USB
                        break;
                        
                    case 2:
                        LATGCLR = (1 << 14);    // clear MEMORY
                        LATFCLR = (1 << 1);     // clear AUDIO
                        LATFSET = (1 << 0);     // set USB
                        break;
                        
                    case 3:
                        LATGCLR = (1 << 14);    // clear MEMORY
                        LATFCLR = (1 << 1);     // clear AUDIO
                        LATFCLR = (1 << 0);     // clear USB
                        break;
                }
                
                if (++led_index == 4)
                    led_index = 0;
                
                if (!(PORTB & (1 << 1)))
                {
                    // MUST disable interrupts and dma, as reset timing is critical
                    // use the compiler built-in function to disable interrupts
                    SYS_INT_StatusGetAndDisable();
                    // use the Peripheral Library Function to suspend the DMA
                    PLIB_DMA_SuspendEnable(DMA_ID_0); // only required if you are using DMA
                    // use the System Service Library Function to initiate the reset
                    SYS_RESET_SoftwareReset(); // to perform the reset
                }
            }
        
            break;
        }

        /* TODO: implement your application state machine.*/
        

        /* The default state should never be executed. */
        default:
        {
            /* TODO: Handle error in application's state machine. */
            break;
        }
    }
}

 

/*******************************************************************************
 End of File
 */
