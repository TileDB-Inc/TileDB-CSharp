using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

public abstract class AggregateOperation
{
    // Prevent inheriting the class outside of this assembly.
    private protected AggregateOperation() { }

    public static AggregateOperation Unary(AggregateOperator op, string fieldName) => new UnaryAggregateOperation(op, fieldName);

    public static AggregateOperation Count { get; } = new CountAggregateOperation();

    internal abstract unsafe ChannelOperationHandle CreateHandle(Context ctx, Query q);

    private sealed class UnaryAggregateOperation : AggregateOperation
    {
        private readonly AggregateOperator _operator;

        private readonly string _fieldName;

        public UnaryAggregateOperation(AggregateOperator op, string fieldName)
        {
            _operator = op;
            _fieldName = fieldName;
        }

        internal override unsafe ChannelOperationHandle CreateHandle(Context ctx, Query q)
        {
            var handle = new ChannelOperationHandle();
            bool successful = false;
            tiledb_channel_operation_t* op = null;
            try
            {
                using var ctxHandle = ctx.Handle.Acquire();
                using var queryHandle = q.Handle.Acquire();
                using var ms_fieldName = new MarshaledString(_fieldName);
                ctx.handle_error(Methods.tiledb_create_unary_aggregate(ctxHandle, queryHandle, _operator.GetHandle(ctxHandle), ms_fieldName, &op));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(ctx, op);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }
            return handle;
        }
    }

    private sealed class CountAggregateOperation : AggregateOperation
    {
        internal override unsafe ChannelOperationHandle CreateHandle(Context ctx, Query q)
        {
            var handle = new ChannelOperationHandle();
            bool successful = false;
            tiledb_channel_operation_t* op = null;
            try
            {
                using var ctxHandle = ctx.Handle.Acquire();
                ctx.handle_error(Methods.tiledb_aggregate_count_get(ctxHandle, &op));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(ctx, op);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }
            return handle;
        }
    }
}
