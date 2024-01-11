using System;
using System.Collections.Generic;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB array schema object.
/// </summary>
public sealed unsafe class ArraySchema : IDisposable
{
    private readonly ArraySchemaHandle _handle;
    private readonly Context _ctx;

    /// <summary>
    /// Creates a new <see cref="ArraySchema"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with this schema.</param>
    /// <param name="arrayType">The array's type.</param>
    public ArraySchema(Context ctx, ArrayType arrayType)
    {
        _ctx = ctx;
        var tiledb_array_type = (tiledb_array_type_t)arrayType;
        _handle = ArraySchemaHandle.Create(_ctx, tiledb_array_type);
    }

    internal ArraySchema(Context ctx, ArraySchemaHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
    }

    /// <summary>
    /// Disposes this <see cref="ArraySchema"/>
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal ArraySchemaHandle Handle => _handle;

    /// <summary>
    /// Adds an <see cref="CSharp.Attribute"/> in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <param name="attr">The attribute to add.</param>
    public void AddAttribute(Attribute attr)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var attrHandle = attr.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_add_attribute(ctxHandle, handle, attrHandle));
    }

    /// <summary>
    /// Adds an array of <see cref="CSharp.Attribute"/>s in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <param name="attrs">The attributes to add.</param>
    public void AddAttributes(params Attribute[] attrs)
    {
        foreach (var t in attrs)
        {
            AddAttribute(t);
        }
    }

    /// <summary>
    /// Adds an <see cref="Enumeration"/> in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <param name="enumeration">The enumeration to add.</param>
    public void AddEnumeration(Enumeration enumeration)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var enumHandle = enumeration.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_add_enumeration(ctxHandle, handle, enumHandle));
    }

    /// <summary>
    /// Sets whether cells with duplicate coordinates are allowed in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <remarks>
    /// Applicable to only <see cref="ArrayType.Sparse"/> arrays.
    /// </remarks>
    public void SetAllowsDups(bool allowsDups)
    {
        int int_allow_dups = allowsDups ? 1 : 0;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_allows_dups(ctxHandle, handle, int_allow_dups));
    }

    /// <summary>
    /// Gets whether cells with duplicate coordinates are allowed in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <remarks>
    /// Applicable to only <see cref="ArrayType.Sparse"/> arrays.
    /// </remarks>
    public bool AllowsDups()
    {
        int allowDups;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_allows_dups(ctxHandle, handle, &allowDups));
        return allowDups > 0;
    }

    /// <summary>
    /// Sets the <see cref="ArraySchema"/>'s <see cref="Domain"/>.
    /// </summary>
    /// <param name="domain">The domain to set</param>
    public void SetDomain(Domain domain)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var domainHandle = domain.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_domain(ctxHandle, handle, domainHandle));
    }

    /// <summary>
    /// Sets the <see cref="ArraySchema"/>'s sparse fragment capacity.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    public void SetCapacity(ulong capacity)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_capacity(ctxHandle, handle, capacity));
    }

    /// <summary>
    /// Sets the <see cref="ArraySchema"/>'s cell order.
    /// </summary>
    /// <param name="layoutType">The cell order.</param>
    public void SetCellOrder(LayoutType layoutType)
    {
        var tiledb_layout = (tiledb_layout_t)layoutType;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_cell_order(ctxHandle, handle, tiledb_layout));
    }

    /// <summary>
    /// Sets the <see cref="ArraySchema"/>'s tile order.
    /// </summary>
    /// <param name="layoutType">The tile order.</param>
    public void SetTileOrder(LayoutType layoutType)
    {
        var tiledb_layout = (tiledb_layout_t)layoutType;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_tile_order(ctxHandle, handle, tiledb_layout));
    }

    /// <summary>
    /// Sets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s coordinates.
    /// </summary>
    /// <param name="filterList">The filter list.</param>
    public void SetCoordsFilterList(FilterList filterList)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterListHandle = filterList.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_coords_filter_list(ctxHandle, handle, filterListHandle));
    }

    /// <summary>
    /// Sets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s
    /// offsets of variable-sized <see cref="CSharp.Attribute"/>s or <see cref="Dimension"/>s.
    /// </summary>
    /// <param name="filterList">The filter list.</param>
    public void SetOffsetsFilterList(FilterList filterList)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterListHandle = filterList.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_offsets_filter_list(ctxHandle, handle, filterListHandle));
    }

    /// <summary>
    /// Sets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s
    /// the validity array of nullable <see cref="CSharp.Attribute"/> values.
    /// </summary>
    /// <param name="filterList">The filter list.</param>
    public void SetValidityFilterList(FilterList filterList)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterListHandle = filterList.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_set_validity_filter_list(ctxHandle, handle, filterListHandle));
    }

    /// <summary>
    /// Performs validity checks in the <see cref="ArraySchema"/>.
    /// </summary>
    public void Check()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_check(ctxHandle, handle));
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/>'s <see cref="CSharp.ArrayType"/>.
    /// </summary>
    public ArrayType ArrayType()
    {
        tiledb_array_type_t tiledb_array_type;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_array_type(ctxHandle, handle, &tiledb_array_type));

        return (ArrayType)tiledb_array_type;
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/>'s sparse fragment capacity.
    /// </summary>
    public ulong Capacity()
    {
        ulong capacity;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_capacity(ctxHandle, handle, &capacity));

        return capacity;
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/>'s cell order.
    /// </summary>
    public LayoutType CellOrder()
    {
        tiledb_layout_t tiledb_layout;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_cell_order(ctxHandle, handle, &tiledb_layout));
        return (LayoutType)tiledb_layout;
    }

    /// <summary>
    /// Gets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s coordinates.
    /// </summary>
    public FilterList CoordsFilterList()
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filter_list_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_coords_filter_list(ctxHandle, schemaHandle, &filter_list_p));
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
    /// Gets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s
    /// offsets of variable-sized <see cref="CSharp.Attribute"/>s or <see cref="Dimension"/>s.
    /// </summary>
    public FilterList OffsetsFilterList()
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filter_list_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_offsets_filter_list(ctxHandle, schemaHandle, &filter_list_p));
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
    /// Gets the <see cref="FilterList"/> of filters that will be applied in the <see cref="ArraySchema"/>'s
    /// the validity array of nullable <see cref="CSharp.Attribute"/> values.
    /// </summary>
    public FilterList ValidityFilterList()
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filter_list_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_validity_filter_list(ctxHandle, schemaHandle, &filter_list_p));
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
    /// Gets the <see cref="ArraySchema"/>'s <see cref="Domain"/>.
    /// </summary>
    public Domain Domain()
    {
        var handle = new DomainHandle();
        var successful = false;
        tiledb_domain_t* domain_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_domain(ctxHandle, schemaHandle, &domain_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(domain_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Domain(_ctx, handle);
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/>'s tile order.
    /// </summary>
    public LayoutType TileOrder()
    {
        tiledb_layout_t tiledb_layout;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_tile_order(ctxHandle, handle, &tiledb_layout));
        return (LayoutType)tiledb_layout;
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/>'s format version.
    /// </summary>
    public uint FormatVersion()
    {
        uint num;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_version(ctxHandle, handle, &num));
        return num;
    }

    /// <summary>
    /// Gets the number of <see cref="CSharp.Attribute"/>s in the <see cref="ArraySchema"/>.
    /// </summary>
    /// <returns></returns>
    public uint AttributeNum()
    {
        uint num;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_num(ctxHandle, handle, &num));
        return num;
    }

    /// <summary>
    /// Gets an <see cref="CSharp.Attribute"/> from the <see cref="ArraySchema"/> by index.
    /// </summary>
    /// <param name="i">The attribute's index.</param>
    public Attribute Attribute(uint i)
    {
        var handle = new AttributeHandle();
        var successful = false;
        tiledb_attribute_t* attribute_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_index(ctxHandle, schemaHandle, i, &attribute_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(attribute_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Attribute(_ctx, handle);
    }

    /// <summary>
    /// Gets an <see cref="CSharp.Attribute"/> from the <see cref="ArraySchema"/> by name.
    /// </summary>
    /// <param name="name">The attribute's name.</param>
    public Attribute Attribute(string name)
    {
        var handle = new AttributeHandle();
        var successful = false;
        tiledb_attribute_t* attribute_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var schemaHandle = _handle.Acquire())
            using (var ms_name = new MarshaledString(name))
            {
                _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_name(ctxHandle, schemaHandle, ms_name, &attribute_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(attribute_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Attribute(_ctx, handle);
    }

    /// <summary>
    /// Checks if an <see cref="CSharp.Attribute"/> with the given name exists in the <see cref="ArraySchema"/> or not.
    /// </summary>
    /// <param name="name">The name to check.</param>
    public bool HasAttribute(string name)
    {
        int has_attr;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_name = new MarshaledString(name);
        _ctx.handle_error(Methods.tiledb_array_schema_has_attribute(ctxHandle, handle, ms_name, &has_attr));
        return has_attr > 0;
    }

    /// <summary>
    /// Load an <see cref="ArraySchema"/> from a URI.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with this schema.</param>
    /// <param name="uri">The array's URI.</param>
    public static ArraySchema Load(Context ctx, string uri)
    {
        var handle = new ArraySchemaHandle();
        var successful = false;
        tiledb_array_schema_t* array_schema_p = null;
        try
        {
            using (var ms_uri = new MarshaledString(uri))
            using (var ctxHandle = ctx.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_array_schema_load(ctxHandle, ms_uri, &array_schema_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(array_schema_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }
        return new ArraySchema(ctx, handle);
    }

    /// <summary>
    /// Gets a collection with all <see cref="CSharp.Attribute"/>s of the <see cref="ArraySchema"/>.
    /// </summary>
    public SortedDictionary<string, Attribute> Attributes()
    {
        var ret = new SortedDictionary<string, Attribute>();
        var attribute_num = AttributeNum();
        for (uint i = 0; i < attribute_num; ++i)
        {
            var attr = Attribute(i);
            ret.Add(attr.Name(), attr);
        }
        return ret;
    }

    /// <summary>
    /// Gets a collection with all <see cref="Dimension"/>s of the <see cref="ArraySchema"/>.
    /// </summary>
    public SortedDictionary<string, Dimension> Dimensions()
    {
        var ret = new SortedDictionary<string, Dimension>();
        using var domain = Domain();
        var ndim = domain.NDim();
        for (uint i = 0; i < ndim; ++i)
        {
            var dim = domain.Dimension(i);
            ret.Add(dim.Name(), dim);
        }
        return ret;
    }

    /// <summary>
    /// Checks if an <see cref="CSharp.Attribute"/> or <see cref="Dimension"/> in the <see cref="ArraySchema"/> is nullable.
    /// </summary>
    /// <param name="name">The attribute or dimension's name.</param>
    public bool IsNullable(string name)
    {
        if (HasAttribute(name))
        {
            using var attr = Attribute(name);
            return attr.Nullable();
        }
        return false;
    }

    /// <summary>
    /// Checks if an <see cref="CSharp.Attribute"/> or <see cref="Dimension"/> in the <see cref="ArraySchema"/> has variable size.
    /// </summary>
    /// <param name="name">The attribute or dimension's name.</param>
    public bool IsVarSize(string name)
    {
        if (string.Equals(name, "coords"))
        {
            return false;
        }

        if (HasAttribute(name))
        {
            using Attribute attr = Attribute(name);
            return attr.CellValNum() == TileDB.CSharp.Attribute.VariableSized;
        }

        using Domain domain = Domain();
        if (domain.HasDimension(name))
        {
            using Dimension dim = domain.Dimension(name);
            return dim.CellValNum() == Dimension.VariableSized;
        }

        return false;
    }
}
