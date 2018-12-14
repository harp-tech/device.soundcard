#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
//
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
//
// ISR(TCD1_OVF_vect, ISR_NAKED)
//
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/
/* DIN0                                                                 */
/************************************************************************/
ISR(PORTB_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* DIN1                                                                 */
/************************************************************************/
ISR(PORTD_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* DIN2                                                                 */
/************************************************************************/
ISR(PORTC_INT0_vect, ISR_NAKED)
{
   reti();
}

/************************************************************************/
/* PIC32_READY                                                          */
/************************************************************************/
#define PIC32_BOOTLOADER_STATE__STANDBY 0
#define PIC32_BOOTLOADER_STATE__START_BOOTLOADER_PROCESS 1
#define PIC32_BOOTLOADER_STATE__WAITING_250MS_FOR_TX_COMPLETE 2
#define PIC32_BOOTLOADER_STATE__WAITING_1SEC_DEBOUNCE 3
#define PIC32_BOOTLOADER_STATE__WAITING_FOR_FINISH 4
uint8_t PIC32_READY_state = 0;

ISR(TCD1_OVF_vect, ISR_NAKED)
{
   if (PIC32_READY_state == PIC32_BOOTLOADER_STATE__WAITING_250MS_FOR_TX_COMPLETE)
   {
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__WAITING_1SEC_DEBOUNCE;
      set_BOOTLOADER_EN;      // Enable buffers and trigger bootloader code on the PIC32
      timer_type1_enable(&TCD1, TIMER_PRESCALER_DIV1024, 31250, INT_LEVEL_LOW);   // 1 second
      reti();
   }
   
   if (PIC32_READY_state == PIC32_BOOTLOADER_STATE__WAITING_1SEC_DEBOUNCE)
   {
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__WAITING_FOR_FINISH;
      timer_type1_stop(&TCD1);
      reti();
   }
   
   reti();
}

ISR(PORTD_INT1_vect, ISR_NAKED)
{
   
   if (PIC32_READY_state == PIC32_BOOTLOADER_STATE__START_BOOTLOADER_PROCESS)
   {
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__WAITING_250MS_FOR_TX_COMPLETE;
      timer_type1_enable(&TCD1, TIMER_PRESCALER_DIV1024, 31250/4, INT_LEVEL_LOW);   // 250 ms
   }
   
   if (PIC32_READY_state == PIC32_BOOTLOADER_STATE__WAITING_FOR_FINISH)
   {
      app_regs.REG_BOOTLOADER = 0;
      clr_BOOTLOADER_EN;
      PIC32_READY_state = PIC32_BOOTLOADER_STATE__STANDBY;
   }
   
   reti();
}