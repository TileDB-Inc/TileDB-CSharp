using System;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB VFS (Virtual File System) file object.
/// </summary>
/// <remarks>
/// This class cannot be directly created.
/// To obtain an instance of it, use <see cref="VFS.Open"/>.
/// </remarks>
public unsafe sealed class VFSFile : IDisposable
{
    private readonly Context _context;
    private readonly VFSFileHandle _handle;

    internal VFSFile(Context context, VFSFileHandle handle)
    {
        _context = context;
        _handle = handle;
    }

    /// <summary>
    /// Whether the file is closed.
    /// </summary>
    public bool IsClosed
    {
        get
        {
            using var contextHandle = _context.Handle.Acquire();
            using var handle = _handle.Acquire();
            int isClosed;
            _context.handle_error(Methods.tiledb_vfs_fh_is_closed(contextHandle, handle, &isClosed));
            return isClosed != 0;
        }
    }

    /// <summary>
    /// Disposes this <see cref="VFSFile"/>.
    /// </summary>
    public void Dispose() => _handle.Dispose();

    /// <summary>
    /// Reads from the file at a specified offset.
    /// </summary>
    /// <param name="offset">The offset to read from.</param>
    /// <param name="buffer">A <see cref="Span{T}"/> of bytes where the file's data will be written into.</param>
    /// <exception cref="Exception">The remaining data in the file after <paramref name="offset"/>
    /// are less than <paramref name="buffer"/>'s <see cref="Span{T}.Length"/>.</exception>
    /// <remarks>If you want to read more than <see cref="int.MaxValue"/> bytes
    /// of data you should call <see cref="ReadExactly(ulong, byte*, ulong)"/>.</remarks>
    public void ReadExactly(ulong offset, Span<byte> buffer)
    {
        fixed(byte* ptr = &MemoryMarshal.GetReference(buffer))
        {
            ReadExactly(offset, ptr, (ulong)buffer.Length);
        }
    }

    /// <summary>
    /// Reads from the file at a specified offset.
    /// </summary>
    /// <param name="offset">The offset to read from.</param>
    /// <param name="buffer">A pointer to the memory region where the file's data will be written into.</param>
    /// <param name="count">The number of bytes to read.</param>
    /// <exception cref="Exception">The remaining data in the file after <paramref name="offset"/>
    /// are less than <paramref name="count"/>.</exception>
    /// <seealso cref="ReadExactly(ulong, Span{byte})"/>
    public void ReadExactly(ulong offset, byte* buffer, ulong count)
    {
        using var contextHandle = _context.Handle.Acquire();
        using var handle = _handle.Acquire();
        _context.handle_error(Methods.tiledb_vfs_read(contextHandle, handle, offset, buffer, count));
    }

    /// <summary>
    /// Writes data to the end of the file.
    /// </summary>
    /// <param name="buffer">A <see cref="ReadOnlySpan{T}"/> of bytes whose content will be written.</param>
    /// <remarks>
    /// <para>If you want to write more than <see cref="int.MaxValue"/> bytes
    /// of data you should call <see cref="Write(byte*, ulong)"/>.</para>
    /// <para>If the file does not exist, it will be created.</para>
    /// </remarks>
    public void Write(ReadOnlySpan<byte> buffer)
    {
        fixed (byte* ptr = &MemoryMarshal.GetReference(buffer))
        {
            Write(ptr, (ulong)buffer.Length);
        }
    }

    /// <summary>
    /// Writes data to the end of the file.
    /// </summary>
    /// <param name="buffer">A pointer to the bytes that will be written.</param>
    /// <param name="count">The number of bytes to write.</param>
    /// <remarks>
    /// If the file does not exist, it will be created.
    /// </remarks>
    public void Write(byte* buffer, ulong count)
    {
        using var contextHandle = _context.Handle.Acquire();
        using var handle = _handle.Acquire();
        _context.handle_error(Methods.tiledb_vfs_write(contextHandle, handle, buffer, count));
    }

    /// <summary>
    /// Closes a file. This flushes the buffered data into the file when the file was opened in write (or append) mode.
    /// </summary>
    /// <remarks>
    /// It is particularly important to be called after <see cref="FileSystemType.S3"/> writes, as otherwise the writes will not take effect.
    /// </remarks>
    public void Close()
    {
        using var contextHandle = _context.Handle.Acquire();
        using var handle = _handle.Acquire();
        _context.handle_error(Methods.tiledb_vfs_close(contextHandle, handle));
    }

    /// <summary>
    /// Flushes the file's internal buffers.
    /// </summary>
    /// <remarks>
    /// This has no effect for <see cref="FileSystemType.S3"/>.
    /// </remarks>
    public void Flush()
    {
        using var contextHandle = _context.Handle.Acquire();
        using var handle = _handle.Acquire();
        _context.handle_error(Methods.tiledb_vfs_sync(contextHandle, handle));
    }
}
