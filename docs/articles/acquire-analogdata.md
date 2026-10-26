# Acquire analog data

The Sound Card samples the voltage on **ADC0** and **ADC1** at 1 kHz and broadcasts each sample as an [`AdcControlState`] event together with the current sound parameters. Refer to the [connections](./ports-connections.md?tabs=analoginputs#connections) article to set up an analog signal on **ADC0**, which we will use for the rest of these examples.

This article covers how to enable the analog data stream and visualize the analog data in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Acquire analog data](../workflows/acquireanalogdata-toplevel.bonsai)
:::

## Enable analog data stream

The stream is off at power-up. Writing `SampleRate1000Hz` to the [`EnableAdcControlState`] register starts it, and writing `Disabled` stops it.

:::workflow
![Enable Analog Data Stream](../workflows/acquireanalogdata-enablestream.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `EnableAdcControlState` - Select `SampleRate1000Hz`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd>. The device starts broadcasting [`AdcControlState`] events at 1 kHz, which are saved in the [logged data](./logging-analysis.md).

## Visualize analog data

Each [`AdcControlState`] event carries five values:

| Field | Description |
| --- | --- |
| `Adc0` | The sampled value on **ADC0**. |
| `Adc1` | The sampled value on **ADC1**. |
| `AttenuationLeft` | The last left channel attenuation written to the attenuation registers. |
| `AttenuationRight` | The last right channel attenuation written to the attenuation registers. |
| `Frequency` | The last sound index or frequency written to the [`Start`] or [`AttenuationStart`] registers. |

The workflow below will show you how to decode the events, select one channel, and plot it:

:::workflow
![Visualize Analog Data](../workflows/acquireanalogdata-visualizedata.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`.
- Insert a [`Parse`] operator and set the `Register` property to `AdcControlState`.
- Insert a [`MemberSelector`] operator and set the `Selector` property to `Adc0`. This selects the data from the first analog input channel.
- Insert a [`VisualizerWindow`] operator and set the `VisualizerType` property to `TimeSeriesVisualizer`.

Run the workflow and press <kbd>A</kbd> to enable the stream. The visualizer plots the voltage on **ADC0** as a 12-bit value.

> [!NOTE]
> The attenuation and frequency fields report the last values written from Bonsai. Sounds triggered from the digital inputs do not update them, and the frequency field keeps its value after a [`Stop`] command.

> [!WARNING]
> **TODO**: Verify the analog input range and scaling on hardware. The repository README states 3.3 V maximum (5 V tolerant), while the firmware subtracts a fixed offset from the 12-bit conversion so that readings span about 0 to 3915. Confirm the full-scale voltage and update this note and the connections article accordingly.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`AdcControlState`]: xref:Harp.SoundCard.AdcControlState
[`AttenuationStart`]: xref:Harp.SoundCard.AttenuationStart
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`EnableAdcControlState`]: xref:Harp.SoundCard.EnableAdcControlState
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MemberSelector`]: xref:Bonsai.Expressions.MemberSelectorBuilder
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`Start`]: xref:Harp.SoundCard.Start
[`Stop`]: xref:Harp.SoundCard.Stop
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
