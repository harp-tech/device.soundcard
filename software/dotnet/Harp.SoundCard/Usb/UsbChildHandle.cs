#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Harp.SoundCard.Usb;

/// <summary>
/// Provides the base for a libusb handle created from a parent handle.
/// </summary>
/// <remarks>
/// The parent handle stays open until this handle is released.
/// </remarks>
internal abstract class UsbChildHandle : SafeHandle
{
    readonly SafeHandle parent;

    protected UsbChildHandle(SafeHandle parent, IntPtr handle)
        : base(IntPtr.Zero, ownsHandle: true)
    {
        var success = false;
        parent.DangerousAddRef(ref success);
        this.parent = parent;
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected abstract void ReleaseNativeHandle();

    protected sealed override bool ReleaseHandle()
    {
        ReleaseNativeHandle();
        parent.DangerousRelease();
        return true;
    }
}
