#nullable enable
using System.IO;

namespace Harp.SoundCard.Usb;

/// <summary>
/// The exception that is thrown when a libusb call fails.
/// </summary>
internal sealed class UsbException : IOException
{
    public UsbException(int errorCode)
        : base($"The USB operation failed with {LibUsb.GetErrorName(errorCode)}.")
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Gets the libusb error code, a negative number.
    /// </summary>
    public int ErrorCode { get; }
}
