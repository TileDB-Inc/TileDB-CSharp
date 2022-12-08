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
        /// Gets the <see cref="Context"/>'s statistics.
        /// </summary>
        public string Stats()
        {
            using var handle = _handle.Acquire();
            sbyte* result;
            handle_error(Methods.tiledb_ctx_get_stats(handle, &result));

            return MarshaledStringOut.GetStringFromNullTerminated(result);
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
        /// An event that gets raised if an operation that used this <see cref="Context"/> failed.
        /// </summary>
        /// <remarks>
        /// By default it throws an exception.
        /// </remarks>
        public event EventHandler<ErrorEventArgs> ErrorHappened = (_, e) =>
        {
            var error_msg = $"Error! Code:{e.Code},Message:{e.Message}";
            throw new Exception(error_msg);
        };

        internal void handle_error(int rc)
        {
            int status = Methods.tiledb_status_code(rc);

            if (status == (int)Status.TILEDB_OK)
            {
                return;
            }

            var sb_message = new StringBuilder();
            tiledb_error_t* p_tiledb_error;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_status_code(Methods.tiledb_ctx_get_last_error(handle, &p_tiledb_error));
            }
            if (status == (int)Status.TILEDB_OK)
            {
                sbyte* messagePtr;
                status = Methods.tiledb_status_code(Methods.tiledb_error_message(p_tiledb_error, &messagePtr));

                if (status == (int)Status.TILEDB_OK)
                {
                    string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                    sb_message.Append(message);
                }
                else
                {
                    var ex_message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                    sb_message.Append(" Context.handle_error,caught exception:" + ex_message);
                }
            }
            else
            {
                var ex_message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                sb_message.Append(" Context.handle_error,caught exception:" + ex_message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            //fire event
            var args = new ErrorEventArgs(rc, sb_message.ToString());
            OnError(args);
        }

        private void OnError(ErrorEventArgs e)
        {
            var handler = ErrorHappened;
            handler(this, e); //fire the event
        }
    }
}
