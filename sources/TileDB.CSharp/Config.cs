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
        private bool disposed_ = false;
    
        public Config()
        {
            handle_ = new TileDB.Interop.ConfigHandle();
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            
            if (!disposed_) {
                if (disposing && (!handle_.IsInvalid)) 
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }

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

            TileDB.Interop.MarshaledString ms_param = new Interop.MarshaledString(param);
            TileDB.Interop.MarshaledString ms_value = new Interop.MarshaledString(value);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_set(handle_, ms_param, ms_value, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                throw new TileDB.ErrorException("Config.set,caught exception:" + message);
            }
            return;
        }

        public string get(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new System.ArgumentException("Config.get, param or value is null or empty!");
            }
 
            TileDB.Interop.MarshaledString ms_param = new Interop.MarshaledString(param);
            TileDB.Interop.MarshaledStringOut ms_result = new Interop.MarshaledStringOut();
            
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = 0;
            fixed (sbyte** p_result = &ms_result.Value) 
            {
                status = TileDB.Interop.Methods.tiledb_config_get(handle_, ms_param, p_result, &p_tiledb_error);
            }
            
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            if (status != (int)Status.TILEDB_OK) 
            {
                string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                throw new TileDB.ErrorException("Config.get,caught exception:" + message);
            }
            return status == 0 ? ms_result: "";
        }

        public void load_from_file(string filename)
        {
            if (string.IsNullOrEmpty(filename) )
            {
                throw new System.ArgumentException("Config.load_from_file, filename is null or empty!");
            }

            TileDB.Interop.MarshaledString ms_filename = new Interop.MarshaledString(filename);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_load_from_file(handle_, ms_filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            if (status != (int)Status.TILEDB_OK) 
            {
                string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                throw new TileDB.ErrorException("Config.load_from_file,caught exception:" + message);
            }

            return ;
        }

        public void save_to_file(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new System.ArgumentException("Config.save_to_file, filename is null or empty!");
            }

            TileDB.Interop.MarshaledString ms_filename = new Interop.MarshaledString(filename);
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_save_to_file(handle_, ms_filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                throw new TileDB.ErrorException("Config.save_to_file,caught exception:" + message);
            }
            return;
        }


    }
}
