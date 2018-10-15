#include <xc.h>
#include <stdbool.h>
#include <math.h>
#include "audio.h"
#include "delay.h"
#include "peripheral/osc/plib_osc.h"


#define RESET_FROM_SOFTWARE 64

/*
 * Initializes the uC digital pins.
 */
void initialize_audio_ios(int reset_reason_type)
{    
    cfg_AUDIO_MUTE;
    cfg_AUDIO_CS;
    cfg_AUDIO_SDO;
    cfg_AUDIO_SCK;
    
    set_AUDIO_CS;               // Un-select audio device's SPI bus
    clr_AUDIO_SCK;              // Leave SCK at low level
    clr_AUDIO_SDO;              // Leave SDO at low level
    
    if (reset_reason_type != RESET_FROM_SOFTWARE)
    {
        cfg_AUDIO_RESET;
        
        clr_AUDIO_RESET;        // Issue a power down / reset
        _ms_delay(100);         // Wait 100 ms
        set_AUDIO_RESET;        // Remove power down / reset
        _ms_delay(200);         // Wait 200 ms
        clr_AUDIO_MUTE;         // Remove mute
    }
    else
    {
        set_AUDIO_RESET;        // Remove power down / reset
        cfg_AUDIO_RESET;
        clr_AUDIO_MUTE;         // Remove mute
    }
}

#define AUDIO_SCK_PULSE for (i = 0; i < 10; i++) set_AUDIO_SCK; for (i = 0; i < 10; i++) clr_AUDIO_SCK;
void update_audio_register(int register_address, int register_content)
{
    int i;
    
    for (i = 0; i < 10; i++)
        clr_AUDIO_CS;
    
    if (register_content & (1 << 15)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 << 14)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 << 13)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 << 12)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    
    if (register_content & (1 << 11)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 << 10)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  9)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  8)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;    
    
    if (register_content & (1 <<  7)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  6)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  5)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  4)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;    
    
    if (register_content & (1 <<  3)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_content & (1 <<  2)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;    
    
    if (register_address & (1 <<  1)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    if (register_address & (1 <<  0)) set_AUDIO_SDO; else clr_AUDIO_SDO; AUDIO_SCK_PULSE;
    
    for (i = 0; i < 10; i++)
        set_AUDIO_CS;
    
    clr_AUDIO_SDO;
}

/*
 * Sets the audio attenuation.
 * 12.7 us @ 200MHz
 */
void update_audio_volume_dBV(float gain, bool update_left, bool update_right)
{
    if (gain > 0)
        return;

    int volume = pow(10, gain / 20) * (pow(2, 14) - 1);    
    int reg_content = volume << 2;
    
    if (update_left)
        update_audio_register(2, reg_content);
    
    if (update_right)
        update_audio_register(3, reg_content);
}

/* 
 * Configure audio DAC IC.
 * Configure the data format, the data rate and number of bits.
 */
void config_audio_dac (int sample_rate)
{
    if (sample_rate == 96000)
    {
        int reg_content = DATA_FORMAT_PCM | OUTPUT_FORMAT_STEREO | PCM_SAMPLE_RATE_96KHz | DE_EMPHASIS_CURVE_NONE | PCM_EF_FORMAT_I2S | PCM_EF_WIDTH_24bits;
        update_audio_register(0, reg_content);
        reg_content = MCLK_mode_256_x_fs;
        update_audio_register(1, reg_content);
        
        SYS_DEVCON_SystemUnlock ( );        
        REFO1CONbits.ACTIVE = 0;
        REFO1CONbits.ON = 0;
        
        /* RODIV */
        PLIB_OSC_ReferenceOscDivisorValueSet ( OSC_ID_0, OSC_REFERENCE_1, 1);
                
        REFO1CONbits.ACTIVE = 1;
        REFO1CONbits.ON = 1;
        SYS_DEVCON_SystemLock ( );
    }
    else
    {
        int reg_content = DATA_FORMAT_PCM | OUTPUT_FORMAT_STEREO | PCM_SAMPLE_RATE_192KHz | DE_EMPHASIS_CURVE_NONE | PCM_EF_FORMAT_I2S | PCM_EF_WIDTH_24bits;
        update_audio_register(0, reg_content);
        reg_content = MCLK_mode_512_x_fs;
        update_audio_register(1, reg_content);
        
        SYS_DEVCON_SystemUnlock ( );      
        REFO1CONbits.ACTIVE = 0;
        REFO1CONbits.ON = 0;
        
        /* RODIV */
        PLIB_OSC_ReferenceOscDivisorValueSet ( OSC_ID_0, OSC_REFERENCE_1, 0);
        
        REFO1CONbits.ACTIVE = 1;
        REFO1CONbits.ON = 1;
        SYS_DEVCON_SystemLock ( );
    }
}