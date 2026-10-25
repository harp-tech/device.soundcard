## Advanced Configuration

The SoundCard has two microcontrollers: an interface controller that handles Harp messages and the digital and analog lines, and a sound memory controller (PIC32) that stores and plays the waveforms. This article will cover the commands that act on the sound memory controller itself.

The complete workflow is shown below:

:::workflow
![Advanced Configuration Top Level](../workflows/advancedconfiguration-toplevel.bonsai)
:::

### Delete Stored Sounds

The sounds uploaded to the device persist across power cycles. To erase every stored sound, write `DeleteAllSounds` to the [`Pic32Commands`] register.

:::workflow
![Delete Stored Sounds](../workflows/advancedconfiguration-deletesounds.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`Pic32CommandsPayload`].
    - `Pic32Commands` - Select `DeleteAllSounds`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to erase the stored sounds. Refer to the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial to store new sounds on the device.

> [!WARNING]
> Deleting sounds erases the onboard flash memory and restarts the sound memory controller. This cannot be undone, and the device does not respond to sound commands while the controller restarts.

> [!WARNING]
> The `EnableBootloader` and `DisableBootloader` values of the [`Pic32Commands`] register are used to update the sound memory controller firmware, as described in the [Installation](installation.md#firmware) article. Sending `EnableBootloader` stops sound playback until the controller is restarted with `DisableBootloader`.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Pic32Commands`]: xref:Harp.SoundCard.Pic32Commands
[`Pic32CommandsPayload`]: xref:Harp.SoundCard.CreatePic32CommandsPayload
