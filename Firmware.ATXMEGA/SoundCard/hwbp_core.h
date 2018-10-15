#ifndef _HWBP_CORE_H_
#define _HWBP_CORE_H_
#include <avr/io.h>



// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif

typedef struct {
	uint32_t second;
	uint16_t usecond;
} timestamp_t;

// It's the first callback used, right after booting the core.
// The pins, ports and external hardware should be initialized.
void core_callback_1st_config_hw_after_boot(void);

// Used to initialize the registers.
// All registers should be written to their default state.
void core_callback_reset_registers(void);

// Call right after load the register with default or EEPROM values.
// The state registers and external hardware should be configured according the value of the registers.
void core_callback_registers_were_reinitialized(void);

// Called when the application must turn off all the visual indicators.
void core_callback_visualen_to_on(void);
// Called when the application can turn on the visual indicators.
void core_callback_visualen_to_off(void);

// When the device goes to Standby Mode.
void core_callback_device_to_standby(void);
// When the device goes to Active Mode.
void core_callback_device_to_active(void);
// When the device goes to Speed Mode.
void core_callback_device_to_speed(void);



// Called before execute the timer interrupts
void core_callback_t_before_exec(void);
// Called after execute the timer interrupts
void core_callback_t_after_exec(void);

// Called every millisecond.
void core_callback_t_1ms(void);
// Called 500 microseconds after the "void core_callback_t_1ms(void)".
void core_callback_t_500us(void);
// Called every time a new second starts.
void core_callback_t_new_second(void);



// Read from an application register.
bool core_read_app_register(uint8_t add, uint8_t type);
// Write to an application register.
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements);
// Read from a common register.
bool hwbp_read_common_reg(uint8_t add, uint8_t type);
// Write to an common register.
bool hwbp_write_common_reg(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements);




// It is mandatory that this function is the first of the application code.
void core_func_start_core (
    const uint16_t who_am_i,
    const uint8_t hwH,
    const uint8_t hwL,
    const uint8_t fwH,
    const uint8_t fwL,
    const uint8_t assembly,
    uint8_t *pointer_to_app_regs,
    const uint16_t app_mem_size_to_save,
    const uint8_t num_of_app_registers,
    const uint8_t *device_name);

// Call this function in case of error
// A power up or reset must be performed to remove the device from this state
void core_func_catastrophic_error_detected(void);
// When a catastrophic error is detected (last callback before go into error state)
// User should shutdown all the peripherals
void core_callback_catastrophic_error_detected(void);

// Used to leave the speed mode.
void core_func_leave_speed_mode_and_go_to_standby_mode(void);


// Used to create a specific timestamp that can be used when sending Events
void core_func_update_user_timestamp(uint32_t seconds, uint16_t useconds);

// Used to read the current user timestamps
void core_func_read_user_timestamp(uint32_t *seconds, uint16_t *useconds);

// Used to save the current timestamp
void core_func_mark_user_timestamp(void);

// Used to send an Event
void core_func_send_event(uint8_t add, bool use_core_timestamp);

// Used to get the content of register R_TIMESTAMP_SECOND.
uint32_t core_func_read_R_TIMESTAMP_SECOND(void);
// Used to get the content of register R_TIMESTAMP_MICRO.
uint16_t core_func_read_R_TIMESTAMP_MICRO(void);



// Return "true" if the LEDs can be ON.
bool core_bool_is_visual_enabled(void);
// Return "true" if the device is in Speed Mode.
bool core_bool_speed_mode_is_in_use(void);



#endif /* _HWBP_CORE_H_ */