using System;
using System.Runtime.InteropServices;

namespace TileDB.CSharp
{
    internal struct HandleHolder<THandle, TNativeObject> : IDisposable where THandle : SafeHandle where TNativeObject : unmanaged
    {
        private readonly THandle _handle;
        private bool _isHeld;

#pragma warning disable S3869 // "SafeHandle.DangerousGetHandle" should not be called
        public static unsafe implicit operator TNativeObject*(HandleHolder<THandle, TNativeObject> holder) =>
            (TNativeObject*)holder._handle.DangerousGetHandle();
#pragma warning restore S3869 // "SafeHandle.DangerousGetHandle" should not be called

        public HandleHolder(THandle handle)
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
            _handle.DangerousRelease();
        }
    }
}
