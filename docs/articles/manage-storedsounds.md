# Manage stored sounds

In addition to controlling sound playback, Bonsai can interface with the device to read sound metadata and manage stored sounds. Refer to the [connections](./ports-connections.md?tabs=powerandpc#connections) article to connect the **USB (Soundbank)** port to the computer, which we will use to communicate with the sound memory controller.

This article covers how to read the metadata of the stored sounds, delete a single stored sound, and delete all stored sounds in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Manage stored sounds](../workflows/managestoredsounds-toplevel.bonsai)
:::

## Read sound metadata

The [`ReadSoundMetadata`] operator reports the metadata for each stored sound.

:::workflow
![Read sound metadata](../workflows/managestoredsounds-readsoundmetadata.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`SelectMany`] operator.
- Insert a [`VisualizerWindow`] operator.

Double-click the [`SelectMany`] operator to open its nested workflow, and add the operator shown below:

:::workflow
![ReadSoundMetadata operator](../workflows/managestoredsounds-readsoundmetadataoperator.bonsai)
:::

- Insert a [`ReadSoundMetadata`] operator and connect it to the `WorkflowOutput` node in place of the `Source1` node. Configure the following properties:
    - `DeviceIndex` - Set the index of the `SoundCard` device to read. If no index is specified, the first `SoundCard` will be used.
    - `SoundIndex` - Set the index of the sound to read (2–31). If no index is specified, every stored sound is read.

Run the workflow and press the <kbd>A</kbd> key. The visualizer displays one line for each stored sound, for instance:

```text
2: 500Hz2sTone (384000 samples, SampleRate96000Hz)
```

The first number is the sound index, followed by the sound name, the length in samples, and the sample rate. The length counts the samples of both channels, so a 2 s sound at 96 kHz has 384000 samples.

## Delete sound

The sounds uploaded to the device persist across power cycles. Writing a sound index (2–31) to the [`DeleteSound`] register deletes the sound stored at that index.

[!INCLUDE [](../includes/deletesound-warning.md)]

:::workflow
![Delete sound](../workflows/managestoredsounds-deletesound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `DeleteSoundPayload`.
    - `DeleteSound` - Set the index of the sound to delete (e.g. `2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>S</kbd> key to delete the sound stored at sound index 2. To store a new sound at that index, use the [Sound Card app](./app.md) or follow the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial.

## Delete all sounds

Writing a non-zero value to the [`DeleteAllSounds`] register deletes every sound stored on the device.

[!INCLUDE [](../includes/deletesound-warning.md)]

:::workflow
![Delete all sounds](../workflows/managestoredsounds-deleteallsounds.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `DeleteAllSoundsPayload`.
    - `DeleteAllSounds` - Set the value to `1` (any non-zero value deletes the sounds).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>D</kbd> key to delete all the stored sounds.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`DeleteAllSounds`]: xref:Harp.SoundCard.DeleteAllSounds
[`DeleteSound`]: xref:Harp.SoundCard.DeleteSound
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`ReadSoundMetadata`]: xref:Harp.SoundCard.ReadSoundMetadata
[`SelectMany`]: xref:Bonsai.Reactive.SelectMany
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
