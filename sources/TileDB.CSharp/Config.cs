using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class Config : IDisposable
    {
        private TileDB.Interop.ConfigHandle handle_;
    
        public Config()
        {
            handle_ = new TileDB.Interop.ConfigHandle();
        }

        public void Dispose()
        {
            if (!handle_.IsInvalid)
            {
                handle_.Dispose();
            }

            System.GC.SuppressFinalize(this);

        }

        internal TileDB.Interop.ConfigHandle Handle
        {
            get { return handle_; }
        }

        public void set(string param, string value)
        {
            if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(value))
            {
                throw new System.ArgumentException("Config.set, param or value is null or empty!");
            }
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("Config.set, invalid handle!");
            }

            TileDB.Interop.MarshaledString ms_param = new Interop.MarshaledString(param);
            TileDB.Interop.MarshaledString ms_value = new Interop.MarshaledString(value);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.Methods.tiledb_config_set(handle_, ms_param, ms_value, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return;
        }

        public string get(string param)
        {

            if (string.IsNullOrEmpty(param) || handle_.IsInvalid) 
            {
                return string.Empty;
            }
            TileDB.Interop.MarshaledString ms_param = new Interop.MarshaledString(param);
            
            TileDB.Interop.MarshaledStringOut ms_results = new Interop.MarshaledStringOut(512);
            IntPtr str_intptr = System.IntPtr.Zero;
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_get(handle_, ms_param, ms_results, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            return status == 0 ? ms_results.ToString() : "";
        }

        public void load_from_file(string filename)
        {
            if (string.IsNullOrEmpty(filename) )
            {
                throw new System.ArgumentException("Config.load_from_file, filename is null or empty!");
            }
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("Config.load_from_file, invalid handle!");
            }

            TileDB.Interop.MarshaledString ms_filename = new Interop.MarshaledString(filename);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.Methods.tiledb_config_load_from_file(handle_, ms_filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return ;
        }

        public void save_to_file(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new System.ArgumentException("Config.save_to_file, filename is null or empty!");
            }
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("Config.save_to_file, invalid handle!");
            }

            TileDB.Interop.MarshaledString ms_filename = new Interop.MarshaledString(filename);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.Methods.tiledb_config_save_to_file(handle_, ms_filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return;
        }


    }
}
