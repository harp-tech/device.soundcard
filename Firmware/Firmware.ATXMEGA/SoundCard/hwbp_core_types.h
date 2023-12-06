#ifndef _HWBP_CORE_TYPES_H_
#define _HWBP_CORE_TYPES_H_

/************************************************************************/
/* Definition of available types                                        */
/************************************************************************/
/*								 Int  |float | nbytes */
/*									   |      |        */
#define TYPE_U8			(0x00 | 0x00 | 1)
#define TYPE_I8			(0x80 | 0x00 | 1)
#define TYPE_U16			(0x00 | 0x00 | 2)
#define TYPE_I16			(0x80 | 0x00 | 2)
#define TYPE_U32			(0x00 | 0x00 | 4)
#define TYPE_I32			(0x80 | 0x00 | 4)
#define TYPE_U64			(0x00 | 0x00 | 8)
#define TYPE_I64			(0x80 | 0x00 | 8)
#define TYPE_FLOAT		(0x00 | 0x40 | 4)

/************************************************************************/
/* Definition of masks for available types                              */
/************************************************************************/
#define MSK_TYPE_INTEGER			0x80
#define MSK_TYPE_FLOAT				0x40
#define MSK_TYPE_LEN					0x0F

#define MSK_TIMESTAMP_AT_PAYLOAD	0x10

/************************************************************************/
/* Maximum size of a received packet                                    */
/************************************************************************/
#define MAX_PACKET_SIZE				255


#endif /* _HWBP_CORE_TYPES_H_ */