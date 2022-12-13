using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB VFS (Virtual File System) object.
    /// </summary>
    public unsafe class VFS : IDisposable
    {
        private readonly VFSHandle handle_;
        private readonly Context ctx_;
        private bool disposed_ = false;

        /// <summary>
        /// Creates a <see cref="VFS"/>.
        /// </summary>
        public VFS() : this(Context.GetDefault()) { }

        /// <summary>
        /// Creates a <see cref="VFS"/> associated with the given <see cref="Context"/>.
        /// </summary>
        /// <param name="ctx">The context to associate the VFS.</param>
        public VFS(Context ctx) 
        {
            ctx_ = ctx;
            handle_ = VFSHandle.Create(ctx_, ctx_.Config().Handle);
        }

        internal VFS(Context ctx, VFSHandle handle) 
        {
            ctx_ = ctx;
            handle_ = handle;
        }

        /// <summary>
        /// Disposes this <see cref="VFS"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed_)
            {
                if (disposing && (!handle_.IsInvalid))
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }
        }

        /// <summary>
        /// Gets the <see cref="Config"/> associated with this <see cref="VFS"/>.
        /// </summary>
        public Config Config() {
            return ctx_.Config();
        }

        /// <summary>
        /// Creates an object-store bucket.
        /// </summary>
        /// <param name="uri">The URI of the bucket to be created.</param>
        public void CreateBucket(string uri) {
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
    }
}
