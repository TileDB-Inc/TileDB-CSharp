using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB attribute object.
/// </summary>
public sealed unsafe class Attribute : IDisposable
{
    private readonly AttributeHandle _handle;
    private readonly Context _ctx;

    /// <summary>
    /// This value indicates a variable-sized attribute.
    /// It may be returned from <see cref="CellValNum"/>
    /// and can be passed to <see cref="SetCellValNum"/>.
    /// </summary>
    public const uint VariableSized = Constants.VariableSizedImpl;

    /// <summary>
    /// Creates an <see cref="Attribute"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with the attribute.</param>
    /// <param name="name">The attribute's name.</param>
    /// <param name="dataType">The attribute's data type.</param>
    public Attribute(Context ctx, string name, DataType dataType)
    {
        _ctx = ctx;
        var tiledb_datatype = (tiledb_datatype_t)dataType;
        _handle = AttributeHandle.Create(_ctx, name, tiledb_datatype);
        if (EnumUtil.IsStringType(dataType))
        {
            SetCellValNum(VariableSized);
        }
    }

    internal Attribute(Context ctx, AttributeHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
    }

    /// <summary>
    /// Disposes this <see cref="Attribute"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal AttributeHandle Handle => _handle;

    /// <summary>
    /// Sets whether this <see cref="Attribute"/> can take null values.
    /// </summary>
    public void SetNullable(bool nullable)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        var int8_nullable = nullable ? (byte)1 : (byte)0;
        _ctx.handle_error(Methods.tiledb_attribute_set_nullable(ctxHandle, handle, int8_nullable));
    }

    /// <summary>
    /// Sets the <see cref="CSharp.FilterList"/> of <see cref="Filter"/>s that will be applied to this <see cref="Attribute"/>.
    /// </summary>
    public void SetFilterList(FilterList filterList)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterListHandle = filterList.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_attribute_set_filter_list(ctxHandle, handle, filterListHandle));
    }

    /// <summary>
    /// Sets the number of values per cell for this attribute.
    /// For variable-sized attributes the value should be <see cref="VariableSized"/>.
    /// </summary>
    public void SetCellValNum(uint cellValNum)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_attribute_set_cell_val_num(ctxHandle, handle, cellValNum));
    }
    
    /// <summary>
    /// Sets the name of the <see cref="Attribute"/>'s enumeration.
    /// </summary>
    public void SetEnumerationName(string enumerationName)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_enumerationName = new MarshaledString(enumerationName);
        _ctx.handle_error(Methods.tiledb_attribute_set_enumeration_name(ctxHandle, handle, ms_enumerationName));
    }

    /// <summary>
    /// Get name of the attribute.
    /// </summary>
    /// <returns></returns>
    public string Name()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* result;
        _ctx.handle_error(Methods.tiledb_attribute_get_name(ctxHandle, handle, &result));

        return MarshaledStringOut.GetStringFromNullTerminated(result);
    }

    /// <summary>
    /// Gets the data type of this <see cref="Attribute"/>.
    /// </summary>
    public DataType Type()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        tiledb_datatype_t tiledb_datatype;
        _ctx.handle_error(Methods.tiledb_attribute_get_type(ctxHandle, handle, &tiledb_datatype));
        return (DataType)tiledb_datatype;
    }

    /// <summary>
    /// Gets whether this <see cref="Attribute"/> can take null values.
    /// </summary>
    public bool Nullable()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        byte nullable;
        _ctx.handle_error(Methods.tiledb_attribute_get_nullable(ctxHandle, handle, &nullable));
        return nullable > 0;
    }

    /// <summary>
    /// Gets the <see cref="CSharp.FilterList"/> of <see cref="Filter"/>s that will be applied to this <see cref="Attribute"/>.
    /// </summary>
    public FilterList FilterList()
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filter_list_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var attributeHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_attribute_get_filter_list(ctxHandle, attributeHandle, &filter_list_p));
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
    /// Gets the number of values per cell for this attribute.
    /// </summary>
    /// <remarks>
    /// For variable-sized attributes the result is <see cref="VariableSized"/>.
    /// </remarks>
    public uint CellValNum()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        uint cell_val_num;
        _ctx.handle_error(Methods.tiledb_attribute_get_cell_val_num(ctxHandle, handle, &cell_val_num));
        return cell_val_num;
    }

    /// <summary>
    /// Gets the cell size of this <see cref="Attribute"/>.
    /// </summary>
    public ulong CellSize()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong cell_size;
        _ctx.handle_error(Methods.tiledb_attribute_get_cell_size(ctxHandle, handle, &cell_size));
        return cell_size;
    }

    /// <summary>
    /// Gets the name of the <see cref="Attribute"/>'s enumeration.
    /// </summary>
    /// <returns>
    /// A string with the name of the attribute's enumeration, or an
    /// empty string if the attribute does not have an enumeration.
    /// </returns>
    public string EnumerationName()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var nameHolder = new StringHandleHolder();
        _ctx.handle_error(Methods.tiledb_attribute_get_enumeration_name(ctxHandle, handle, &nameHolder._handle));

        return nameHolder.ToString();
    }

    /// <summary>
    /// Sets the fill value of a nullable multi-valued <see cref="Attribute"/>.
    /// Used when the fill value is multi-valued.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    /// <param name="data">An array of values that will be used as the fill value.</param>
    private void SetFillValue<T>(T[] data) where T : struct
    {
        ErrorHandling.CheckDataType<T>(Type());
        if (data.Length == 0)
        {
            throw new ArgumentException("Attribute.SetFillValue, data is empty!");
        }

        var cell_val_num = this.CellValNum();

        if (cell_val_num != VariableSized && cell_val_num != data.Length)
        {
            throw new ArgumentException("Attribute.SetFillValue_nullable, data length is not equal to cell_val_num!");
        }

        ulong size;
        if (cell_val_num == VariableSized)
        {
            size = (ulong)(data.Length* Marshal.SizeOf(data[0]));
        }
        else
        {
            size = cell_val_num * (ulong)(Marshal.SizeOf(data[0]));
        }

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        fixed (T* dataPtr = &data[0])
            _ctx.handle_error(Methods.tiledb_attribute_set_fill_value(ctxHandle, handle, dataPtr, size));
    }

    /// <summary>
    /// Sets the fill value of a nullable <see cref="Attribute"/>.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    /// <param name="value">The attribute's fill value.</param>
    public void SetFillValue<T>(T value) where T : struct
    {
        if (typeof(T) == typeof(bool))
        {
            SetFillValue<byte>(Convert.ToByte(value));
            return;
        }

        var cell_val_num = this.CellValNum();
        var data = cell_val_num == VariableSized
            ? [value]
            : Enumerable.Repeat(value, (int)cell_val_num).ToArray();
        SetFillValue(data);
    }

    /// <summary>
    /// Sets the fill value of a variable-sized <see cref="Attribute"/> to a string.
    /// </summary>
    /// <param name="value">The attribute's fill value.</param>
    public void SetFillValue(string value)
    {
        var str_bytes = Encoding.ASCII.GetBytes(value);
        SetFillValue(str_bytes);
    }

    private ReadOnlySpan<byte> get_fill_value()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* value_p;
        ulong size;
        _ctx.handle_error(Methods.tiledb_attribute_get_fill_value(ctxHandle, handle, &value_p, &size));

        return new ReadOnlySpan<byte>(value_p, (int)size);
    }

    /// <summary>
    /// Gets the fill value of the <see cref="Attribute"/>.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    public T[] FillValue<T>() where T : struct
    {
        if (typeof(T) == typeof(char))
        {
            throw new NotSupportedException("Attribute.FillValue, please use FillValue<byte> for TILEDB_CHAR attributes!");
        }
        var fill_bytes = get_fill_value();
        var span = MemoryMarshal.Cast<byte, T>(fill_bytes);
        return span.ToArray();
    }

    /// <summary>
    /// Gets the fill value of the attribute rendered as a string.
    /// </summary>
    /// <exception cref="NotSupportedException">The attribute is not string-typed.</exception>
    public string FillValue()
    {
        var datatype = Type();
        if (!EnumUtil.IsStringType(datatype))
        {
            throw new NotSupportedException("Attribute.FillValue, please use FillValue<T> for non-string attribute!");
        }
        var fill_bytes = get_fill_value();
        return MarshaledStringOut.GetString(fill_bytes, datatype);
    }

    /// <summary>
    /// Sets the fill value and validity of a nullable <see cref="Attribute"/>.
    /// Used when the fill value is multi-valued.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    /// <param name="data">The attribute's fill value.</param>
    /// <param name="valid">Whether the fill value will be non-null.</param>
    private void SetFillValueNullable<T>(T[] data, bool valid) where T : struct
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Attribute.SetFillValueNullable, data is empty!");
        }

        var cell_val_num = this.CellValNum();
        if (cell_val_num != VariableSized && cell_val_num != data.Length)
        {
            throw new ArgumentException("Attribute.SetFillValueNullable, data length is not equal to cell_val_num!");
        }

        ulong size;
        if (cell_val_num == VariableSized)
        {
            size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
        }
        else
        {
            size = cell_val_num * (ulong)Marshal.SizeOf(data[0]);
        }

        var validity = valid ? (byte)1 : (byte)0;

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        fixed (T* dataPtr = &data[0])
            _ctx.handle_error(Methods.tiledb_attribute_set_fill_value_nullable(ctxHandle, handle, dataPtr, size, validity));
    }

    /// <summary>
    /// Sets the fill value and validity of a nullable <see cref="Attribute"/>.
    /// Used when the attribute is single-valued or all its values are the same.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    /// <param name="value">The attribute's fill value.</param>
    /// <param name="valid">Whether the fill value will be non-null.</param>
    public void SetFillValueNullable<T>(T value, bool valid) where T : struct
    {
        var cell_val_num = this.CellValNum();
        var data = cell_val_num == VariableSized ? [value] : Enumerable.Repeat(value, (int)cell_val_num).ToArray();
        SetFillValueNullable(data, valid);
    }

    /// <summary>
    /// Sets the fill value and validity of a string-typed <see cref="Attribute"/>.
    /// </summary>
    /// <param name="value">The attribute's fill value.</param>
    /// <param name="valid">Whether the fill value will be non-null.</param>
    public void SetFillValueNullable(string value, bool valid)
    {
        var str_bytes = Encoding.ASCII.GetBytes(value);
        SetFillValueNullable(str_bytes, valid);
    }

    /// <summary>
    /// Gets the <see cref="Attribute"/>'s fill value as an array of bytes, and its validity.
    /// </summary>
    private (byte[] bytearray, bool valid) get_fill_value_nullable()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* value_p;
        ulong size;
        var byte_valid = default(byte);
        _ctx.handle_error(Methods.tiledb_attribute_get_fill_value_nullable(ctxHandle, handle, &value_p, &size, &byte_valid));

        var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
        return (fill_span.ToArray(), byte_valid>0);
    }

    /// <summary>
    /// Gets the <see cref="Attribute"/>'s fill value and validity.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    public Tuple<T[], bool> FillValueNullable<T>() where T : struct
    {
        var fill_value = get_fill_value_nullable();
        Span<byte> byteSpan = fill_value.bytearray;
        var span = MemoryMarshal.Cast<byte, T>(byteSpan);
        return new Tuple<T[], bool>(span.ToArray(), fill_value.valid);
    }

    /// <summary>
    /// Gets the <see cref="Attribute"/>'s fill value as a string.
    /// </summary>
    public string FillValueNullable()
    {
        var datatype = Type();
        if (!EnumUtil.IsStringType(datatype))
        {
            throw new NotSupportedException("Attribute.FillValueNullable, please use fill_value<T> for non-string attribute!");
        }
        var (fill_bytes, _) = get_fill_value_nullable();
        return MarshaledStringOut.GetString(fill_bytes, datatype);
    }

    /// <summary>
    /// Creates an <see cref="Attribute"/>.
    /// </summary>
    /// <typeparam name="T">The attribute's data type.</typeparam>
    /// <param name="ctx">The <see cref="Context"/> associated with the attribute.</param>
    /// <param name="name">The attribute's name.</param>
    public static Attribute Create<T>(Context ctx, string name)
    {
        var datatype = EnumUtil.TypeToDataType(typeof(T));
        return new Attribute(ctx, name, datatype);
    }
}
