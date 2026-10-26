---
uid: Harp.SoundCard.Device
---

Use the [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) to initialize the device, log data, and send commands to and receive messages from the Sound Card.

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

Check out the following in-depth guides to learn how to access the device functionality with the `Harp.SoundCard` package:
- [Play sounds](../articles/play-sound.md)
- [Adjust volume](../articles/adjust-volume.md)
- [Configure digital inputs](../articles/configure-digitalinputs.md)
- [Control digital outputs](../articles/control-digitaloutputs.md)
- [Acquire analog data](../articles/acquire-analogdata.md)
- [Advanced configuration](../articles/advanced-configuration.md)

Refer to the register table below for a complete listing of the available registers on the device.

<table>
  <thead>
    <tr><th colspan="2">Sound Card</th></tr>
  </thead>
  <tbody>
    <tr><td>whoAmI</td><td>1280</td></tr>
    <tr><td>firmwareVersion</td><td>3.0</td></tr>
    <tr><td>hardwareTargets</td><td>1.1</td></tr>
  </tbody>
</table>

### Registers

| name | address | type | length | access | description | range | interfaceType |
|-|-|-|-|-|-|-|-|
| [Start](xref:Harp.SoundCard.Start) | 32 | U16 |  | Write, Event | Starts the sound index if the value is less than 32, or otherwise the waveform generator at the specified frequency in Hz. | [2:40000] | |
| [Stop](xref:Harp.SoundCard.Stop) | 33 | U8 |  | Write, Event | Stops the frequency generator process with any non-zero input value. Silencing a sound index is not implemented. Any non-zero write clears a pending start, so a new sound can be started immediately. The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz. |  | |
| [AttenuationLeft](xref:Harp.SoundCard.AttenuationLeft) | 34 | U16 |  | Write | Specifies the attenuation for the left channel (1 LSB is 0.1dB). |  | |
| [AttenuationRight](xref:Harp.SoundCard.AttenuationRight) | 35 | U16 |  | Write | Specifies the attenuation for the right channel (1 LSB is 0.1dB). |  | |
| [AttenuationBoth](xref:Harp.SoundCard.AttenuationBoth) | 36 | U16 | 2 | Write | Specifies the attenuation for the left and right channels simultaneously [Att L] [Att R] (1 LSB is 0.1dB). |  | |
| [AttenuationStart](xref:Harp.SoundCard.AttenuationStart) | 37 | U16 | 3 | Write | Specifies attenuation and simultaneously starts the sound index or frequency [Index] [Att L] [Att R]. |  | |
| [InputState](xref:Harp.SoundCard.InputState) | 40 | U8 |  | Event | Reports the state of the digital inputs. Events are sent only for inputs configured in Digital mode. |  | [DigitalInputs](xref:Harp.SoundCard.DigitalInputs) |
| [ConfigureDI0](xref:Harp.SoundCard.ConfigureDI0) | 41 | U8 |  | Write | Specifies the configuration for the digital input 0 (DI0). |  | [DigitalInputConfiguration](xref:Harp.SoundCard.DigitalInputConfiguration) |
| [ConfigureDI1](xref:Harp.SoundCard.ConfigureDI1) | 42 | U8 |  | Write | Specifies the configuration for the digital input 1 (DI1). |  | [DigitalInputConfiguration](xref:Harp.SoundCard.DigitalInputConfiguration) |
| [StartOnDI0](xref:Harp.SoundCard.StartOnDI0) | 43 | U16 |  | Write | Specifies the sound index or frequency to start when triggering DI0, following the same rules as Start. | [2:40000] | |
| [StartOnDI1](xref:Harp.SoundCard.StartOnDI1) | 44 | U16 |  | Write | Specifies the sound index or frequency to start when triggering DI1, following the same rules as Start. | [2:40000] | |
| [AttenuationLeftDI0](xref:Harp.SoundCard.AttenuationLeftDI0) | 45 | U16 |  | Write | Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0. |  | |
| [AttenuationLeftDI1](xref:Harp.SoundCard.AttenuationLeftDI1) | 46 | U16 |  | Write | Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1. |  | |
| [AttenuationRightDI0](xref:Harp.SoundCard.AttenuationRightDI0) | 47 | U16 |  | Write | Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0. |  | |
| [AttenuationRightDI1](xref:Harp.SoundCard.AttenuationRightDI1) | 48 | U16 |  | Write | Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1. |  | |
| [ConfigureDO0](xref:Harp.SoundCard.ConfigureDO0) | 52 | U8 |  | Write | Specifies the configuration of the digital output 0 (DO0). |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [ConfigureDO1](xref:Harp.SoundCard.ConfigureDO1) | 53 | U8 |  | Write | Specifies the configuration of the digital output 1 (DO1). |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [ConfigureDO2](xref:Harp.SoundCard.ConfigureDO2) | 54 | U8 |  | Write | Specifies the configuration of the digital output 2 (DO2). |  | [DigitalOutputConfiguration](xref:Harp.SoundCard.DigitalOutputConfiguration) |
| [OutputSet](xref:Harp.SoundCard.OutputSet) | 58 | U8 |  | Write | Sets the specified digital output lines. |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputClear](xref:Harp.SoundCard.OutputClear) | 59 | U8 |  | Write | Clears the specified digital output lines. |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputToggle](xref:Harp.SoundCard.OutputToggle) | 60 | U8 |  | Write | Toggles the specified digital output lines. |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [OutputState](xref:Harp.SoundCard.OutputState) | 61 | U8 |  | Write | Writes the state of all digital output lines. |  | [DigitalOutputs](xref:Harp.SoundCard.DigitalOutputs) |
| [EnableAdcControlState](xref:Harp.SoundCard.EnableAdcControlState) | 64 | U8 |  | Write | Specifies the configuration of the ADC control stream. |  | [AdcControlStateConfiguration](xref:Harp.SoundCard.AdcControlStateConfiguration) |
| [AdcControlState](xref:Harp.SoundCard.AdcControlState) | 65 | U16 | 5 | Event | Reports sampled analog input data and current sound parameters. |  | [AdcControlStatePayload](xref:Harp.SoundCard.AdcControlStatePayload) |
| [Pic32Commands](xref:Harp.SoundCard.Pic32Commands) | 82 | U8 |  | Write | Sends commands to the PIC32 micro-controller |  | [Pic32Command](xref:Harp.SoundCard.Pic32Command) |
