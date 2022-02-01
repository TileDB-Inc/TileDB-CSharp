using System;
using System.Text;
namespace TileDB
{
    public unsafe class Context : IDisposable
    {
        private TileDB.Interop.ContextHandle handle_;
        private TileDB.Config config_;
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
            if (!handle_.IsInvalid)
            {
                handle_.Dispose();
            }

            System.GC.SuppressFinalize(this);

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
            
            TileDB.Interop.MarshaledStringOut result_out = new Interop.MarshaledStringOut(2048);
            int status = TileDB.Interop.Methods.tiledb_ctx_get_stats(handle_, result_out);
            if(status == 0) 
            {
                result = result_out.ToString();
            }
            else
            {
                handle_error(status);
            }
 
            return result;
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
            if(status ==0)
            {
                TileDB.Interop.MarshaledStringOut str_out = new Interop.MarshaledStringOut(2048);
  
                status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, str_out);
                if(status ==0)
                {
                    result = str_out.ToString(); 
                }
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
            if (status == 0)
            {
                TileDB.Interop.MarshaledStringOut str_out = new Interop.MarshaledStringOut(2048);

                status = TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, str_out);
                if (status == 0)
                {
                    message = str_out.ToString();
                }
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