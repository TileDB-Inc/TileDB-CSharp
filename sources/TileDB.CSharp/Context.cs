using System;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Context : IDisposable
    {
        private readonly ContextHandle _handle;
        private readonly Config _config;
        private bool _disposed;

        public Context()
        {
            _handle = new ContextHandle();
            _config = new Config();
        }

        public Context(Config config)
        {
            _handle = new ContextHandle(config.Handle);
            _config = config;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing && !_handle.IsInvalid) 
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        internal ContextHandle Handle => _handle;


        private static Context? _default;
        /// <summary>
        /// Get default context.
        /// </summary>
        /// <returns></returns>
        public static Context GetDefault() 
        {
            if (_default == null)
            {
                _default = new Context(new Config());
            }
            return _default;
        }

        /// <summary>
        /// Get statistic string.
        /// </summary>
        /// <returns></returns>
        public string Stats()
        {    
            var result_out = new MarshaledStringOut();
            fixed (sbyte** p_result = &result_out.Value) 
            {
                handle_error(Methods.tiledb_ctx_get_stats(_handle, p_result));
            }
            
            return result_out;
        }

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>

        public Config Config()
        {
            return _config;
        }

        /// <summary>
        /// Get last error message.
        /// </summary>
        /// <returns></returns>
        public string last_error()
        {
            var sb_result = new StringBuilder();
             
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            var status = Methods.tiledb_ctx_get_last_error(_handle, &p_tiledb_error);
            if(status == (int)Status.TILEDB_OK)
            {
                var str_out = new MarshaledStringOut();

                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
                
                if(status == (int)Status.TILEDB_OK)
                {
                    sb_result.Append(str_out);
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
        /// Cancel tasks.
        /// </summary>
        public void cancel_tasks()
        {
            handle_error(Methods.tiledb_ctx_cancel_tasks(_handle));
        }

        /// <summary>
        /// Set tag.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void set_tag(string key, string value)
        {
            if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) 
            {
                throw new ArgumentException("Context.set_tag, key or value is null or empty!");
            }
 
            var ms_key = new MarshaledString(key);
            var ms_value = new MarshaledString(value);
            handle_error(Methods.tiledb_ctx_set_tag(_handle, ms_key, ms_value));
        }

        #region error
        /// <summary>
        /// Default event handler is just printing
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorHappened = (_,e) => {
            var error_msg = $"Error! Code:{e.Code},Message:{e.Message}";
            throw new Exception(error_msg);
        };

 
        internal void handle_error(int rc) 
        {
            if (rc == (int)Status.TILEDB_OK) 
            {
                return;
            }

            var sb_message = new StringBuilder();
            var tiledb_error = new tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            var status = Methods.tiledb_ctx_get_last_error(_handle, &p_tiledb_error);
            if (status == (int)Status.TILEDB_OK)
            {
                var str_out = new MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
              
                if (status == (int)Status.TILEDB_OK)
                {
                    sb_message.Append(str_out);
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

        #endregion error


    }
}