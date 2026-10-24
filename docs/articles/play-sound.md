## Play Sounds

The SoundCard supports playback of waveforms stored in its onboard memory. It also includes an internal sine wave generator for pure tones. The following article will walk through how to play these sounds in Bonsai using key presses as an example trigger.

The complete workflow is shown below:

:::workflow
![Play Sound Top Level](../workflows/playsound-toplevel.bonsai)
:::

> [!WARNING]
> You can find and add these operators to the workflow from the Bonsai [Toolbox](https://bonsai-rx.org/docs/articles/editor.html?tabs=mouse-controls#toolbox). Make sure to use the device-specific versions, e.g. `Device (Harp.SoundCard)` instead of `Device (Harp)`. If correctly selected, the names of these operators in the workflow panel will change to reflect either the name of the device or the selected register/payload.

### Play Sound Index

Sounds can be played from the `SoundCard` onboard memory by specifying the sound index in the [`PlaySoundOrFrequency`] register payload. Sound duration and amplitude are determined by the properties of the stored waveform.

:::workflow
![Play Sound Index Keydown](../workflows/playsound-indexkeydown.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator to construct a [`HarpMessage`] command and configure these properties:
    - `Payload` - Select [`PlaySoundOrFrequencyPayload`] from the property dropdown menu.
    - `PlaySoundOrFrequency` - Set the index of the sound you want to play from the `SoundCard` onboard memory (2-31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to play the sound.

> [!TIP]
> You can terminate the sound early by playing an empty sound index.

### Play Pure Tone Frequency

Pure tones can be played from the `SoundCard` internal sine wave generator by specifying the frequency in the [`PlaySoundOrFrequency`] register payload. Sounds will be played continuously at the device's max amplitude until stopped.

:::workflow
![Play Sound Frequency](../workflows/playsound-frequency.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`PlaySoundOrFrequencyPayload`].
    - `PlaySoundOrFrequency` - Set the desired frequency in Hz (e.g. 1000).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`StopPayload`].
    - `Stop` - Set the value to 1 (any non-zero value stops playback).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press the <kbd>S</kbd> key to play the sound, and press the <kbd>D</kbd> key to stop playback.

> [!WARNING]
> The [`Stop`] register can only be used to stop playback from the internal sine wave generator, not sounds from the onboard memory.

### Alternative: Play Sound with Timer

You can replace [`KeyDown`] with other operators to trigger sound playback with other events in Bonsai, for instance a [`Timer`].

:::workflow
![Play Sound Index Timer](../workflows/playsound-indextimer.bonsai)
:::

- Insert a [`Timer`] operator and set the `DueTime` property to the number of seconds to wait before playing the sound (e.g. 5 seconds).
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`PlaySoundOrFrequencyPayload`].
    - `PlaySoundOrFrequency` - Set the index of the sound you want to play from the `SoundCard` onboard memory (2-31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and the sound will play after 5 seconds.

### Visualize Sound Events

To visualize which sound was played and when, we can filter the [`HarpMessages`] coming from the device and decode them using the workflow below:

:::workflow
![Play Sound Visualize Events](../workflows/playsound-visualizeevents.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`. This will listen to [`HarpMessages`] broadcast from the [`PublishSubject`] named `SoundCard Events` in the Harp device pattern.
- Insert a [`FilterMessageType`] operator and configure the `MessageType` property to `Event`.
- Insert a [`Parse`] operator and configure the `Register` property to `TimestampedPlaySoundOrFrequency`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

Run the workflow and press the <kbd>A</kbd> key to play a sound. The visualizer will display:

```text
2@15346.973344
```

The first number corresponds to the `Payload` value, in this case the sound index or frequency, and the second number is the timestamp on the device clock.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`Device`]: xref:Harp.SoundCard.Device
[`FilterMessageType`]: xref:Bonsai.Harp.FilterMessageType
[`HarpMessage`]: xref:Bonsai.Harp.HarpMessage
[`HarpMessages`]: xref:Bonsai.Harp.HarpMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`PlaySoundOrFrequency`]: xref:Harp.SoundCard.PlaySoundOrFrequency
[`PlaySoundOrFrequencyPayload`]: xref:Harp.SoundCard.CreatePlaySoundOrFrequencyPayload
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`Stop`]: xref:Harp.SoundCard.Stop
[`StopPayload`]: xref:Harp.SoundCard.CreateStopPayload
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`Timer`]: xref:Bonsai.Reactive.Timer
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
