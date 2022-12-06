using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
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
            _handle = QueryConditionHandle.Create(_ctx);
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
        public void Init<T>(string attribute_name, T condition_value, QueryConditionOperatorType optype) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_attribute_name = new MarshaledString(attribute_name);
            _ctx.handle_error(Methods.tiledb_query_condition_init(ctxHandle, handle, ms_attribute_name,
                &condition_value, (ulong)sizeof(T), (tiledb_query_condition_op_t)optype));
        }

        /// <summary>
        /// Initialize a TileDB query string condition object.
        /// </summary>
        /// <param name="attribute_name"></param>
        /// <param name="condition_value"></param>
        /// <param name="optype"></param>
        public void Init(string attribute_name, string condition_value, QueryConditionOperatorType optype)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_attribute_name = new MarshaledString(attribute_name);
            using var ms_condition_value = new MarshaledString(condition_value);
            ulong size = (ulong)ms_condition_value.Length;
            _ctx.handle_error(Methods.tiledb_query_condition_init(ctxHandle, handle, ms_attribute_name,
                ms_condition_value, size, (tiledb_query_condition_op_t)optype));
        }

        public QueryCondition Combine(QueryCondition rhs, QueryConditionCombinationOperatorType combination_optype)
        {
            var handle = new QueryConditionHandle();
            var successful = false;
            tiledb_query_condition_t* condition_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var lhsHandle = _handle.Acquire())
                using (var rhsHandle = rhs.Handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_query_condition_combine(ctxHandle, lhsHandle, rhsHandle,
                        (tiledb_query_condition_combination_op_t)combination_optype, &condition_p));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(condition_p);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }

            return new QueryCondition(_ctx, handle);
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
            var ret = new QueryCondition(ctx);
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
        public static QueryCondition Create<T>(Context ctx, string attribute_name, T value, QueryConditionOperatorType optype) where T : struct
        {
            var ret = new QueryCondition(ctx);
            ret.Init(attribute_name, value, optype);
            return ret;
        }
    }
}
