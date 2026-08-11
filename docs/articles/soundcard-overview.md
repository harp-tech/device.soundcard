## SoundCard

High performance sound card with two output channels using 24 bits DACs at 192kHz sample rate.

![Harp SoundCard](../images/OEPS-SoundCard.png){width=450}

### Key Features

- Internal memory to store sounds, enabling low-latency sound delivery
- Pre-selected sounds can be triggered using an external TTL
- Internal wave generator allows the user to configure a pure tone without loading a sound file

### Specs

- Maximum sampling rate: 192 kHz
- Number of channels: 2
- Bit depth: 24 bits
- Input voltage: 12 V DC
- Output voltage: 2 V rms
- Flash memory: 30 indices, 8 MB per index (2 million samples)
- Sound duration: 10.922 s at 96 kHz sample rate, or 5.461 s at 192 kHz sample rate
- Timestamp resolution: 32 µs
- Digital outputs: 3 (3.3 V or 5 V - default 5 V, adjustable by jumper setting) 
- Digital inputs: 3 (5 V tolerant)
- Analog inputs: 2 (5 V max, 12-bit analog-to-digital conversion)

### Benchmarks

- THD: -111 dB (1 kHz @ 2 V rms)
- Noise Floor: 20 µV rms | -94 dB (20 Hz – 80 kHz)
- SNR: 100 dB | 113 dBA (20 Hz – 80 kHz @ 2 V rms)

### Hardware

| Version | Notes |
| ------- | ----- |
| 2.2 | <ul><li>Minor routing and silkscreen revision</li></ul> |
| 2.1 | <ul><li>Added analog inputs</li></ul> |
| 1.1 | <ul><li>Original board design</li></ul> |

### Firmware

| Version | Notes |
| ------- | ----- |
| 2.2 | <ul><li>Update harp core to 1.13</li><li>Bpod serial communication support dropped</li></ul> |
| 2.1 | <ul><li>New sine wave generator</li><li>Refactor interface scripts to take libusb dependency from NuGet</li><li>Add prototype device schema and interface</li><li>Update interface to use new generators</li></ul> |
| 2.0 | <ul><li>Initial firmware release</li></ul> |

[!INCLUDE [](version-footer.md)]