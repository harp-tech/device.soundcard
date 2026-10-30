# Audio Amplifier

The Harp Sound Card Audio Amplifier is a peripheral for the Sound Card that amplifies one output channel with low distortion to drive a speaker. Refer to the [connections](../ports-connections.md?tabs=audio#connections) article to set up the amplifier.

![Harp Audio Amplifier](../../images/audioamplifier-board.jpg){width=450}

## Key features

- Unity gain, so the Sound Card output level is reproduced at the speaker.
- Flat frequency response from 20 Hz to 80 kHz.

## Specs

- Number of channels: 1
- Input voltage: 2 × 12 V DC (positive and negative supply)
- Maximum output: 4 W @ 8 Ω / 2 W @ 4 Ω
- Speaker output (with Sound Card): 0.5 W @ 8 Ω / 1 W @ 4 Ω
- Compatible speakers: 4 Ω impedance or higher; the XT25SC90-04 (Peerless by Tymphany) offers a good frequency response up to 80 kHz
- Input connector: RCA input from sound card
- Output connector: Banana plug to speaker

## Benchmarks

- SNR: 119 dB (A-weighted, 80 kHz bandwidth)
- Spectral variation: <0.1 dB (20 Hz–80 kHz)
- Distortion: <0.03% @ 1 W

## Hardware

| Version | Compatible Sound Card | Notes |
| ------- | -------------------- | ----- |
| 2.0 | >= v1.0  | <ul><li>Initial version</li></ul> |

Assembled units are available from the [Open Ephys store](https://open-ephys.org/harp), or build your own using the hardware design files in the [Audio Amplifier](https://github.com/harp-tech/peripheral.audioamp) repository.

[!INCLUDE [](../../includes/version-footer.md)]
