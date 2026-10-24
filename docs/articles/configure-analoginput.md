## Configure Analog Input

This article will cover how to configure the analog input channels on the SoundCard to read analog data coming from other devices. Refer to the [connections](./connections.md#analog-input) article to set up the hardware connection.

The complete workflow is shown below:

:::workflow
![Configure Analog Input Top Level](../workflows/configureAI-toplevel.bonsai)
:::

> [!NOTE]
> The SoundCard samples the analog inputs at 1 kHz and stores the latest values in the [`AnalogData`] register. The device does not broadcast [`AnalogData`] events, so this workflow polls the register with read requests instead.

### Poll Analog Data

To read the analog inputs at regular intervals, send a read request to the [`AnalogData`] register with a [`Timer`].

:::workflow
![Poll Analog Data](../workflows/configureAI-pollingrequest.bonsai)
:::

- Insert a [`Timer`] source and set the `Period` property to the polling interval (e.g. 00:00:00.1 to poll every 100 ms).
- Insert a [`CreateMessage`] operator and configure these properties:
    - `MessageType` - Select `Read`.
    - `Payload` - Select [`AnalogDataPayload`].
    - `AnalogData` - Leave at the default, the payload value is ignored by the device for a read request.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

The device replies to each polling request with a [`HarpMessage`] containing the sampled values of both analog input channels, which can be found in the [logged data](./logging-analysis.md). Move on to the next section to visualize the analog data while the workflow is running.

### Visualize Analog Data

To visualize the analog data, parse the [`HarpMessages`] coming from the device and decode them using the workflow below:

:::workflow
![Visualize Analog Data](../workflows/configureAI-readanalogdata.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`. This will listen to [`HarpMessages`] broadcast from the [`PublishSubject`] named `SoundCard Events` in the Harp device pattern.
- Insert a [`Parse`] operator and configure the `Register` property to `AnalogData`.
- Right-click on the [`Parse`] operator, select "Harp.SoundCard.AnalogDataPayload" > "Adc0" from the context menu. This will select data from the first analog input channel.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the analog input values when the workflow starts.

Run the workflow and the visualizer will display a continuous stream of readings like this:

```text
2509
2510
```

> [!NOTE]
> The analog input lines accept voltages from 0 to 5 V which are subsequently digitized by an onboard 12-bit analog-to-digital converter (ADC).

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`AnalogData`]: xref:Harp.SoundCard.AnalogData
[`AnalogDataPayload`]: xref:Harp.SoundCard.CreateAnalogDataPayload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`HarpMessage`]: xref:Bonsai.Harp.HarpMessage
[`HarpMessages`]: xref:Bonsai.Harp.HarpMessage
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`Timer`]: xref:Bonsai.Reactive.Timer
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
