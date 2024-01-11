using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB VFS (Virtual File System) object.
/// </summary>
public unsafe sealed class VFS : IDisposable
{
    private readonly VFSHandle handle_;
    private readonly Context ctx_;

    /// <summary>
    /// Creates a <see cref="VFS"/>.
    /// </summary>
    public VFS() : this(null, null) { }

    /// <summary>
    /// Creates a <see cref="VFS"/> associated with the given <see cref="Context"/>.
    /// </summary>
    /// <param name="ctx">The context to associate the VFS with.</param>
    public VFS(Context ctx) : this(ctx, null) { }

    /// <summary>
    /// Creates a <see cref="VFS"/> associated with the given <see cref="Context"/>.
    /// </summary>
    /// <param name="ctx">The context to associate the VFS with. Defaults to <see cref="Context.GetDefault"/></param>
    /// <param name="config">The <see cref="VFS"/>' <see cref="CSharp.Config"/>. Defaults to <paramref name="ctx"/>'s config.</param>
#pragma warning disable S3427 // Method overloads with default parameter values should not overlap 
    public VFS(Context? ctx = null, Config? config = null)
#pragma warning restore S3427 // Method overloads with default parameter values should not overlap 
    {
        ctx_ = ctx ?? Context.GetDefault();
        handle_ = VFSHandle.Create(ctx_, config?.Handle);
    }

    /// <summary>
    /// Disposes this <see cref="VFS"/>.
    /// </summary>
    public void Dispose()
    {
        handle_.Dispose();
    }

    /// <summary>
    /// Gets the <see cref="Config"/> associated with this <see cref="VFS"/>.
    /// </summary>
    public Config Config()
    {
        var handle = new ConfigHandle();
        tiledb_config_t* config = null;
        var successful = false;
        try
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var vfsHandle = handle_.Acquire();
            ctx_.handle_error(Methods.tiledb_vfs_get_config(ctxHandle, vfsHandle, &config));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(config);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }
        return new Config(handle);
    }

    /// <summary>
    /// Creates an object-store bucket.
    /// </summary>
    /// <param name="uri">The URI of the bucket to be created.</param>
    public void CreateBucket(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_create_bucket(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Removes an object-store bucket.
    /// </summary>
    /// <param name="uri">The URI of the bucket to be removed.</param>
    public void RemoveBucket(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_remove_bucket(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Deletes the contents of an object-store bucket.
    /// </summary>
    /// <param name="uri">The URI of the bucket to be emptied.</param>
    public void EmptyBucket(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_empty_bucket(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Checks whether a bucket is empty or not.
    /// </summary>
    /// <param name="uri">The URI of the bucket to be checked.</param>
    public bool IsEmptyBucket(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        int is_empty = 0;
        ctx_.handle_error(Methods.tiledb_vfs_is_empty_bucket(ctxHandle, handle, ms_uri, &is_empty));
        return is_empty > 0;
    }

    /// <summary>
    /// Checks whether a URI points to a bucket.
    /// </summary>
    /// <param name="uri">The URI to check.</param>
    public bool IsBucket(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        int is_bucket = 0;
        ctx_.handle_error(Methods.tiledb_vfs_is_bucket(ctxHandle, handle, ms_uri, &is_bucket));
        return is_bucket > 0;
    }

    /// <summary>
    /// Creates a directory.
    /// </summary>
    /// <param name="uri">The directory's URI.</param>
    public void CreateDir(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_create_dir(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Checks whether a URI points to a directory.
    /// </summary>
    /// <param name="uri">The URI to check.</param>
    public bool IsDir(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        int is_dir = 0;
        ctx_.handle_error(Methods.tiledb_vfs_is_dir(ctxHandle, handle, ms_uri, &is_dir));
        return is_dir > 0;
    }

    /// <summary>
    /// Removes a directory.
    /// </summary>
    /// <param name="uri">The directory's URI.</param>
    public void RemoveDir(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_remove_dir(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Checks whether a URI points to a file.
    /// </summary>
    /// <param name="uri">The URI to check.</param>
    public bool IsFile(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        int is_file = 0;
        ctx_.handle_error(Methods.tiledb_vfs_is_file(ctxHandle, handle, ms_uri, &is_file));
        return is_file > 0;
    }

    /// <summary>
    /// Removes a file.
    /// </summary>
    /// <param name="uri">The file's URI.</param>
    public void RemoveFile(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_remove_file(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Gets the size of a directory.
    /// </summary>
    /// <param name="uri">The directory's URI.</param>
    public ulong DirSize(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ulong size = 0;
        ctx_.handle_error(Methods.tiledb_vfs_dir_size(ctxHandle, handle, ms_uri, &size));
        return size;
    }

    /// <summary>
    /// Gets the size of a file.
    /// </summary>
    /// <param name="uri">The file's URI.</param>
    public ulong FileSize(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ulong size = 0;
        ctx_.handle_error(Methods.tiledb_vfs_file_size(ctxHandle, handle, ms_uri, &size));
        return size;
    }

    /// <summary>
    /// Renames a file.
    /// </summary>
    /// <param name="old_uri">The source URI of the file.</param>
    /// <param name="new_uri">The destination URI of the file. Will be overwritten if it exists.</param>
    public void MoveFile(string old_uri, string new_uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_old_uri = new MarshaledString(old_uri);
        using var ms_new_uri = new MarshaledString(new_uri);
        ctx_.handle_error(Methods.tiledb_vfs_move_file(ctxHandle, handle, ms_old_uri, ms_new_uri));
    }

    /// <summary>
    /// Renames a directory.
    /// </summary>
    /// <param name="old_uri">The source URI of the directory.</param>
    /// <param name="new_uri">The destination URI of the directory. Will be overwritten if it exists.</param>
    public void MoveDir(string old_uri, string new_uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_old_uri = new MarshaledString(old_uri);
        using var ms_new_uri = new MarshaledString(new_uri);
        ctx_.handle_error(Methods.tiledb_vfs_move_dir(ctxHandle, handle, ms_old_uri, ms_new_uri));
    }

    /// <summary>
    /// Copies a file.
    /// </summary>
    /// <param name="old_uri">The source URI of the file.</param>
    /// <param name="new_uri">The destination URI of the file. Will be overwritten if it exists.</param>
    public void CopyFile(string old_uri, string new_uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_old_uri = new MarshaledString(old_uri);
        using var ms_new_uri = new MarshaledString(new_uri);
        ctx_.handle_error(Methods.tiledb_vfs_copy_file(ctxHandle, handle, ms_old_uri, ms_new_uri));
    }

    /// <summary>
    /// Copies a directory.
    /// </summary>
    /// <param name="old_uri">The source URI of the directory.</param>
    /// <param name="new_uri">The destination URI of the directory. Will be overwritten if it exists.</param>
    public void CopyDir(string old_uri, string new_uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_old_uri = new MarshaledString(old_uri);
        using var ms_new_uri = new MarshaledString(new_uri);
        ctx_.handle_error(Methods.tiledb_vfs_copy_dir(ctxHandle, handle, ms_old_uri, ms_new_uri));
    }

    /// <summary>
    /// Touches a file, i.e., creates a new empty file.
    /// </summary>
    /// <param name="uri">The file's URI.</param>
    public void Touch(string uri)
    {
        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx_.handle_error(Methods.tiledb_vfs_touch(ctxHandle, handle, ms_uri));
    }

    /// <summary>
    /// Opens a file.
    /// </summary>
    /// <param name="uri">The file's URI.</param>
    /// <param name="mode">The mode in which the file is opened.</param>
    /// <returns>A <see cref="VFSFile"/> object that can be used to perform operations on the file.</returns>
    public VFSFile Open(string uri, VfsMode mode)
    {
        VFSFileHandle handle = VFSFileHandle.Open(ctx_, handle_, uri, (tiledb_vfs_mode_t)mode);
        return new(ctx_, handle);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static int VisitCallback(sbyte* uriPtr, void* data)
    {
        string uri = MarshaledStringOut.GetStringFromNullTerminated(uriPtr);
        VisitCallbackData* callbackData = (VisitCallbackData*)data;
        Debug.Assert(callbackData->Exception is null);
        bool shouldContinue;
        try
        {
            shouldContinue = callbackData->Invoke(uri);
        }
        catch (Exception e)
        {
            callbackData->Exception = ExceptionDispatchInfo.Capture(e);
            shouldContinue = false;
        }
        return shouldContinue ? 1 : 0;
    }

    /// <summary>
    /// Lists the top-level children of a directory.
    /// </summary>
    /// <param name="uri">The URI of the directory.</param>
    /// <returns>A <see cref="List{T}"/> containing the children of the directory in <paramref name="uri"/>.</returns>
    /// <remarks>
    /// This function does not visit the children recursively; only the
    /// top-level children of <paramref name="uri"/> will be visited.
    /// </remarks>
    public List<string> GetChildren(string uri)
    {
        var list = new List<string>();
        VisitChildren(uri, static (uri, list) =>
        {
            list.Add(uri);
            return true;
        }, list);
        return list;
    }

    /// <summary>
    /// Visits the top-level children of a directory.
    /// </summary>
    /// <param name="uri">The URI of the directory to visit.</param>
    /// <param name="callback">A callback delegate that will be called with the URI of each child and
    /// <paramref name="callbackArg"/>, and returns whether to continue visiting.</param>
    /// <param name="callbackArg">An argument that will be passed to <paramref name="callback"/>.</param>
    /// <typeparam name="T">The type of <paramref name="callbackArg"/>.</typeparam>
    /// <remarks>
    /// This function does not visit the children recursively; only the
    /// top-level children of <paramref name="uri"/> will be visited.
    /// </remarks>
    public void VisitChildren<T>(string uri, Func<string, T, bool> callback, T callbackArg)
    {
        ValueTuple<Func<string, T, bool>, T> data = (callback, callbackArg);
        var callbackData = new VisitCallbackData()
        {
            Callback = static (uri, arg) =>
            {
                var dataPtr = (ValueTuple<Func<string, T, bool>, T>*)arg;
                return dataPtr->Item1(uri, dataPtr->Item2);
            },
            CallbackArgument = (IntPtr)(&data)
        };

        using var ctxHandle = ctx_.Handle.Acquire();
        using var handle = handle_.Acquire();
        using var ms_uri = new MarshaledString(uri);
        // Taking a pointer to callbackData is safe; the callback will be invoked only
        // during the call to tiledb_vfs_ls. Contrast this with tiledb_query_submit_async where we
        // had to use a GCHandle because the callback might be invoked after we return from it.
        // We also are not susceptible to GC holes; callbackData is in the stack and won't be moved around.
        ctx_.handle_error(Methods.tiledb_vfs_ls(ctxHandle, handle, ms_uri, &VisitCallback, &callbackData));
        callbackData.Exception?.Throw();
    }

    private struct VisitCallbackData
    {
        public Func<string, IntPtr, bool> Callback;

        public IntPtr CallbackArgument;

        // If the callback threw an exception we will save it here and rethrow it once we leave native code.
        // The reason we do this is that throwing exceptions in P/Invoke is not portable (works only on Windows
        // and because the native library is compiled with MSVC).
        public ExceptionDispatchInfo? Exception;

        public bool Invoke(string uri) => Callback(uri, CallbackArgument);
    }
}
