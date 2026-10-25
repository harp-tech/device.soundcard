## Configure Digital Inputs

This article will cover how to configure the digital input channels on the SoundCard to trigger sound playback from an external device, set the volume of the triggered sounds, and visualize the state of the inputs. Refer to the [connections](./connections.md) article to set up the hardware connection.

Sound triggering is available on **IN0** and **IN1**. **IN2** can only be read as a plain digital input and does not report events.

The complete workflow is shown below:

:::workflow
![Configure Digital Input Top Level](../workflows/configureDI-toplevel.bonsai)
:::

### Trigger Sound Playback

To trigger sound playback from a digital input, set the configuration mode for the selected channel with the [`ConfigureDI0`] register and the sound index or frequency to start with the [`StartOnDI0`] register. Once set, a rising edge on **IN0** starts the sound directly on the device, following the same rules as the [`Start`] register: values from 2 to 31 select a stored sound, and values from 32 to 40000 set the frequency of a pure tone in Hz.

The [`ConfigureDI0`] register accepts four modes:

- `Digital` - The input is read as a plain digital input and reports its state as [`InputState`] events.
- `Start` - A rising edge starts the sound.
- `StartAndStop` - A rising edge starts the sound and a falling edge stops it. Only pure tones can be stopped.
- `Stop` - A rising edge stops the pure tone that is playing.

:::workflow
![Configure Digital Input Trigger](../workflows/configureDI-triggersound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDI0Payload`].
    - `ConfigureDI0` - Select `Start`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a second [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`StartOnDI0Payload`].
    - `StartOnDI0` - Set the sound index (2–31) or frequency in Hz (32–40000) to start (e.g. 2).
- Insert another [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then send the TTL signal from the other device to trigger sound playback.

> [!NOTE]
> Sounds triggered from the digital inputs are reported as [`Start`] events and pulse any digital output configured with `PulseOnStart`, exactly like sounds started from Bonsai. Refer to the [Play Sounds](play-sound.md) article to visualize the events.

> [!TIP]
> At power-up both inputs are in `Digital` mode, [`StartOnDI0`] is set to 2000 Hz, and [`StartOnDI1`] is set to 4000 Hz. Set every register your experiment depends on explicitly, as shown here, rather than relying on these defaults.

### Set Trigger Volume

Sounds triggered from a digital input play at the attenuation stored in the [`AttenuationLeftDI0`] and [`AttenuationRightDI0`] registers, in 0.1 dB steps, independently of the attenuation set for sounds started from Bonsai. The default is 60 (-6 dB) on both channels.

:::workflow
![Configure Digital Input Volume](../workflows/configureDI-triggervolume.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`AttenuationLeftDI0Payload`].
    - `AttenuationLeftDI0` - Set the attenuation of the left channel (e.g. 200 = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a second [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`AttenuationRightDI0Payload`].
    - `AttenuationRightDI0` - Set the attenuation of the right channel (e.g. 200 = -20 dB).
- Insert another [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press <kbd>S</kbd>, and trigger **IN0** again. The sound plays at the new volume.

> [!NOTE]
> These registers only affect sounds triggered from the digital inputs. Use the registers in the [Adjust Volume](adjust-volume.md) article to set the volume of sounds started from Bonsai.

### Visualize Digital Input Events

An input in `Digital` mode reports every rising and falling edge as an [`InputState`] event. This example configures **IN1** as a digital input, so **IN0** keeps triggering sounds, and visualizes the events.

:::workflow
![Configure Digital Input Visualize Events](../workflows/configureDI-visualizeevents.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDI1Payload`].
    - `ConfigureDI1` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`. This will listen to [`HarpMessages`] broadcast from the [`PublishSubject`] named `SoundCard Events` in the Harp device pattern.
- Insert a [`Parse`] operator and configure the `Register` property to `TimestampedInputState`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

Run the workflow, press <kbd>D</kbd>, and send a TTL signal to **IN1**. The visualizer will display:

```text
DI1@15346.973344
None@15347.102112
```

The first value lists the inputs that read high, in this case `DI1` on the rising edge and `None` on the falling edge, and the second number is the timestamp on the device clock.

> [!WARNING]
> Only inputs in `Digital` mode report events, and each event only refreshes the bit of the input that changed. The `DI0` bit is stale while **IN0** is in a trigger mode, and the `DI2` bit never updates from events.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`AttenuationLeftDI0`]: xref:Harp.SoundCard.AttenuationLeftDI0
[`AttenuationLeftDI0Payload`]: xref:Harp.SoundCard.CreateAttenuationLeftDI0Payload
[`AttenuationRightDI0`]: xref:Harp.SoundCard.AttenuationRightDI0
[`AttenuationRightDI0Payload`]: xref:Harp.SoundCard.CreateAttenuationRightDI0Payload
[`ConfigureDI0`]: xref:Harp.SoundCard.ConfigureDI0
[`ConfigureDI0Payload`]: xref:Harp.SoundCard.CreateConfigureDI0Payload
[`ConfigureDI1Payload`]: xref:Harp.SoundCard.CreateConfigureDI1Payload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`HarpMessages`]: xref:Bonsai.Harp.HarpMessage
[`InputState`]: xref:Harp.SoundCard.InputState
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`Start`]: xref:Harp.SoundCard.Start
[`StartOnDI0`]: xref:Harp.SoundCard.StartOnDI0
[`StartOnDI0Payload`]: xref:Harp.SoundCard.CreateStartOnDI0Payload
[`StartOnDI1`]: xref:Harp.SoundCard.StartOnDI1
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
