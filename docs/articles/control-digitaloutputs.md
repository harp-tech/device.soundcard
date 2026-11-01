# Control digital outputs

The Sound Card can send a TTL pulse from its digital outputs (**OUT0**–**OUT2**) every time a sound starts, and the outputs can also be controlled directly from Bonsai. Upload a sound to sound index 2 with the [Sound Card GUI](./soundcard-gui.md) or in [Bonsai](../tutorials/upload-waveform-bonsai.md). Refer to the [connections](./ports-connections.md?tabs=digitaloutputs#connections) article to connect **OUT0** to a digital input on an external device, which we will use for the rest of these examples.

This article covers how to send a pulse when a sound starts, set and clear outputs, toggle outputs, and write all outputs at once in Bonsai.

The complete workflow is shown below. Copy and paste it into Bonsai to get started immediately, or build each section by following the step-by-step instructions in the following sections.

:::workflow
![Control digital outputs](../workflows/controldigitaloutputs-toplevel.bonsai)
:::

[!INCLUDE [](../includes/digitaloutputs-load-warning.md)]

## Pulse on sound start

Each digital output line has a configuration mode register that sets what the line does (e.g. [`ConfigureDO0`] for **OUT0**). In `PulseOnStart` mode, the line sends a 400 µs pulse every time a sound or pure tone starts and every time the frequency of the pure tone changes. This applies whether the sound was started from Bonsai or from a digital input.

:::workflow
![Pulse on sound start](../workflows/controldigitaloutputs-signalsound.bonsai)
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

Run the workflow and press <kbd>A</kbd> to set the configuration, then press <kbd>S</kbd> to play sound index 2 and check the TTL pulse on the external device.

## Set and clear outputs

In `Digital` mode, the output line can be set directly (e.g. to control external devices). This example reconfigures **OUT0** to `Digital` mode first with [`ConfigureDO0`], so the same connection can be used. It then demonstrates how to control the output line with [`OutputSet`] and [`OutputClear`].

:::workflow
![Set and clear outputs](../workflows/controldigitaloutputs-setclearoutputs.bonsai)
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

Run the workflow and press <kbd>D</kbd> to enable `Digital` mode, then press <kbd>F</kbd> or <kbd>G</kbd> to set or clear **OUT0** and check the TTL signal on the external device.

> [!NOTE]
> A single command can control several lines at once. To set or clear multiple lines simultaneously, type the names separated by a comma (e.g. `DO1, DO2`) in the `OutputSet` or `OutputClear` field. Lines not specified remain unchanged.

> [!NOTE]
> Software-triggered outputs are subject to operating system and hardware communication latencies (typically 10-15 ms).

## Toggle outputs

[`OutputToggle`] flips the output line between low and high with a single command, so a workflow event can alternate the line without keeping track of its current state.

:::workflow
![Toggle outputs](../workflows/controldigitaloutputs-toggleoutputs.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `H`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputTogglePayload`.
    - `OutputToggle` - Select the line to toggle (e.g. `DO0`).
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>H</kbd> repeatedly to toggle **OUT0** between high and low, then check the TTL signal on the external device.

## Write all outputs

[`OutputState`] writes the state of all digital output lines with a single command. Selected lines are set high and all other lines are set low. Use it to set the whole output bank to a known state, for example when initializing an experiment.

:::workflow
![Write all outputs](../workflows/controldigitaloutputs-writealloutputs.bonsai)
:::

- Insert a [`KeyDown`] operator and set the `Filter` property to `J`.
- Insert a [`CreateMessage`] operator and configure the following properties:
    - `Payload` - Select `OutputStatePayload`.
    - `OutputState` - Select the lines to set high (e.g. `DO0, DO1`). All other lines are set low.
- Insert a [`MulticastSubject`] operator named `SoundCard Commands`.

Run the workflow and press <kbd>J</kbd>, then check the TTL signal on the external device. **OUT0** and **OUT1** will be set to high and **OUT2** will go low.

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
