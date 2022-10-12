using System;
using TileDB.Interop;

namespace TileDB.CSharp 
{
    public unsafe class VFS : IDisposable
    {
        private readonly VFSHandle handle_;
        private readonly Context ctx_;
        private bool disposed_ = false;

        public VFS() : this(Context.GetDefault()) { }

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

        #region capi functions 

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>
        public Config Config() {
            return ctx_.Config();
        }

        /// <summary>
        /// Create bucket.
        /// </summary>
        /// <param name="uri"></param>
        public void CreateBucket(string uri) {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_create_bucket(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Remove bucket.
        /// </summary>
        /// <param name="uri"></param>
        public void RemoveBucket(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_remove_bucket(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Empty bucket.
        /// </summary>
        /// <param name="uri"></param>
        public void EmptyBucket(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_empty_bucket(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Test if a bucket is empty or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool IsEmptyBucket(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            int is_empty = 0;
            ctx_.handle_error(Methods.tiledb_vfs_is_empty_bucket(ctxHandle, handle, ms_uri, &is_empty));
            return is_empty > 0;
        }

        /// <summary>
        /// Test if it is bucket or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool IsBucket(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            int is_bucket = 0;
            ctx_.handle_error(Methods.tiledb_vfs_is_bucket(ctxHandle, handle, ms_uri, &is_bucket));
            return is_bucket > 0;
        }

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="uri"></param>
        public void CreateDir(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_create_dir(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Test if it is directory or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool IsDir(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            int is_dir = 0;
            ctx_.handle_error(Methods.tiledb_vfs_is_dir(ctxHandle, handle, ms_uri, &is_dir));
            return is_dir > 0;
        }

        /// <summary>
        /// Remove a directory.
        /// </summary>
        /// <param name="uri"></param>
        public void RemoveDir(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_remove_dir(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Test if it is file or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool IsFile(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            int is_file = 0;
            ctx_.handle_error(Methods.tiledb_vfs_is_file(ctxHandle, handle, ms_uri, &is_file));
            return is_file > 0;
        }

        /// <summary>
        /// Remove a file.
        /// </summary>
        /// <param name="uri"></param>
        public void RemoveFile(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_remove_file(ctxHandle, handle, ms_uri));
        }

        /// <summary>
        /// Get directory size.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public UInt64 DirSize(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            UInt64 size = 0;
            ctx_.handle_error(Methods.tiledb_vfs_dir_size(ctxHandle, handle, ms_uri, &size));
            return size;
        }

        /// <summary>
        /// Get file size.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public UInt64 FileSize(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            UInt64 size = 0;
            ctx_.handle_error(Methods.tiledb_vfs_file_size(ctxHandle, handle, ms_uri, &size));
            return size;
        }

        /// <summary>
        /// Move file.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void MoveFile(string old_uri, string new_uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_old_uri = new MarshaledString(old_uri);
            MarshaledString ms_new_uri = new MarshaledString(new_uri);
            ctx_.handle_error(Methods.tiledb_vfs_move_file(ctxHandle, handle, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Move directory.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void MoveDir(string old_uri, string new_uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_old_uri = new MarshaledString(old_uri);
            MarshaledString ms_new_uri = new MarshaledString(new_uri);
            ctx_.handle_error(Methods.tiledb_vfs_move_dir(ctxHandle, handle, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Copy file.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void CopyFile(string old_uri, string new_uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_old_uri = new MarshaledString(old_uri);
            MarshaledString ms_new_uri = new MarshaledString(new_uri);
            ctx_.handle_error(Methods.tiledb_vfs_copy_file(ctxHandle, handle, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Copy directory.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void CopyDir(string old_uri, string new_uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_old_uri = new MarshaledString(old_uri);
            MarshaledString ms_new_uri = new MarshaledString(new_uri);
            ctx_.handle_error(Methods.tiledb_vfs_copy_dir(ctxHandle, handle, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Touch a file.
        /// </summary>
        /// <param name="uri"></param>
        public void Touch(string uri)
        {
            using var ctxHandle = ctx_.Handle.Acquire();
            using var handle = handle_.Acquire();
            MarshaledString ms_uri = new MarshaledString(uri);
            ctx_.handle_error(Methods.tiledb_vfs_touch(ctxHandle, handle, ms_uri));
        }
        #endregion capi functions
    }
}
