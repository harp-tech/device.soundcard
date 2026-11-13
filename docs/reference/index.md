---
uid: Harp.SoundCard.Device
title: Registers
---

# Sound Card device

<div class="text-secondary d-flex flex-wrap column-gap-4">
  <dl><div><dt>WhoAmI</dt><dd>1280</dd></div></dl>
  <dl><div><dt>Log files</dt><dd><code>SoundCard_&lt;address&gt;.bin</code></dd></div></dl>
</div>

Use the [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) to initialize the device, log data, and send commands to and receive messages from the Sound Card.

:::workflow
![Harp device pattern](../workflows/harp-devicepattern.bonsai)
:::

Check out the following in-depth guides to learn how to access the device functionality with the `Harp.SoundCard` package:

- [Play sounds](../articles/play-sound.md)
- [Adjust volume](../articles/adjust-volume.md)
- [Configure digital inputs](../articles/configure-digital-inputs.md)
- [Control digital outputs](../articles/control-digital-outputs.md)
- [Acquire analog data](../articles/acquire-analog-data.md)
- [Manage stored sounds](../articles/manage-stored-sounds.md)

## Registers

The core registers implemented by every Harp device, such as `WhoAmI` and `OperationControl`, are described in the [Device Registers and Operation specification](https://harp-tech.org/protocol/Device.html).

[!INCLUDE [](../../artifacts/docs/registers/registers.md)]
