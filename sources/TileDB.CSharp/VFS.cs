using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileDB.Interop;
namespace TileDB.CSharp 
{
    public unsafe class VFS : IDisposable
    {
        private TileDB.Interop.VFSHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;

        public VFS() 
        {
            ctx_ = Context.GetDefault();
            handle_ = new TileDB.Interop.VFSHandle(ctx_.Handle, ctx_.Config().Handle);
        }

        public VFS(Context ctx) 
        {
            ctx_ = ctx;
            handle_ = new TileDB.Interop.VFSHandle(ctx_.Handle, ctx_.Config().Handle);
        }

        internal VFS(Context ctx, TileDB.Interop.VFSHandle handle) 
        {
            ctx_ = ctx;
            handle_ = handle;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
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
        public void create_bucket(string uri) {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_create_bucket(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Remove bucket.
        /// </summary>
        /// <param name="uri"></param>
        public void remove_bucket(string uri) {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_remove_bucket(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Empty bucket.
        /// </summary>
        /// <param name="uri"></param>
        public void empty_bucket(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_empty_bucket(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Test if a bucket is empty or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool is_empty_bucket(string uri) {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            int is_empty = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_is_empty_bucket(ctx_.Handle, handle_, ms_uri, &is_empty));
            return is_empty > 0;
        }

        /// <summary>
        /// Test if it is bucket or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool is_bucket(string uri) {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            int is_bucket = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_is_bucket(ctx_.Handle, handle_, ms_uri, &is_bucket));
            return is_bucket > 0;
        }

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="uri"></param>
        public void create_dir(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_create_dir(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Test if it is directory or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool is_dir(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            int is_dir = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_is_dir(ctx_.Handle, handle_, ms_uri, &is_dir));
            return is_dir > 0;
        }

        /// <summary>
        /// Remove a directory.
        /// </summary>
        /// <param name="uri"></param>
        public void remove_dir(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_remove_dir(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Test if it is file or not.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public bool is_file(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            int is_file = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_is_file(ctx_.Handle, handle_, ms_uri, &is_file));
            return is_file > 0;
        }

        /// <summary>
        /// Remove a file.
        /// </summary>
        /// <param name="uri"></param>
        public void remove_file(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_remove_file(ctx_.Handle, handle_, ms_uri));
        }

        /// <summary>
        /// Get directory size.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public UInt64 dir_size(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            UInt64 size = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_dir_size(ctx_.Handle, handle_, ms_uri, &size));
            return size;
        }

        /// <summary>
        /// Get file size.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public UInt64 file_size(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            UInt64 size = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_file_size(ctx_.Handle, handle_, ms_uri, &size));
            return size;
        }

        /// <summary>
        /// Move file.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void move_file(string old_uri, string new_uri) 
        {
            TileDB.Interop.MarshaledString ms_old_uri = new TileDB.Interop.MarshaledString(old_uri);
            TileDB.Interop.MarshaledString ms_new_uri = new TileDB.Interop.MarshaledString(new_uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_move_file(ctx_.Handle, handle_, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Move directory.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void move_dir(string old_uri, string new_uri)
        {
            TileDB.Interop.MarshaledString ms_old_uri = new TileDB.Interop.MarshaledString(old_uri);
            TileDB.Interop.MarshaledString ms_new_uri = new TileDB.Interop.MarshaledString(new_uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_move_dir(ctx_.Handle, handle_, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Copy file.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void copy_file(string old_uri, string new_uri)
        {
            TileDB.Interop.MarshaledString ms_old_uri = new TileDB.Interop.MarshaledString(old_uri);
            TileDB.Interop.MarshaledString ms_new_uri = new TileDB.Interop.MarshaledString(new_uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_copy_file(ctx_.Handle, handle_, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Copy directory.
        /// </summary>
        /// <param name="old_uri"></param>
        /// <param name="new_uri"></param>
        public void copy_dir(string old_uri, string new_uri)
        {
            TileDB.Interop.MarshaledString ms_old_uri = new TileDB.Interop.MarshaledString(old_uri);
            TileDB.Interop.MarshaledString ms_new_uri = new TileDB.Interop.MarshaledString(new_uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_copy_dir(ctx_.Handle, handle_, ms_old_uri, ms_new_uri));
        }

        /// <summary>
        /// Touch a file.
        /// </summary>
        /// <param name="uri"></param>
        public void touch(string uri)
        {
            TileDB.Interop.MarshaledString ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_vfs_touch(ctx_.Handle, handle_, ms_uri));
        }

        #endregion capi functions


    }//class VFS

}//namespace 