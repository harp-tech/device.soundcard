## Adjust Volume

By default, the [`PlaySoundOrFrequency`] register will play the sound at the amplitude of the stored waveform or at maximum amplitude for the pure tone frequency generator. This article will demonstrate how to adjust the volume by setting the attenuation level with other registers.

> [!NOTE]
> The maximum volume for playback of stored waveforms depends on its recorded amplitude. Upload waveforms at full 24-bit depth to maximize the volume.

> [!WARNING]
> Attenuation is set on a per-channel basis, does not stack (each command replaces the previous value), and persists. Any sound you play afterwards with the [`PlaySoundOrFrequency`] register will play at the most recently set channel attenuation level until you change it.

The complete workflow is shown below:

:::workflow
![Adjust Volume Top Level](../workflows/adjustvolume-toplevel.bonsai)
:::

### Attenuate and Play Sound

Use the [`AttenuationAndPlaySoundOrFreq`] register as a drop-in replacement for the [`PlaySoundOrFrequency`] register to start sound playback and set the channel attenuation at the same time in 0.1 dB steps.

:::workflow
![Attenuate Channels and Play Sound](../workflows/adjustvolume-attenuation.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`AttenuationAndPlaySoundOrFreqPayload`].
    - `AttenuationAndPlaySoundOrFreq` - Click on the dialog button in the property grid to open the member collection editor. Add three members:
        - The sound index or pure tone frequency to be played (e.g. 2).
        - The attenuation of the left channel (e.g. 200 = -20 dB).
        - The attenuation of the right channel (e.g. 200 = -20 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to play the sound at reduced volume.

> [!TIP]
> Just like the [`PlaySoundOrFrequency`] register, pure tone playback must be stopped explicitly via the [`Stop`] register.

### Attenuate Channels

Use the [`AttenuationBoth`] register to adjust the channel volume before or during sound playback.

:::workflow
![Attenuate Both Channels](../workflows/adjustvolume-attenuateboth.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`AttenuationBothPayload`].
    - `AttenuationBoth` - Click on the dialog button in the property grid to open the member collection editor. Add two members:
        - The left channel attenuation (e.g. 100 = -10 dB).
        - The right channel attenuation (e.g. 100 = -10 dB).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to play the sound with the [`AttenuationAndPlaySoundOrFreq`] register and <kbd>S</kbd> to dynamically change the volume while the sound is playing. If you are using the same values as the examples above, the sound will become louder (going from more attenuation to less).

> [!TIP]
> Use the [`AttenuationLeft`] or [`AttenuationRight`] channels to set the attenuation level for each channel independently.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`AttenuationAndPlaySoundOrFreq`]: xref:Harp.SoundCard.AttenuationAndPlaySoundOrFreq
[`AttenuationAndPlaySoundOrFreqPayload`]: xref:Harp.SoundCard.CreateAttenuationAndPlaySoundOrFreqPayload
[`AttenuationLeft`]: xref:Harp.SoundCard.AttenuationLeft
[`AttenuationRight`]: xref:Harp.SoundCard.AttenuationRight
[`AttenuationBoth`]: xref:Harp.SoundCard.AttenuationBoth
[`AttenuationBothPayload`]: xref:Harp.SoundCard.CreateAttenuationBothPayload
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`PlaySoundOrFrequency`]: xref:Harp.SoundCard.PlaySoundOrFrequency
[`Stop`]: xref:Harp.SoundCard.Stop