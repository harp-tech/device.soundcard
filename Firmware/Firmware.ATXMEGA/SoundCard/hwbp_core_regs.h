#ifndef _HWBP_CORE_REGS_H_
#define _HWBP_CORE_REGS_H_


/************************************************************************/
/* Common Bank Registers                                                */
/************************************************************************/
/* Registers */
#define ADD_R_WHO_AM_I          0x00    // U16
#define ADD_R_HW_VERSION_H      0x01    // U8
#define ADD_R_HW_VERSION_L      0x02    // U8
#define ADD_R_ASSEMBLY_VERSION  0x03    // U8
#define ADD_R_CORE_VERSION_H    0x04    // U8
#define ADD_R_CORE_VERSION_L    0x05    // U8
#define ADD_R_FW_VERSION_H      0x06    // U8
#define ADD_R_FW_VERSION_L      0x07    // U8
#define ADD_R_TIMESTAMP_SECOND  0x08    // U32
#define ADD_R_TIMESTAMP_MICRO   0x09    // U16
#define ADD_R_OPERATION_CTRL    0x0A    // U8
#define ADD_R_RESET_DEV         0x0B    // U8
#define ADD_R_DEVICE_NAME       0x0C    // U8
#define ADD_R_SERIAL_NUMBER     0x0D    // U16
#define ADD_R_CONFIG            0x0E    // U8
#define ADD_R_TIMESTAMP_OFFSET  0x0F    // U8

/* Memory limits */
#define COMMON_BANK_ADD_MAX             0x0F
#define COMMON_BANK_ABSOLUTE_ADD_MAX    0x1C

/* R_OPERATION_CTRL */
#define MSK_OP_MODE	        (3<<0)

#define GM_OP_MODE_STANDBY  (0<<0)
#define GM_OP_MODE_ACTIVE   (1<<0)
#define GM_OP_MODE_SPEED    (3<<0)

#define B_DUMP              (1<<3)
#define B_MUTE_RPL          (1<<4)
#define B_VISUALEN          (1<<5)
#define B_OPLEDEN           (1<<6)
#define B_ALIVE_EN          (1<<7)

/* ADD_R_MEMORY */
#define B_RST_DEF           (1<<0)
#define B_RST_EE            (1<<1)

#define B_SAVE              (1<<2)

#define B_NAME_TO_DEFAULT   (1<<3)

#define B_BOOT_DEF          (1<<6)
#define B_BOOT_EE           (1<<7)

/* ADD_R_CONFIG */
#define B_CLK_REP           (1<<0)
#define B_CLK_GEN           (1<<1)
#define B_CLK_SAVE          (1<<2)
#define B_REP_ABLE          (1<<3)
#define B_GEN_ABLE          (1<<4)
#define B_CLK_UNLOCK        (1<<6)
#define B_CLK_LOCK          (1<<7)


#endif /* _HWBP_CORE_REGS_H_ */