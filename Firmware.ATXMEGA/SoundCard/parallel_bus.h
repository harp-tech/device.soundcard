#ifndef _PARALLEL_BUS_H_
#define _PARALLEL_BUS_H_
#include <avr/io.h>


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
/* Prototypes                                                           */
/************************************************************************/
void par_cmd_start_sound(uint8_t sound_index);
void par_cmd_stop(uint8_t sound_index);


#endif /* _PARALLEL_BUS_H_ */