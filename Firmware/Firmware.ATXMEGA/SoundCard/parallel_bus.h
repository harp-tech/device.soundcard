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
void par_cmd_stop(void);
bool par_cmd_stop_callback (void);

void par_cmd_start_sound(uint16_t sound_index, int16_t amplitude_left, int16_t amplitude_right);
bool par_cmd_start_sound_callback (void);

void par_cmd_delete_sound(uint8_t sound_index, bool delete_all);
bool par_cmd_delete_sound_callback (void);

void par_cmd_update_frequency(uint16_t sound_index);
bool par_cmd_update_frequency_callback (void);

void par_cmd_update_amplitude(int16_t amplitude_left, int16_t amplitude_right);
bool par_cmd_update_amplitude_callback (void);

void par_cmd_update_amplitude_left(int16_t amplitude_left);
bool par_cmd_update_amplitude_left_callback (void);

void par_cmd_update_amplitude_right(int16_t amplitude_right);
bool par_cmd_update_amplitude_right_callback (void);




#endif /* _PARALLEL_BUS_H_ */