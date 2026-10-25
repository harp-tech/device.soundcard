## Control Digital Outputs

This article will cover how to configure the digital output channels on the SoundCard to emit TTL pulses on sound playback, and how to drive them directly in response to other workflow events. Refer to the [connections](./connections.md) article to set up the hardware connection.

The complete workflow is shown below:

:::workflow
![Configure Digital Output Top Level](../workflows/configureDO-toplevel.bonsai)
:::

### Signal Sound Playback

To signal sound playback on a digital output, set the [`ConfigureDO0`] register to `PulseOnStart`. Once set, **OUT0** emits a 400 µs pulse every time a sound or pure tone starts and every time the frequency of the pure tone changes, whether the sound was started from Bonsai or from a digital input.

:::workflow
![Configure DO Signal Sound](../workflows/configureDO-signalsound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDO0Payload`].
    - `ConfigureDO0` - Select `PulseOnStart`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then play a sound and check the TTL pulse on the other device.

> [!NOTE]
> **OUT0** is in `PulseOnStart` mode at power-up, and **OUT1** and **OUT2** are in `Digital` mode. The workflow sets the mode explicitly so that the example does not depend on what ran before it.

### Signal Workflow Events

You can also control the digital output lines directly to signal other workflow events in Bonsai. This example drives **OUT1**, leaving **OUT0** to signal sound playback.

:::workflow
![Configure DO Signal Workflow Events](../workflows/configureDO-signalworkflowevents.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDO1Payload`].
    - `ConfigureDO1` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a second [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputSetPayload`] to set the line high.
    - `OutputSet` - Select the channel to drive (e.g. `DO1`).
- Insert another [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputClearPayload`] to clear the line low.
    - `OutputClear` - Select the channel to drive (e.g. `DO1`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputTogglePayload`] to toggle the line between low and high.
    - `OutputToggle` - Select the channel to toggle (e.g. `DO1`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>S</kbd>, <kbd>D</kbd>, or <kbd>F</kbd> to set, clear, or toggle **OUT1**, then check the TTL signal on the other device.

> [!NOTE]
> A single command can drive several lines at once. To set, clear, or toggle multiple lines simultaneously, type the names separated by a comma (e.g. `DO1, DO2`) in the `OutputSet`, `OutputClear`, or `OutputToggle` field. Lines not specified remain unchanged.

> [!WARNING]
> Set a line to `Digital` mode before driving it. A line in `PulseOnStart` mode that was set high is cleared 400 µs after the next sound starts.

> [!NOTE]
> Software-triggered outputs are subject to operating system and hardware communication latencies.

### Set All Outputs

Use the [`OutputState`] register to write the state of all three digital output lines with a single command.

:::workflow
![Configure DO Set Output State](../workflows/configureDO-setoutputstate.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `G`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputStatePayload`].
    - `OutputState` - Select the lines to set high (e.g. `DO1, DO2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>G</kbd>. **OUT1** and **OUT2** go high and **OUT0** goes low.

> [!WARNING]
> [`OutputState`] acts on every digital output line of the SoundCard. Any line not specified in the payload will be cleared low. Make sure to list every line that should stay high (e.g. `DO1, DO2`). To change only selected lines, use [`OutputSet`], [`OutputClear`], or [`OutputToggle`].

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`ConfigureDO0`]: xref:Harp.SoundCard.ConfigureDO0
[`ConfigureDO0Payload`]: xref:Harp.SoundCard.CreateConfigureDO0Payload
[`ConfigureDO1Payload`]: xref:Harp.SoundCard.CreateConfigureDO1Payload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`OutputClear`]: xref:Harp.SoundCard.OutputClear
[`OutputClearPayload`]: xref:Harp.SoundCard.CreateOutputClearPayload
[`OutputSet`]: xref:Harp.SoundCard.OutputSet
[`OutputSetPayload`]: xref:Harp.SoundCard.CreateOutputSetPayload
[`OutputState`]: xref:Harp.SoundCard.OutputState
[`OutputStatePayload`]: xref:Harp.SoundCard.CreateOutputStatePayload
[`OutputToggle`]: xref:Harp.SoundCard.OutputToggle
[`OutputTogglePayload`]: xref:Harp.SoundCard.CreateOutputTogglePayload
