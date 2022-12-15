using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB query condition object.
    /// </summary>
    public sealed unsafe class QueryCondition : IDisposable
    {
        private readonly QueryConditionHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        /// <summary>
        /// Creates a <see cref="QueryCondition"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> associated with this query condition.</param>
        /// <remarks>
        /// The query condition must be initialized by calling
        /// <see cref="Init(string, string, QueryConditionOperatorType)"/>
        /// or <see cref="Init{T}(string, T, QueryConditionOperatorType)"/>.
        /// </remarks>
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

        /// <summary>
        /// Disposes this <see cref="QueryCondition"/>.
        /// </summary>
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
        /// Initializes this <see cref="QueryCondition"/> with a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
        /// <param name="condition_value">The value to compare the attribute with.</param>
        /// <param name="optype">The type of the relationship between the attribute with
        /// the name <paramref name="attribute_name"/> and <paramref name="condition_value"/>.</param>
        /// <remarks>
        /// Query conditions created with <see cref="Create(Context, string, string, QueryConditionOperatorType)"/>
        /// must not call this method.
        /// </remarks>
        public void Init<T>(string attribute_name, T condition_value, QueryConditionOperatorType optype) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_attribute_name = new MarshaledString(attribute_name);
            T[] data = new T[1] { condition_value };
            ulong size = (ulong)Marshal.SizeOf(data[0]);
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_condition_init(ctxHandle, handle, ms_attribute_name,
                    (void*)dataGcHandle.AddrOfPinnedObject(), size, (tiledb_query_condition_op_t)optype));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Initializes this <see cref="QueryCondition"/> with a string.
        /// </summary>
        /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
        /// <param name="condition_value">The string to compare the attribute with.</param>
        /// <param name="optype">The type of the relationship between the attribute with
        /// the name <paramref name="attribute_name"/> and <paramref name="condition_value"/>.</param>
        /// <remarks>
        /// Query conditions created with <see cref="Create(Context, string, string, QueryConditionOperatorType)"/>
        /// must not call this method.
        /// </remarks>
        public void Init(string attribute_name, string condition_value, QueryConditionOperatorType optype)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_attribute_name = new MarshaledString(attribute_name);
            byte[] data = Encoding.ASCII.GetBytes(condition_value);
            ulong size = (ulong)data.Length;
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_condition_init(ctxHandle, handle, ms_attribute_name,
                    (void*)dataGcHandle.AddrOfPinnedObject(), size, (tiledb_query_condition_op_t)optype));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Combines this <see cref="QueryCondition"/> with another one.
        /// </summary>
        /// <param name="rhs">The other query condition to combine.</param>
        /// <param name="combination_optype">The type of the combination.</param>
        /// <returns>A new query condition that combines this one with <paramref name="rhs"/>.</returns>
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
        /// Creates a new <see cref="QueryCondition"/> with a string.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> associated with the query condition.</param>
        /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
        /// <param name="value">The value to compare the attribute with.</param>
        /// <param name="optype">The type of the relationship between the attribute with
        /// the name <paramref name="attribute_name"/> and <paramref name="value"/>.</param>
        public static QueryCondition Create(Context ctx, string attribute_name, string value, QueryConditionOperatorType optype)
        {
            var ret = new QueryCondition(ctx);
            ret.Init(attribute_name, value, optype);
            return ret;
        }

        /// <summary>
        /// Creates a new query condition with datatype <typeparamref name="T"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> associated with the query condition.</param>
        /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
        /// <param name="value">The value to compare the attribute with.</param>
        /// <param name="optype">The type of the relationship between the attribute with
        /// the name <paramref name="attribute_name"/> and <paramref name="value"/>.</param>
        public static QueryCondition Create<T>(Context ctx, string attribute_name, T value, QueryConditionOperatorType optype) where T : struct
        {
            var ret = new QueryCondition(ctx);
            ret.Init(attribute_name, value, optype);
            return ret;
        }
    }
}
