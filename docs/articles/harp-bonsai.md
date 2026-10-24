## Bonsai

Bonsai is a visual reactive programming language for building interactive experiments and processing data streams in real time. It supports a growing ecosystem of hardware and software packages that are commonly used in neuroscience. This article will cover how to set up the SoundCard in Bonsai.

>[!TIP]
> More information on Bonsai can be found in the official [documentation](https://bonsai-rx.org/docs/).

### First Steps

We will use a simple example to connect and test the device in Bonsai. This example will trigger the playback of a sound 5 seconds after the workflow is run.

Before beginning:
- Connect both [USB](connections.md) cables to the computer.
- Upload a sound to the sound index 2 using the [SoundCard GUI](./upload-waveform-gui.md).
- Launch "Bonsai" from the Windows Start menu.
- Hover over the workflow cell below, and click on the "Copy" icon on the top right.
- Paste the workflow into Bonsai.

:::workflow
![SoundCard First Steps](../workflows/soundcard-firststeps.bonsai)
:::

> [!TIP]
> The [Harp device pattern](https://harp-tech.org/articles/operators.html#device-pattern) will initialize the device, log data, and provide hooks to send commands as well as receive messages from the SoundCard using the [Harp communication protocol](https://harp-tech.org/protocol/BinaryProtocol-8bit.html). If your workflow does not look like the one above, make sure that the [Harp.SoundCard](./installation.md#software-packages) package is installed.

- Click on the [`SoundCard (Device)`] operator and set the `PortName` property to the communications port for the device (e.g. COM8).
- Click on the [`SoundCardDataWriter (DeviceDataWriter)`] operator and set the `Path` property for the name and location of the save file (e.g. `Data\SoundCard.harp`).
- Press the "Start" button in Bonsai to run the workflow.

If the connection is successful, you will hear the sound play. If you do not hear anything, or an error appears in Bonsai, check out the [troubleshooting](troubleshooting.md) section.

Otherwise, the device is ready to use! We suggest going through the "Bonsai Workflows" section if you are not familiar with using Harp devices in Bonsai.

Alternatively, if you have experience with Harp devices, you can check the [register table](xref:Harp.SoundCard) in the reference to access the device functionality directly.

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`SoundCard (Device)`]: xref:Harp.SoundCard.Device
[`SoundCardDataWriter (DeviceDataWriter)`]: xref:Harp.SoundCard.DeviceDataWriter
