# Advanced configuration

The Sound Card has two microcontrollers: an interface controller that handles Harp communication, and a sound memory controller (PIC32) that stores and plays the waveforms.

This article covers how to delete a single stored sound and how to delete all stored sounds in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Advanced configuration](../workflows/advancedconfiguration-toplevel.bonsai)
:::

> [!WARNING]
> Deleting a sound cannot be undone. It also stops any sound or pure tone that is playing.

## Delete sound

The sounds uploaded to the device persist across power cycles. Writing a sound index (2–31) to the [`DeleteSound`] register deletes the sound stored at that index.

:::workflow
![Delete sound](../workflows/advancedconfiguration-deletesound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `DeleteSoundPayload`.
    - `DeleteSound` - Set the index of the sound to delete (e.g. `2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to delete the sound stored at sound index 2. To store a new sound at that index, use the [Sound Card app](./app.md) or follow the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial.

## Delete all sounds

Writing a non-zero value to the [`DeleteAllSounds`] register deletes every sound stored on the device.

:::workflow
![Delete all sounds](../workflows/advancedconfiguration-deleteallsounds.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `DeleteAllSoundsPayload`.
    - `DeleteAllSounds` - Set the value to `1` (any non-zero value deletes the sounds).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>S</kbd> key to delete all the stored sounds.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`DeleteAllSounds`]: xref:Harp.SoundCard.DeleteAllSounds
[`DeleteSound`]: xref:Harp.SoundCard.DeleteSound
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
