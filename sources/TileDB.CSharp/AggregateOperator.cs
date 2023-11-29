using System;
using TileDB.Interop;

namespace TileDB.CSharp;

public abstract class AggregateOperator
{
    // Prevent inheriting the class outside of this assembly.
    private protected AggregateOperator() { }

    internal abstract unsafe tiledb_channel_operator_t* GetHandle(tiledb_ctx_t* ctx);

    public static AggregateOperator Sum { get; } = new SimpleAggregateOperator(OperatorKind.Sum);

    public static AggregateOperator Min { get; } = new SimpleAggregateOperator(OperatorKind.Min);

    public static AggregateOperator Max { get; } = new SimpleAggregateOperator(OperatorKind.Max);

    public static AggregateOperator Mean { get; } = new SimpleAggregateOperator(OperatorKind.Mean);

    public static AggregateOperator NullCount { get; } = new SimpleAggregateOperator(OperatorKind.NullCount);

    private enum OperatorKind
    {
        Sum,
        Min,
        Max,
        Mean,
        NullCount
    }

    private sealed class SimpleAggregateOperator : AggregateOperator
    {
        private readonly OperatorKind _kind;

        public SimpleAggregateOperator(OperatorKind kind)
        {
            _kind = kind;
        }

        internal override unsafe tiledb_channel_operator_t* GetHandle(tiledb_ctx_t* ctx)
        {
            tiledb_channel_operator_t* op;
            var ret = _kind switch
            {
                OperatorKind.Sum => Methods.tiledb_channel_operator_sum_get(ctx, &op),
                OperatorKind.Min => Methods.tiledb_channel_operator_min_get(ctx, &op),
                OperatorKind.Max => Methods.tiledb_channel_operator_max_get(ctx, &op),
                OperatorKind.Mean => Methods.tiledb_channel_operator_mean_get(ctx, &op),
                OperatorKind.NullCount => Methods.tiledb_channel_operator_null_count_get(ctx, &op),
                _ => throw new InvalidOperationException(),
            };
            ErrorHandling.ThrowOnError(ret);
            return op;
        }
    }
}
