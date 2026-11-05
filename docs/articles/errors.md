# Errors

This article covers how to resolve common errors on the Sound Card.

## COM port errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist."**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB (Computer)](ports-connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: In Bonsai, running the workflow throws an error "Access to the port `ComX` is denied"**

A: Only one interface connection to the Sound Card can be opened at one time. Check that Bonsai and the Sound Card GUI are not running simultaneously, and that multiple instances of either are not running. Sometimes, the port can also be locked by a program that did not terminate correctly; restarting the computer fixes it.

[!INCLUDE [](../includes/version-footer.md)]

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device
