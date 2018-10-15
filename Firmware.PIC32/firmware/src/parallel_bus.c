#include <xc.h>
#include <stdbool.h>
#include "ios.h"
#include "parallel_bus.h"

void initialize_par_ios(void)
{
    cfg_PAR_BUS;
    cfg_PAR_CMD_WRITE;
    cfg_PAR_CMD_LATCH;
    cfg_PAR_CMD_ERROR;
    
    clr_PAR_CMD_LATCH;
    clr_PAR_CMD_ERROR;
}

bool par_bus_previous_CMD_WRITE = false;

int par_bus_check_for_command(void)
{    
    if (read_PAR_CMD_WRITE)
    {
        if (!par_bus_previous_CMD_WRITE)
        {
            par_bus_previous_CMD_WRITE = true;
            
            int cmd_received = read_PAR_BUS;
            set_PAR_CMD_LATCH;
            return cmd_received;
        }
    }
    else
    {
        if (par_bus_previous_CMD_WRITE)
        {
            par_bus_previous_CMD_WRITE = false;
            
            clr_PAR_CMD_LATCH;
            return -1;
        }
    }
    
    return -1;
}