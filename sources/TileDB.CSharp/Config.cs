using System;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
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
        /// Set parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void Set(string param, string value)
        {
            if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Config.set, param or value is null or empty!");
            }

            using var ms_param = new MarshaledString(param);
            using var ms_value = new MarshaledString(value);

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_set(handle, ms_param, ms_value, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);
        }

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public string Get(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException("Config.get, param or value is null or empty!");
            }

            using var ms_param = new MarshaledString(param);
            sbyte* result;

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_get(handle, ms_param, &result, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);

            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Unset a parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void Unset(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException("Config.set, param is null or empty!");
            }

            using var ms_param = new MarshaledString(param);

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_unset(handle, ms_param, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);
        }

        /// <summary>
        /// Load from a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void LoadFromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Config.load_from_file, filename is null or empty!");
            }

            using var ms_filename = new MarshaledString(filename);

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_load_from_file(handle, ms_filename, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);
        }

        /// <summary>
        /// Save to a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void SaveToFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Config.save_to_file, filename is null or empty!");
            }

            using var ms_filename = new MarshaledString(filename);

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_save_to_file(handle, ms_filename, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);
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
