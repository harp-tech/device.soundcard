## SoundCard GUI

The SoundCard GUI provides an easy to use interface to generate and upload waveforms to the SoundCard.

> [!TIP]
> Alternatively, you can play [pure tones](play-sound.md) with the onboard sine wave generator or [upload waveforms in Bonsai](../tutorials/upload-waveform-bonsai.md), which offers greater flexibility and functionality.

Before beginning, follow the audio setup [guide](connections.md#connections) and connect the [USB Mini-B and USB Micro-B](connections.md#connections) cables to the computer. Launch "Harp.SoundCard.App" from the Windows Start menu.

![SoundCard GUI](../images/gui-labelled.svg)

1. Select the port for the SoundCard, and press "Connect". The device details will display on the right side if it is successfully connected.

> [!TIP]
> If you run into an error, check out the troubleshooting [guide](./troubleshooting.md).

2. Select the tab for either pure tone or white noise generation.

3. Adjust the parameters for the sound accordingly. Take note that amplitude can be adjusted as a fraction of full-scale (linear) or in dBFS (logarithmic). Select the radio button for the option you want.

> [!TIP]
> dBFS is the standard unit for audio levels and maps more naturally to perceived loudness.

4. To prevent "pops" and "clicks" from abrupt transitions during sound onset or sound offset, a windowing function can be applied to fade in and out the sound. Select the channels to apply the window to.

5. Adjust the window properties in this panel.

6. Choose the sampling rate and click on "Generate". The choice of sampling rate will determine the range of reproducible frequencies.

7. Select the channels to upload.

8. Configure the sound index to upload and click on "Send to device".

9. Select the sound index and press "Play" to test the generated sound.

10. Save the waveform to a binary file or load a previously saved binary file.

[!INCLUDE [](version-footer.md)]