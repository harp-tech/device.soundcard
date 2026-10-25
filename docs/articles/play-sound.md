## Play Sounds

The SoundCard supports playback of waveforms stored in its onboard memory. It also includes an internal sine wave generator for pure tones. The following article will walk through how to play these sounds in Bonsai using key presses as an example trigger.

The complete workflow is shown below:

:::workflow
![Play Sound Top Level](../workflows/playsound-toplevel.bonsai)
:::

> [!WARNING]
> You can find and add these operators to the workflow from the Bonsai [Toolbox](https://bonsai-rx.org/docs/articles/editor.html?tabs=mouse-controls#toolbox). Make sure to use the device-specific versions, e.g. `Device (Harp.SoundCard)` instead of `Device (Harp)`. If correctly selected, the names of these operators in the workflow panel will change to reflect either the name of the device or the selected register/payload.

### Play Sound Index

Sounds can be played from the `SoundCard` onboard memory by specifying the sound index in the [`Start`] register payload. Values from 2 to 31 select a sound index, and values from 32 upwards are interpreted as a frequency for the sine wave generator instead. Sound duration and amplitude are determined by the properties of the stored waveform.

:::workflow
![Play Sound Index Keydown](../workflows/playsound-indexkeydown.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator to construct a [`HarpMessage`] command and configure these properties:
    - `Payload` - Select [`StartPayload`] from the property dropdown menu.
    - `Start` - Set the index of the sound you want to play from the `SoundCard` onboard memory (2–31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to play the sound.

> [!TIP]
> Starting a new sound index while another is playing replaces it immediately. The [`Stop`] register does not stop sound indices, so to end a sound early, start another index, for example a short silent waveform.

> [!WARNING]
> Starting a sound index that has not been uploaded to the device does nothing: the command is accepted but no sound plays and no [`Start`] event is emitted. Refer to the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial to store sounds on the device.

### Play Pure Tone Frequency

Pure tones can be played from the `SoundCard` internal sine wave generator by specifying the frequency in Hz in the [`Start`] register payload (32–40000). Tones play continuously at the current channel attenuation until stopped.

:::workflow
![Play Sound Frequency](../workflows/playsound-frequency.bonsai)
:::

- Insert a [`KeyDown`] source and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`StartPayload`].
    - `Start` - Set the desired frequency in Hz (e.g. 1000).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] source and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`StopPayload`].
    - `Stop` - Set the value to 1 (any non-zero value stops playback).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press the <kbd>S</kbd> key to play the tone, and press the <kbd>D</kbd> key to stop playback.

> [!WARNING]
> The [`Stop`] register can only be used to stop playback from the internal sine wave generator, not sounds from the onboard memory. Closing the connection to the device also stops the sine wave generator.

> [!WARNING]
> Writing the frequency that is already playing to the [`Start`] register emits no [`Start`] event, and the device then rejects every further [`Start`] command with an error until you write to the [`Stop`] register. Write a different frequency to change the tone, or stop it first.

### Alternative: Play Sound with Timer

You can replace [`KeyDown`] with other operators to trigger sound playback with other events in Bonsai, for instance a [`Timer`].

:::workflow
![Play Sound Index Timer](../workflows/playsound-indextimer.bonsai)
:::

- Insert a [`Timer`] operator and set the `DueTime` property to the number of seconds to wait before playing the sound (e.g. 5 seconds).
- Insert a [`CreateMessage`] operator and configure these properties:
    - `Payload` - Select [`StartPayload`].
    - `Start` - Set the index of the sound you want to play from the `SoundCard` onboard memory (2–31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and the sound will play after 5 seconds.

### Visualize Sound Events

The SoundCard reports every sound start as a [`Start`] event and every stop as a [`Stop`] event. This includes sounds started or stopped from the digital inputs, so the events are the record of what the device actually played and when. To visualize them, filter the [`HarpMessages`] coming from the device and decode them using the workflow below:

:::workflow
![Play Sound Visualize Events](../workflows/playsound-visualizeevents.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`. This will listen to [`HarpMessages`] broadcast from the [`PublishSubject`] named `SoundCard Events` in the Harp device pattern.
- Insert a [`FilterMessageType`] operator and configure the `MessageType` property to `Event`. This drops the replies to the [`Start`] and [`Stop`] commands sent by this workflow, which carry the same register addresses as the events.
- Insert a [`Parse`] operator and configure the `Register` property to `TimestampedStart`.
- Insert a [`VisualizerWindow`] operator. This will automatically open a window displaying the parsed events when the workflow starts.

In a separate branch from the [`FilterMessageType`] operator:

- Insert a [`Parse`] operator and configure the `Register` property to `TimestampedStop`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow and press the <kbd>A</kbd> key to play a sound. The first visualizer will display:

```text
2@15346.973344
```

The first number corresponds to the `Payload` value, in this case the sound index or frequency that started, and the second number is the timestamp on the device clock. Press <kbd>S</kbd> and then <kbd>D</kbd> to see a [`Stop`] event in the second visualizer. Its payload is the last value written to the [`Stop`] register.

> [!NOTE]
> The [`Start`] event is emitted when the device confirms that playback began, about 0.2 to 0.3 ms after the command. A new [`Start`] command sent before that confirmation is rejected with an error.

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
[`PublishSubject`]: xref:Bonsai.Reactive.PublishSubject
[`Start`]: xref:Harp.SoundCard.Start
[`StartPayload`]: xref:Harp.SoundCard.CreateStartPayload
[`Stop`]: xref:Harp.SoundCard.Stop
[`StopPayload`]: xref:Harp.SoundCard.CreateStopPayload
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`Timer`]: xref:Bonsai.Reactive.Timer
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
