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
            TileDB.Interop.tiledb_ctx_t* p = handle_;

            TileDB.Interop.tiledb_config_t* tiledb_config = config_.Handle;
            TileDB.Interop.Methods.tiledb_ctx_alloc(tiledb_config, &p);
            
        }

        public Context(Config config)
        {
            handle_ = new TileDB.Interop.ContextHandle();
            config_ = config;
            TileDB.Interop.tiledb_ctx_t* p = handle_;

            TileDB.Interop.tiledb_config_t* tiledb_config = config_.Handle;
            TileDB.Interop.Methods.tiledb_ctx_alloc(tiledb_config, &p);
        }

        public void Dispose()
        {
            TileDB.Interop.tiledb_ctx_t* p = handle_;
            TileDB.Interop.Methods.tiledb_ctx_free(&p);
        }

        internal TileDB.Interop.ContextHandle Handle 
        {
            get { return handle_; }
        }



        public string stats()
        {
            string result = "";
            TileDB.Interop.tiledb_ctx_t* p = handle_;
            TileDB.Interop.MarshaledStringOut result_out = new Interop.MarshaledStringOut(2048);
            int status = TileDB.Interop.Methods.tiledb_ctx_get_stats(p, result_out);
            if(status == 0) 
            {
                result = result_out.ToString();
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

            TileDB.Interop.tiledb_ctx_t* p = handle_;
             
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_ctx_get_last_error(p, &p_tiledb_error);
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

        public int cancel_tasks()
        {
            TileDB.Interop.tiledb_ctx_t* p = handle_;   
            return TileDB.Interop.Methods.tiledb_ctx_cancel_tasks(p);
        }

        public int set_tag(string key, string value)
        {
            TileDB.Interop.tiledb_ctx_t* p = handle_;
            TileDB.Interop.MarshaledString ms_key = new Interop.MarshaledString(key);
            TileDB.Interop.MarshaledString ms_value = new Interop.MarshaledString(value);
            return TileDB.Interop.Methods.tiledb_ctx_set_tag(p, ms_key, ms_value);
        }



    }
}