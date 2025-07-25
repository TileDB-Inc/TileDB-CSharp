using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB query condition object.
/// </summary>
public sealed unsafe class QueryCondition : IDisposable
{
    private readonly QueryConditionHandle _handle;
    private readonly Context _ctx;

    /// <summary>
    /// Creates a <see cref="QueryCondition"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with this query condition.</param>
    /// <remarks>
    /// The query condition must be initialized by calling
    /// <see cref="Init(string, string, QueryConditionOperatorType)"/>
    /// or <see cref="Init{T}(string, T, QueryConditionOperatorType)"/>.
    /// </remarks>
    private QueryCondition(Context ctx)
    {
        _ctx = ctx;
        _handle = QueryConditionHandle.Create(_ctx);
    }

    private QueryCondition(Context ctx, QueryConditionHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
    }

    /// <summary>
    /// Disposes this <see cref="QueryCondition"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal QueryConditionHandle Handle => _handle;

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
    private void Init<T>(string attribute_name, T condition_value, QueryConditionOperatorType optype) where T : struct
    {
        ErrorHandling.ThrowIfManagedType<T>();
        Init(attribute_name, &condition_value, (ulong)sizeof(T), optype);
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
    private void Init(string attribute_name, string condition_value, QueryConditionOperatorType optype)
    {
        using var ms_condition_value = new MarshaledString(condition_value);
        Init(attribute_name, ms_condition_value, (ulong)ms_condition_value.Length, optype);
    }

    private void Init(string attribute_name, void* condition_value, ulong condition_value_size, QueryConditionOperatorType optype)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_attribute_name = new MarshaledString(attribute_name);
        _ctx.handle_error(Methods.tiledb_query_condition_init(ctxHandle, handle, ms_attribute_name, condition_value, condition_value_size, (tiledb_query_condition_op_t)optype));
    }

    private void SetUseEnumeration(bool value)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_query_condition_set_use_enumeration(ctxHandle, handle, value ? 1 : 0));
    }

    private static QueryCondition Combine(QueryCondition lhs, QueryCondition? rhs, QueryConditionCombinationOperatorType combination_optype)
    {
        var ctx = lhs._ctx;
        if (rhs is { _ctx: Context rhsCtx } && ctx != rhsCtx)
        {
            ThrowHelpers.ThrowQueryConditionDifferentContexts();
        }

        var handle = new QueryConditionHandle();
        var successful = false;
        tiledb_query_condition_t* condition_p = null;
        try
        {
            using (var ctxHandle = ctx.Handle.Acquire())
            using (var lhsHandle = lhs.Handle.Acquire())
            using (var rhsHandle = rhs?.Handle?.Acquire() ?? default)
            {
                ctx.handle_error(Methods.tiledb_query_condition_combine(ctxHandle, lhsHandle, rhsHandle,
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

        return new QueryCondition(ctx, handle);
    }

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
        return Create(ctx, attribute_name, value, optype, true);
    }

    /// <summary>
    /// Creates a new query condition with datatype <typeparamref name="T"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with the query condition.</param>
    /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
    /// <param name="value">The value to compare the attribute with.</param>
    /// <param name="optype">The type of the relationship between the attribute with
    /// the name <paramref name="attribute_name"/> and <paramref name="value"/>.</param>
    /// <param name="useEnumeration">Whether to match the query condition on the
    /// enumerated value instead of the underlying value. Optional, defaults to
    /// <see langword="true"/>.</param>
    public static QueryCondition Create<T>(Context ctx, string attribute_name, T value, QueryConditionOperatorType optype, bool useEnumeration = true) where T : struct
    {
        var ret = new QueryCondition(ctx);
        ret.Init(attribute_name, value, optype);
        if (!useEnumeration)
        {
            ret.SetUseEnumeration(false);
        }
        return ret;
    }

    /// <summary>
    /// Creates a new query condition that is satisfied where the given attribute is null.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with the query condition.</param>
    /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
    public static QueryCondition CreateIsNull(Context ctx, string attribute_name)
    {
        var ret = new QueryCondition(ctx);
        ret.Init(attribute_name, null, 0, QueryConditionOperatorType.Equal);
        return ret;
    }

    /// <summary>
    /// Creates a new query condition that is satisfied where the given attribute is not null.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with the query condition.</param>
    /// <param name="attribute_name">The name of the attribute the query condition refers to.</param>
    public static QueryCondition CreateIsNotNull(Context ctx, string attribute_name)
    {
        var ret = new QueryCondition(ctx);
        ret.Init(attribute_name, null, 0, QueryConditionOperatorType.NotEqual);
        return ret;
    }

    /// <summary>
    /// Creates the conjunction of two <see cref="QueryCondition"/>s.
    /// </summary>
    /// <param name="lhs">The first query condition.</param>
    /// <param name="rhs">The second query condition.</param>
    /// <returns>A query condition that will be satisfied if both
    /// <paramref name="lhs"/> and <paramref name="rhs"/> are satisfied.</returns>
    public static QueryCondition operator &(QueryCondition lhs, QueryCondition rhs) =>
        Combine(lhs, rhs, QueryConditionCombinationOperatorType.And);

    /// <summary>
    /// Creates the disjunction of two <see cref="QueryCondition"/>s.
    /// </summary>
    /// <param name="lhs">The first query condition.</param>
    /// <param name="rhs">The second query condition.</param>
    /// <returns>A query condition that will be satisfied if at least one of
    /// <paramref name="lhs"/> or <paramref name="rhs"/> are satisfied.</returns>
    public static QueryCondition operator |(QueryCondition lhs, QueryCondition rhs) =>
        Combine(lhs, rhs, QueryConditionCombinationOperatorType.Or);

    /// <summary>
    /// Creates the negation of a <see cref="QueryCondition"/>.
    /// </summary>
    /// <param name="condition">The query condition to negate.</param>
    /// <returns>A query condition that will be satisfied if
    /// <paramref name="condition"/> is not satisfied.</returns>
    public static QueryCondition operator !(QueryCondition condition)
    {
        var ctx = condition._ctx;

        var handle = new QueryConditionHandle();
        var successful = false;
        tiledb_query_condition_t* condition_p = null;
        try
        {
            using (var ctxHandle = ctx.Handle.Acquire())
            using (var conditionHandle = condition.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_query_condition_negate(ctxHandle, conditionHandle, &condition_p));
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

        return new QueryCondition(ctx, handle);
    }
}
