# Play sounds

The Sound Card plays waveforms stored in its onboard memory and pure tones from an internal sine wave generator on the **Left Channel** and **Right Channel** ports. Refer to the [connections](./ports-connections.md?tabs=audio#connections) article to set up an amplifier and speaker on an output channel, which we will use for the rest of these examples.

This article covers how to play a stored sound, play and stop a pure tone, and visualize sound events in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Play sounds](../workflows/playsound-toplevel.bonsai)
:::

## Play sound index

Sounds are played from the onboard memory by writing the sound index to the [`Start`] register. Values from 2 to 31 select a sound index, and values from 32 upwards are interpreted as a frequency for the sine wave generator instead. Sound duration and amplitude are determined by the properties of the stored waveform.

:::workflow
![Play Sound Index](../workflows/playsound-indexkeydown.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Start` - Set the index of the sound you want to play from the onboard memory (2–31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press the <kbd>A</kbd> key to play the sound.

> [!TIP]
> Starting a new sound index while another is playing replaces it immediately. The [`Stop`] register does not stop sound indices, so to end a sound early, start another index, for example a short silent waveform.

> [!WARNING]
> Starting a sound index that has not been uploaded to the device does nothing: the command is accepted but no sound plays and no [`Start`] event is emitted. Refer to the [Upload waveforms with Bonsai](../tutorials/upload-waveform-bonsai.md) tutorial to store sounds on the device.

## Play pure tone frequency

Pure tones are played from the internal sine wave generator by writing the frequency in Hz to the [`Start`] register (32–40000). Tones play continuously at the current channel attenuation until stopped.

:::workflow
![Play Pure Tone Frequency](../workflows/playsound-frequency.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Start` - Set the desired frequency in Hz (e.g. 1000).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Stop` - Set the value to 1 (any non-zero value stops playback).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press the <kbd>S</kbd> key to play the tone, and press the <kbd>D</kbd> key to stop playback.

> [!WARNING]
> The [`Stop`] register can only be used to stop playback from the internal sine wave generator, not sounds from the onboard memory. Closing the connection to the device also stops the sine wave generator.

> [!WARNING]
> Writing the frequency that is already playing to the [`Start`] register emits no [`Start`] event, and the device then rejects every further [`Start`] command with an error until you write to the [`Stop`] register. Write a different frequency to change the tone, or stop it first.

## Visualize sound events

The Sound Card reports every sound start as a [`Start`] event and every stop as a [`Stop`] event. This includes sounds started or stopped from the digital inputs, so the events are the record of what the device actually played and when. The workflow below will show you how to filter, decode and visualize the events:

:::workflow
![Visualize Sound Events](../workflows/playsound-visualizeevents.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`.
- Insert a [`FilterMessageType`] operator and set the `MessageType` property to `Event`. This drops the replies to the [`Start`] and [`Stop`] commands sent by this workflow, which carry the same register addresses as the events.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedStart`.
- Insert a [`VisualizerWindow`] operator.

In a separate branch from the [`FilterMessageType`] operator:

- Insert a [`Parse`] operator and set the `Register` property to `TimestampedStop`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow and press the <kbd>A</kbd> key to play a sound. The first visualizer will display:

```text
2@15346.973344
```

The first number corresponds to the `Payload` value, in this case the sound index or frequency that started, and the second number is the timestamp on the device clock. Press <kbd>S</kbd> and then <kbd>D</kbd> to see a [`Stop`] event in the second visualizer. Its payload is the last value written to the [`Stop`] register.

> [!NOTE]
> The [`Start`] event is emitted when the device confirms that playback began, about 0.2 to 0.3 ms after the command. A new [`Start`] command sent before that confirmation is rejected with an error.

## Alternative: play sound with Timer

You can replace [`KeyDown`] with other operators to trigger sound playback with other events in Bonsai, for instance a [`Timer`].

:::workflow
![Play Sound Timer](../workflows/playsound-timer.bonsai)
:::

- Insert a [`Timer`] operator and set the `DueTime` property to the number of seconds to wait before playing the sound (e.g. 2 seconds).
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Start` - Set the index of the sound you want to play from the onboard memory (2–31).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and the sound will play after 2 seconds.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`FilterMessageType`]: xref:Bonsai.Harp.FilterMessageType
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`Parse`]: xref:Harp.SoundCard.Parse
[`Start`]: xref:Harp.SoundCard.Start
[`Stop`]: xref:Harp.SoundCard.Stop
[`SubscribeSubject`]: xref:Bonsai.Expressions.SubscribeSubject
[`Timer`]: xref:Bonsai.Reactive.Timer
[`VisualizerWindow`]: xref:Bonsai.Design.VisualizerWindow
