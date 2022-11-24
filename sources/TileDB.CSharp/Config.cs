using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
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
        /// Get last error message.
        /// </summary>
        /// <param name="pTileDBError"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private static string GetLastError(tiledb_error_t* pTileDBError, int status)
        {
            var sb_result = new StringBuilder();
            if (Enum.IsDefined(typeof(Status), status))
            {
                sb_result.Append("Status: " + (Status)status).ToString();
                sbyte* messagePtr;
                Methods.tiledb_error_message(pTileDBError, &messagePtr);
                string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                sb_result.Append(", Message: " + message);
            }
            else
            {
                sb_result.Append("Unknown error with code: " + status);
            }

            return sb_result.ToString();
        }

        // This function is unsafe because the input spans must be null-terminated.
        // C# 11's u8 literals guarantee it, as well as the MarshaledString class.
        // Furthermore the returned span points to memory managed by the Core so it
        // must not be returned to user code.
        private ReadOnlySpan<byte> GetUnsafe(ReadOnlySpan<byte> param)
        {
            byte* result;
            tiledb_error_t* p_tiledb_error;
            int status;
            fixed (byte* paramPtr = param)
            {
                using var handle = _handle.Acquire();
                status = Methods.tiledb_config_get(handle, (sbyte*)paramPtr, (sbyte**)&result, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var errMessage = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException(errMessage);
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            return MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(result);
        }

        // This function is unsafe because the input spans must be null-terminated.
        // C# 11's u8 literals guarantee it, as well as the MarshaledString class.
        private void SetUnsafe(ReadOnlySpan<byte> param, ReadOnlySpan<byte> value)
        {
            tiledb_error_t* p_tiledb_error;
            int status;
            fixed (byte* paramPtr = param, valuePtr = value)
            {
                using var handle = _handle.Acquire();
                status = Methods.tiledb_config_set(handle, (sbyte*)paramPtr, (sbyte*)valuePtr, &p_tiledb_error);
            }
            if (status != (int)Status.TILEDB_OK)
            {
                var errMessage = GetLastError(p_tiledb_error, status);
                Methods.tiledb_error_free(&p_tiledb_error);
                throw new ErrorException(errMessage);
            }
            Methods.tiledb_error_free(&p_tiledb_error);
        }

        internal bool GetBoolUnsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);

            if (value.SequenceEqual("true"u8))
            {
                return true;
            }

            Debug.Assert(param.SequenceEqual("false"u8), "Invalid boolean value");
            return false;
        }

        private void SetBoolUnsafe(ReadOnlySpan<byte> param, bool value) =>
            SetUnsafe(param, value ? "true"u8 : "false"u8);

        private uint GetUInt32Unsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);

            bool success = Utf8Parser.TryParse(value, out uint result, out _);
            Debug.Assert(success);
            return result;
        }

        private void SetUInt32Unsafe(ReadOnlySpan<byte> param, uint value)
        {
            Span<byte> buffer = stackalloc byte[32];
            Debug.Assert(Utf8Formatter.TryFormat(ulong.MaxValue, buffer, out _));
            buffer.Clear();

            bool success = Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
            Debug.Assert(success);
            buffer = buffer[..bytesWritten];
            SetUnsafe(param, buffer);
        }

        private long GetInt64Unsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);

            bool success = Utf8Parser.TryParse(value, out long result, out _);
            Debug.Assert(success);
            return result;
        }

        private void SetInt64Unsafe(ReadOnlySpan<byte> param, long value)
        {
            Span<byte> buffer = stackalloc byte[32];
            Debug.Assert(Utf8Formatter.TryFormat(long.MaxValue, buffer, out _));
            buffer.Clear();

            bool success = Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
            Debug.Assert(success);
            buffer = buffer[..bytesWritten];
            SetUnsafe(param, buffer);
        }

        private ulong GetUInt64Unsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);

            bool success = Utf8Parser.TryParse(value, out ulong result, out _);
            Debug.Assert(success);
            return result;
        }

        private void SetUInt64Unsafe(ReadOnlySpan<byte> param, ulong value)
        {
            Span<byte> buffer = stackalloc byte[32];
            Debug.Assert(Utf8Formatter.TryFormat(ulong.MaxValue, buffer, out _));
            buffer.Clear();

            bool success = Utf8Formatter.TryFormat(value, buffer, out int bytesWritten);
            Debug.Assert(success);
            buffer = buffer[..bytesWritten];
            SetUnsafe(param, buffer);
        }

        private float GetSingleUnsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);

            bool success = Utf8Parser.TryParse(value, out ulong result, out _);
            Debug.Assert(success);
            return result;
        }

        private void SetSingleUnsafe(ReadOnlySpan<byte> param, float value)
        {
            Span<byte> buffer = stackalloc byte[128];
            buffer.Clear();

            if (Utf8Formatter.TryFormat(value, buffer, out int bytesWritten))
            {
                buffer = buffer[..bytesWritten];
            }
            else
            {
                buffer = Encoding.ASCII.GetBytes(value.ToString(CultureInfo.InvariantCulture));
            }
            SetUnsafe(param, buffer);
        }

        private string GetStringUnsafe(ReadOnlySpan<byte> param)
        {
            ReadOnlySpan<byte> value = GetUnsafe(param);
            return MarshaledStringOut.GetString(value);
        }

        private void SetStringUnsafe(ReadOnlySpan<byte> param, string value)
        {
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(value);

            using var ms_value = new MarshaledString(value);
            ReadOnlySpan<byte> valueSpan = new(ms_value.Value, ms_value.Length);
            SetUnsafe(param, valueSpan);
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
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(param);
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(value);

            using var ms_param = new MarshaledString(param);
            ReadOnlySpan<byte> paramSpan = new(ms_param.Value, ms_param.Length);
            SetStringUnsafe(paramSpan, value);
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
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(param);

            using var ms_param = new MarshaledString(param);
            ReadOnlySpan<byte> paramSpan = new(ms_param.Value, ms_param.Length);
            ReadOnlySpan<byte> result = GetUnsafe(paramSpan);

            return MarshaledStringOut.GetString(result);
        }

        /// <summary>
        /// Unset a parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="ErrorException"></exception>
        public void Unset(string param)
        {
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(param);

            using var ms_param = new MarshaledString(param);

            tiledb_error_t* p_tiledb_error;
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
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(filename);

            using var ms_filename = new MarshaledString(filename);

            tiledb_error_t* p_tiledb_error;
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
            ArgumentExceptionCompat.ThrowIfNullOrEmpty(filename);

            using var ms_filename = new MarshaledString(filename);
            tiledb_error_t* p_tiledb_error;
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
