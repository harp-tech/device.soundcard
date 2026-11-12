## About

`Harp.SoundCard.App` is a desktop app for configuring and testing [Harp SoundCard](https://harp-tech.org/device.soundcard) devices and for uploading sounds to them.

As with other Harp devices, the SoundCard device can also be used from [Bonsai](https://bonsai-rx.org/).

## How to Use

The app is distributed as a .NET tool. Installing it requires the [.NET SDK](https://dotnet.microsoft.com/download), version 8 or later:

```text
dotnet tool install -g Harp.SoundCard.App
```

Once installed, the app is launched from a terminal:

```text
harp.soundcard
```

To connect to a device as soon as the app opens, pass its serial port:

```text
harp.soundcard --port COM3
```

### Linux

The app accesses the serial port, so the user account needs to belong to the `dialout` group or equivalent. On Ubuntu and Fedora the command to add an account to that group is:

```text
sudo usermod -a -G dialout <USERNAME>
```

## Additional Documentation

For additional documentation and examples, refer to the [official Harp SoundCard documentation](https://harp-tech.org/device.soundcard).

## Feedback & Contributing

`Harp.SoundCard.App` is released as open source under the [MIT license](https://licenses.nuget.org/MIT). Bug reports and contributions are welcome at [the GitHub repository](https://github.com/harp-tech/device.soundcard).
