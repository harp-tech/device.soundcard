# Harp.SoundCard.App

The Harp SoundCard GUI application can be used to configure and test Harp SoundCard devices.

The GUI was developed using [.NET](https://dotnet.microsoft.com/), [AvaloniaUI](https://avaloniaui.net/) with ReactiveUI
and makes direct use of the [Bonsai.Harp](https://github.com/bonsai-rx/harp) library.

As with other Harp devices, the SoundCard can also be used in [Bonsai](https://bonsai-rx.org/).

## Installation

Go to the Releases page to download the latest version for your Operating System.

Currently, there are x64 and arm64 builds for Windows, Mac and Linux.

Portable builds are also available.

### Linux

Since the application accesses the serial port, your user needs to be on the `dialout` group or equivalent.

There might be other alternatives to this, but at least on Ubuntu and Fedora que command that you need to run to add
your user to the `dialout` group is:

```sh
sudo usermod -a -G dialout <USERNAME>
```

### macOS

On macOS, you will need to install `libusb` using Homebrew:

```sh
brew install libusb
```

After that, you will need to set the `DYLD_LIBRARY_PATH` environment variable to the location of the `homebrew`
library paths.

You can set this permanently by adding the following line to your shell profile (e.g., `.bash_profile`, `.zshrc`, etc.):

```sh
export DYLD_LIBRARY_PATH="/usr/local/lib:$DYLD_LIBRARY_PATH"
```

## For developers

### Build Windows installer using NSIS manually

- Install NSIS 3 on your Windows machine
- Build and publish the application using the .NET 8 SDK command-line tools
  
  ```sh
  dotnet publish Harp.SoundCard.App.sln -r win-x64 -f net8.0 --self-contained /p:Configuration=Release
  ```

- Run makesis to generate the installer

    ```sh
    makensis.exe /DVERSION_MAJOR=1 /DVERSION_MINOR=2 /DVERSION_BUILD=0 /DARCHITECTURE=win-x64 .\Harp.SoundCard.nsi
    // Adjust the version numbers and architecture as needed (e.g., arm64)
    ```

- The installer will be available at `.\bin\Release\net8.0\<ARCHITECTURE>\Harp.SoundCard.App.vx.x.x-<ARCHITECTURE>.exe`

### Build .app image for macOS

The project uses dotnet-bundle (https://github.com/egramtel/dotnet-bundle) to generate a .app image for macOS.

To build the .app image, run the following commands on the solution folder:

```sh
dotnet restore -r osx-x64 -p:TargetFramework=net8.0
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true -p:TargetFramework=net8.0
```

### Build tar.gz package for Linux (either in Linux or WSL)

To build the tar.gz package, run the following commands on the solution folder:

First you need to make sure that the [`dotnet-packaging`](https://github.com/quamotion/dotnet-packaging) tool is
installed

Run the following commands to install the tool:

```sh
dotnet tool install --global dotnet-tarball
```

Then run the following commands to build the tar.gz package:

```sh
dotnet restore -r linux-x64 -p:TargetFramework=net8.0
dotnet msbuild -p:RuntimeIdentifier=linux-x64 -property:Configuration=Release -p:UseAppHost=true -p:TargetFramework=net8.0 /t:CreateTarball
```
