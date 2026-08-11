## Configure Digital Input

This article will cover how to configure the digital input channels on the SoundCard to trigger sound playback from an external device. Refer to the [connections](./connections.md) article to set up the hardware connection.

The complete workflow is shown below:

:::workflow
![Configure Digital Input Top Level](../workflows/configureDI-toplevel.bonsai)
:::

### Configure Digital Input Trigger

To trigger sound playback from a digital input, set the configuration mode for the selected channel and the index for the stored sound to play. Once set, a rising edge on that digital input channel will play the sound directly on the device.

:::workflow
![Configure Digital Input Trigger](../workflows/configureDI-triggersound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`ConfigureDI0Payload`].
    - `ConfigureDI0` - Select `StartSound`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a second [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`SoundIndexDI0Payload`].
    - `SoundIndexDI0` - Set the sound index for playback.
- Insert another [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then send the TTL signal from the other device to trigger sound playback.

> [!WARNING]
> Only sound index triggering is supported currently.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`ConfigureDI0Payload`]: xref:Harp.SoundCard.CreateConfigureDI0Payload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`SoundIndexDI0Payload`]: xref:Harp.SoundCard.CreateSoundIndexDI0Payload