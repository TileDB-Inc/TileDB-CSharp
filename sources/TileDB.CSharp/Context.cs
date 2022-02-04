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



        public string stats()
        {
            string result = "";
            if (handle_.IsInvalid) 
            {
                return result;
            }
            
            TileDB.Interop.MarshaledStringOut result_out = new Interop.MarshaledStringOut();
            fixed (sbyte** p_result = &result_out.Value) 
            {
                handle_error(TileDB.Interop.Methods.tiledb_ctx_get_stats(handle_, p_result));
            }
            
            return result_out;
        }

        public Config config()
        {
            return config_;
        }

        public string last_error()
        {
            string result = "";
            if (handle_.IsInvalid) 
            {
                return result;
            }
             
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_ctx_get_last_error(handle_, &p_tiledb_error);
            if(status == (int)Status.TILEDB_OK)
            {
                TileDB.Interop.MarshaledStringOut str_out = new Interop.MarshaledStringOut();

                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
                
                if(status == (int)Status.TILEDB_OK)
                {
                    result = str_out; 
                }
                else
                {
                    string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknow error with code:" + status.ToString());
                    throw new TileDB.ErrorException("Context.last_error,caught exception:" + message);
                }
            }
            else
            {
                string message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknow error with code:" + status.ToString());
                throw new TileDB.ErrorException("Context.last_error,caught exception:" + message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            
            return result;
        }

        public void cancel_tasks()
        {
            if (handle_.IsInvalid) 
            {
                return ;
            }
            handle_error(TileDB.Interop.Methods.tiledb_ctx_cancel_tasks(handle_));
        }

        public void set_tag(string key, string value)
        {
            if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) 
            {
                throw new System.ArgumentException("Context.set_tag, key or value is null or empty!");
            }
            if (handle_.IsInvalid) 
            {
                throw new System.InvalidOperationException("Context.set_tag, invalid handle!");
            }
 
            TileDB.Interop.MarshaledString ms_key = new Interop.MarshaledString(key);
            TileDB.Interop.MarshaledString ms_value = new Interop.MarshaledString(value);
            handle_error(TileDB.Interop.Methods.tiledb_ctx_set_tag(handle_, ms_key, ms_value));
        }

        #region error
        /// <summary>
        /// Default event handler is just printing
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorHappened = new EventHandler<ErrorEventArgs>(
            (o,e) => {
                System.Console.WriteLine("Error! Code:{0},Message:{1}", e.Code, e.Message);
            }
            );

 
        internal void handle_error(int rc) 
        {
            if (rc == (int)Status.TILEDB_OK) 
            {
                return;
            }
            string message = "";
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_ctx_get_last_error(handle_, &p_tiledb_error);
            if (status == (int)Status.TILEDB_OK)
            {
                TileDB.Interop.MarshaledStringOut str_out = new Interop.MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
              
                if (status == (int)Status.TILEDB_OK)
                {
                    message = str_out;
                }
                else
                {
                    string ex_message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknow error with code:" + status.ToString());
                    throw new TileDB.ErrorException("Context.handle_error,caught exception:" + ex_message);
                }
            }
            else
            {
                string ex_message = Enum.IsDefined(typeof(TileDB.Status), status) ? ((TileDB.Status)status).ToString() : ("Unknow error with code:" + status.ToString());
                throw new TileDB.ErrorException("Context.handle_error,caught exception:" + ex_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            ErrorEventArgs args = new ErrorEventArgs(rc, message);
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