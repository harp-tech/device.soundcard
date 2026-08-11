## Errors

This article covers how to resolve common connection errors.

### COM Port Errors

**Q: In Bonsai, running the workflow throws an error "The port `ComX` does not exist."**

A: Either the wrong communications port in the `PortName` property in [`Device`] was selected, or the [USB](connections.md) cable is not properly connected. Try selecting a different communications port and checking the connection.

**Q: In Bonsai, running the workflow throws an error "Access to the port `ComX` is denied"**

A: Only one interface connection to the SoundCard can be opened at one time. Check that Bonsai and the SoundCard GUI are not running simultaneously. Also check that multiple instances of either are not running.

Sometimes, the port can also be locked by a program that did not terminate correctly; restarting the computer fixes it.

Another possible source of the error is that the wrong communications port was selected, try selecting a different communications port for the device.

<!--Reference Style Links -->
[`Device`]: xref:Harp.SoundCard.Device

[!INCLUDE [](version-footer.md)]