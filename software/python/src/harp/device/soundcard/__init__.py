# This file was automatically generated and should not be edited directly.
# To make changes, edit the device metadata and regenerate the interface.

import enum
from typing import Any, ClassVar

import numpy as np
from harp.protocol import (
    AnonymousPayload,
    BitMask,
    Field,
    GroupMask,
    IdentityConverter,
    PayloadType,
    RegisterBase,
    RegisterU16,
    RegisterU16Array,
    RegisterU8,
    StructPayload,
)
from harp.device.core import (
    EnableFlag,
    REGISTER_MAP as _CORE_REGISTER_MAP,
)


__all__ = [
    "DEVICE_NAME",
    "WHO_AM_I",
    "DigitalInputs",
    "DigitalOutputs",
    "DIMode",
    "DOMode",
    "AdcMode",
    "StartPayload",
    "DeleteSoundPayload",
    "InputStatePayload",
    "ConfigureDI0Payload",
    "ConfigureDI1Payload",
    "StartOnDI0Payload",
    "StartOnDI1Payload",
    "ConfigureDO0Payload",
    "ConfigureDO1Payload",
    "ConfigureDO2Payload",
    "OutputSetPayload",
    "OutputClearPayload",
    "OutputTogglePayload",
    "OutputStatePayload",
    "EnableAdcControlStatePayload",
    "AdcControlStatePayload",
    "Start",
    "Stop",
    "AttenuationLeft",
    "AttenuationRight",
    "AttenuationBoth",
    "AttenuationStart",
    "DeleteSound",
    "DeleteAllSounds",
    "InputState",
    "ConfigureDI0",
    "ConfigureDI1",
    "StartOnDI0",
    "StartOnDI1",
    "AttenuationLeftDI0",
    "AttenuationLeftDI1",
    "AttenuationRightDI0",
    "AttenuationRightDI1",
    "ConfigureDO0",
    "ConfigureDO1",
    "ConfigureDO2",
    "OutputSet",
    "OutputClear",
    "OutputToggle",
    "OutputState",
    "EnableAdcControlState",
    "AdcControlState",
    "REGISTER_MAP",
]

DEVICE_NAME: str = "SoundCard"
WHO_AM_I: int = 1280


class DigitalInputs(enum.IntFlag):
    """Specifies the state of the digital input lines."""

    DI0 = 0x1
    """Digital input 0."""

    DI1 = 0x2
    """Digital input 1."""

    DI2 = 0x4
    """Digital input 2 (no associated event)."""


class DigitalOutputs(enum.IntFlag):
    """Specifies the state of the digital output lines."""

    DO0 = 0x1
    """Digital output 0."""

    DO1 = 0x2
    """Digital output 1."""

    DO2 = 0x4
    """Digital output 2."""


class DIMode(enum.IntEnum):
    """Specifies the operation mode of a digital input."""

    DIGITAL = 0
    """Used as a pure digital input."""

    START_AND_STOP = 1
    """Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge and stop when falling edge (frequency only)."""

    START = 2
    """Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge."""

    STOP = 3
    """Stops frequency when rising edge."""


class DOMode(enum.IntEnum):
    """Specifies the operation mode of a digital output."""

    DIGITAL = 0
    """Used as a pure digital output."""

    PULSE_ON_START = 1
    """Positive 400us pulse when a sound or frequency starts."""


class AdcMode(enum.IntEnum):
    """Specifies the operation mode of an analog input."""

    ANALOG_INPUT = 0
    """Used as a pure analog input."""

    ATTENUATION = 1
    """Controls the attenuation of the left channel from ADC0 and of the right channel from ADC1."""

    ATTENUATION_BOTH = 2
    """Controls the attenuation of both channels."""

    FREQUENCY = 3
    """Controls the frequency of the waveform generator from ADC1 only."""


class StartPayload(AnonymousPayload[np.uint16]):
    """Represents the Start payload."""

    __value__: np.uint16 = Field(IdentityConverter(np.uint16), default=np.uint16(2))


class DeleteSoundPayload(AnonymousPayload[np.uint8]):
    """Represents the DeleteSound payload."""

    __value__: np.uint8 = Field(IdentityConverter(np.uint8), default=np.uint8(2))


class InputStatePayload(AnonymousPayload[np.uint8]):
    """Represents the InputState payload."""

    __value__: DigitalInputs = BitMask(enum=DigitalInputs)


class ConfigureDI0Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureDI0 payload."""

    __value__: DIMode = GroupMask(enum=DIMode, mask=0xFF)


class ConfigureDI1Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureDI1 payload."""

    __value__: DIMode = GroupMask(enum=DIMode, mask=0xFF)


class StartOnDI0Payload(AnonymousPayload[np.uint16]):
    """Represents the StartOnDI0 payload."""

    __value__: np.uint16 = Field(IdentityConverter(np.uint16), default=np.uint16(2000))


class StartOnDI1Payload(AnonymousPayload[np.uint16]):
    """Represents the StartOnDI1 payload."""

    __value__: np.uint16 = Field(IdentityConverter(np.uint16), default=np.uint16(4000))


class ConfigureDO0Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureDO0 payload."""

    __value__: DOMode = GroupMask(enum=DOMode, mask=0xFF)


class ConfigureDO1Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureDO1 payload."""

    __value__: DOMode = GroupMask(enum=DOMode, mask=0xFF)


class ConfigureDO2Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureDO2 payload."""

    __value__: DOMode = GroupMask(enum=DOMode, mask=0xFF)


class OutputSetPayload(AnonymousPayload[np.uint8]):
    """Represents the OutputSet payload."""

    __value__: DigitalOutputs = BitMask(enum=DigitalOutputs)


class OutputClearPayload(AnonymousPayload[np.uint8]):
    """Represents the OutputClear payload."""

    __value__: DigitalOutputs = BitMask(enum=DigitalOutputs)


class OutputTogglePayload(AnonymousPayload[np.uint8]):
    """Represents the OutputToggle payload."""

    __value__: DigitalOutputs = BitMask(enum=DigitalOutputs)


class OutputStatePayload(AnonymousPayload[np.uint8]):
    """Represents the OutputState payload."""

    __value__: DigitalOutputs = BitMask(enum=DigitalOutputs)


class EnableAdcControlStatePayload(AnonymousPayload[np.uint8]):
    """Represents the EnableAdcControlState payload."""

    __value__: EnableFlag = GroupMask(enum=EnableFlag, mask=0xFF)


class AdcControlStatePayload(StructPayload[np.uint16], length=5):
    """Represents the AdcControlState payload."""

    adc0: np.uint16 = Field(IdentityConverter(np.uint16))
    """The sampled analog input value on ADC0."""

    adc1: np.uint16 = Field(IdentityConverter(np.uint16), offset=1)
    """The sampled analog input value on ADC1."""

    attenuation_left: np.uint16 = Field(IdentityConverter(np.uint16), offset=2)
    """The last attenuation of the left channel set by a register write."""

    attenuation_right: np.uint16 = Field(IdentityConverter(np.uint16), offset=3)
    """The last attenuation of the right channel set by a register write."""

    frequency: np.uint16 = Field(IdentityConverter(np.uint16), offset=4)
    """The last sound index or frequency set by a register write, following the same rules as Start."""


class _ConfigureAdc0Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureAdc0 payload."""

    __value__: AdcMode = GroupMask(enum=AdcMode, mask=0xFF)


class _ConfigureAdc1Payload(AnonymousPayload[np.uint8]):
    """Represents the ConfigureAdc1 payload."""

    __value__: AdcMode = GroupMask(enum=AdcMode, mask=0xFF)


class _Pic32BootloaderPayload(AnonymousPayload[np.uint8]):
    """Represents the Pic32Bootloader payload."""

    __value__: EnableFlag = GroupMask(enum=EnableFlag, mask=0xFF)


class Start(RegisterBase[np.uint16]):
    """Starts the sound index if the value is less than 32, or otherwise the waveform generator at the specified frequency in Hz."""

    address: ClassVar[int] = 32
    payload_type: ClassVar[PayloadType] = PayloadType.U16
    payload_class = StartPayload


class Stop(RegisterU8):
    """Stops the sound or frequency being played when the written value is non-zero."""

    address: ClassVar[int] = 33


class AttenuationLeft(RegisterU16):
    """Specifies the attenuation for the left channel (1 LSB is 0.1dB)."""

    address: ClassVar[int] = 34


class AttenuationRight(RegisterU16):
    """Specifies the attenuation for the right channel (1 LSB is 0.1dB)."""

    address: ClassVar[int] = 35


class AttenuationBoth(RegisterU16Array):
    """Specifies the attenuation for the left and right channels simultaneously [Att L] [Att R] (1 LSB is 0.1dB)."""

    address: ClassVar[int] = 36
    length: int = 2


class AttenuationStart(RegisterU16Array):
    """Specifies attenuation and simultaneously starts the sound index or frequency [Index] [Att L] [Att R]."""

    address: ClassVar[int] = 37
    length: int = 3


class DeleteSound(RegisterBase[np.uint8]):
    """Deletes the sound stored at the specified index."""

    address: ClassVar[int] = 38
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = DeleteSoundPayload


class DeleteAllSounds(RegisterU8):
    """Deletes all sounds stored in the device when the written value is non-zero."""

    address: ClassVar[int] = 39


class InputState(RegisterBase[DigitalInputs]):
    """Reports the state of the digital inputs. Events are sent only for inputs configured in Digital mode."""

    address: ClassVar[int] = 40
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = InputStatePayload


class ConfigureDI0(RegisterBase[DIMode]):
    """Specifies the operation mode of the digital input 0 (DI0)."""

    address: ClassVar[int] = 41
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigureDI0Payload


class ConfigureDI1(RegisterBase[DIMode]):
    """Specifies the operation mode of the digital input 1 (DI1)."""

    address: ClassVar[int] = 42
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigureDI1Payload


class StartOnDI0(RegisterBase[np.uint16]):
    """Specifies the sound index or frequency to start when triggering DI0, following the same rules as Start."""

    address: ClassVar[int] = 43
    payload_type: ClassVar[PayloadType] = PayloadType.U16
    payload_class = StartOnDI0Payload


class StartOnDI1(RegisterBase[np.uint16]):
    """Specifies the sound index or frequency to start when triggering DI1, following the same rules as Start."""

    address: ClassVar[int] = 44
    payload_type: ClassVar[PayloadType] = PayloadType.U16
    payload_class = StartOnDI1Payload


class AttenuationLeftDI0(RegisterU16):
    """Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0."""

    address: ClassVar[int] = 45


class AttenuationLeftDI1(RegisterU16):
    """Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1."""

    address: ClassVar[int] = 46


class AttenuationRightDI0(RegisterU16):
    """Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0."""

    address: ClassVar[int] = 47


class AttenuationRightDI1(RegisterU16):
    """Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1."""

    address: ClassVar[int] = 48


class _Reserved2(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 49


class _Reserved3(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 50


class _Reserved4(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 51


class ConfigureDO0(RegisterBase[DOMode]):
    """Specifies the operation mode of the digital output 0 (DO0)."""

    address: ClassVar[int] = 52
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigureDO0Payload


class ConfigureDO1(RegisterBase[DOMode]):
    """Specifies the operation mode of the digital output 1 (DO1)."""

    address: ClassVar[int] = 53
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigureDO1Payload


class ConfigureDO2(RegisterBase[DOMode]):
    """Specifies the operation mode of the digital output 2 (DO2)."""

    address: ClassVar[int] = 54
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = ConfigureDO2Payload


class _Reserved5(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 55


class _Reserved6(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 56


class _Reserved7(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 57


class OutputSet(RegisterBase[DigitalOutputs]):
    """Sets the specified digital output lines."""

    address: ClassVar[int] = 58
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = OutputSetPayload


class OutputClear(RegisterBase[DigitalOutputs]):
    """Clears the specified digital output lines."""

    address: ClassVar[int] = 59
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = OutputClearPayload


class OutputToggle(RegisterBase[DigitalOutputs]):
    """Toggles the specified digital output lines."""

    address: ClassVar[int] = 60
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = OutputTogglePayload


class OutputState(RegisterBase[DigitalOutputs]):
    """Writes the state of all digital output lines."""

    address: ClassVar[int] = 61
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = OutputStatePayload


class _Reserved8(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 62


class _Reserved9(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 63


class EnableAdcControlState(RegisterBase[EnableFlag]):
    """Specifies whether the ADC control state is reported, sampled at 1 kHz."""

    address: ClassVar[int] = 64
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = EnableAdcControlStatePayload


class AdcControlState(RegisterBase[AdcControlStatePayload]):
    """Reports sampled analog input data and current sound parameters."""

    address: ClassVar[int] = 65
    payload_type: ClassVar[PayloadType] = PayloadType.U16
    payload_class = AdcControlStatePayload


class _ConfigureAdc0(RegisterBase[AdcMode]):
    """Specifies the operation mode of analog input ADC0 (not implemented yet)."""

    address: ClassVar[int] = 66
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = _ConfigureAdc0Payload


class _ConfigureAdc1(RegisterBase[AdcMode]):
    """Specifies the operation mode of analog input ADC1 (not implemented yet)."""

    address: ClassVar[int] = 67
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = _ConfigureAdc1Payload


class _Reserved10(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 68


class _Reserved11(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 69


class _Reserved12(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 70


class _Reserved13(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 71


class _Reserved14(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 72


class _Reserved15(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 73


class _Reserved16(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 74


class _Reserved17(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 75


class _Reserved18(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 76


class _Reserved19(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 77


class _Reserved20(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 78


class _Reserved21(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 79


class _Reserved22(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 80


class _Reserved23(RegisterU8):
    """Reserved for future use."""

    address: ClassVar[int] = 81


class _Pic32Bootloader(RegisterBase[EnableFlag]):
    """Specifies whether the PIC32 micro-controller bootloader is enabled. While enabled, the serial port is connected to the PIC32 bootloader."""

    address: ClassVar[int] = 82
    payload_type: ClassVar[PayloadType] = PayloadType.U8
    payload_class = _Pic32BootloaderPayload


REGISTER_MAP: dict[int, type[RegisterBase[Any]]] = {
    **_CORE_REGISTER_MAP,
    32: Start,
    33: Stop,
    34: AttenuationLeft,
    35: AttenuationRight,
    36: AttenuationBoth,
    37: AttenuationStart,
    38: DeleteSound,
    39: DeleteAllSounds,
    40: InputState,
    41: ConfigureDI0,
    42: ConfigureDI1,
    43: StartOnDI0,
    44: StartOnDI1,
    45: AttenuationLeftDI0,
    46: AttenuationLeftDI1,
    47: AttenuationRightDI0,
    48: AttenuationRightDI1,
    49: _Reserved2,
    50: _Reserved3,
    51: _Reserved4,
    52: ConfigureDO0,
    53: ConfigureDO1,
    54: ConfigureDO2,
    55: _Reserved5,
    56: _Reserved6,
    57: _Reserved7,
    58: OutputSet,
    59: OutputClear,
    60: OutputToggle,
    61: OutputState,
    62: _Reserved8,
    63: _Reserved9,
    64: EnableAdcControlState,
    65: AdcControlState,
    66: _ConfigureAdc0,
    67: _ConfigureAdc1,
    68: _Reserved10,
    69: _Reserved11,
    70: _Reserved12,
    71: _Reserved13,
    72: _Reserved14,
    73: _Reserved15,
    74: _Reserved16,
    75: _Reserved17,
    76: _Reserved18,
    77: _Reserved19,
    78: _Reserved20,
    79: _Reserved21,
    80: _Reserved22,
    81: _Reserved23,
    82: _Pic32Bootloader,
}
