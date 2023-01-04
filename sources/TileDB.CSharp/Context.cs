using System;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB context.
    /// </summary>
    public sealed unsafe class Context : IDisposable
    {
        private readonly ContextHandle _handle;
        private readonly Config _config;

        private bool IsDefault { get; init; }

        /// <summary>
        /// Creates a <see cref="Context"/>.
        /// </summary>
        public Context()
        {
            _handle = ContextHandle.Create();
            _config = new Config();
        }

        /// <summary>
        /// Creates a <see cref="Context"/> with an associated <see cref="CSharp.Config"/>.
        /// </summary>
        /// <param name="config">The context's config.</param>
        public Context(Config config)
        {
            _handle = ContextHandle.Create(config.Handle);
            _config = config;
        }

        /// <summary>
        /// Disposes the <see cref="Context"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method on the context returned by <see cref="GetDefault"/> will have no effect.
        /// </remarks>
        public void Dispose()
        {
            if (IsDefault)
            {
                return;
            }

            _handle.Dispose();
        }

        internal ContextHandle Handle => _handle;

        private static readonly Context _default = new() { IsDefault = true };

        /// <summary>
        /// Gets the default <see cref="Context"/>.
        /// </summary>
        public static Context GetDefault() => _default;

        /// <summary>
        /// Gets a JSON string with statistics about the context.
        /// </summary>
        public string Stats()
        {
            sbyte* result = null;
            try
            {
                using var handle = _handle.Acquire();
                ErrorHandling.ThrowOnError(Methods.tiledb_ctx_get_stats(handle, &result));

                return MarshaledStringOut.GetStringFromNullTerminated(result);
            }
            finally
            {
                if (result is not null)
                {
                    ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Context"/>'s <see cref="Config"/>.
        /// </summary>

        public Config Config()
        {
            return _config;
        }

        /// <summary>
        /// Gets the last error message associated with this <see cref="Context"/>.
        /// </summary>
        public string LastError()
        {
            var sb_result = new StringBuilder();

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_ctx_get_last_error(handle, &p_tiledb_error);
            }
            if (status == (int)Status.TILEDB_OK)
            {
                sbyte* messagePtr;

                status = Methods.tiledb_error_message(p_tiledb_error, &messagePtr);

                if (status == (int)Status.TILEDB_OK)
                {
                    string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                    sb_result.Append(message);
                }
                else
                {
                    var message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                    sb_result.Append(" Context.last_error,caught exception:" + message);
                }
            }
            else
            {
                var message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                sb_result.Append(" Context.last_error,caught exception:" + message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            return sb_result.ToString();
        }

        /// <summary>
        /// Cancels the asynchronous tasks associated with this <see cref="Context"/>.
        /// </summary>
        public void CancelTasks()
        {
            using var handle = _handle.Acquire();
            handle_error(Methods.tiledb_ctx_cancel_tasks(handle));
        }

        /// <summary>
        /// Checks whether the given <see cref="FileSystemType"/> is supported.
        /// </summary>
        /// <param name="fileSystem">The file system type to check.</param>
        public bool IsFileSystemSupported(FileSystemType fileSystem)
        {
            using var handle = _handle.Acquire();
            int result;
            handle_error(Methods.tiledb_ctx_is_supported_fs(handle, (tiledb_filesystem_t)fileSystem, &result));
            return result != 0;
        }

        /// <summary>
        /// Sets a string key-value “tag” on the given context.
        /// </summary>
        /// <param name="key">The tag's key.</param>
        /// <param name="value">The tag's value.</param>
        /// <exception cref="ArgumentException"><paramref name="key"/> or
        /// <paramref name="value"/> are <see langword="null"/> or empty.</exception>
        public void SetTag(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Context.set_tag, key or value is null or empty!");
            }

            using var handle = _handle.Acquire();
            using var ms_key = new MarshaledString(key);
            using var ms_value = new MarshaledString(value);
            handle_error(Methods.tiledb_ctx_set_tag(handle, ms_key, ms_value));
        }

        /// <summary>
        /// Deprecated.
        /// </summary>
        [Obsolete(Obsoletions.ContextErrorHappenedMessage, DiagnosticId = Obsoletions.ContextErrorHappenedDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        public event EventHandler<ErrorEventArgs>? ErrorHappened;

        internal void handle_error(int rc)
        {
            var status = (Status)Methods.tiledb_status_code(rc);

            if (status == Status.TILEDB_OK)
            {
                return;
            }

            string message;
            tiledb_error_t* p_tiledb_error;
            using (var handle = _handle.Acquire())
            {
                status = (Status)Methods.tiledb_status_code(Methods.tiledb_ctx_get_last_error(handle, &p_tiledb_error));
            }
            if (status == Status.TILEDB_OK)
            {
                sbyte* messagePtr;
                status = (Status)Methods.tiledb_status_code(Methods.tiledb_error_message(p_tiledb_error, &messagePtr));

                if (status == Status.TILEDB_OK)
                {
                    message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                }
                else
                {
                    message = $"Error during tiledb_error_message: {status}";
                }
            }
            else
            {
                message = $"Error during tiledb_ctx_get_last_error: {status}";
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            throw new TileDBException(message) { StatusCode = (int)status };
        }
    }
}
