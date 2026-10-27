# Adjust volume

By default, the Sound Card plays stored sounds at the amplitude of the stored waveform and pure tones at maximum amplitude. The volume can be adjusted by setting an attenuation for the output channels. 

Refer to the [connections](./ports-connections.md?tabs=audio#connections) article to set up an amplifier and speaker on an output channel. Then, upload a sound to sound index 2 with the [Sound Card GUI](./soundcard-gui.md), or in [Bonsai](../tutorials/upload-waveform-bonsai.md). We will use this setup for the rest of these examples.

This article covers how to set the attenuation when starting a sound and how to change it while a sound is playing in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Adjust volume](../workflows/adjustvolume-toplevel.bonsai)
:::

## Attenuate and play sound

[`AttenuationStart`] starts playback of a sound index or pure tone and sets the attenuation of the left and right channels at the same time,  in 0.1 dB steps. Each command replaces the previous value rather than adding to it. The attenuation also stays in effect: any sound you play afterwards with [`Start`] plays at that level until you change it.

:::workflow
![Attenuate and Play Sound](../workflows/adjustvolume-attenuation.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationStart` - Click on the dialog button in the property grid to open the member collection editor. Add three members:
        - The sound index or pure tone frequency to be played (e.g. `2`).
        - The attenuation of the left channel (e.g. `200` = -20 dB).
        - The attenuation of the right channel (e.g. `200` = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to play the sound at reduced volume.

> [!NOTE]
> The maximum volume for playback of stored waveforms depends on the recorded amplitude. Upload waveforms at full 24-bit depth to maximize the volume.

> [!TIP]
> Just like the [`Start`] register, pure tone playback must be stopped explicitly via the [`Stop`] register. The device reports an [`AttenuationStart`] command as a [`Start`] event.

## Attenuate channels

Use [`AttenuationBoth`] to set the attenuation of both channels before or during sound playback.

:::workflow
![Attenuate Channels](../workflows/adjustvolume-attenuateboth.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `AttenuationBoth` - Click on the dialog button in the property grid to open the member collection editor. Add two members:
        - The left channel attenuation (e.g. `100` = -10 dB).
        - The right channel attenuation (e.g. `100` = -10 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to play the sound with [`AttenuationStart`] and <kbd>S</kbd> to change the volume with [`AttenuationBoth`] while the sound is playing. With the values above, the sound becomes louder, going from more attenuation to less.

> [!NOTE]
> These registers set the volume for sounds started from Bonsai. Sounds triggered from the digital inputs use their own attenuation registers, as described in the [Configure digital inputs](configure-digitalinputs.md) article.

> [!NOTE]
> Use the [`AttenuationLeft`] or [`AttenuationRight`] registers to set the attenuation of each channel independently.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`AttenuationBoth`]: xref:Harp.SoundCard.AttenuationBoth
[`AttenuationLeft`]: xref:Harp.SoundCard.AttenuationLeft
[`AttenuationRight`]: xref:Harp.SoundCard.AttenuationRight
[`AttenuationStart`]: xref:Harp.SoundCard.AttenuationStart
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Start`]: xref:Harp.SoundCard.Start
[`Stop`]: xref:Harp.SoundCard.Stop
