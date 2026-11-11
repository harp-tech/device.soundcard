#nullable enable
using System;
using System.Linq;

namespace Harp.SoundCard.Usb;

/// <summary>
/// Represents a reference to an attached USB device.
/// </summary>
internal sealed class UsbDevice : UsbChildHandle
{
    internal UsbDevice(UsbContext context, IntPtr device)
        : base(context, device)
    {
    }

    /// <summary>
    /// Reads the device descriptor without opening the device.
    /// </summary>
    public LibUsb.DeviceDescriptor GetDescriptor()
    {
        LibUsb.ThrowOnError(LibUsb.libusb_get_device_descriptor(this, out var descriptor));
        return descriptor;
    }

    /// <summary>
    /// Returns the physical location of the device, as its bus number followed by the port
    /// number at each hub between the root and the device.
    /// </summary>
    /// <remarks>
    /// The location identifies a device even when several devices report the same serial number,
    /// and it stays the same while the device restarts in the same port.
    /// </remarks>
    public string GetLocation()
    {
        const int MaxDepth = 7;
        var ports = new byte[MaxDepth];
        var count = LibUsb.libusb_get_port_numbers(this, ports, ports.Length);
        LibUsb.ThrowOnError(count);
        var bus = LibUsb.libusb_get_bus_number(this);
        return $"{bus}-{string.Join(".", ports.Take(count))}";
    }

    /// <summary>
    /// Opens the device for communication.
    /// </summary>
    public UsbDeviceHandle Open()
    {
        LibUsb.ThrowOnError(LibUsb.libusb_open(this, out var handle));
        return new UsbDeviceHandle(this, handle);
    }

    protected override void ReleaseNativeHandle()
    {
        LibUsb.libusb_unref_device(handle);
    }
}
