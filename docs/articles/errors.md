# Errors

This article covers how to resolve common errors on the Sound Card.

## COM port errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist."**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB (Computer)](ports-connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: In Bonsai, running the workflow throws an error "Access to the port `ComX` is denied"**

A: Only one interface connection to the Sound Card can be opened at one time. Check that Bonsai and the Sound Card GUI are not running simultaneously, and that multiple instances of either are not running. Sometimes, the port can also be locked by a program that did not terminate correctly; restarting the computer fixes it.

## Device errors

**Q: A [`Start`] command plays nothing, or Bonsai reports an error reply from the device.**

A: One possible reason is that the sound index has not been uploaded to the device: the command is accepted but nothing plays. Another is that the device is still confirming a previous start, which takes about 0.3 ms, or that the frequency written is the one already playing; in both cases the device rejects further [`Start`] commands until a [`Stop`] command is written. Write to the [`Stop`] register and try again.

**Q: Sounds triggered from a digital input play at a different volume than sounds started from Bonsai.**

A: Sounds triggered from the digital inputs use their own attenuation registers, which default to -6 dB. Refer to the [Configure digital inputs](configure-digitalinputs.md) article to set them.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device
[`Start`]: xref:Harp.SoundCard.Start
[`Stop`]: xref:Harp.SoundCard.Stop
