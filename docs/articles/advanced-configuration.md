# Advanced configuration

The Sound Card has two microcontrollers: an interface controller that handles Harp messages and the digital and analog lines, and a sound memory controller (PIC32) that stores and plays the waveforms.

This article covers how to send commands to the sound memory controller in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Advanced configuration](../workflows/advancedconfiguration-toplevel.bonsai)
:::

## Delete stored sounds

The sounds uploaded to the device persist across power cycles. Writing `DeleteAllSounds` to the [`Pic32Commands`] register erases every stored sound.

:::workflow
![Delete Stored Sounds](../workflows/advancedconfiguration-deletesounds.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `Pic32CommandsPayload`.
    - `Pic32Commands` - Select `DeleteAllSounds`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to erase the stored sounds. To store new sounds on the device, use the [Sound Card GUI](./soundcard-gui.md) or follow the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial.

> [!WARNING]
> Deleting sounds erases the onboard flash memory and restarts the sound memory controller. This cannot be undone, and the device does not respond to sound commands while the controller restarts.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Pic32Commands`]: xref:Harp.SoundCard.Pic32Commands
