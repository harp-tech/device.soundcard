## Logging and Analysis

This article covers how data from the device is logged to disk, and how to read and plot the logged data with `harp-python`.

### Log Data

Data from the device is logged by the [`DeviceDataWriter`] operator in the Harp device pattern, which saves raw data from all device registers in the Harp binary format to the folder set in its `Path` property:

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

While the workflow is running, registers are logged as the device produces messages (events and command echoes). Two properties of the [`Device`] operator are also important for logging:

- `DumpRegisters` - Enabled by default, this property logs a read of every register when the device initializes, capturing the initial state of the device at the start of the experiment.
- `Heartbeat` - Disabled by default, enable it to regularly log the device's hardware timestamp.

> [!WARNING]
> The register dump can be used as an approximate start time for the workflow or experiment, but keep in mind that other devices in the workflow may initialize at a different time.

### Analyze Data

The `harp-python` library imports data stored in the Harp binary format as [pandas](https://pandas.pydata.org/) DataFrames, which can then be analyzed with any `pandas` compatible plotting or analysis library.

The following example demonstrates how to read and plot data from the [`PlaySoundOrFrequency`](play-sound.md) register.

> [!NOTE]
> This example requires a Python environment with [harp-python](installation.md#software-packages) and [`matplotlib`](https://matplotlib.org/) installed.

```python
# Import the dependencies
import harp
import matplotlib.pyplot as plt

# Create a device object by loading the saved folder
device = harp.create_reader("./Data/SoundCard.harp")

# Read data from a specific register
play_sound_or_frequency_df = device.PlaySoundOrFrequency.read()

# Inspect DataFrame
print(play_sound_or_frequency_df.head())

# Plot sound onset times
play_sound_or_frequency_df.plot()
plt.show()
```

[!INCLUDE [](version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device
[`DeviceDataWriter`]: xref:Harp.SoundCard.DeviceDataWriter
