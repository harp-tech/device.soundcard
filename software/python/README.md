# harp-soundcard

The Python interface for the [Harp SoundCard](https://harp-tech.org/device.soundcard) device. It carries the register classes for the device, the payload and enumeration types those registers use, and a `REGISTER_MAP` pairing every address with its register.

## Installation

```text
pip install harp-soundcard
```

## Reading and writing registers

Register classes are passed to `read` and `write` on an open device transport, such as the serial transport in [Harp Python](https://harp-tech.org/python/).

```python
from harp import serial
from harp.device import client, soundcard

SERIAL_PORT = "/dev/ttyUSB0"  # or "COMx" in Windows, where "x" is the serial port number

with serial.open_device(client.Device, port=SERIAL_PORT) as device:
    print("device:", soundcard.DEVICE_NAME, "WhoAmI:", soundcard.WHO_AM_I)
    print("digital inputs:", device.read(soundcard.InputState).payload)
```

`open_dataset` in `harp.data` takes this module as its second argument, and decodes recorded data through the same register classes.

Uploading sounds to the device is not part of this package. Sounds are uploaded with the SoundCard app or the .NET interface.

## Feedback and contributing

`harp-soundcard` is released as open source under the MIT license. Bug reports and contributions are welcome at [the GitHub repository](https://github.com/harp-tech/device.soundcard).
