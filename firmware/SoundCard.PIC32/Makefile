PROCESSOR = 32MZ2048EFM100
CORE_VERSION = 1.15
PREVIEW_VERSION ?= 0

# Read build metadata from the ATxmega app.h if not defined
APP_H = ../SoundCard/app.h
HASH := \#
APP_H_CONTENTS := $(shell cat $(APP_H))
finddef = $(filter +%+,$(patsubst $(HASH)define:$(1):%,+%+,$(subst $(HASH)define$() $(1)$() ,$(HASH)define:$(1):,$(strip $(APP_H_CONTENTS)))))
APP_VERSION_NAMES = $(sort $(filter %_VERSION,$(APP_H_CONTENTS)))
APP_VERSION_VALUES = $(subst +,,$(foreach name,$(APP_VERSION_NAMES),$(call finddef,$(name))))
FIRMWARE_VERSION ?= $(word 1,$(APP_VERSION_VALUES)).$(word 3,$(APP_VERSION_VALUES))
HARDWARE_VERSION ?= $(word 2,$(APP_VERSION_VALUES)).$(word 4,$(APP_VERSION_VALUES))
PRJ_NAME = $(subst ",,$(subst +,,$(call finddef,DEVICE_NAME))).PIC32

# Version formats
FW_VERSION_WORDS = $(subst ., ,$(firstword $(subst -, ,$(FIRMWARE_VERSION))))
HW_VERSION_WORDS = $(subst ., ,$(firstword $(subst -, ,$(HARDWARE_VERSION))))
FIRMWARE_FORMAT = $(word 1,$(FW_VERSION_WORDS)).$(word 2,$(FW_VERSION_WORDS))
HARDWARE_FORMAT = $(word 1,$(HW_VERSION_WORDS)).$(word 2,$(HW_VERSION_WORDS))

# Files and directories
HARMONY_DIR = microchip/harmony/v2_04/framework
SRC_DIR = firmware/src
CONFIG_DIR = $(SRC_DIR)/system_config/default
BUILD_DIR = bin
OBJ_DIR = bin/obj
PREVIEW_SUFFIX = $(if $(PREVIEW_VERSION:v%=),-preview$(PREVIEW_VERSION),)
BUILD_NAME = $(PRJ_NAME)-fw$(FIRMWARE_FORMAT)-harp$(CORE_VERSION)-hw$(HARDWARE_FORMAT)-ass0$(PREVIEW_SUFFIX)
OUT_DIR = $(BUILD_DIR)/fw$(FIRMWARE_VERSION)-hw$(HARDWARE_VERSION)
ELF =$(OBJ_DIR)/$(PRJ_NAME).elf
IMAGE ?= $(OBJ_DIR)/$(PRJ_NAME).hex
LINKER_SCRIPT = $(CONFIG_DIR)/app_mz.ld

# Sources in link order
SRCS = \
$(CONFIG_DIR)/framework/system/clk/src/sys_clk_pic32mz.c \
$(CONFIG_DIR)/framework/system/devcon/src/sys_devcon.c \
$(CONFIG_DIR)/framework/system/devcon/src/sys_devcon_pic32mz.c \
$(CONFIG_DIR)/framework/system/devcon/src/sys_devcon_cache_pic32mz.S \
$(CONFIG_DIR)/framework/system/ports/src/sys_ports_static.c \
$(CONFIG_DIR)/framework/system/reset/src/sys_reset.c \
$(CONFIG_DIR)/system_init.c \
$(CONFIG_DIR)/system_interrupt.c \
$(CONFIG_DIR)/system_exceptions.c \
$(CONFIG_DIR)/system_tasks.c \
$(SRC_DIR)/app.c \
$(SRC_DIR)/main.c \
$(SRC_DIR)/sounds_allocation.c \
$(SRC_DIR)/memory.c \
$(SRC_DIR)/audio.c \
$(SRC_DIR)/ios.c \
$(SRC_DIR)/delay.c \
$(SRC_DIR)/parallel_bus.c \
$(HARMONY_DIR)/driver/i2s/src/dynamic/drv_i2s_dma.c \
$(HARMONY_DIR)/driver/tmr/src/dynamic/drv_tmr.c \
$(HARMONY_DIR)/driver/usb/usbhs/src/dynamic/drv_usbhs.c \
$(HARMONY_DIR)/driver/usb/usbhs/src/dynamic/drv_usbhs_device.c \
$(HARMONY_DIR)/system/dma/src/sys_dma.c \
$(HARMONY_DIR)/system/int/src/sys_int_pic32.c \
$(HARMONY_DIR)/system/tmr/src/sys_tmr.c \
$(HARMONY_DIR)/usb/src/dynamic/usb_device.c \
$(HARMONY_DIR)/usb/src/dynamic/usb_device_endpoint_functions.c

OBJS = $(addprefix $(OBJ_DIR)/,$(addsuffix .o,$(basename $(notdir $(SRCS)))))
INCS = \
-I"$(HARMONY_DIR)" \
-I"$(SRC_DIR)" \
-I"$(CONFIG_DIR)" \
-I"$(CONFIG_DIR)/framework"

vpath %.c $(sort $(dir $(filter %.c,$(SRCS))))
vpath %.S $(sort $(dir $(filter %.S,$(SRCS))))

# Compiler flags
CC = xc32-gcc
BIN2HEX = xc32-bin2hex
CFLAGS = \
-g \
-x c \
-mprocessor=$(PROCESSOR) \
-ffunction-sections \
-O3 \
-fno-common \
-DXPRJ_default=default \
-no-legacy-libc

# Assembler flags
ASFLAGS = \
-mprocessor=$(PROCESSOR) \
-DXPRJ_default=default \
-no-legacy-libc \
-Wa,--defsym=__MPLAB_BUILD=1,--gdwarf-2

# Linker flags
LDFLAGS = \
-mprocessor=$(PROCESSOR) \
-DXPRJ_default=default \
-no-legacy-libc \
-Wl,--defsym=__MPLAB_BUILD=1,--script="$(LINKER_SCRIPT)",--defsym=_min_heap_size=0,--gc-sections,--no-code-in-dinit,--no-dinit-in-serial-mem

all: $(OUT_DIR)/$(BUILD_NAME).hex

clean:
	rm -rf "$(OBJ_DIR)"
	rm -f $(OUT_DIR)/$(BUILD_NAME).hex

$(OUT_DIR) $(OBJ_DIR):
	mkdir -p "$@"

$(OBJ_DIR)/%.o: %.c | $(OBJ_DIR)
	$(CC) $(CFLAGS) -c $(INCS) -MMD -MP -MF "$(@:.o=.d)" -o $@ $<

$(OBJ_DIR)/%.o: %.S | $(OBJ_DIR)
	$(CC) $(ASFLAGS) -c -MMD -MP -MF "$(@:.o=.d)" -o $@ $<

$(ELF): $(OBJS) $(LINKER_SCRIPT)
	$(CC) -o $@ $(OBJS) $(LDFLAGS),-Map="$(@:.elf=.map)"

$(OBJ_DIR)/$(PRJ_NAME).hex: $(ELF)
	$(BIN2HEX) $<

$(OUT_DIR)/$(BUILD_NAME).hex: $(IMAGE) | $(OUT_DIR)
	cp $< $@

-include $(wildcard $(OBJ_DIR)/*.d)

.PHONY: all clean
