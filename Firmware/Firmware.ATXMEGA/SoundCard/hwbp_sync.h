#ifndef _HWBP_SYNC_H_
#define _HWBP_SYNC_H_
#include <avr/io.h>


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void initialize_timestamp_uart (uint32_t * timestamp_pointer);
void reset_sync_counter (void);
void trigger_sync_timer (void);


/************************************************************************/
/* Reset sync counter                                                   */
/************************************************************************/
#if defined(__AVR_ATxmega16A4U__)
    #define RESET_TIMESTAMP_COUNTER reset_sync_counter()
#else
    #define RESET_TIMESTAMP_COUNTER
#endif


/************************************************************************/
/* Trigger timer                                                        */
/************************************************************************/
#if defined(__AVR_ATxmega16A4U__)
    #define SYNC_TRIGGER_TIMER trigger_sync_timer()
#else
    #define SYNC_TRIGGER_TIMER
#endif


/************************************************************************/
/* Control when the device lost sync                                    */
/************************************************************************/
#if defined(__AVR_ATxmega16A4U__)
    #define INCREASE_LOST_SYNC_COUNTER
#else
    #define INCREASE_LOST_SYNC_COUNTER device_lost_sync_counter++
#endif


#endif /* _HWBP_SYNC_H_ */