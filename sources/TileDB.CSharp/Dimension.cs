using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB dimension object.
/// </summary>
public sealed unsafe class Dimension : IDisposable
{
    private readonly DimensionHandle _handle;
    private readonly Context _ctx;

    /// <summary>
    /// This value indicates a variable-sized attribute.
    /// It may be returned from <see cref="CellValNum"/>
    /// and can be passed to <see cref="SetCellValNum"/>.
    /// </summary>
    public const uint VariableSized = Constants.VariableSizedImpl;

    internal Dimension(Context ctx, DimensionHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
    }

    /// <summary>
    /// Disposes the <see cref="Dimension"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal DimensionHandle Handle => _handle;

    /// <summary>
    /// Set filter list.
    /// </summary>
    /// <param name="filterList"></param>
    public void SetFilterList(FilterList filterList)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterListHandle = filterList.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_dimension_set_filter_list(ctxHandle, handle, filterListHandle));
    }

    /// <summary>
    /// Sets the number of values per cell for this dimension.
    /// For variable-sized attributes the value should be <see cref="VariableSized"/>.
    /// </summary>
    public void SetCellValNum(uint cellValNum)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_dimension_set_cell_val_num(ctxHandle, handle, cellValNum));
    }

    /// <summary>
    /// Gets the filter list of this dimension.
    /// </summary>
    public FilterList FilterList()
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filter_list_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var dimensionHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_dimension_get_filter_list(ctxHandle, dimensionHandle, &filter_list_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(filter_list_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new FilterList(_ctx, handle);
    }

    /// <summary>
    /// Gets the number of values per cell for the dimension.
    /// </summary>
    /// <returns>
    /// The number of values per cell for the dimension,
    /// or <see cref="VariableSized"/> for variable-sized dimensions.
    /// </returns>
    public uint CellValNum()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        uint cell_value_num;
        _ctx.handle_error(Methods.tiledb_dimension_get_cell_val_num(ctxHandle, handle, &cell_value_num));
        return cell_value_num;
    }

    /// <summary>
    /// Get name of the dimension.
    /// </summary>
    /// <returns></returns>
    public string Name()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* name;
        _ctx.handle_error(Methods.tiledb_dimension_get_name(ctxHandle, handle, &name));

        return MarshaledStringOut.GetStringFromNullTerminated(name);
    }

    /// <summary>
    /// Get <see cref="DataType"/> of the dimension.
    /// </summary>
    public DataType Type()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        tiledb_datatype_t tiledb_datatype;

        _ctx.handle_error(Methods.tiledb_dimension_get_type(ctxHandle, handle, &tiledb_datatype));

        return (DataType)tiledb_datatype;
    }

    /// <summary>
    /// Gets the allowed starting and ending values of the dimension, inclusive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public (T Start, T End) GetDomain<T>() where T : struct
    {
        ErrorHandling.CheckDataType<T>(Type());
        void* value_p;

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_dimension_get_domain(ctxHandle, handle, &value_p));

        return Unsafe.ReadUnaligned<SequentialPair<T>>(value_p);
    }

    /// <summary>
    /// Gets the dimension's tile extent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T TileExtent<T>() where T : struct
    {
        ErrorHandling.CheckDataType<T>(Type());
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* value_p;
        _ctx.handle_error(Methods.tiledb_dimension_get_tile_extent(ctxHandle, handle, &value_p));

        return Unsafe.ReadUnaligned<T>(value_p);
    }

    /// <summary>
    /// Gets the dimension's tile extent as string.
    /// </summary>
    public string TileExtentToStr()
    {
        var datatype = Type();
        var t = EnumUtil.DataTypeToType(datatype);
        return System.Type.GetTypeCode(t) switch
        {
            TypeCode.Int16 => Format<short>(),
            TypeCode.Int32 => Format<int>(),
            TypeCode.Int64 => Format<long>(),
            TypeCode.UInt16 => Format<ushort>(),
            TypeCode.UInt32 => Format<uint>(),
            TypeCode.UInt64 => Format<ulong>(),
            TypeCode.Single => Format<float>(),
            TypeCode.Double => Format<double>(),
            _ => ","
        };

        string Format<T>() where T : struct, IFormattable => TileExtent<T>().ToString(null, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the dimension's domain as string.
    /// </summary>
    public string DomainToStr()
    {
        var datatype = Type();
        var t = EnumUtil.DataTypeToType(datatype);
        return System.Type.GetTypeCode(t) switch
        {
            TypeCode.Int16 => Format<short>(),
            TypeCode.Int32 => Format<int>(),
            TypeCode.Int64 => Format<long>(),
            TypeCode.UInt16 => Format<ushort>(),
            TypeCode.UInt32 => Format<uint>(),
            TypeCode.UInt64 => Format<ulong>(),
            TypeCode.Single => Format<float>(),
            TypeCode.Double => Format<double>(),
            _ => ","
        };

        string Format<T>() where T : struct
        {
            (T Start, T End) = GetDomain<T>();
            return FormattableString.Invariant($"{Start},{End}");
        }
    }

    /// <summary>
    /// Creates a <see cref="Dimension"/>.
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="ctx">The <see cref="Context"/> associated with the dimension.</param>
    /// <param name="name">The dimension's name.</param>
    /// <param name="boundLower">The dimension's lower bound, inclusive.</param>
    /// <param name="boundUpper">The dimension's upper bound, inclusive.</param>
    /// <param name="extent">The dimension's tile extent.</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    public static Dimension Create<T>(Context ctx, string name, T boundLower, T boundUpper, T extent)
    {
        var tiledb_datatype = (tiledb_datatype_t)EnumUtil.TypeToDataType(typeof(T));

        if (tiledb_datatype == tiledb_datatype_t.TILEDB_STRING_ASCII)
        {
            var str_dim_handle = DimensionHandle.Create(ctx, name, tiledb_datatype, null, null);
            return new Dimension(ctx, str_dim_handle);
        }

        SequentialPair<T> data = (boundLower, boundUpper);
        var handle = DimensionHandle.Create(ctx, name, tiledb_datatype, &data, &extent);
        return new Dimension(ctx, handle);
    }

    /// <summary>
    /// Create a string <see cref="Dimension"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with the dimension.</param>
    /// <param name="name">The dimension's name.</param>
    /// <returns></returns>
    public static Dimension CreateString(Context ctx, string name)
    {
        var str_dim_handle = DimensionHandle.Create(ctx, name, tiledb_datatype_t.TILEDB_STRING_ASCII, null, null);
        return new Dimension(ctx, str_dim_handle);
    }
}
