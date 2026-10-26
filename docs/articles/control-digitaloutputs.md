# Control digital outputs

The Sound Card emits a TTL pulse on **OUT0**, **OUT1**, or **OUT2** every time a sound starts, and the same lines can be driven directly from Bonsai. Refer to the [connections](./ports-connections.md?tabs=digitaloutputs#connections) article to set up a digital input of another device on **OUT0** and **OUT1**, which we will use for the rest of these examples.

This article covers how to signal sound playback on an output, drive the outputs from workflow events, and set all outputs at once in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Control digital outputs](../workflows/controldigitaloutputs-toplevel.bonsai)
:::

## Signal sound playback

The [`ConfigureDO0`] register set to `PulseOnStart` makes **OUT0** emit a 400 µs pulse every time a sound or pure tone starts and every time the frequency of the pure tone changes, whether the sound was started from Bonsai or from a digital input.

:::workflow
![Signal Sound Playback](../workflows/controldigitaloutputs-signalsound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDO0` - Select `PulseOnStart`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then play a sound and check the TTL pulse on the other device.

> [!NOTE]
> **OUT0** is in `PulseOnStart` mode at power-up, and **OUT1** and **OUT2** are in `Digital` mode. The workflow sets the mode explicitly so that the example does not depend on what ran before it.

## Signal workflow events

The [`OutputSet`], [`OutputClear`], and [`OutputToggle`] registers drive the output lines directly to signal other workflow events in Bonsai. This example drives **OUT1**, leaving **OUT0** to signal sound playback, and sets its mode to `Digital` first with the [`ConfigureDO1`] register.

:::workflow
![Signal Workflow Events](../workflows/controldigitaloutputs-signalworkflowevents.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `ConfigureDO1` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `OutputSet` - Select the line to set high (e.g. `DO1`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `OutputClear` - Select the line to clear low (e.g. `DO1`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `OutputToggle` - Select the line to toggle between low and high (e.g. `DO1`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>S</kbd>, <kbd>D</kbd>, or <kbd>F</kbd> to set, clear, or toggle **OUT1**, then check the TTL signal on the other device.

> [!NOTE]
> A single command can drive several lines at once. To set, clear, or toggle multiple lines simultaneously, type the names separated by a comma (e.g. `DO1, DO2`) in the `OutputSet`, `OutputClear`, or `OutputToggle` field. Lines not specified remain unchanged.

> [!WARNING]
> Set a line to `Digital` mode before driving it. A line in `PulseOnStart` mode that was set high is cleared 400 µs after the next sound starts.

> [!NOTE]
> Software-triggered outputs are subject to operating system and hardware communication latencies.

## Set all outputs

The [`OutputState`] register writes the state of all three digital output lines with a single command.

:::workflow
![Set All Outputs](../workflows/controldigitaloutputs-setoutputstate.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `G`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `OutputState` - Select the lines to set high (e.g. `DO1, DO2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>G</kbd>. **OUT1** and **OUT2** go high and **OUT0** goes low.

> [!WARNING]
> [`OutputState`] acts on every digital output line of the Sound Card. Any line not specified in the payload will be cleared low. Make sure to list every line that should stay high (e.g. `DO1, DO2`). To change only selected lines, use [`OutputSet`], [`OutputClear`], or [`OutputToggle`].

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`ConfigureDO0`]: xref:Harp.SoundCard.ConfigureDO0
[`ConfigureDO1`]: xref:Harp.SoundCard.ConfigureDO1
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`OutputClear`]: xref:Harp.SoundCard.OutputClear
[`OutputSet`]: xref:Harp.SoundCard.OutputSet
[`OutputState`]: xref:Harp.SoundCard.OutputState
[`OutputToggle`]: xref:Harp.SoundCard.OutputToggle
