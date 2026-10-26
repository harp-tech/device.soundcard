# Installation

This page covers the software you'll need to interact with the Sound Card, as well as how to update the firmware on the device.

# Software packages

These steps are only required the first time you connect the device to a new computer, and you can install just the packages for the functionality you need.

# [Driver](#tab/driver)

The WinUSB driver is required to upload waveforms to the onboard sound memory bank.

![Zadig binding WinUSB driver](../images/installation-zadig.jpg){width=575}

- Download and launch [Zadig](https://zadig.akeo.ie/).
- Connect the [USB (Soundbank)](ports-connections.md) cable to the computer.
- Select "Harp Sound Card" from the list. If the device is not available, go to "Options" > "List All Devices".
- Select the "WinUSB" driver and click "Install Driver".

# [Sound Card GUI](#tab/soundcardgui)

The Sound Card GUI offers a graphical interface for [generating and uploading waveforms](soundcard-gui.md).

![Sound Card GUI](../images/installation-gui.jpg){width=600}

- Download and install the [Sound Card GUI](https://github.com/fchampalimaud/device.soundcard/releases).

> [!NOTE]
> Alternatively, waveforms can be [generated and uploaded in Bonsai](../tutorials/upload-waveform-bonsai.md).

# [Bonsai](#tab/bonsai)

[Bonsai](https://bonsai-rx.org/) is a visual reactive programming language that provides flexible and comprehensive control of the Sound Card.

![Bonsai Package Manager](../images/installation-packagemanager.jpg){width=600}

- Download and install [Bonsai](https://bonsai-rx.org/docs/articles/installation.html).
- Launch Bonsai and install the `Harp.SoundCard` package by searching for it in the [Bonsai package manager](https://bonsai-rx.org/docs/articles/packages.html). Tick the "Show advanced" checkbox if it does not appear.
- Install the `Bonsai.StarterPack` package to follow along with the examples in this user guide.

# [Python](#tab/python)

The [Harp](https://harp-tech.org/python/) library provides a Python interface for controlling Harp devices and [loading](logging-analysis.md) recorded data. To install the full library, install it in a Python environment with:

```cmd
pip install harp
```

To install only the package necessary for loading data:

```cmd
pip install harp-data
```

> [!NOTE]
> Substitute `uv add` for `pip install` if you are using the [uv](https://docs.astral.sh/uv/) package manager.

***

# Firmware

New features are added and bugs are fixed with firmware updates which are published on the [release page](https://github.com/harp-tech/device.soundcard/releases) in the Sound Card repository. Each firmware release is tagged with a `fw` version prefix (e.g. `fw3.0-harp1.15`), and the `.hex` files can be found in the "Assets" section. Download the two files that match the [hardware version](soundcard.md#hardware) of your device:

- `SoundCard-*.hex` - Firmware for the 8-bit device interface microcontroller.
- `SoundCard.PIC32-*.hex` - Firmware for the 32-bit sound memory microcontroller.

> [!TIP]
> The hardware version is printed on the board silkscreen and should match the `hwX.X` portion of the firmware filename.

To update the firmware:

1. Install the [LabVIEW Runtime](https://bitbucket.org/fchampalimaud/downloads/downloads/Runtime-1.0.zip).
2. Install the [Harp Convert to CSV](https://bitbucket.org/fchampalimaud/downloads/downloads/Harp_Convert_To_CSV_v1.8.3.zip) application.
3. Open the Harp Convert to CSV application and write *bootloader* in the "List" box on the "Options" tab.
4. Select the COM port of the device.
5. Select the firmware file to load for each of the two microcontrollers.

> [!NOTE]
> This method should be replaced by either [Harp Toolkit](https://harp-tech.org/toolkit/) or [Harp Regulator](https://github.com/harp-tech/harp-regulator) when they become available and support the Sound Card.

[!INCLUDE [](../includes/version-footer.md)]
