using System;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class ConfigIterator : IDisposable
    {
        private readonly ConfigIteratorHandle _handle;
        private readonly ConfigHandle _hConfig;
        private bool _disposed;

        internal ConfigIterator(ConfigHandle hConfig, string prefix)
        {
            _hConfig = hConfig;
            _handle = ConfigIteratorHandle.Create(hConfig, prefix);
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

        internal ConfigIteratorHandle Handle => _handle;

        private string GetLastError(tiledb_error_t* pTiledbError, int status)
        {
            var sb_result = new StringBuilder();
            if (Enum.IsDefined(typeof(Status), status))
            {
                sb_result.Append("Status: " + (Status)status).ToString();
                sbyte* messagePtr;
                Methods.tiledb_error_message(pTiledbError, &messagePtr);
                string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                sb_result.Append(", Message: " + message);
            }
            else
            {
                sb_result.Append("Unknown error with code: " + status);
            }

            return sb_result.ToString();
        }

        public Tuple<string, string> Here()
        {
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            sbyte* paramPtr;
            sbyte* valuePtr;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_iter_here(handle, &paramPtr, &valuePtr, &p_tiledb_error);
            }

            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.set, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            string param = MarshaledStringOut.GetStringFromNullTerminated(paramPtr);
            string value = MarshaledStringOut.GetStringFromNullTerminated(valuePtr);
            return new Tuple<string, string>(param, value);
        }

        public void Next()
        {
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_iter_next(handle, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("ConfigIterator.next, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        public bool Done()
        {
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            int c_done;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_iter_done(handle, &c_done, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("ConfigIterator.done, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
            return c_done == 1;
        }

        public void Reset(string prefix)
        {
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            using var ms_prefix = new MarshaledString(prefix);
            int status;
            using (var configHandle = _hConfig.Acquire())
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_config_iter_reset(configHandle, handle, ms_prefix, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var err_message = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException("Config.reset, caught exception:" + err_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }
    }
}
