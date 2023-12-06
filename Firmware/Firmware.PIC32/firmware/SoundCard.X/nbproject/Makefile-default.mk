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
SOURCEFILES_QUOTED_IF_SPACED=../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S ../src/system_config/default/framework/system/ports/src/sys_ports_static.c ../src/system_config/default/framework/system/reset/src/sys_reset.c ../src/system_config/default/system_init.c ../src/system_config/default/system_interrupt.c ../src/system_config/default/system_exceptions.c ../src/system_config/default/system_tasks.c ../src/app.c ../src/main.c ../src/sounds_allocation.c ../src/memory.c ../src/audio.c ../src/ios.c ../src/delay.c ../src/parallel_bus.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ${OBJECTDIR}/_ext/68765530/sys_reset.o ${OBJECTDIR}/_ext/1688732426/system_init.o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ${OBJECTDIR}/_ext/1360937237/app.o ${OBJECTDIR}/_ext/1360937237/main.o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ${OBJECTDIR}/_ext/1360937237/memory.o ${OBJECTDIR}/_ext/1360937237/audio.o ${OBJECTDIR}/_ext/1360937237/ios.o ${OBJECTDIR}/_ext/1360937237/delay.o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o ${OBJECTDIR}/_ext/432949496/drv_tmr.o ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o ${OBJECTDIR}/_ext/1156607186/sys_dma.o ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o ${OBJECTDIR}/_ext/174249679/sys_tmr.o ${OBJECTDIR}/_ext/61412248/usb_device.o ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o
POSSIBLE_DEPFILES=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d ${OBJECTDIR}/_ext/68765530/sys_reset.o.d ${OBJECTDIR}/_ext/1688732426/system_init.o.d ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d ${OBJECTDIR}/_ext/1360937237/app.o.d ${OBJECTDIR}/_ext/1360937237/main.o.d ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d ${OBJECTDIR}/_ext/1360937237/memory.o.d ${OBJECTDIR}/_ext/1360937237/audio.o.d ${OBJECTDIR}/_ext/1360937237/ios.o.d ${OBJECTDIR}/_ext/1360937237/delay.o.d ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o.d ${OBJECTDIR}/_ext/432949496/drv_tmr.o.d ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o.d ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o.d ${OBJECTDIR}/_ext/1156607186/sys_dma.o.d ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o.d ${OBJECTDIR}/_ext/174249679/sys_tmr.o.d ${OBJECTDIR}/_ext/61412248/usb_device.o.d ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ${OBJECTDIR}/_ext/68765530/sys_reset.o ${OBJECTDIR}/_ext/1688732426/system_init.o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ${OBJECTDIR}/_ext/1360937237/app.o ${OBJECTDIR}/_ext/1360937237/main.o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ${OBJECTDIR}/_ext/1360937237/memory.o ${OBJECTDIR}/_ext/1360937237/audio.o ${OBJECTDIR}/_ext/1360937237/ios.o ${OBJECTDIR}/_ext/1360937237/delay.o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o ${OBJECTDIR}/_ext/432949496/drv_tmr.o ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o ${OBJECTDIR}/_ext/1156607186/sys_dma.o ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o ${OBJECTDIR}/_ext/174249679/sys_tmr.o ${OBJECTDIR}/_ext/61412248/usb_device.o ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o

# Source Files
SOURCEFILES=../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S ../src/system_config/default/framework/system/ports/src/sys_ports_static.c ../src/system_config/default/framework/system/reset/src/sys_reset.c ../src/system_config/default/system_init.c ../src/system_config/default/system_interrupt.c ../src/system_config/default/system_exceptions.c ../src/system_config/default/system_tasks.c ../src/app.c ../src/main.c ../src/sounds_allocation.c ../src/memory.c ../src/audio.c ../src/ios.c ../src/delay.c ../src/parallel_bus.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c



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
MP_LINKER_FILE_OPTION=,--script="..\src\system_config\default\app_mz.ld"
# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assembleWithPreprocess
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  .generated_files/flags/default/f125c4375bad883ddd53d12f8832b69886309d8c .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.ok ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.err 
	${MP_CC} $(MP_EXTRA_AS_PRE)  -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1 -c -mprocessor=$(MP_PROCESSOR_OPTION)  -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d"  -o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  -Wa,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_AS_POST),-MD="${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d",--defsym=__ICD2RAM=1,--defsym=__MPLAB_DEBUG=1,--gdwarf-2,--defsym=__DEBUG=1,--defsym=__MPLAB_DEBUGGER_ICD4=1 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d" "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d" -t $(SILENT) -rsi ${MP_CC_DIR}../ 
	
else
${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  .generated_files/flags/default/91d8c560b3be21969e5b97eb0245b3daea84dadc .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.ok ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.err 
	${MP_CC} $(MP_EXTRA_AS_PRE)  -c -mprocessor=$(MP_PROCESSOR_OPTION)  -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d"  -o ${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_cache_pic32mz.S  -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  -Wa,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_AS_POST),-MD="${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d",--gdwarf-2 
	@${FIXDEPS} "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.d" "${OBJECTDIR}/_ext/340578644/sys_devcon_cache_pic32mz.o.asm.d" -t $(SILENT) -rsi ${MP_CC_DIR}../ 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o: ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c  .generated_files/flags/default/82817633159a0915d6e10ecc58c47c632b2ac172 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/639803181" 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" -o ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/340578644/sys_devcon.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon.c  .generated_files/flags/default/90986d1cdce492ed9a3e6cee32c43b4e3e4453c8 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ../src/system_config/default/framework/system/devcon/src/sys_devcon.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c  .generated_files/flags/default/8d575f3cabe15948e5fed1b8fd7e5818d4e2f07a .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/822048611/sys_ports_static.o: ../src/system_config/default/framework/system/ports/src/sys_ports_static.c  .generated_files/flags/default/5b486c578a7697524249f420f49798b8470c4afa .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/822048611" 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" -o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ../src/system_config/default/framework/system/ports/src/sys_ports_static.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/68765530/sys_reset.o: ../src/system_config/default/framework/system/reset/src/sys_reset.c  .generated_files/flags/default/641c82160b3c247ef9e0e35d4242b0616629cad4 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/68765530" 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o.d 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" -o ${OBJECTDIR}/_ext/68765530/sys_reset.o ../src/system_config/default/framework/system/reset/src/sys_reset.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_init.o: ../src/system_config/default/system_init.c  .generated_files/flags/default/172c1c9af57fb9272e7369513e3bf0dcc297b2c8 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_init.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_init.o ../src/system_config/default/system_init.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_interrupt.o: ../src/system_config/default/system_interrupt.c  .generated_files/flags/default/549e5af45376813c84e5844615ba718413cf4abf .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ../src/system_config/default/system_interrupt.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_exceptions.o: ../src/system_config/default/system_exceptions.c  .generated_files/flags/default/5c5c395960bba6b0d44b5a34982713824044f3e4 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ../src/system_config/default/system_exceptions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_tasks.o: ../src/system_config/default/system_tasks.c  .generated_files/flags/default/d476b5d3992340ac522f36c16ca870de19059885 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ../src/system_config/default/system_tasks.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/app.o: ../src/app.c  .generated_files/flags/default/8252512c35ee19bb97b2c9463382d4803fcb43f4 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/app.o.d" -o ${OBJECTDIR}/_ext/1360937237/app.o ../src/app.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/main.o: ../src/main.c  .generated_files/flags/default/a8fc00d7dc308cc2c108e3ff402d2626f205e1d9 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/main.o.d" -o ${OBJECTDIR}/_ext/1360937237/main.o ../src/main.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/sounds_allocation.o: ../src/sounds_allocation.c  .generated_files/flags/default/b52a1af31f39873a25c1a077ea7100e4d6a991c3 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" -o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ../src/sounds_allocation.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/memory.o: ../src/memory.c  .generated_files/flags/default/65748de32a4999e2feab5ee0531068b25b833793 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/memory.o.d" -o ${OBJECTDIR}/_ext/1360937237/memory.o ../src/memory.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/audio.o: ../src/audio.c  .generated_files/flags/default/22675cec9615ced574a89399cce785daa5a10e3b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/audio.o.d" -o ${OBJECTDIR}/_ext/1360937237/audio.o ../src/audio.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/ios.o: ../src/ios.c  .generated_files/flags/default/8a4b3535b8912857b7d8b6874512fb25b5728a5b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/ios.o.d" -o ${OBJECTDIR}/_ext/1360937237/ios.o ../src/ios.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/delay.o: ../src/delay.c  .generated_files/flags/default/8d8c13a665c99c9cd6747817451143ae296a4290 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/delay.o.d" -o ${OBJECTDIR}/_ext/1360937237/delay.o ../src/delay.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/parallel_bus.o: ../src/parallel_bus.c  .generated_files/flags/default/8dc550692cd92d464d15ada6c3628ed05dc90528 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" -o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ../src/parallel_bus.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c  .generated_files/flags/default/32779fa55a051fb8c093c1287ab11b7350a44922 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/499533577" 
	@${RM} ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o.d" -o ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/432949496/drv_tmr.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c  .generated_files/flags/default/c00d0c8234efb6d6a0819361692f326c7082aa33 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/432949496" 
	@${RM} ${OBJECTDIR}/_ext/432949496/drv_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/432949496/drv_tmr.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/432949496/drv_tmr.o.d" -o ${OBJECTDIR}/_ext/432949496/drv_tmr.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1312608989/drv_usbhs.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c  .generated_files/flags/default/1dd3e703e45c15705b1a09495a1e38c14145835b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1312608989" 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o.d 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1312608989/drv_usbhs.o.d" -o ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c  .generated_files/flags/default/6818c29e90daa4e4d5930454ab605ef9da90e955 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1312608989" 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o.d" -o ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1156607186/sys_dma.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c  .generated_files/flags/default/ff652d4ff3a19d25eaca7c56692a62b4d4994596 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1156607186" 
	@${RM} ${OBJECTDIR}/_ext/1156607186/sys_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/1156607186/sys_dma.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1156607186/sys_dma.o.d" -o ${OBJECTDIR}/_ext/1156607186/sys_dma.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/967880027/sys_int_pic32.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c  .generated_files/flags/default/5766c4e401324dd682ff7e0a0ad775857e30005e .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/967880027" 
	@${RM} ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o.d 
	@${RM} ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/967880027/sys_int_pic32.o.d" -o ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/174249679/sys_tmr.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c  .generated_files/flags/default/18c0e07bfe96dcc44a3456e484ee0c8feccdee2e .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/174249679" 
	@${RM} ${OBJECTDIR}/_ext/174249679/sys_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/174249679/sys_tmr.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/174249679/sys_tmr.o.d" -o ${OBJECTDIR}/_ext/174249679/sys_tmr.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/61412248/usb_device.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c  .generated_files/flags/default/bbd2c08ef91fd5be4fb883ea4df7a15cc72c16bb .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/61412248" 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/61412248/usb_device.o.d" -o ${OBJECTDIR}/_ext/61412248/usb_device.o ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c  .generated_files/flags/default/98e2f7da6a434107e15081221e235478d3055507 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/61412248" 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o.d 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_ICD4=1  -fframe-base-loclist  -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o.d" -o ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
else
${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o: ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c  .generated_files/flags/default/1d5f95d1344e2ed48506d652b9cdf2f6e5e7b89a .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/639803181" 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o.d" -o ${OBJECTDIR}/_ext/639803181/sys_clk_pic32mz.o ../src/system_config/default/framework/system/clk/src/sys_clk_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/340578644/sys_devcon.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon.c  .generated_files/flags/default/f68ebb07122db10e0f14b1e89df9fe6eaa866230 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon.o ../src/system_config/default/framework/system/devcon/src/sys_devcon.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o: ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c  .generated_files/flags/default/d4b1cf89733d8875e57e176d010c9c6810c8ede5 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/340578644" 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d 
	@${RM} ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o.d" -o ${OBJECTDIR}/_ext/340578644/sys_devcon_pic32mz.o ../src/system_config/default/framework/system/devcon/src/sys_devcon_pic32mz.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/822048611/sys_ports_static.o: ../src/system_config/default/framework/system/ports/src/sys_ports_static.c  .generated_files/flags/default/157aebac31706f5a0e74904005a810cad132353e .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/822048611" 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d 
	@${RM} ${OBJECTDIR}/_ext/822048611/sys_ports_static.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/822048611/sys_ports_static.o.d" -o ${OBJECTDIR}/_ext/822048611/sys_ports_static.o ../src/system_config/default/framework/system/ports/src/sys_ports_static.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/68765530/sys_reset.o: ../src/system_config/default/framework/system/reset/src/sys_reset.c  .generated_files/flags/default/f749e14db995cec8fc81838fc43a77ebb716f668 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/68765530" 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o.d 
	@${RM} ${OBJECTDIR}/_ext/68765530/sys_reset.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/68765530/sys_reset.o.d" -o ${OBJECTDIR}/_ext/68765530/sys_reset.o ../src/system_config/default/framework/system/reset/src/sys_reset.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_init.o: ../src/system_config/default/system_init.c  .generated_files/flags/default/d41c988996c719a24a58420965c1ecc013e70b15 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_init.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_init.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_init.o ../src/system_config/default/system_init.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_interrupt.o: ../src/system_config/default/system_interrupt.c  .generated_files/flags/default/771344aa682b5d463e6dd9dfabb59060551f420b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_interrupt.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_interrupt.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_interrupt.o ../src/system_config/default/system_interrupt.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_exceptions.o: ../src/system_config/default/system_exceptions.c  .generated_files/flags/default/e12807786892829474790e7ceb6ffa8e0ac91936 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_exceptions.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_exceptions.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_exceptions.o ../src/system_config/default/system_exceptions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1688732426/system_tasks.o: ../src/system_config/default/system_tasks.c  .generated_files/flags/default/f88b7331a9f2bab625008726def42e51e507b6a5 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1688732426" 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o.d 
	@${RM} ${OBJECTDIR}/_ext/1688732426/system_tasks.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1688732426/system_tasks.o.d" -o ${OBJECTDIR}/_ext/1688732426/system_tasks.o ../src/system_config/default/system_tasks.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/app.o: ../src/app.c  .generated_files/flags/default/913fd2e4f2362c5023854cf71e086ed3dd028d51 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/app.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/app.o.d" -o ${OBJECTDIR}/_ext/1360937237/app.o ../src/app.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/main.o: ../src/main.c  .generated_files/flags/default/39a4685ee9523c2cbd923bcb495c7924421dae95 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/main.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/main.o.d" -o ${OBJECTDIR}/_ext/1360937237/main.o ../src/main.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/sounds_allocation.o: ../src/sounds_allocation.c  .generated_files/flags/default/759e287425c8e098bee6a8710b91304bb9f1258b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/sounds_allocation.o.d" -o ${OBJECTDIR}/_ext/1360937237/sounds_allocation.o ../src/sounds_allocation.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/memory.o: ../src/memory.c  .generated_files/flags/default/2e097118f65af8bc51608e2f789eb6afcc4fb03 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/memory.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/memory.o.d" -o ${OBJECTDIR}/_ext/1360937237/memory.o ../src/memory.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/audio.o: ../src/audio.c  .generated_files/flags/default/3dfb67b0866a38cae8ed064712baf9a7927ce046 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/audio.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/audio.o.d" -o ${OBJECTDIR}/_ext/1360937237/audio.o ../src/audio.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/ios.o: ../src/ios.c  .generated_files/flags/default/e8ac07455de464e4379064e731fdb752aa465409 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/ios.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/ios.o.d" -o ${OBJECTDIR}/_ext/1360937237/ios.o ../src/ios.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/delay.o: ../src/delay.c  .generated_files/flags/default/4180ded545a054056cd72762f5113b8aa5d372d1 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/delay.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/delay.o.d" -o ${OBJECTDIR}/_ext/1360937237/delay.o ../src/delay.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1360937237/parallel_bus.o: ../src/parallel_bus.c  .generated_files/flags/default/1ae4685177c594d26d362aa93e0349b60a57b3e8 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1360937237" 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d 
	@${RM} ${OBJECTDIR}/_ext/1360937237/parallel_bus.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1360937237/parallel_bus.o.d" -o ${OBJECTDIR}/_ext/1360937237/parallel_bus.o ../src/parallel_bus.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c  .generated_files/flags/default/749b4ffec002759da83408199c1bde9eeb83bc7a .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/499533577" 
	@${RM} ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o.d" -o ${OBJECTDIR}/_ext/499533577/drv_i2s_dma.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/i2s/src/dynamic/drv_i2s_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/432949496/drv_tmr.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c  .generated_files/flags/default/ef876519e1e35e378fa7d76e90496bd9875e5227 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/432949496" 
	@${RM} ${OBJECTDIR}/_ext/432949496/drv_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/432949496/drv_tmr.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/432949496/drv_tmr.o.d" -o ${OBJECTDIR}/_ext/432949496/drv_tmr.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/tmr/src/dynamic/drv_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1312608989/drv_usbhs.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c  .generated_files/flags/default/4cfe1da55bc56b907e5e06e715fe2c231e24111 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1312608989" 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o.d 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1312608989/drv_usbhs.o.d" -o ${OBJECTDIR}/_ext/1312608989/drv_usbhs.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c  .generated_files/flags/default/bd2d6f6294e22b4ae4e4e8e2daf6dc5a0196f25d .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1312608989" 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o.d" -o ${OBJECTDIR}/_ext/1312608989/drv_usbhs_device.o ../../../../../../../../../../microchip/harmony/v2_04/framework/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/1156607186/sys_dma.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c  .generated_files/flags/default/9e89671b725bb354d6ddda59e3ace64f48636624 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/1156607186" 
	@${RM} ${OBJECTDIR}/_ext/1156607186/sys_dma.o.d 
	@${RM} ${OBJECTDIR}/_ext/1156607186/sys_dma.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/1156607186/sys_dma.o.d" -o ${OBJECTDIR}/_ext/1156607186/sys_dma.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/dma/src/sys_dma.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/967880027/sys_int_pic32.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c  .generated_files/flags/default/cb930a505fd437eba6a45416d067fa5c49b0279b .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/967880027" 
	@${RM} ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o.d 
	@${RM} ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/967880027/sys_int_pic32.o.d" -o ${OBJECTDIR}/_ext/967880027/sys_int_pic32.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/int/src/sys_int_pic32.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/174249679/sys_tmr.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c  .generated_files/flags/default/da0f4824616541d4497d06ef2ae782279862589e .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/174249679" 
	@${RM} ${OBJECTDIR}/_ext/174249679/sys_tmr.o.d 
	@${RM} ${OBJECTDIR}/_ext/174249679/sys_tmr.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/174249679/sys_tmr.o.d" -o ${OBJECTDIR}/_ext/174249679/sys_tmr.o ../../../../../../../../../../microchip/harmony/v2_04/framework/system/tmr/src/sys_tmr.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/61412248/usb_device.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c  .generated_files/flags/default/bfefe9432f1ef3d520d84325da9a60c3a273540 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/61412248" 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device.o.d 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/61412248/usb_device.o.d" -o ${OBJECTDIR}/_ext/61412248/usb_device.o ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o: ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c  .generated_files/flags/default/e408aaa9568325bb1624e90c48805f85425651f0 .generated_files/flags/default/b9a611e7d3f9e13f36a974aabff414fb1e968c
	@${MKDIR} "${OBJECTDIR}/_ext/61412248" 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o.d 
	@${RM} ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o 
	${MP_CC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -O3 -fno-common -I"../../../microchip/harmony/v2_04/framework" -I"../../microchip/harmony/v2_04/framework" -I"../../../../../../../microchip/harmony/v2_04/framework" -I"../src" -I"../src/system_config/default" -I"../src/default" -I"../../../../../../../../../microchip/harmony/v2_04/framework" -I"../src/system_config/default/framework" -I"../../../../../../../../../../microchip/harmony/v2_04/framework" -MP -MMD -MF "${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o.d" -o ${OBJECTDIR}/_ext/61412248/usb_device_endpoint_functions.o ../../../../../../../../../../microchip/harmony/v2_04/framework/usb/src/dynamic/usb_device_endpoint_functions.c    -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)    
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compileCPP
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk  ../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a ../../../../../../../../../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a  ../src/system_config/default/app_mz.ld
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -g -mdebugger -D__MPLAB_DEBUGGER_ICD4=1 -mprocessor=$(MP_PROCESSOR_OPTION)  -o dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}    ..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a ..\..\..\..\..\..\..\..\..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a      -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)   -mreserve=data@0x0:0x37F   -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1,-D=__DEBUG_D,--defsym=__MPLAB_DEBUGGER_ICD4=1,--defsym=_min_heap_size=0,--gc-sections,--no-code-in-dinit,--no-dinit-in-serial-mem,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--report-mem,--memorysummary,dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml 
	
else
dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk  ../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a ../../../../../../../../../../microchip/harmony/v2_04/bin/framework/peripheral/PIC32MZ2048EFM100_peripherals.a ../src/system_config/default/app_mz.ld
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -mprocessor=$(MP_PROCESSOR_OPTION)  -o dist/${CND_CONF}/${IMAGE_TYPE}/SoundCard.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}    ..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a ..\..\..\..\..\..\..\..\..\..\microchip\harmony\v2_04\bin\framework\peripheral\PIC32MZ2048EFM100_peripherals.a      -DXPRJ_default=$(CND_CONF)  -no-legacy-libc  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=_min_heap_size=0,--gc-sections,--no-code-in-dinit,--no-dinit-in-serial-mem,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--report-mem,--memorysummary,dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml 
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
