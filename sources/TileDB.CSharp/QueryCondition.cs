using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class QueryCondition : IDisposable
    {
        private readonly QueryConditionHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;


        public QueryCondition(Context ctx)
        {
            _ctx = ctx;
            _handle = new QueryConditionHandle(_ctx.Handle);
        }

        internal QueryCondition(Context ctx, QueryConditionHandle handle)
        {
            _ctx = ctx;
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

        internal QueryConditionHandle Handle => _handle;

        #region capi functions
        /// <summary>
        /// Initialize a TileDB query condition object.
        /// </summary>
        /// <param name="attribute_name"></param>
        /// <param name="condition_value"></param>
        /// <param name="optype"></param>
        public void Init<T>(string attribute_name, T condition_value, QueryConditionOperatorType optype) where T: struct
        {
            var ms_attribute_name = new MarshaledString(attribute_name);
            T[] data = new T[1] { condition_value };
            ulong size = (ulong)Marshal.SizeOf(data[0]);
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_condition_init(_ctx.Handle, _handle, ms_attribute_name,
                    (void*)dataGcHandle.AddrOfPinnedObject(), size, (tiledb_query_condition_op_t)optype));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Initialize a TileDB query string condition object.
        /// </summary>
        /// <param name="attribute_name"></param>
        /// <param name="condition_value"></param>
        /// <param name="optype"></param>
        public void Init(string attribute_name, string condition_value, QueryConditionOperatorType optype) 
        {
            var ms_attribute_name = new MarshaledString(attribute_name);
            byte[] data = Encoding.ASCII.GetBytes(condition_value);
            ulong size = (ulong)data.Length;
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_condition_init(_ctx.Handle, _handle, ms_attribute_name,
                    (void*)dataGcHandle.AddrOfPinnedObject(), size, (tiledb_query_condition_op_t)optype));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        public QueryCondition Combine(QueryCondition rhs, QueryConditionCombinationOperatorType combination_optype)
        {
            tiledb_query_condition_t* condition_p = null;
            _ctx.handle_error(Methods.tiledb_query_condition_combine(_ctx.Handle, _handle, rhs.Handle,(tiledb_query_condition_combination_op_t)combination_optype, &condition_p));

            if (condition_p == null)
            {
                throw new ErrorException("QueryCondition.combine, query condition pointer is null");
            }

            return new QueryCondition(_ctx, condition_p);
        }

        #endregion capi functions

        /// <summary>
        /// Create a new query condition with a string.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="attribute_name"></param>
        /// <param name="value"></param>
        /// <param name="optype"></param>
        /// <returns></returns>
        public static QueryCondition Create(Context ctx, string attribute_name, string value, QueryConditionOperatorType optype)
        {
            var ret =  new QueryCondition(ctx);
            ret.Init(attribute_name, value, optype);
            return ret;
        }

        /// <summary>
        /// Create a new query condition with datatype T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="attribute_name"></param>
        /// <param name="value"></param>
        /// <param name="optype"></param>
        /// <returns></returns>
        public static QueryCondition Create<T>(Context ctx, string attribute_name, T value, QueryConditionOperatorType optype) where T: struct
        {
            var ret = new QueryCondition(ctx);
            ret.Init<T>(attribute_name, value, optype);
            return ret;
        }


    }//class
}//namespace