## Acquire Analog Data

This article will cover how to stream the analog input channels on the SoundCard to read analog data coming from other devices. Refer to the [connections](./connections.md#analog-input) article to set up the hardware connection.

The complete workflow is shown below:

:::workflow
![Acquire Analog Data Top Level](../workflows/acquireanalogdata-toplevel.bonsai)
:::

> [!NOTE]
> The SoundCard samples **ADC0** and **ADC1** at 1 kHz and broadcasts each sample as an [`AdcControlState`] event together with the current sound parameters. The stream is off at power-up and has to be enabled.

### Enable Analog Data Stream

To start the stream, write `SampleRate1000Hz` to the [`EnableAdcControlState`] register. Write `Disabled` to stop it.

:::workflow
![Enable Analog Data Stream](../workflows/acquireanalogdata-enablestream.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`EnableAdcControlStatePayload`].
    - `EnableAdcControlState` - Select `SampleRate1000Hz`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd>. The device starts broadcasting [`AdcControlState`] events at 1 kHz, which are saved in the [logged data](./logging-analysis.md). Move on to the next section to visualize the analog data while the workflow is running.

### Visualize Analog Data

Each [`AdcControlState`] event carries five values:

| Field | Description |
| --- | --- |
| `Adc0` | The sampled value on **ADC0**. |
| `Adc1` | The sampled value on **ADC1**. |
| `AttenuationLeft` | The last left channel attenuation written to the attenuation registers. |
| `AttenuationRight` | The last right channel attenuation written to the attenuation registers. |
| `Frequency` | The last sound index or frequency written to the [`Start`] or [`AttenuationStart`] registers. |

To plot the analog data, parse the [`HarpMessages`] coming from the device, select one channel, and decode them using the workflow below:

:::workflow
![Visualize Analog Data](../workflows/acquireanalogdata-visualizedata.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`. This will listen to [`HarpMessages`] broadcast from the [`PublishSubject`] named `SoundCard Events` in the Harp device pattern.
- Insert a [`Parse`] operator and configure the `Register` property to `AdcControlState`.
- Insert a [`MemberSelector`] operator and set the `Selector` property to `Adc0`. This selects the data from the first analog input channel.
- Insert a [`VisualizerWindow`] operator and set the `VisualizerType` property to `TimeSeriesVisualizer`. This will automatically open a window plotting the analog input values when the workflow starts.

Run the workflow and press <kbd>A</kbd> to enable the stream. The visualizer plots the voltage on **ADC0** as a 12-bit value.

> [!NOTE]
> The attenuation and frequency fields report the last values written from Bonsai. Sounds triggered from the digital inputs do not update them, and the frequency field keeps its value after a [`Stop`] command.

> [!WARNING]
> **TODO**: Verify the analog input range and scaling on hardware. The repository README states 3.3 V maximum (5 V tolerant), while the firmware subtracts a fixed offset from the 12-bit conversion so that readings span about 0 to 3915. Confirm the full-scale voltage and update this note and the connections article accordingly.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`AdcControlState`]: xref:Harp.SoundCard.AdcControlState
[`AttenuationStart`]: xref:Harp.SoundCard.AttenuationStart
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`EnableAdcControlState`]: xref:Harp.SoundCard.EnableAdcControlState
[`EnableAdcControlStatePayload`]: xref:Harp.SoundCard.CreateEnableAdcControlStatePayload
[`HarpMessages`]: xref:Bonsai.Harp.HarpMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MemberSelector`]: xref:Bonsai.Expressions.MemberSelectorBuilder
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`Start`]: xref:Harp.SoundCard.Start
[`Stop`]: xref:Harp.SoundCard.Stop
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
