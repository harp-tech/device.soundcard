---
uid: Harp.SoundCard.Device
---

Use the [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) to initialize the device, log data, and send commands to and receive messages from the SoundCard.

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

Check out the following in-depth guides to learn how to access the device functionality with the `Harp.SoundCard` package:
- [Play Sounds](../articles/play-sound.md)
- [Adjust Volume](../articles/adjust-volume.md)
- [Configure Digital Input](../articles/configure-digitalinput.md)
- [Configure Digital Output](../articles/configure-digitaloutput.md)
- [Configure Analog Input](../articles/configure-analoginput.md)

Refer to the register table below for a complete listing of the available registers on the device.

<table>
  <thead>
    <tr><th colspan="2">SoundCard</th></tr>
  </thead>
  <tbody>
    <tr><td>whoAmI</td><td>1280</td></tr>
    <tr><td>firmwareVersion</td><td>2.2</td></tr>
    <tr><td>hardwareTargets</td><td>1.1</td></tr>
  </tbody>
</table>

### Registers

| name | address | type | length | access | description | range | interfaceType |
|-|-|-|-|-|-|-|-|
| [PlaySoundOrFrequency](xref:Harp.SoundCard.PlaySoundOrFrequency) | 32 | U16 |  | Write | Starts the sound index (if less than 32) or frequency (if greater or equal than 32) |  | |
| [Stop](xref:Harp.SoundCard.Stop) | 33 | U8 |  | Write | Any value will stop the current sound |  | |
| [AttenuationLeft](xref:Harp.SoundCard.AttenuationLeft) | 34 | U16 |  | Write | Configure left channel's attenuation (1 LSB is 0.1dB) |  | |
| [AttenuationRight](xref:Harp.SoundCard.AttenuationRight) | 35 | U16 |  | Write | Configure right channel's attenuation (1 LSB is 0.1dB) |  | |
| [AttenuationBoth](xref:Harp.SoundCard.AttenuationBoth) | 36 | U16 | 2 | Write | Configures both attenuation on right and left channels [Att R] [Att L] |  | |
| [AttenuationAndPlaySoundOrFreq](xref:Harp.SoundCard.AttenuationAndPlaySoundOrFreq) | 37 | U16 | 3 | Write | Configures attenuation and plays sound index [Att R] [Att L] [Index] |  | |
| [InputState](xref:Harp.SoundCard.InputState) | 40 | U8 |  | Event | State of the digital inputs |  | [DigitalInputs](xref:Harp.SoundCard.DigitalInputs) |
| [ConfigureDI0](xref:Harp.SoundCard.ConfigureDI0) | 41 | U8 |  | Write | Configuration of the digital input 0 (DI0) |  | [DigitalInputConfiguration](xref:Harp.SoundCard.DigitalInputConfiguration) |
| [ConfigureDI1](xref:Harp.SoundCard.ConfigureDI1) | 42 | U8 |  | Write | Configuration of the digital input 1 (DI1) |  | [DigitalInputConfiguration](xref:Harp.SoundCard.DigitalInputConfiguration) |
| [ConfigureDI2](xref:Harp.SoundCard.ConfigureDI2) | 43 | U8 |  | Write | Configuration of the digital input 2 (DI2) |  | [DigitalInputConfiguration](xref:Harp.SoundCard.DigitalInputConfiguration) |
| [SoundIndexDI0](xref:Harp.SoundCard.SoundIndexDI0) | 44 | U8 |  | Write | Specifies the sound index to be played when triggering DI0 |  | |
| [SoundIndexDI1](xref:Harp.SoundCard.SoundIndexDI1) | 45 | U8 |  | Write | Specifies the sound index to be played when triggering DI1 |  | |
| [SoundIndexDI2](xref:Harp.SoundCard.SoundIndexDI2) | 46 | U8 |  | Write | Specifies the sound index to be played when triggering DI2 |  | |
| [FrequencyDI0](xref:Harp.SoundCard.FrequencyDI0) | 47 | U16 |  | Write | Specifies the sound frequency to be played when triggering DI0 |  | |
| [FrequencyDI1](xref:Harp.SoundCard.FrequencyDI1) | 48 | U16 |  | Write | Specifies the sound frequency to be played when triggering DI1 |  | |
| [FrequencyDI2](xref:Harp.SoundCard.FrequencyDI2) | 49 | U16 |  | Write | Specifies the sound frequency to be played when triggering DI2 |  | |
| [AttenuationLeftDI0](xref:Harp.SoundCard.AttenuationLeftDI0) | 50 | U16 |  | Write | Left channel's attenuation (1 LSB is 0.5dB) when triggering DI0 |  | |
| [AttenuationLeftDI1](xref:Harp.SoundCard.AttenuationLeftDI1) | 51 | U16 |  | Write | Left channel's attenuation (1 LSB is 0.5dB) when triggering DI1 |  | |
| [AttenuationLeftDI2](xref:Harp.SoundCard.AttenuationLeftDI2) | 52 | U16 |  | Write | Left channel's attenuation (1 LSB is 0.5dB) when triggering DI2 |  | |
| [AttenuationRightDI0](xref:Harp.SoundCard.AttenuationRightDI0) | 53 | U16 |  | Write | Right channel's attenuation (1 LSB is 0.5dB) when triggering DI0 |  | |
| [AttenuationRightDI1](xref:Harp.SoundCard.AttenuationRightDI1) | 54 | U16 |  | Write | Right channel's attenuation (1 LSB is 0.5dB) when triggering DI1 |  | |
| [AttenuationRightDI2](xref:Harp.SoundCard.AttenuationRightDI2) | 55 | U16 |  | Write | Right channel's attenuation (1 LSB is 0.5dB) when triggering DI2 |  | |
| [AttenuationAndSoundIndexDI0](xref:Harp.SoundCard.AttenuationAndSoundIndexDI0) | 56 | U16 | 3 | Write | Sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index] |  | |
| [AttenuationAndSoundIndexDI1](xref:Harp.SoundCard.AttenuationAndSoundIndexDI1) | 57 | U16 | 3 | Write | Sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index] |  | |
| [AttenuationAndSoundIndexDI2](xref:Harp.SoundCard.AttenuationAndSoundIndexDI2) | 58 | U16 | 3 | Write | Sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index] |  | |
| [AttenuationAndFrequencyDI0](xref:Harp.SoundCard.AttenuationAndFrequencyDI0) | 59 | U16 | 2 | Write | Sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency] |  | |
| [AttenuationAndFrequencyDI1](xref:Harp.SoundCard.AttenuationAndFrequencyDI1) | 60 | U16 | 2 | Write | Sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency] |  | |
| [AttenuationAndFrequencyDI2](xref:Harp.SoundCard.AttenuationAndFrequencyDI2) | 61 | U16 | 2 | Write | Sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency] |  | |
| [ConfigureDO0](xref:Harp.SoundCard.ConfigureDO0) | 65 | U8 |  | Write | Configuration of the digital output 0 (DO0) |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [ConfigureDO1](xref:Harp.SoundCard.ConfigureDO1) | 66 | U8 |  | Write | Configuration of the digital output 1 (DO1) |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [ConfigureDO2](xref:Harp.SoundCard.ConfigureDO2) | 67 | U8 |  | Write | Configuration of the digital output 2 (DO2 |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [PulseDO0](xref:Harp.SoundCard.PulseDO0) | 68 | U8 |  | Write | Pulse for the digital output 0 (DO0) | [1:255] | |
| [PulseDO1](xref:Harp.SoundCard.PulseDO1) | 69 | U8 |  | Write | Pulse for the digital output 1 (DO1) | [1:255] | |
| [PulseDO2](xref:Harp.SoundCard.PulseDO2) | 70 | U8 |  | Write | Pulse for the digital output 2 (DO2) | [1:255] | |
| [OutputSet](xref:Harp.SoundCard.OutputSet) | 74 | U8 |  | Write | Set the specified digital output lines |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputClear](xref:Harp.SoundCard.OutputClear) | 75 | U8 |  | Write | Clear the specified digital output lines |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputToggle](xref:Harp.SoundCard.OutputToggle) | 76 | U8 |  | Write | Toggle the specified digital output lines |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputState](xref:Harp.SoundCard.OutputState) | 77 | U8 |  | Write | Write the state of all digital output lines |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [ConfigureAdc](xref:Harp.SoundCard.ConfigureAdc) | 80 | U8 |  | Write | Configuration of Analog Inputs |  | [AdcConfiguration](xref:Harp.SoundCard.AdcConfiguration) |
| [AnalogData](xref:Harp.SoundCard.AnalogData) | 81 | U16 | 5 | Event | Contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used. |  | [AnalogDataPayload](xref:Harp.SoundCard.AnalogDataPayload) |
| [Commands](xref:Harp.SoundCard.Commands) | 82 | U8 |  | Write | Send commands to PIC32 micro-controller |  | [ControllerCommand](xref:Harp.SoundCard.ControllerCommand) |
| [EnableEvents](xref:Harp.SoundCard.EnableEvents) | 86 | U8 |  | Write | Specifies the active events in the SoundCard device |  | [SoundCardEvents](xref:Harp.SoundCard.SoundCardEvents) |
