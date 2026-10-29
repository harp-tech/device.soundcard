# Control digital outputs

The Sound Card can emit a TTL pulse on its digital outputs (**OUT0**–**OUT2**) every time a sound starts, and the outputs can also be controlled directly from Bonsai. Refer to the [connections](./ports-connections.md?tabs=digitaloutputs#connections) article to connect **OUT0** to a digital input on another device, which we will use for the rest of these examples.

This article covers how to signal sound playback on an output, set and clear outputs, toggle outputs, and write all outputs at once in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Control digital outputs](../workflows/controldigitaloutputs-toplevel.bonsai)
:::

[!INCLUDE [](../includes/digitaloutputs-load-warning.md)]

## Signal sound playback

Each digital output line has a configuration mode register that sets what the line does (e.g. [`ConfigureDO0`] for **OUT0**). In `PulseOnStart` mode, the line emits a 400 µs pulse every time a sound or pure tone starts and every time the frequency of the pure tone changes, whether the sound was started from Bonsai or from a digital input.

:::workflow
![Signal Sound Playback](../workflows/controldigitaloutputs-signalsound.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `A`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigureDO0Payload`.
    - `ConfigureDO0` - Select `PulseOnStart`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `S`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `StartPayload`.
    - `Start` - Set the index of the sound to play (e.g. `2`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>A</kbd> to set the configuration, then press <kbd>S</kbd> to play sound index 2 and check the TTL pulse on the other device.

## Set and clear outputs

In `Digital` mode, the output line can be controlled directly to signal other workflow events in Bonsai. This example reconfigures **OUT0** to `Digital` mode first with the [`ConfigureDO0`] register, so the same connection can be used. It then demonstrates how to control the output line with [`OutputSet`] and [`OutputClear`].

:::workflow
![Set and Clear Outputs](../workflows/controldigitaloutputs-setclearoutputs.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `D`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `ConfigureDO0Payload`.
    - `ConfigureDO0` - Select `Digital`.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `F`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputSetPayload`.
    - `OutputSet` - Select the line to set high (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

In a separate branch:

- Insert a [`KeyDown`] operator and set the `Filter` property to `G`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputClearPayload`.
    - `OutputClear` - Select the line to clear low (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>D</kbd> to enable `Digital` mode, then press <kbd>F</kbd> or <kbd>G</kbd> to set or clear **OUT0** and check the TTL signal on the other device.

> [!NOTE]
> A single command can control several lines at once. To set or clear multiple lines simultaneously, type the names separated by a comma (e.g. `DO1, DO2`) in the `OutputSet` or `OutputClear` field. Lines not specified remain unchanged.

> [!NOTE]
> Software-triggered outputs are subject to operating system and hardware communication latencies (typically 10-15 ms).

## Toggle outputs

The [`OutputToggle`] register flips the output line between low and high with a single command, so a workflow event can alternate the line without keeping track of its current state.

:::workflow
![Toggle Outputs](../workflows/controldigitaloutputs-toggleoutputs.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `H`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputTogglePayload`.
    - `OutputToggle` - Select the line to toggle (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>H</kbd> repeatedly to toggle **OUT0** between high and low, then check the TTL signal on the other device.

## Write all outputs

The [`OutputState`] register writes the state of all digital output lines with a single command. Selected lines are set high and all other lines are set low. Use it to set the whole output bank to a known state, for example when initializing an experiment.

:::workflow
![Write All Outputs](../workflows/controldigitaloutputs-writealloutputs.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `J`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputStatePayload`.
    - `OutputState` - Select the lines to set high (e.g. `DO0, DO1`). All other lines are set low.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>J</kbd>. **OUT0** goes high and **OUT1** and **OUT2** go low.

> [!WARNING]
> [`OutputState`] acts on every digital output line of the Sound Card. To change only selected lines, use [`OutputSet`], [`OutputClear`], or [`OutputToggle`].

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`ConfigureDO0`]: xref:Harp.SoundCard.ConfigureDO0
[`CreateMessage`]: xref:Harp.SoundCard.CreateMessage
[`KeyDown`]: xref:Bonsai.Windows.Input.KeyDown
[`MulticastSubject`]: xref:Bonsai.Expressions.MulticastSubject
[`OutputClear`]: xref:Harp.SoundCard.OutputClear
[`OutputSet`]: xref:Harp.SoundCard.OutputSet
[`OutputState`]: xref:Harp.SoundCard.OutputState
[`OutputToggle`]: xref:Harp.SoundCard.OutputToggle
