#ifndef _BPOD_CMDS_
#define _BPOD_CMDS_
#include <avr/io.h>

/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void load_device_name_to_bpod_cmds (uint8_t fwH, uint8_t device_name_in[], uint8_t name_length);

#endif /* _BPOD_CMDS_ */