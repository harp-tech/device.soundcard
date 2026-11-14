CORE = ATxmega128A4U
MCU = atxmega128a4u
CORE_VERSION = 1.15
PREVIEW_VERSION ?= 0

# Read build metadata from app.h if not defined
HASH := \#
APP_H_CONTENTS := $(shell cat app.h)
finddef = $(filter +%+,$(patsubst $(HASH)define:$(1):%,+%+,$(subst $(HASH)define$() $(1)$() ,$(HASH)define:$(1):,$(strip $(APP_H_CONTENTS)))))
APP_VERSION_NAMES = $(sort $(filter %_VERSION,$(APP_H_CONTENTS)))
APP_VERSION_VALUES = $(subst +,,$(foreach name,$(APP_VERSION_NAMES),$(call finddef,$(name))))
FIRMWARE_VERSION ?= $(word 1,$(APP_VERSION_VALUES)).$(word 3,$(APP_VERSION_VALUES))
HARDWARE_VERSION ?= $(word 2,$(APP_VERSION_VALUES)).$(word 4,$(APP_VERSION_VALUES))
PRJ_NAME = $(subst ",,$(subst +,,$(call finddef,DEVICE_NAME)))

# Version flags
FW_VERSION_WORDS = $(subst ., ,$(firstword $(subst -, ,$(FIRMWARE_VERSION))))
HW_VERSION_WORDS = $(subst ., ,$(firstword $(subst -, ,$(HARDWARE_VERSION))))
MAJOR_FW_VERSION = $(word 1,$(FW_VERSION_WORDS))
MINOR_FW_VERSION = $(word 2,$(FW_VERSION_WORDS))
PATCH_FW_VERSION = $(or $(word 3,$(FW_VERSION_WORDS)), 0)
MAJOR_HW_VERSION = $(word 1,$(HW_VERSION_WORDS))
MINOR_HW_VERSION = $(word 2,$(HW_VERSION_WORDS))
PATCH_HW_VERSION = $(or $(word 3,$(HW_VERSION_WORDS)), 0)
FIRMWARE_FORMAT = $(MAJOR_FW_VERSION).$(MINOR_FW_VERSION)
HARDWARE_FORMAT = $(MAJOR_HW_VERSION).$(MINOR_HW_VERSION)

# Files and directories
CORE_DIR = core
BUILD_DIR = bin
PREVIEW_SUFFIX = $(if $(PREVIEW_VERSION:v%=),-preview$(PREVIEW_VERSION),)
BUILD_NAME = $(PRJ_NAME)-fw$(FIRMWARE_FORMAT)-harp$(CORE_VERSION)-hw$(HARDWARE_FORMAT)-ass0$(PREVIEW_SUFFIX)
OUT_DIR = $(BUILD_DIR)/fw$(FIRMWARE_VERSION)-hw$(HARDWARE_VERSION)
SRCS = $(sort $(wildcard *.c))
OBJS = $(SRCS:%.c=$(OUT_DIR)/%.o)
INCS = -I"$(CORE_DIR)"
LIBS = -l$(CORE)-$(CORE_VERSION)

# Compiler flags
CC = avr-gcc
CFLAGS = \
-x c \
-funsigned-char \
-funsigned-bitfields \
-ffunction-sections \
-fdata-sections \
-fpack-struct \
-fshort-enums \
-mrelax \
-O1 \
-g2 \
-Wall \
-mmcu=$(MCU) \
-DMAJOR_FW_VERSION=$(MAJOR_FW_VERSION) \
-DMINOR_FW_VERSION=$(MINOR_FW_VERSION) \
-DPATCH_FW_VERSION=$(PATCH_FW_VERSION) \
-DMAJOR_HW_VERSION=$(MAJOR_HW_VERSION) \
-DMINOR_HW_VERSION=$(MINOR_HW_VERSION) \
-DPATCH_HW_VERSION=$(PATCH_HW_VERSION) \
-std=gnu99

# Linker flags
LDFLAGS = \
-Wl,--start-group \
-Wl,-lm \
-Wl,$(LIBS) \
-Wl,--end-group \
-Wl,-L"$(CORE_DIR)" \
-Wl,--gc-sections \
-mrelax \
-mmcu=$(MCU)

# Object copy flags
OBJCOPY = avr-objcopy
OBJCOPY_FLAGS = \
-O ihex \
-R .eeprom \
-R .fuse \
-R .lock \
-R .signature \
-R .user_signatures

all: $(OUT_DIR)/$(BUILD_NAME).hex

clean:
	rm -f $(OBJS) $(OBJS:.o=.d)
	rm -f $(OUT_DIR)/$(BUILD_NAME).elf
	rm -f $(OUT_DIR)/$(BUILD_NAME).hex
	rm -f $(OUT_DIR)/$(BUILD_NAME).map

$(BUILD_DIR):
	mkdir "$(BUILD_DIR)"

$(OUT_DIR): | $(BUILD_DIR)
	mkdir "$(OUT_DIR)"

$(OUT_DIR)/%.o: %.c | $(OUT_DIR)
	$(CC) $(CFLAGS) -c $(INCS) -MD -MP -MF "$(@:.o=.d)" -MT"$@" -o $@ $<

$(OUT_DIR)/$(BUILD_NAME).elf: $(OBJS)
	$(CC) -o $@ $(OBJS) -Wl,-Map="$(@:.elf=.map)" $(LDFLAGS)

$(OUT_DIR)/$(BUILD_NAME).hex: $(OUT_DIR)/$(BUILD_NAME).elf
	$(OBJCOPY) $(OBJCOPY_FLAGS) $< $@

-include $(wildcard $(OUT_DIR)/*.d)

.PHONY: all clean
