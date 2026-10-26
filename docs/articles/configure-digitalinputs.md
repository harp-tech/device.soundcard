# Configure digital inputs

The Sound Card starts and stops sounds from TTL signals on **IN0** and **IN1**, with a volume set per input; **IN2** can only be read as a plain digital input and does not report events. Refer to the [connections](./ports-connections.md?tabs=digitalinputs#connections) article to set up a digital signal on **IN0**, which we will use for the rest of these examples.

This article covers how to trigger sound playback from a digital input, set the volume of triggered sounds, and visualize the state of the inputs in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Configure digital inputs](../workflows/configuredigitalinputs-toplevel.bonsai)
:::

## Trigger sound playback

The [`ConfigureDI0`] register sets what a signal on **IN0** does, and the [`StartOnDI0`] register sets the sound index or frequency to start, following the same rules as the [`Start`] register: values from 2 to 31 select a stored sound, and values from 32 to 40000 set the frequency of a pure tone in Hz. The [`ConfigureDI0`] register accepts four modes:

- `Digital` - The input is read as a plain digital input and reports its state as [`InputState`] events.
- `Start` - A rising edge starts the sound.
- `StartAndStop` - A rising edge starts the sound and a falling edge stops it. Only pure tones can be stopped.
- `Stop` - A rising edge stops the pure tone that is playing.

:::workflow
![Trigger Sound Playback](../workflows/configuredigitalinputs-triggersound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDI0` - Select `Start`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `StartOnDI0` - Set the sound index (2–31) or frequency in Hz (32–40000) to start (e.g. 2).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then send the TTL signal from the other device to trigger sound playback.

> [!NOTE]
> Sounds triggered from the digital inputs are reported as [`Start`] events and pulse any digital output configured with `PulseOnStart`, exactly like sounds started from Bonsai. Refer to the [Play sounds](play-sound.md) article to visualize the events.

> [!TIP]
> At power-up both inputs are in `Digital` mode, [`StartOnDI0`] is set to 2000 Hz, and [`StartOnDI1`] is set to 4000 Hz. Set every register your experiment depends on explicitly, as shown here, rather than relying on these defaults.

## Set trigger volume

Sounds triggered from a digital input play at the attenuation stored in the [`AttenuationLeftDI0`] and [`AttenuationRightDI0`] registers, in 0.1 dB steps, independently of the attenuation set for sounds started from Bonsai. The default is 60 (-6 dB) on both channels.

:::workflow
![Set Trigger Volume](../workflows/configuredigitalinputs-triggervolume.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationLeftDI0` - Set the attenuation of the left channel (e.g. 200 = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationRightDI0` - Set the attenuation of the right channel (e.g. 200 = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press <kbd>S</kbd>, and trigger **IN0** again. The sound plays at the new volume.

> [!NOTE]
> These registers only affect sounds triggered from the digital inputs. Use the registers in the [Adjust volume](adjust-volume.md) article to set the volume of sounds started from Bonsai.

## Visualize digital input events

An input in `Digital` mode reports every rising and falling edge as an [`InputState`] event. This example configures **IN1** as a digital input, so **IN0** keeps triggering sounds, and visualizes the events.

:::workflow
![Visualize Digital Input Events](../workflows/configuredigitalinputs-visualizeevents.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDI1` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedInputState`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow, press <kbd>D</kbd>, and send a TTL signal to **IN1**. The visualizer will display:

```text
DI1@15346.973344
None@15347.102112
```

The first value lists the inputs that read high, in this case `DI1` on the rising edge and `None` on the falling edge, and the second number is the timestamp on the device clock.

> [!WARNING]
> Only inputs in `Digital` mode report events, and each event only refreshes the bit of the input that changed. The `DI0` bit is stale while **IN0** is in a trigger mode, and the `DI2` bit never updates from events.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`AttenuationLeftDI0`]: xref:Harp.SoundCard.AttenuationLeftDI0
[`AttenuationRightDI0`]: xref:Harp.SoundCard.AttenuationRightDI0
[`ConfigureDI0`]: xref:Harp.SoundCard.ConfigureDI0
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`InputState`]: xref:Harp.SoundCard.InputState
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`Start`]: xref:Harp.SoundCard.Start
[`StartOnDI0`]: xref:Harp.SoundCard.StartOnDI0
[`StartOnDI1`]: xref:Harp.SoundCard.StartOnDI1
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
