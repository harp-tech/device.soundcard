#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Harp.SoundCard.Usb;

/// <summary>
/// Represents a libusb session, which owns every device and handle created from it.
/// </summary>
internal sealed class UsbContext : SafeHandle
{
    internal UsbContext()
        : base(IntPtr.Zero, ownsHandle: true)
    {
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// Creates a new libusb session.
    /// </summary>
    /// <exception cref="DllNotFoundException">The libusb library is not installed.</exception>
    public static UsbContext Create()
    {
        var context = new UsbContext();
        LibUsb.ThrowOnError(LibUsb.libusb_init(out var handle));
        context.SetHandle(handle);
        return context;
    }

    /// <summary>
    /// Returns every USB device currently attached to the system.
    /// </summary>
    /// <remarks>
    /// The caller owns the returned devices and must dispose each of them.
    /// </remarks>
    public UsbDevice[] GetDevices()
    {
        var count = LibUsb.libusb_get_device_list(this, out var list);
        LibUsb.ThrowOnError((int)count);
        try
        {
            var devices = new UsbDevice[count];
            for (int i = 0; i < devices.Length; i++)
            {
                var device = Marshal.ReadIntPtr(list, i * IntPtr.Size);
                devices[i] = new UsbDevice(this, LibUsb.libusb_ref_device(device));
            }

            return devices;
        }
        finally
        {
            LibUsb.libusb_free_device_list(list, unrefDevices: 1);
        }
    }

    protected override bool ReleaseHandle()
    {
        LibUsb.libusb_exit(handle);
        return true;
    }
}
