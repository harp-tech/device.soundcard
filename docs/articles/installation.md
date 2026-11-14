# Installation

This page covers the software you'll need to interact with the Sound Card, as well as how to update the firmware on the device.

## Software packages

These steps are only required the first time you connect the device to a new computer, and you can install just the packages for the functionality you need.

# [Driver](#tab/driver)

The WinUSB driver is required to upload waveforms to the onboard sound memory bank.

![Zadig binding WinUSB driver](../images/installation-zadig.jpg){width=575}

- Download and launch [Zadig](https://zadig.akeo.ie/).
- Connect the [USB (Soundbank)](ports-connections.md) cable to the computer.
- Select "Harp Sound Card" from the list. If the device is not available, go to "Options" > "List All Devices".
- Select the "WinUSB" driver and click "Install Driver".

# [App](#tab/app)

The [Sound Card app](app.md) is a desktop app for generating and uploading waveforms to the device without using Bonsai. It is distributed as a [.NET tool](https://learn.microsoft.com/dotnet/core/tools/global-tools).

![Sound Card app](../images/soundcard-app.png){width=600}

- Download and install the [.NET SDK](https://dotnet.microsoft.com/download), version 8 or later.
- Install the Sound Card app from a terminal with:

```cmd
dotnet tool install -g Harp.SoundCard.App
```

To update the Sound Card app to the latest release, run `dotnet tool update -g Harp.SoundCard.App`.

> [!NOTE]
> Alternatively, waveforms can be [generated and uploaded in Bonsai](../tutorials/upload-waveform-bonsai.md).

# [Bonsai](#tab/bonsai)

[Bonsai](https://bonsai-rx.org/) is a visual reactive programming language that provides flexible and comprehensive control of the Sound Card.

![Bonsai package manager](../images/installation-packagemanager.jpg){width=600}

- Download and install [Bonsai](https://bonsai-rx.org/docs/articles/installation.html).
- Launch Bonsai and install the `Harp.SoundCard` package by searching for it in the [Bonsai package manager](https://bonsai-rx.org/docs/articles/packages.html).
- Install the `Bonsai.StarterPack` package to follow along with the examples in this user guide.

# [Python](#tab/python)

The [Harp](https://harp-tech.org/python/) library provides a Python interface for controlling Harp devices and [loading](logging-analysis.md) recorded data. To install the full library, install it in a Python environment with:

```cmd
pip install harp
```

To install the interface for the Sound Card:

```cmd
pip install harp-soundcard
```

To install only the package necessary for loading data:

```cmd
pip install harp-data
```

> [!NOTE]
> Substitute `uv add` for `pip install` if you are using the [uv](https://docs.astral.sh/uv/) package manager.

***

## Firmware update

New features are added and bugs are fixed with firmware updates which are published on the [release page](https://github.com/harp-tech/device.soundcard/releases) in the device GitHub repository. Each release contains `.hex` files tagged with the firmware (`fw`) and hardware (`hw`) version. For instance, `SoundCard-fw3.0-harp1.15-hw2.2-ass0.hex` is firmware v3.0 for a hardware v2.2 Sound Card. Download these two hex files from the "Assets" section under each release, making sure that the `hw` version matches the [hardware version](device.md#hardware) of the device:

- `SoundCard-*.hex` - Firmware for the 8-bit device interface microcontroller.
- `SoundCard.PIC32-*.hex` - Firmware for the 32-bit sound memory microcontroller.

> [!TIP]
> The hardware version is printed on the board silkscreen (e.g. `harp sound card v2.2`).

The firmware is updated from a terminal with the [Harp Toolkit](https://harp-tech.org/toolkit/), a command-line tool for working with Harp devices. Like the Sound Card app, it is distributed as a .NET tool.

- Download and install the [.NET SDK](https://dotnet.microsoft.com/download), version 8 or later.
- Install the toolkit from a terminal with:

```cmd
dotnet tool install -g Harp.Toolkit
```

- Navigate to the folder containing the two downloaded `.hex` files, and update the firmware with the serial port of the device and the file for the device interface microcontroller. The sound memory microcontroller file is loaded automatically at the same time:

```cmd
harp.toolkit update --port COM3 SoundCard-fw3.0-harp1.15-hw2.2-ass0.hex
```

The toolkit checks that the file matches the device before writing it, and waits for the device to restart with the new firmware. Run `harp.toolkit list` to find the serial port of the device. The toolkit [firmware update](https://harp-tech.org/toolkit/articles/update.html) documentation describes every option.

[!INCLUDE [](../includes/version-footer.md)]
