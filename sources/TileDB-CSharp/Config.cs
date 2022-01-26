using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class Config : IDisposable
    {
        private TileDB.Interop.ConfigHandle handle_;
    
        public Config()
        {
            handle_ = new TileDB.Interop.ConfigHandle();

            TileDB.Interop.tiledb_config_t* p = handle_;

            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            TileDB.Interop.Methods.tiledb_config_alloc(&p, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

        }

        public void Dispose()
        {
            TileDB.Interop.tiledb_config_t* p = handle_;
            TileDB.Interop.Methods.tiledb_config_free(&p);
        }



        internal TileDB.Interop.ConfigHandle Handle
        {
            get { return handle_; }
        }

        public int set(string param, string value)
        {
            int status = -1;

            TileDB.Interop.tiledb_config_t* p = handle_;
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            status = TileDB.Interop.Methods.tiledb_config_set(p, param, value, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return status;
        }

        public string? get(string param)
        {
            TileDB.Interop.tiledb_config_t* p = handle_;
            IntPtr str_intptr = System.IntPtr.Zero;
            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            int status = TileDB.Interop.Methods.tiledb_config_get(p, param, out str_intptr, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);
            return status !=0 ?  "" : System.Runtime.InteropServices.Marshal.PtrToStringAnsi(str_intptr);
        }

        public int load_from_file(string filename)
        {
            int status = -1;

            TileDB.Interop.tiledb_config_t* p = handle_;

            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            status = TileDB.Interop.Methods.tiledb_config_load_from_file(p, filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return status;
        }

        public int save_to_file(string filename)
        {
            int status = -1;

            TileDB.Interop.tiledb_config_t* p = handle_;

            TileDB.Interop.tiledb_error_t tiledb_error = new TileDB.Interop.tiledb_error_t();
            TileDB.Interop.tiledb_error_t* p_tiledb_error = &tiledb_error;
            status = TileDB.Interop.Methods.tiledb_config_save_to_file(p, filename, &p_tiledb_error);
            TileDB.Interop.Methods.tiledb_error_free(&p_tiledb_error);

            return status;
        }


    }
}
