using System;
using System.Runtime.InteropServices;

namespace TileDB.CSharp.Marshalling;

internal struct SafeHandleHolder<TNativeObject> : IDisposable where TNativeObject : unmanaged
{
    private readonly SafeHandle? _handle;
    private bool _isHeld;

#pragma warning disable S3869 // "SafeHandle.DangerousGetHandle" should not be called
    public static unsafe implicit operator TNativeObject*(SafeHandleHolder<TNativeObject> holder) =>
        (TNativeObject*)(holder._handle?.DangerousGetHandle() ?? (IntPtr)0);
#pragma warning restore S3869 // "SafeHandle.DangerousGetHandle" should not be called

    public SafeHandleHolder(SafeHandle handle)
    {
        _handle = handle;
        _isHeld = false;
        _handle.DangerousAddRef(ref _isHeld);
    }

    public void Dispose()
    {
        if (!_isHeld)
        {
            return;
        }

        _isHeld = false;
        _handle?.DangerousRelease();
    }
}
