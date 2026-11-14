# Play sounds

The Sound Card plays waveforms stored in its onboard memory and pure tones from an internal sine wave generator. Upload a sound to sound index 2 with the [Sound Card app](./app.md) or in [Bonsai](../tutorials/upload-waveform-bonsai.md), which we will use for these examples.

[!INCLUDE [](../includes/puretone-rightchannel-note.md)]

This article covers how to play and stop a stored sound, play and stop a pure tone, and visualize sound events in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Play sounds](../workflows/playsound-toplevel.bonsai)
:::

## Play sound index

Play sounds from the onboard sound bank by writing the sound index to the [`Start`] register. Values from 2 to 31 select a sound index, and values from 32 upwards are interpreted as a frequency for the sine wave generator instead (covered in the next section). Sound duration and amplitude are determined by the properties of the stored waveform.

A stored sound plays to the end unless it is stopped early. Writing a non-zero value to the [`Stop`] register ends the sound that is playing, once the audio already queued for output has played.

:::workflow
![Play sound index](../workflows/playsound-indexkeydown.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StartPayload`.
    - `Start` - Set the index of the sound you want to play from the onboard memory (e.g. `2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StopPayload`.
    - `Stop` - Set the value to `1` (any non-zero value stops playback).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press the <kbd>A</kbd> key to play the sound, and press the <kbd>S</kbd> key to stop it early.

> [!TIP]
> Starting another sound index replaces the one that is playing, and starting an empty sound index does not interrupt it. A pure tone, covered in the next section, started while a stored sound is playing waits and starts when the sound ends. Until then, the device refuses further [`Start`] commands.

## Play pure tone

Pure tones are played from the internal sine wave generator by writing the frequency in Hz (32–40000) to [`Start`]. A tone plays continuously until a [`Stop`] command is sent.

:::workflow
![Play pure tone](../workflows/playsound-frequency.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StartPayload`.
    - `Start` - Set the desired frequency in Hz (e.g. `500`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StopPayload`.
    - `Stop` - Set the value to `1` (any non-zero value stops playback).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow, press the <kbd>D</kbd> key to play the tone, and press the <kbd>F</kbd> key to stop it.

> [!NOTE]
> Closing the connection to the device also stops any sound or pure tone that is playing.

## Visualize sound events

The Sound Card reports every sound start as a [`Start`] event, and every sound stop as a [`Stop`] event. This includes sounds started or stopped from the [digital inputs](./configure-digital-inputs.md). A stored sound that plays to the end reports no [`Stop`] event, and neither does a stop command sent while nothing is playing. The workflow below will show you how to filter, decode and visualize the events:

:::workflow
![Visualize sound events](../workflows/playsound-visualizeevents.bonsai)
:::

- Insert a [`SubscribeSubject`] operator named `SoundCard Events`.
- Insert a [`FilterMessageType`] operator and set the `MessageType` property to `Event`. The device responds to each [`Start`] and [`Stop`] command with a `Write` reply when it receives the command and an `Event` message when it is executed. Filtering for `Event` keeps the execution message and timestamps.
- Insert a [`Parse`] operator and set the `Register` property to `TimestampedStart`.
- Insert a [`VisualizerWindow`] operator.

In a separate branch from the [`FilterMessageType`] operator:

- Insert a [`Parse`] operator and set the `Register` property to `TimestampedStop`.
- Insert a [`VisualizerWindow`] operator.

Run the workflow and press the <kbd>D</kbd> key to play a pure tone. The first visualizer will display:

```text
500@350.214528
```

The first number is the [`Start`] sound index or frequency that was played, and the second number is the timestamp on the device clock.

Press the <kbd>F</kbd> key to stop the tone. The second visualizer will display:

```text
1@352.880416
```

The first number is the non-zero value we used for [`Stop`], and the second number is the timestamp on the device clock when the [`Stop`] command was executed.

## Alternative: play sound with Timer

You can replace [`KeyDown`] with other operators to play sounds with other triggers in Bonsai, for instance a [`Timer`] to send triggers after a set time has elapsed.

:::workflow
![Alternative: play sound with Timer](../workflows/playsound-timer.bonsai)
:::

- Insert a [`Timer`] operator and configure the following properties:
    - `DueTime` - Set the number of seconds to wait after the workflow starts to play the sound (e.g. `00:00:02` for two seconds).
    - `Period` - Leave this unspecified so that the [`Timer`] only triggers once.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StartPayload`.
    - `Start` - Set the index of the sound you want to play from the onboard memory (e.g. `2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow. The sound plays 2 seconds after the workflow starts.

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
