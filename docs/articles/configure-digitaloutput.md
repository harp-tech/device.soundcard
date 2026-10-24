## Configure Digital Output

This article will cover how to configure the digital output channels on the SoundCard to emit TTL signals on sound playback or in response to other workflow events. Refer to the [connections](./connections.md) article to set up the hardware connection.

The complete workflow is shown below:

:::workflow
![Configure Digital Output Top Level](../workflows/configureDO-toplevel.bonsai)
:::

### Signal Sound Playback

To signal sound playback on a digital output, set the configuration mode for the selected channel. Once set, the digital output channel will be driven high for sound onset and low for sound offset.

:::workflow
![Configure DO Signal Sound](../workflows/configureDO-signalsound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDO0Payload`].
    - `ConfigureDO0` - Select `HighWhenSound`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then play a sound and check the TTL signal on the other device.

> [!WARNING]
> Only sound index reporting on `DO0` is supported currently.

### Signal Workflow Events

You can also control the digital output lines directly to signal other workflow events in Bonsai.

:::workflow
![Configure DO Signal Workflow Events](../workflows/configureDO-signalworkflowevents.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputSetPayload`] to drive the line high.
    - `OutputSet` - Select the channel to drive (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputClearPayload`] to drive the line low.
    - `OutputClear` - Select the channel to drive (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`OutputTogglePayload`] to toggle the line between low and high.
    - `OutputToggle` - Select the channel to toggle (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>S</kbd>, <kbd>D</kbd>, or <kbd>F</kbd> to set, clear, or toggle `DO0`, then check the TTL signal on the other device.

> [!NOTE]
> Software-triggered outputs are subject to operating system and hardware communication latencies.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`ConfigureDO0Payload`]: xref:Harp.SoundCard.CreateConfigureDO0Payload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`OutputSetPayload`]: xref:Harp.SoundCard.CreateOutputSetPayload
[`OutputClearPayload`]: xref:Harp.SoundCard.CreateOutputClearPayload
[`OutputTogglePayload`]: xref:Harp.SoundCard.CreateOutputTogglePayload