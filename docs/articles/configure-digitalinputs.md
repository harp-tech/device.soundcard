# Configure digital inputs

The digital inputs on the Sound Card can start and stop sound playback from TTL signals on **IN0** and **IN1**, with a volume set per input. **IN2** does not support triggering, but can be read as a plain digital input. Refer to the [connections](./ports-connections.md?tabs=digitalinputs#connections) article to set up a digital signal on **IN0**, which we will use for the rest of these examples.

This article covers how to configure the digital input mode, set the volume of triggered sounds, and visualize the state of the inputs in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Configure digital inputs](../workflows/configuredigitalinputs-toplevel.bonsai)
:::

## Set mode and sound

Each digital input line has a configuration mode register that sets what the signal does (e.g. [`ConfigureDI0`] for **IN0**) as well as a start sound register that sets the sound index or frequency to play (e.g. [`StartOnDI0`] for **IN0**).

[`ConfigureDI0`] has four options:

- `Digital` - The input is read as a regular digital input and reports its state as [`InputState`] events.
- `Start` - A rising edge starts the sound. Use this mode for sound index playback. For pure tone playback, pair it with the [`Stop`] register or with another digital input in `Stop` mode.
- `StartAndStop` - A rising edge starts the sound and a falling edge stops the sound. Use this mode for pure tone playback as the stop signal only acts on the sine wave generator.
- `Stop` - A rising edge stops the pure tone that is playing. Use this mode for pure tone playback by pairing it with a [`Start`] register or another digital input in `Start` mode.

[`StartOnDI0`] follows the same rules as [`Start`], whereby values from 2 to 31 select a stored sound index, and values from 32 to 40000 set the frequency of a pure tone in Hz.

There are multiple ways to combine and use the digital input functionality. This example will demonstrate how to configure the digital input to start and stop pure tone playback.

:::workflow
![Set Mode and Sound](../workflows/configuredigitalinputs-setmodeandsound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDI0` - Select `StartAndStop`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `StartOnDI0` - Set the frequency in Hz to start (e.g. `500`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then send a TTL signal from the other device to **IN0**. The 500 Hz tone plays while the signal is high and stops when it goes low.

> [!NOTE]
> Just like sounds triggered from Bonsai, sounds triggered from the digital inputs report [`Start`] and [`Stop`] events. Refer to the [Play sounds](play-sound.md) article to visualize the events.

## Set trigger volume

Sounds triggered from a digital input play at the attenuation stored in the [`AttenuationLeftDI0`] and [`AttenuationRightDI0`] registers, in 0.1 dB steps, independently of the attenuation set for sounds started from Bonsai. The default is 60 (-6 dB) on both channels.

:::workflow
![Set Trigger Volume](../workflows/configuredigitalinputs-triggervolume.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationLeftDI0` - Set the attenuation of the left channel (e.g. `200` = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationRightDI0` - Set the attenuation of the right channel (e.g. `200` = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press <kbd>S</kbd>, and trigger **IN0** again. The sound plays at the new volume.

> [!NOTE]
> These registers only affect sounds triggered from the digital inputs. Use the registers in the [Adjust volume](adjust-volume.md) article to set the volume of sounds started from Bonsai.

## Visualize digital input events

The digital inputs do not report a line status under any of the trigger mode configurations (`Start`, `StartAndStop`, `Stop`). Set the inputs to `Digital` mode to use them as general purpose digital inputs. Rising and falling edges are reported as [`InputState`] events. This example reconfigures **IN0** to `Digital` mode, so the same connection can be used, and demonstrates how to visualize those events.

:::workflow
![Visualize Digital Input Events](../workflows/configuredigitalinputs-visualizeevents.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDI0` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedInputState`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow, press <kbd>D</kbd>, and send a TTL signal to **IN0**. The visualizer will display:

```text
DI0@15346.973344
None@15347.102112
```

The first value lists the inputs that read high, in this case `DI0` on the rising edge and `None` on the falling edge, and the second number is the timestamp on the device clock.

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
[`Stop`]: xref:Harp.SoundCard.Stop
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
