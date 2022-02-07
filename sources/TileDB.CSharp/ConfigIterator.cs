using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class ConfigIterator : IDisposable
    {
        private TileDB.Interop.ConfigIteratorHandle handle_;
        private TileDB.Interop.ConfigHandle hconfig_;
        private bool disposed_ = false;

        public ConfigIterator(TileDB.Interop.ConfigHandle hconfig, string prefix)
        {
            hconfig_ = hconfig;
            handle_ = new TileDB.Interop.ConfigIteratorHandle(hconfig, prefix);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            
            if (!disposed_) {
                if (disposing && (!handle_.IsInvalid)) 
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }

        }

        internal TileDB.Interop.ConfigIteratorHandle Handle
        {
            get { return handle_; }
        }

        internal string get_last_error(TileDB.Interop.tiledb_error_t *p_tiledb_error, int status)
        {
            StringBuilder sb_result = new StringBuilder();
            if (Enum.IsDefined(typeof(TileDB.Status), status))
            {
                sb_result.Append( "Status: " + (TileDB.Status)status).ToString();
                TileDB.Interop.MarshaledStringOut str_out = new Interop.MarshaledStringOut();
                fixed(sbyte** p_str = &str_out.Value) 
                {
                    TileDB.Interop.Methods.tiledb_error_message(p_tiledb_error, p_str);
                }
                sb_result.Append(", Message: " + str_out);                    
            } else {
                sb_result.Append("Unknown error with code: " + status.ToString());
            }
            
            return sb_result.ToString();
        }

        public Tuple<string, string> here()
        {
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.MarshaledStringOut s_param = new Interop.MarshaledStringOut();
            TileDB.Interop.MarshaledStringOut s_value = new Interop.MarshaledStringOut();
            int status;
            fixed(sbyte** param_str = &s_param.Value, value_str = &s_value.Value)
            {
                status = TileDB.Interop.Methods.tiledb_config_iter_here(handle_, param_str, value_str, &p_tiledb_error);
            }

            if (status != (int)Status.TILEDB_OK)
            {
                string err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.set, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            
            return new Tuple<string, string>(s_param, s_value);
        }

        public void next()
        {
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_iter_next(handle_, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                string err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("ConfigIterator.next, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }

        public bool done() 
        {
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            Int32 c_done;
            int status = TileDB.Interop.Methods.tiledb_config_iter_done(handle_, &c_done, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                string err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("ConfigIterator.done, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);            
            return c_done == 1;
        }

        public void reset(string prefix)
        {
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.MarshaledString ms_prefix = new Interop.MarshaledString(prefix);
            int status = TileDB.Interop.Methods.tiledb_config_iter_reset(hconfig_, handle_, ms_prefix, &p_tiledb_error);
            if (status != (int)Status.TILEDB_OK)
            {
                string err_message = get_last_error(p_tiledb_error, status);
                TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
                throw new TileDB.ErrorException("Config.reset, caught exception:" + err_message);
            }
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
        }
    }
}