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

        /// <summary>
        /// Get last error message.
        /// </summary>
        /// <param name="p_tiledb_error"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        internal string get_last_error(TileDB.Interop.tiledb_error_t *p_tiledb_error, int status)
        {
            var sb_result = new StringBuilder();
            if (Enum.IsDefined(typeof(TileDB.Status), status))
            {
                sb_result.Append( "Status: " + (TileDB.Status)status).ToString();
                var str_out = new Interop.MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
                sb_result.Append(", Message: " + str_out);                    
            } else {
                sb_result.Append("Unknown error with code: " + status.ToString());
            }
            
            return sb_result.ToString();
        }

        /// <summary>
        /// Set parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="TileDB.ErrorException"></exception>
        public void set(string param, string value)
        {
            if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(value))
            {
                throw new System.ArgumentException("Config.set, param or value is null or empty!");
            }

            var ms_param = new Interop.MarshaledString(param);
            var ms_value = new Interop.MarshaledString(value);
            var tiledb_error = new TileDB.Interop.tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_config_set(handle_, ms_param, ms_value, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.set, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="TileDB.ErrorException"></exception>
        public string get(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new System.ArgumentException("Config.get, param or value is null or empty!");
            }
 
            var ms_param = new Interop.MarshaledString(param);
            var ms_result = new Interop.MarshaledStringOut();
            
            var tiledb_error = new TileDB.Interop.tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            var status = 0;
            fixed (sbyte** p_result = &ms_result.Value) 
            {
                status = TileDB.Interop.Methods.tiledb_config_get(handle_, ms_param, p_result, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.get, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            return ms_result;
        }

        /// <summary>
        /// Unset a parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="TileDB.ErrorException"></exception>
        public void unset(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new System.ArgumentException("Config.set, param is null or empty!");
            }

            var ms_param = new Interop.MarshaledString(param);
            var tiledb_error = new TileDB.Interop.tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_config_unset(handle_, ms_param, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.unset, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Load from a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="TileDB.ErrorException"></exception>
        public void load_from_file(string filename)
        {
            if (string.IsNullOrEmpty(filename) )
            {
                throw new System.ArgumentException("Config.load_from_file, filename is null or empty!");
            }

            var ms_filename = new Interop.MarshaledString(filename);
            var tiledb_error = new TileDB.Interop.tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_config_load_from_file(handle_, ms_filename, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.load_from_file, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Save to a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="TileDB.ErrorException"></exception>
        public void save_to_file(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new System.ArgumentException("Config.save_to_file, filename is null or empty!");
            }

            var ms_filename = new Interop.MarshaledString(filename);
            var tiledb_error = new TileDB.Interop.tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_config_save_to_file(handle_, ms_filename, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.save_to_file, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Get iterator with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public ConfigIterator iterate(string prefix)
        {
            return new ConfigIterator(handle_, prefix);
        }

        /// <summary>
        /// Compare with other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool cmp(ref Config other)
        {
            byte equal;
            TileDB.Interop.Methods.tiledb_config_compare(handle_, other.handle_, &equal);
            return equal == 1;
        }
    }
}