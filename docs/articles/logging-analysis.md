# Logging and analysis

This article covers how data from the device is logged to disk, and how to read and plot the logged data using the [`harp-data`](installation.md#software-packages) Python package.

## Log data

Data from the device is logged by the [`DeviceDataWriter`] operator in the Harp device pattern, which saves raw data from all device registers in the Harp binary format to the folder set in its `Path` property:

:::workflow
![Harp Device Pattern](../workflows/harp-devicepattern.bonsai)
:::

While the workflow is running, data is logged as the device executes commands and reports events. Two properties of the [`Device`] operator are also important for logging:

- `DumpRegisters` - Enabled by default, the register dump records the value of every register when the device initializes, along with the Harp timestamp for when the value was read. This is useful for capturing the initial state of the device at the start of the experiment.
- `Heartbeat` - Disabled by default, set this property to `True` to regularly log the device's Harp timestamp.

> [!WARNING]
> The Harp timestamps from the register dump can be used as an approximate start time for the workflow or experiment, but keep in mind that other devices in the workflow may initialize at a different time.

## Analyze data

The [`harp-data`](https://harp-tech.org/python/) package imports data stored in the Harp binary format as [pandas](https://pandas.pydata.org/) DataFrames, which can then be analyzed with any `pandas` compatible plotting or analysis library.

The following example demonstrates how to read and plot data from the [`Start`](play-sound.md) register, which records every sound the device started.

> [!NOTE]
> This example requires a Python environment with [harp-data](installation.md#software-packages) and [`matplotlib`](https://matplotlib.org/) (used by `pandas` as a plotting backend) installed.

```python
# Import the dependencies
import matplotlib.pyplot as plt
from harp import data

# Finds device.yml in the folder, builds the device module, returns a dataset reader
reader = data.open_dataset("SoundCard.harp")

# Lists every register in the reader by name and address
print(reader.contents)

# Load data from a particular register
df = reader.read("Start")   # by name
df = reader.read(32)        # or by address

# Inspect DataFrame
print(df.head())

# Plot the sound index or frequency of every start against its timestamp
df.plot()
plt.show()
```

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device
[`DeviceDataWriter`]: xref:Harp.SoundCard.DeviceDataWriter
