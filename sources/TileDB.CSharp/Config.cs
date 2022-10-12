using System;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Config : IDisposable
    {
        private readonly ConfigHandle _handle;
        private bool _disposed;
    
        public Config()
        {
            _handle = ConfigHandle.Create();
        }

        internal Config(ConfigHandle handle) 
        {
            _handle = handle;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing && (!_handle.IsInvalid)) 
            {
                _handle.Dispose();
            }

            _disposed = true;

        }

        internal ConfigHandle Handle => _handle;

        /// <summary>
        /// Get last error message.
        /// </summary>
        /// <param name="pTileDBError"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private static string GetLastError(tiledb_error_t *pTileDBError, int status)
        {
            var sb_result = new StringBuilder();
            if (Enum.IsDefined(typeof(Status), status))
            {
                sb_result.Append( "Status: " + (Status)status).ToString();
                var str_out = new MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    Methods.tiledb_error_message(pTileDBError, p_str);
                }
                sb_result.Append(", Message: " + str_out);                    
            } else {
                sb_result.Append("Unknown error with code: " + status);
            }
            
            return sb_result.ToString();
        }

        /// <summary>
        /// Set parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public void Set(string param, string value)
        {
            if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Config.set, param or value is null or empty!");
            }

            var ms_param = new MarshaledString(param);
            var ms_value = new MarshaledString(value);
            var tiledb_error = new tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_set(handle, ms_param, ms_value, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.set, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public string Get(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException("Config.get, param or value is null or empty!");
            }
 
            var ms_param = new MarshaledString(param);
            var ms_result = new MarshaledStringOut();
            
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            fixed (sbyte** p_result = &ms_result.Value)
            {
                int status;
                using (var handle = _handle.Acquire())
                {
                    status = Methods.tiledb_config_get(handle, ms_param, p_result, &p_tiledb_error);
                }
                if (status != (int)Status.TILEDB_OK)
                {
                    var err_message = GetLastError(p_tiledb_error, status);
                    Methods.tiledb_error_free(&p_tiledb_error);
                    throw new ErrorException("Config.get, caught exception:" + err_message);
                }
            }

            Methods.tiledb_error_free(&p_tiledb_error);
            return ms_result;
        }

        /// <summary>
        /// Unset a parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public void Unset(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException("Config.set, param is null or empty!");
            }

            var ms_param = new MarshaledString(param);
            var tiledb_error = new tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_unset(handle, ms_param, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.unset, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Load from a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public void LoadFromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) )
            {
                throw new ArgumentException("Config.load_from_file, filename is null or empty!");
            }

            var ms_filename = new MarshaledString(filename);
            var tiledb_error = new tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_load_from_file(handle, ms_filename, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.load_from_file, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Save to a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public void SaveToFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Config.save_to_file, filename is null or empty!");
            }

            var ms_filename = new MarshaledString(filename);
            var tiledb_error = new tiledb_error_t();

            var p_tiledb_error = &tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_save_to_file(handle, ms_filename, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.save_to_file, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        /// <summary>
        /// Get iterator with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public ConfigIterator Iterate(string prefix)
        {
            return new ConfigIterator(_handle, prefix);
        }

        /// <summary>
        /// Compare with other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Cmp(ref Config other)
        {
            using var handle = Handle.Acquire();
            using var otherHandle = other.Handle.Acquire();
            byte equal;
            Methods.tiledb_config_compare(handle, otherHandle, &equal);
            return equal == 1;
        }
    }
}