#nullable enable
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Harp.SoundCard.Usb;

/// <summary>
/// Provides the subset of the libusb 1.0 API used to talk to USB devices that have no serial port.
/// </summary>
internal static class LibUsb
{
#if NETFRAMEWORK
    const string LibraryName = "libusb-1.0.dll";
#else
    const string LibraryName = "libusb-1.0";
    static readonly string[] WindowsCandidates = { "libusb-1.0.dll" };
    static readonly string[] LinuxCandidates = { "libusb-1.0.so.0", "libusb-1.0.so" };
    static readonly string[] MacCandidates =
    {
        "libusb-1.0.dylib",
        "libusb-1.0.0.dylib",
        "/opt/homebrew/lib/libusb-1.0.dylib",
        "/usr/local/lib/libusb-1.0.dylib",
        "/opt/local/lib/libusb-1.0.dylib"
    };
#endif

    public const byte EndpointIn = 0x80;
    public const byte RequestTypeStandard = 0x00;
    public const byte RequestTypeVendor = 0x40;
    public const byte RecipientDevice = 0x00;
    public const byte RecipientInterface = 0x01;
    public const byte RequestGetDescriptor = 0x06;
    public const byte DescriptorTypeConfiguration = 0x02;

    public const int ErrorAccess = -3;
    public const int ErrorNotFound = -5;
    public const int ErrorTimeout = -7;
    public const int ErrorPipe = -9;
    public const int ErrorNotSupported = -12;

#if !NETFRAMEWORK
    static LibUsb()
    {
        NativeLibrary.SetDllImportResolver(typeof(LibUsb).Assembly, ResolveLibrary);
    }

    static IntPtr ResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != LibraryName)
        {
            return IntPtr.Zero;
        }

        var candidates = OperatingSystem.IsWindows() ? WindowsCandidates
            : OperatingSystem.IsMacOS() ? MacCandidates
            : LinuxCandidates;
        foreach (var candidate in candidates)
        {
            if (NativeLibrary.TryLoad(candidate, assembly, searchPath, out var handle))
            {
                return handle;
            }
        }

        return IntPtr.Zero;
    }
#endif

    public static void ThrowOnError(int result)
    {
        if (result < 0)
        {
            throw new UsbException(result);
        }
    }

    public static string GetErrorName(int errorCode)
    {
        var name = libusb_error_name(errorCode);
        return name == IntPtr.Zero ? $"LIBUSB_ERROR {errorCode}" : Marshal.PtrToStringAnsi(name) ?? string.Empty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceDescriptor
    {
        public byte bLength;
        public byte bDescriptorType;
        public ushort bcdUSB;
        public byte bDeviceClass;
        public byte bDeviceSubClass;
        public byte bDeviceProtocol;
        public byte bMaxPacketSize0;
        public ushort idVendor;
        public ushort idProduct;
        public ushort bcdDevice;
        public byte iManufacturer;
        public byte iProduct;
        public byte iSerialNumber;
        public byte bNumConfigurations;
    }

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_init(out IntPtr context);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern void libusb_exit(IntPtr context);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern nint libusb_get_device_list(UsbContext context, out IntPtr list);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern void libusb_free_device_list(IntPtr list, int unrefDevices);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern IntPtr libusb_ref_device(IntPtr device);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern void libusb_unref_device(IntPtr device);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_get_device_descriptor(UsbDevice device, out DeviceDescriptor descriptor);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern byte libusb_get_bus_number(UsbDevice device);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_get_port_numbers(UsbDevice device, [Out] byte[] portNumbers, int length);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_open(UsbDevice device, out IntPtr handle);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern void libusb_close(IntPtr handle);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_get_configuration(UsbDeviceHandle handle, out int configuration);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_set_configuration(UsbDeviceHandle handle, int configuration);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_claim_interface(UsbDeviceHandle handle, int interfaceNumber);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_release_interface(UsbDeviceHandle handle, int interfaceNumber);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_clear_halt(UsbDeviceHandle handle, byte endpoint);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_control_transfer(
        UsbDeviceHandle handle,
        byte requestType,
        byte request,
        ushort value,
        ushort index,
        [In, Out] byte[]? data,
        ushort length,
        uint timeout);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_bulk_transfer(
        UsbDeviceHandle handle,
        byte endpoint,
        [In, Out] byte[] data,
        int length,
        out int transferred,
        uint timeout);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    public static extern int libusb_get_string_descriptor_ascii(
        UsbDeviceHandle handle,
        byte index,
        [Out] byte[] data,
        int length);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Winapi)]
    static extern IntPtr libusb_error_name(int errorCode);
}
