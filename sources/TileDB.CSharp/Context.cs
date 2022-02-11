using System;
using System.Text;
namespace TileDB
{
    public unsafe class Context : IDisposable
    {
        private TileDB.Interop.ContextHandle handle_;
        private TileDB.Config config_;
        private bool disposed_ = false;

        public Context()
        {
            handle_ = new TileDB.Interop.ContextHandle();
            config_ = new Config();
        }

        public Context(Config config)
        {
            handle_ = new TileDB.Interop.ContextHandle(config.Handle);
            config_ = config;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing) 
        {
            if (!disposed_) 
            {
                if (disposing && !handle_.IsInvalid) 
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }
        }

        internal TileDB.Interop.ContextHandle Handle 
        {
            get { return handle_; }
        }


        private static Context? default_ = null;
        /// <summary>
        /// Get default context.
        /// </summary>
        /// <returns></returns>
        public static Context GetDefault() 
        {
            if (default_ == null)
            {
                default_ = new Context(new Config());
            }
            return default_;
        }

        /// <summary>
        /// Get statistic string.
        /// </summary>
        /// <returns></returns>
        public string stats()
        {    
            var result_out = new Interop.MarshaledStringOut();
            fixed (sbyte** p_result = &result_out.Value) 
            {
                handle_error(TileDB.Interop.Methods.tiledb_ctx_get_stats(handle_, p_result));
            }
            
            return result_out;
        }

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>

        public Config config()
        {
            return config_;
        }

        /// <summary>
        /// Get last error message.
        /// </summary>
        /// <returns></returns>
        public string last_error()
        {
            var sb_result = new StringBuilder();
             
            var tiledb_error = new TileDB.Interop.tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_ctx_get_last_error(handle_, &p_tiledb_error);
            if(status == (int)Status.TILEDB_OK)
            {
                var str_out = new Interop.MarshaledStringOut();

                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
                
                if(status == (int)Status.TILEDB_OK)
                {
                    sb_result.Append(str_out);
                }
                else
                {
                    var message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                    sb_result.Append(" Context.last_error,caught exception:" + message);
                }
            }
            else
            {
                var message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                sb_result.Append(" Context.last_error,caught exception:" + message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            
            return sb_result.ToString();
        }

        /// <summary>
        /// Cancel tasks.
        /// </summary>
        public void cancel_tasks()
        {
            handle_error(TileDB.Interop.Methods.tiledb_ctx_cancel_tasks(handle_));
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
                throw new System.ArgumentException("Context.set_tag, key or value is null or empty!");
            }
 
            var ms_key = new Interop.MarshaledString(key);
            var ms_value = new Interop.MarshaledString(value);
            handle_error(TileDB.Interop.Methods.tiledb_ctx_set_tag(handle_, ms_key, ms_value));
        }

        #region error
        /// <summary>
        /// Default event handler is just printing
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorHappened = new EventHandler<ErrorEventArgs>(
            (o,e) => {
                string error_msg = string.Format("Error! Code:{0},Message:{1}", e.Code, e.Message);
                throw new System.Exception(error_msg);
            }
            );

 
        internal void handle_error(int rc) 
        {
            if (rc == (int)Status.TILEDB_OK) 
            {
                return;
            }

            var sb_message = new StringBuilder();
            var tiledb_error = new TileDB.Interop.tiledb_error_t();
            var p_tiledb_error = &tiledb_error;
            var status = TileDB.Interop.Methods.tiledb_ctx_get_last_error(handle_, &p_tiledb_error);
            if (status == (int)Status.TILEDB_OK)
            {
                var str_out = new Interop.MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
              
                if (status == (int)Status.TILEDB_OK)
                {
                    sb_message.Append(str_out);
                }
                else
                {
                    var ex_message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                    sb_message.Append(" Context.handle_error,caught exception:" + ex_message);
                }
            }
            else
            {
                var ex_message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknown error with code:" + status.ToString());
                sb_message.Append(" Context.handle_error,caught exception:" + ex_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            //fire event
            var args = new ErrorEventArgs(rc, sb_message.ToString());
            OnError(args);
        }

        protected void OnError(ErrorEventArgs e) 
        {
            var handler = ErrorHappened;
            if (handler != null) 
            {
                handler(this, e); //fire the event
            }
        }

        #endregion error


    }
}