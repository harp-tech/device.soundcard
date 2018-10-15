#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
ifeq "${IGNORE_LOCAL}" "TRUE"
# do not include local makefile. User is passing all local related variables already
else
include Makefile
# Include makefile containing local settings
ifeq "$(wildcard nbproject/Makefile-local-default.mk)" "nbproject/Makefile-local-default.mk"
include nbproject/Makefile-local-default.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=default
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
OUTPUT_SUFFIX=elf
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

ifeq ($(COMPARE_BUILD), true)
COMPARISON_BUILD=-mafrlcsj
else
COMPARISON_BUILD=
endif

ifdef SUB_IMAGE_ADDRESS

else
SUB_IMAGE_ADDRESS_COMMAND=
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S ../src/system_config/default/framework/system/ports/src/sys_ports_static.c ../src/system_config/default/framework/system/reset/src/sys_reset.c ../src/system_config/default/system_init.c ../src/system_config/default/system_interrupt.c ../src/system_config/default/system_exceptions.c ../src/system_config/default/system_tasks.c ../src/app.c ../src/main.c ../src/sounds_allocation.c ../src/memory.c ../src/audio.c ../src/ios.c ../src/delay.c ../src/parallel_bus.c ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ${OBJECTDIR}/_ext/68765530/sys_reset.o ${OBJECTDIR}/_ext/1688732426/system_init.o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ${OBJECTDIR}/_ext/1360937237/app.o ${OBJECTDIR}/_ext/1360937237/main.o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ${OBJECTDIR}/_ext/1360937237/memory.o ${OBJECTDIR}/_ext/1360937237/audio.o ${OBJECTDIR}/_ext/1360937237/ios.o ${OBJECTDIR}/_ext/1360937237/delay.o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o ${OBJECTDIR}/_ext/574987929/drv_tmr.o ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o ${OBJECTDIR}/_ext/1271493987/sys_dma.o ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o ${OBJECTDIR}/_ext/59362878/sys_tmr.o ${OBJECTDIR}/_ext/672064217/usb_device.o ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o
POSSIBLE_DEPFILES=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d ${OBJECTDIR}/_ext/68765530/sys_reset.o.d ${OBJECTDIR}/_ext/1688732426/system_init.o.d ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d ${OBJECTDIR}/_ext/1360937237/app.o.d ${OBJECTDIR}/_ext/1360937237/main.o.d ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d ${OBJECTDIR}/_ext/1360937237/memory.o.d ${OBJECTDIR}/_ext/1360937237/audio.o.d ${OBJECTDIR}/_ext/1360937237/ios.o.d ${OBJECTDIR}/_ext/1360937237/delay.o.d ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d ${OBJECTDIR}/_ext/574987929/drv_tmr.o.d ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d ${OBJECTDIR}/_ext/1271493987/sys_dma.o.d ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d ${OBJECTDIR}/_ext/59362878/sys_tmr.o.d ${OBJECTDIR}/_ext/672064217/usb_device.o.d ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ${OBJECTDIR}/_ext/68765530/sys_reset.o ${OBJECTDIR}/_ext/1688732426/system_init.o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ${OBJECTDIR}/_ext/1360937237/app.o ${OBJECTDIR}/_ext/1360937237/main.o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ${OBJECTDIR}/_ext/1360937237/memory.o ${OBJECTDIR}/_ext/1360937237/audio.o ${OBJECTDIR}/_ext/1360937237/ios.o ${OBJECTDIR}/_ext/1360937237/delay.o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o ${OBJECTDIR}/_ext/574987929/drv_tmr.o ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o ${OBJECTDIR}/_ext/1271493987/sys_dma.o ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o ${OBJECTDIR}/_ext/59362878/sys_tmr.o ${OBJECTDIR}/_ext/672064217/usb_device.o ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o

# Source Files
SOURCEFILES=../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S ../src/system_config/default/framework/system/ports/src/sys_ports_static.c ../src/system_config/default/framework/system/reset/src/sys_reset.c ../src/system_config/default/system_init.c ../src/system_config/default/system_interrupt.c ../src/system_config/default/system_exceptions.c ../src/system_config/default/system_tasks.c ../src/app.c ../src/main.c ../src/sounds_allocation.c ../src/memory.c ../src/audio.c ../src/ios.c ../src/delay.c ../src/parallel_bus.c ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c


CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
# fixDeps replaces a bunch of sed/cat/printf statements that slow down the build
FIXDEPS=fixDeps

.build-conf:  ${BUILD_SUBPROJECTS}
ifneq ($(INFORMATION_MESSAGE), )
	@echo $(INFORMATION_MESSAGE)
endif
	${MAKE}  -f nbproject/Makefile-default.mk dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=32MZ2048EFM100
MP_LINKER_FILE_OPTION=
# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assembleWithPreprocess
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.ok ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.err 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d" "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d" -t $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC} $(MP_EXTRA_AS_PRE)  -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1 -c -mprocessor=$(MP_PROCESSOR_OPTION)  -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d"  -o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  -Wa,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_AS_POST),-MD="${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d",--defsym=__ICD2RAM=1,--defsym=__MPLAB_DEBUG=1,--gdwarf-2,--defsym=__DEBUG=1,--defsym=__MPLAB_DEBUGGER_ICD4=1
	
else
${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.ok ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.err 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d" "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d" -t $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC} $(MP_EXTRA_AS_PRE)  -c -mprocessor=$(MP_PROCESSOR_OPTION)  -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d"  -o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  -Wa,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_AS_POST),-MD="${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d",--gdwarf-2
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o: ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/639803181" 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" -o ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/340578644/sys_devcon.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ../src/system_config/default/framework/system/devcon/src/sys_devcon.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/822048611/sys_ports_static.o: ../src/system_config/default/framework/system/ports/src/sys_ports_static.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/822048611" 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" -o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ../src/system_config/default/framework/system/ports/src/sys_ports_static.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/68765530/sys_reset.o: ../src/system_config/default/framework/system/reset/src/sys_reset.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/68765530" 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o.d 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" -o ${OBJECTDIR}/_ext/68765530/sys_reset.o ../src/system_config/default/framework/system/reset/src/sys_reset.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_init.o: ../src/system_config/default/system_init.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_init.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_init.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_init.o ../src/system_config/default/system_init.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_interrupt.o: ../src/system_config/default/system_interrupt.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ../src/system_config/default/system_interrupt.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_exceptions.o: ../src/system_config/default/system_exceptions.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ../src/system_config/default/system_exceptions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_tasks.o: ../src/system_config/default/system_tasks.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ../src/system_config/default/system_tasks.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/app.o: ../src/app.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/app.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/app.o.d" -o ${OBJECTDIR}/_ext/1360937237/app.o ../src/app.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/main.o: ../src/main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/main.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/main.o.d" -o ${OBJECTDIR}/_ext/1360937237/main.o ../src/main.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/sounds_allocation.o: ../src/sounds_allocation.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" -o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ../src/sounds_allocation.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/memory.o: ../src/memory.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/memory.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/memory.o.d" -o ${OBJECTDIR}/_ext/1360937237/memory.o ../src/memory.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/audio.o: ../src/audio.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/audio.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/audio.o.d" -o ${OBJECTDIR}/_ext/1360937237/audio.o ../src/audio.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/ios.o: ../src/ios.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/ios.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/ios.o.d" -o ${OBJECTDIR}/_ext/1360937237/ios.o ../src/ios.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/delay.o: ../src/delay.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/delay.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/delay.o.d" -o ${OBJECTDIR}/_ext/1360937237/delay.o ../src/delay.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/parallel_bus.o: ../src/parallel_bus.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" -o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ../src/parallel_bus.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/508403848" 
	@${RM} ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d" -o ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/574987929/drv_tmr.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/574987929" 
	@${RM} ${OBJECTDIR}/_ext/574987929/drv_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/574987929/drv_tmr.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/574987929/drv_tmr.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/574987929/drv_tmr.o.d" -o ${OBJECTDIR}/_ext/574987929/drv_tmr.o ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1511061074/drv_usbhs.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1511061074" 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d" -o ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1511061074" 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d" -o ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1271493987/sys_dma.o: ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1271493987" 
	@${RM} ${OBJECTDIR}/_ext/1271493987/sys_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/1271493987/sys_dma.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1271493987/sys_dma.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1271493987/sys_dma.o.d" -o ${OBJECTDIR}/_ext/1271493987/sys_dma.o ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o: ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1082766828" 
	@${RM} ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d 
	@${RM} ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d" -o ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/59362878/sys_tmr.o: ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/59362878" 
	@${RM} ${OBJECTDIR}/_ext/59362878/sys_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/59362878/sys_tmr.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/59362878/sys_tmr.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/59362878/sys_tmr.o.d" -o ${OBJECTDIR}/_ext/59362878/sys_tmr.o ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/672064217/usb_device.o: ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/672064217" 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/672064217/usb_device.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/672064217/usb_device.o.d" -o ${OBJECTDIR}/_ext/672064217/usb_device.o ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o: ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/672064217" 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d" -o ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
else
${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o: ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/639803181" 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" -o ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/340578644/sys_devcon.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ../src/system_config/default/framework/system/devcon/src/sys_devcon.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/822048611/sys_ports_static.o: ../src/system_config/default/framework/system/ports/src/sys_ports_static.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/822048611" 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" -o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ../src/system_config/default/framework/system/ports/src/sys_ports_static.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/68765530/sys_reset.o: ../src/system_config/default/framework/system/reset/src/sys_reset.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/68765530" 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o.d 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" -o ${OBJECTDIR}/_ext/68765530/sys_reset.o ../src/system_config/default/framework/system/reset/src/sys_reset.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_init.o: ../src/system_config/default/system_init.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_init.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_init.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_init.o ../src/system_config/default/system_init.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_interrupt.o: ../src/system_config/default/system_interrupt.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ../src/system_config/default/system_interrupt.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_exceptions.o: ../src/system_config/default/system_exceptions.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ../src/system_config/default/system_exceptions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1688732426/system_tasks.o: ../src/system_config/default/system_tasks.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ../src/system_config/default/system_tasks.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/app.o: ../src/app.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/app.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/app.o.d" -o ${OBJECTDIR}/_ext/1360937237/app.o ../src/app.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/main.o: ../src/main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/main.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/main.o.d" -o ${OBJECTDIR}/_ext/1360937237/main.o ../src/main.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/sounds_allocation.o: ../src/sounds_allocation.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" -o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ../src/sounds_allocation.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/memory.o: ../src/memory.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/memory.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/memory.o.d" -o ${OBJECTDIR}/_ext/1360937237/memory.o ../src/memory.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/audio.o: ../src/audio.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/audio.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/audio.o.d" -o ${OBJECTDIR}/_ext/1360937237/audio.o ../src/audio.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/ios.o: ../src/ios.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/ios.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/ios.o.d" -o ${OBJECTDIR}/_ext/1360937237/ios.o ../src/ios.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/delay.o: ../src/delay.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/delay.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/delay.o.d" -o ${OBJECTDIR}/_ext/1360937237/delay.o ../src/delay.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1360937237/parallel_bus.o: ../src/parallel_bus.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" -o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ../src/parallel_bus.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/508403848" 
	@${RM} ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o.d" -o ${OBJECTDIR}/_ext/508403848/drv_i2s_dma.o ../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/574987929/drv_tmr.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/574987929" 
	@${RM} ${OBJECTDIR}/_ext/574987929/drv_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/574987929/drv_tmr.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/574987929/drv_tmr.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/574987929/drv_tmr.o.d" -o ${OBJECTDIR}/_ext/574987929/drv_tmr.o ../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1511061074/drv_usbhs.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1511061074" 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1511061074/drv_usbhs.o.d" -o ${OBJECTDIR}/_ext/1511061074/drv_usbhs.o ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o: ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1511061074" 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o.d" -o ${OBJECTDIR}/_ext/1511061074/drv_usbhs_device.o ../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1271493987/sys_dma.o: ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1271493987" 
	@${RM} ${OBJECTDIR}/_ext/1271493987/sys_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/1271493987/sys_dma.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1271493987/sys_dma.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1271493987/sys_dma.o.d" -o ${OBJECTDIR}/_ext/1271493987/sys_dma.o ../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o: ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/1082766828" 
	@${RM} ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d 
	@${RM} ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o.d" -o ${OBJECTDIR}/_ext/1082766828/sys_int_pic32.o ../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/59362878/sys_tmr.o: ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/59362878" 
	@${RM} ${OBJECTDIR}/_ext/59362878/sys_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/59362878/sys_tmr.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/59362878/sys_tmr.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/59362878/sys_tmr.o.d" -o ${OBJECTDIR}/_ext/59362878/sys_tmr.o ../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/672064217/usb_device.o: ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/672064217" 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/672064217/usb_device.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/672064217/usb_device.o.d" -o ${OBJECTDIR}/_ext/672064217/usb_device.o ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o: ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/_ext/672064217" 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d 
	@${RM} ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o 
	@${FIXDEPS} "${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d" $(SILENT) -rsi ${MP_CC_DIR}../  -c ${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O1 -I"../../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../src/system_config/default/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -MMD -MF "${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o.d" -o ${OBJECTDIR}/_ext/672064217/usb_device_endpoint_functions.o ../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD) 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compileCPP
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk  ../../../../../../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a  
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -mdebugger -D__MPLAB_DEBUGGER_ICD4=1 -mprocessor=$(MP_PROCESSOR_OPTION)  -o dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}    ..\..\..\..\..\..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a      -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)   -mreserve=data@0x0:0x37F   -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1,-D=__DEBUG_D,--defsym=__MPLAB_DEBUGGER_ICD4=1,--defsym=_min_heap_size=0,--gc-sections,--no-code-in-dinit,--no-dinit-in-serial-mem,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--memorysummary,dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml
	
else
dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk  ../../../../../../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a 
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -mprocessor=$(MP_PROCESSOR_OPTION)  -o dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}    ..\..\..\..\..\..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a      -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=_min_heap_size=0,--gc-sections,--no-code-in-dinit,--no-dinit-in-serial-mem,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--memorysummary,dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml
	${MP_CC_DIR}\\xc32-bin2hex dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} 
endif


# Subprojects
.build-subprojects:


# Subprojects
.clean-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r build/default
	${RM} -r dist/default

# Enable dependency checking
.dep.inc: .depcheck-impl

DEPFILES=$(shell mplabwildcard ${POSSIBLE_DEPFILES})
ifneq (${DEPFILES},)
include ${DEPFILES}
endif
