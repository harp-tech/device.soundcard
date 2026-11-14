# Errors

This article covers how to resolve common errors on the Sound Card.

## COM port errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist".**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB (Computer)](ports-connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: Running the Bonsai workflow or connecting to the device in the Sound Card app throws an error "Access to the port `ComX` is denied".**

A: Only one interface connection to the Sound Card can be opened at one time. Check that Bonsai and the [Sound Card app](app.md) are not running simultaneously, and that multiple instances of either are not running. Sometimes, the port can also be locked by a program that did not terminate correctly; restarting the computer fixes it.

## Sound upload errors

**Q: Uploading a sound or reading sound metadata throws an error "Sound Card not detected. Check that its soundbank USB port is connected to the computer.".**

A: The computer cannot find the sound memory controller. Check that the [USB (Soundbank)](ports-connections.md?tabs=power-and-pc#connections) cable is connected and that the device is powered.

**Q: Uploading a sound throws an error "The Sound Card is playing a sound at 192 kHz and cannot receive a new sound.".**

A: The device cannot receive a new sound while a sound with a 192 kHz sample rate is playing. Stop playback with the [`Stop`] register, or wait for the sound to end, and upload it again.

## No sound

**Q: The sound does not play.**

A: Check the [AUDIO](ports-connections.md#indicators) LED when the sound starts:

- If the LED stays off, the sound index may be empty. Starting an empty sound index does nothing, so check the stored sounds as described in the [Manage stored sounds](manage-stored-sounds.md) article.
- If the LED turns on but nothing is heard, check the amplifier, speaker and cables by following the [audio connection](ports-connections.md?tabs=audio#connections) instructions. Pure tones play only on the **Right Channel**.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device
[`Stop`]: xref:Harp.SoundCard.Stop
