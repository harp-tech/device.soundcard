#nullable enable
using System;
using System.IO;

namespace Harp.SoundCard.Usb;

/// <summary>
/// Represents an open USB device.
/// </summary>
internal sealed class UsbDeviceHandle : UsbChildHandle
{
    internal UsbDeviceHandle(UsbDevice device, IntPtr handle)
        : base(device, handle)
    {
    }

    /// <summary>
    /// Reads the string descriptor at the specified index, or returns <see langword="null"/>
    /// if the device has no string at that index.
    /// </summary>
    public string? GetStringDescriptor(byte index)
    {
        if (index == 0)
        {
            return null;
        }

        var buffer = new byte[256];
        var length = LibUsb.libusb_get_string_descriptor_ascii(this, index, buffer, buffer.Length);
        LibUsb.ThrowOnError(length);
        return System.Text.Encoding.ASCII.GetString(buffer, 0, length);
    }

    /// <summary>
    /// Reads the complete descriptor of the active configuration, including the interface and
    /// endpoint descriptors it contains.
    /// </summary>
    public byte[] GetConfigurationDescriptor(uint timeout)
    {
        const int HeaderLength = 9;
        const byte RequestType = LibUsb.EndpointIn | LibUsb.RequestTypeStandard | LibUsb.RecipientDevice;
        const ushort Value = LibUsb.DescriptorTypeConfiguration << 8;

        var header = new byte[HeaderLength];
        var read = LibUsb.libusb_control_transfer(
            this, RequestType, LibUsb.RequestGetDescriptor, Value, 0, header, HeaderLength, timeout);
        LibUsb.ThrowOnError(read);
        if (read < HeaderLength)
        {
            throw new IOException("The USB device returned a truncated configuration descriptor.");
        }

        var totalLength = (ushort)(header[2] | header[3] << 8);
        var descriptor = new byte[totalLength];
        read = LibUsb.libusb_control_transfer(
            this, RequestType, LibUsb.RequestGetDescriptor, Value, 0, descriptor, totalLength, timeout);
        LibUsb.ThrowOnError(read);
        if (read < totalLength)
        {
            throw new IOException("The USB device returned a truncated configuration descriptor.");
        }

        return descriptor;
    }

    /// <summary>
    /// Gets the value of the active configuration, or zero if the device is unconfigured.
    /// </summary>
    public int GetConfiguration()
    {
        LibUsb.ThrowOnError(LibUsb.libusb_get_configuration(this, out var configuration));
        return configuration;
    }

    /// <summary>
    /// Sets the active configuration.
    /// </summary>
    /// <remarks>
    /// Setting the configuration that is already active resets the device, so check the
    /// active configuration first.
    /// </remarks>
    public void SetConfiguration(int configuration)
    {
        LibUsb.ThrowOnError(LibUsb.libusb_set_configuration(this, configuration));
    }

    public void ClaimInterface(int interfaceNumber)
    {
        LibUsb.ThrowOnError(LibUsb.libusb_claim_interface(this, interfaceNumber));
    }

    public void ReleaseInterface(int interfaceNumber)
    {
        LibUsb.libusb_release_interface(this, interfaceNumber);
    }

    public void ClearHalt(byte endpoint)
    {
        LibUsb.ThrowOnError(LibUsb.libusb_clear_halt(this, endpoint));
    }

    public int ControlTransfer(byte requestType, byte request, ushort value, ushort index, byte[]? data, uint timeout)
    {
        var length = (ushort)(data?.Length ?? 0);
        var transferred = LibUsb.libusb_control_transfer(this, requestType, request, value, index, data, length, timeout);
        LibUsb.ThrowOnError(transferred);
        return transferred;
    }

    public int BulkTransfer(byte endpoint, byte[] data, int length, uint timeout)
    {
        LibUsb.ThrowOnError(LibUsb.libusb_bulk_transfer(this, endpoint, data, length, out var transferred, timeout));
        return transferred;
    }

    protected override void ReleaseNativeHandle()
    {
        LibUsb.libusb_close(handle);
    }
}
