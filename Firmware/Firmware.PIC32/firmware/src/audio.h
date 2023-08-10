#ifndef AUDIO_H
#define	AUDIO_H

#include <stdbool.h>

/* The configuration of the I2S lines are made using Harmony using these settings:
 * -> Pin Number 68: SS4 (out)
 * -> Pin Number 69: SCK4
 * -> Pin Number 70: SDO4
 * -> Pin Number 72: REFCLKO1
 * -> Pin Number 73: REFCLKI1
 */

// MUTE @ RF2 as output
#define cfg_AUDIO_MUTE TRISFCLR = (1 << 2)
#define set_AUDIO_MUTE  LATFSET = (1 << 2)
#define clr_AUDIO_MUTE  LATFCLR = (1 << 2)
#define tgl_AUDIO_MUTE  LATFINV = (1 << 2)

// DAC_!RST @ RF5 as output
#define cfg_AUDIO_RESET TRISFCLR = (1 << 5)
#define set_AUDIO_RESET  LATFSET = (1 << 5)
#define clr_AUDIO_RESET  LATFCLR = (1 << 5)
#define tgl_AUDIO_RESET  LATFINV = (1 << 5)

// !SS1 @ RB8 as output
#define cfg_AUDIO_CS TRISBCLR = (1 << 8)
#define set_AUDIO_CS  LATBSET = (1 << 8)
#define clr_AUDIO_CS  LATBCLR = (1 << 8)
#define tgl_AUDIO_CS  LATBINV = (1 << 8)

// SDO1 @ RB9 as output
#define cfg_AUDIO_SDO TRISBCLR = (1 << 9)
#define set_AUDIO_SDO  LATBSET = (1 << 9)
#define clr_AUDIO_SDO  LATBCLR = (1 << 9)
#define tgl_AUDIO_SDO  LATBINV = (1 << 9)

// SCK1 @ RB10 as output
#define cfg_AUDIO_SCK TRISBCLR = (1 << 10)
#define set_AUDIO_SCK  LATBSET = (1 << 10)
#define clr_AUDIO_SCK  LATBCLR = (1 << 10)
#define tgl_AUDIO_SCK  LATBINV = (1 << 10)





// Register's addresses
#define SPI_AUDIO_ADD_DAC_REG0 0
#define SPI_AUDIO_ADD_DAC_REG1 1
#define SPI_AUDIO_ADD_VOLUME_LEFT 2
#define SPI_AUDIO_ADD_VOLUME_RIGHT 3

// Register's contents
#define POWER_DOWN (1 << 15)

#define MUTE (1 << 14)

#define DATA_FORMAT_PCM (0 << 12)
#define DATA_FORMAT_EXTERNAL_DF (2 << 12)
#define DATA_FORMAT_SACD_SLAVE (2 << 12)
#define DATA_FORMAT_SACD_MASTER (3 << 12)

#define OUTPUT_FORMAT_STEREO (0 << 10)
#define OUTPUT_FORMAT_MONO_LEFT (2 << 10)
#define OUTPUT_FORMAT_MONO_RIGHT (3 << 10)

#define PCM_SAMPLE_RATE_48KHz (0 << 8)
#define PCM_SAMPLE_RATE_96KHz (1 << 8)
#define PCM_SAMPLE_RATE_192KHz (2 << 8)

#define DE_EMPHASIS_CURVE_NONE (0 << 6)
#define DE_EMPHASIS_CURVE_44KHz (1 << 6)
#define DE_EMPHASIS_CURVE_32KHz (2 << 6)
#define DE_EMPHASIS_CURVE_48KHz (3 << 6)

#define PCM_EF_FORMAT_I2S (0 << 4)
#define PCM_EF_FORMAT_RIGHT_JUSTIFIED (1 << 4)
#define PCM_EF_FORMAT_DSP (2 << 4)
#define PCM_EF_FORMAT_LEFT_JUSTIFIED (3 << 4)

#define PCM_EF_WIDTH_24bits (0 << 2)
#define PCM_EF_WIDTH_20bits (1 << 2)
#define PCM_EF_WIDTH_18bits (2 << 2)
#define PCM_EF_WIDTH_16bits (3 << 2)

#define MCLK_mode_256_x_fs (0 << 9)
#define MCLK_mode_512_x_fs (1 << 9)
#define MCLK_mode_768_x_fs (2 << 9)

#define ZERO_FLAG_POLARITY_HIGH (0 << 8)
#define ZERO_FLAG_POLARITY_LOW (1 << 8)

#define SACD_BIT_RATE_64_x_fs (0 << 7)
#define SACD_BIT_RATE_128_x_fs (1 << 7)

#define SACD_MODE_NORMAL (0 << 6)
#define SACD_MODE_PHASE_MODE (1 << 6)

#define SACD_PHASE0 (0 << 4)
#define SACD_PHASE1 (1 << 4)
#define SACD_PHASE2 (2 << 4)
#define SACD_PHASE3 (3 << 4)

#define SACD_BIT_NOT_INVERTED (0 << 3)
#define SACD_BIT_INVERTED (1 << 3)

#define SACD_MCLK_to_BCLK_RISING_EDGE (0 << 2)
#define SACD_MCLK_to_BCLK_FALLING_EDGE (1 << 2)


void initialize_audio_ios(int reset_reason_type);

void update_audio_register(int register_address, int register_content);
void update_audio_volume_dBV(float gain, bool update_left, bool update_right);
void update_audio_volume_int(int att_left, int att_right);
void config_audio_dac (int sample_rate, bool update_internal_clock);


#endif	/* AUDIO_H */