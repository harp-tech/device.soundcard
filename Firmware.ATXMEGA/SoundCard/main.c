#include <avr/io.h>

#include "app.h"
#include "app_ios_and_regs.h"

int main(void)
{
	/* Initialize device */
	hwbp_app_initialize();

	/* Enable interrupts */
	hwbp_app_enable_interrupts;
	
	/* Infinite loop */
	while(1);
		//__asm volatile("sleep");
}