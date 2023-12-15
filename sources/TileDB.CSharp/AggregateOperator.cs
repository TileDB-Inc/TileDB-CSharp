using System;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents an aggregate operator that can be applied over a query field.
/// </summary>
/// <remarks>
/// This class abstracts the <c>tiledb_channel_operator_t</c> type of the TileDB C API.
/// </remarks>
public abstract class AggregateOperator
{
    // Prevent inheriting the class outside of this assembly.
    private protected AggregateOperator() { }

    internal abstract unsafe tiledb_channel_operator_t* GetHandle(tiledb_ctx_t* ctx);

    /// <summary>
    /// Computes the sum of all values in the field.
    /// </summary>
    public static AggregateOperator Sum { get; } = new SimpleAggregateOperator(OperatorKind.Sum);

    /// <summary>
    /// Computes the minimum value in the field.
    /// </summary>
    public static AggregateOperator Min { get; } = new SimpleAggregateOperator(OperatorKind.Min);

    /// <summary>
    /// Computes the maximum value in the field.
    /// </summary>
    public static AggregateOperator Max { get; } = new SimpleAggregateOperator(OperatorKind.Max);

    /// <summary>
    /// Computes the mean of all values in the field.
    /// </summary>
    public static AggregateOperator Mean { get; } = new SimpleAggregateOperator(OperatorKind.Mean);

    /// <summary>
    /// Counts the number of null values in the field.
    /// </summary>
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
