# Bonsai

Bonsai is a visual reactive programming language for building interactive experiments and processing data streams in real time. It supports a growing ecosystem of hardware and software packages that are commonly used in neuroscience. This article will cover how to set up the Sound Card in Bonsai.

> [!TIP]
> More information on Bonsai can be found in the official [documentation](https://bonsai-rx.org/docs/).

## First steps

We will use an introductory example to get started with the device in Bonsai. This example plays a sound 5 seconds after the workflow starts. We revisit this example in more detail in the "Examples" section.

Before beginning:

- Connect the [USB (Computer)](ports-connections.md) cable to the computer.
- Upload a sound to sound index 2 with the [Sound Card GUI](./soundcard-gui.md) or in [Bonsai](../tutorials/upload-waveform-bonsai.md).
- Launch "Bonsai" from the Windows Start menu.
- Hover over the workflow cell below, and click on the "Copy" icon on the top right.
- Paste the workflow into Bonsai.

:::workflow
![Sound Card First Steps](../workflows/soundcard-firststeps.bonsai)
:::

> [!TIP]
> If your workflow does not look like the one above, make sure that the [Harp.SoundCard and Bonsai.StarterPack](./installation.md#software-packages) packages are installed.

- Click on the [`SoundCard (Device)`] operator and set the `PortName` property to the communications port for the device (e.g. `COM8`).
- Click on the [`SoundCardDataWriter (DeviceDataWriter)`] operator and set the `Path` property for the name and location of the save folder (e.g. `SoundCard.harp`).
- Press the "Start" button in Bonsai to run the workflow.

After 5 seconds, the sound plays and the **AUDIO** LED on the board turns on for its duration.

The device is ready to use! If, instead, you hear nothing or an error appears in Bonsai, check out the [troubleshooting](errors.md) section.

Next, we suggest going through the "Examples" section if you are not familiar with using Harp devices in Bonsai.

Alternatively, if you have experience with Harp devices, you can check the [register table](xref:Harp.SoundCard) in the reference to access the device functionality directly.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`SoundCard (Device)`]: xref:Harp.SoundCard.Device
[`SoundCardDataWriter (DeviceDataWriter)`]: xref:Harp.SoundCard.DeviceDataWriter
