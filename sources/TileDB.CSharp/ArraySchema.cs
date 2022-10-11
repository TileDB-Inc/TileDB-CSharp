using System;
using System.Collections.Generic;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class ArraySchema : IDisposable
    {
        private readonly ArraySchemaHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        public ArraySchema(Context ctx, ArrayType arrayType)
        {
            _ctx = ctx;
            var tiledb_array_type = (tiledb_array_type_t)arrayType;
            _handle = ArraySchemaHandle.Create(_ctx, tiledb_array_type);
        }

        private ArraySchema(Context ctx, string uri)
        {
            _ctx = ctx;
            tiledb_array_schema_t* array_schema_p;
            var ms_uri = new MarshaledString(uri);
            using var ctxHandle = ctx.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_load(ctxHandle, ms_uri, &array_schema_p));
            _handle = ArraySchemaHandle.CreateUnowned(array_schema_p);
        }

        internal ArraySchema(Context ctx, ArraySchemaHandle handle)
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

        internal ArraySchemaHandle Handle => _handle;


        #region capi functions

        /// <summary>
        /// Add an attribute.
        /// </summary>
        /// <param name="attr"></param>
        public void AddAttribute(Attribute attr)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var attrHandle = attr.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_add_attribute(ctxHandle, handle, attrHandle));
        }

        public void AddAttributes(params Attribute[] attrs)
        {
            foreach (var t in attrs)
            {
                AddAttribute(t);
            }
        }

        /// <summary>
        /// Set if duplicate is allowed or not
        /// </summary>
        /// <param name="allowsDups"></param>
        public void SetAllowsDups(bool allowsDups)
        {
            var int_allow_dups = allowsDups ? 1 : 0;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_allows_dups(ctxHandle, handle, int_allow_dups));
        }

        /// <summary>
        /// Get if duplicate is allowed or not.
        /// </summary>
        /// <returns></returns>
        public bool AllowsDups()
        {
            var allowDups = 0;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_allows_dups(ctxHandle, handle, &allowDups));
            return allowDups > 0;
        }

        /// <summary>
        /// Set domain.
        /// </summary>
        /// <param name="domain"></param>
        public void SetDomain(Domain domain)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var domainHandle = domain.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_domain(ctxHandle, handle, domainHandle));
        }

        /// <summary>
        /// Set capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public void SetCapacity(ulong capacity)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_capacity(ctxHandle, handle, capacity));
        }

        /// <summary>
        /// Set cell order.
        /// </summary>
        /// <param name="layoutType"></param>
        public void SetCellOrder(LayoutType layoutType)
        {
            var tiledb_layout = (tiledb_layout_t)layoutType;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_cell_order(ctxHandle, handle, tiledb_layout));
        }

        /// <summary>
        /// Set tile order.
        /// </summary>
        /// <param name="layoutType"></param>
        public void SetTileOrder(LayoutType layoutType)
        {
            var tiledb_layout = (tiledb_layout_t)layoutType;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_tile_order(ctxHandle, handle, tiledb_layout));
        }

        /// <summary>
        /// Set coordinates filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void SetCoordsFilterList(FilterList filterList)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var filterListHandle = filterList.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_coords_filter_list(ctxHandle, handle, filterListHandle));
        }

        /// <summary>
        /// Set offsets filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void SetOffsetsFilterList(FilterList filterList)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var filterListHandle = filterList.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_set_offsets_filter_list(ctxHandle, handle, filterListHandle));
        }

        /// <summary>
        /// Check if it is valid or not.
        /// </summary>
        /// <returns></returns>
        public void Check()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_check(ctxHandle, handle));
        }



        /// <summary>
        /// Get ArrayType.
        /// </summary>
        /// <returns></returns>
        public ArrayType ArrayType()
        {
            tiledb_array_type_t tiledb_array_type;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_array_type(ctxHandle, handle, &tiledb_array_type));

            return (ArrayType)tiledb_array_type;
        }

        /// <summary>
        /// Get capacity.
        /// </summary>
        /// <returns></returns>
        public ulong Capacity()
        {
            ulong capacity;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_capacity(ctxHandle, handle, &capacity));

            return capacity;
        }

        /// <summary>
        /// Get cell order.
        /// </summary>
        /// <returns></returns>
        public LayoutType CellOrder()
        {
            tiledb_layout_t tiledb_layout;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_cell_order(ctxHandle, handle, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get coordinates filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList CoordsFilterList()
        {

            tiledb_filter_list_t* filter_list_p;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_coords_filter_list(ctxHandle, handle, &filter_list_p));
            return new FilterList(_ctx, FilterListHandle.CreateUnowned(filter_list_p));
        }

        /// <summary>
        /// Get offsets filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList OffsetsFilterList()
        {
            tiledb_filter_list_t* filter_list_p;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_offsets_filter_list(ctxHandle, handle, &filter_list_p));
            return new FilterList(_ctx, FilterListHandle.CreateUnowned(filter_list_p));
        }

        /// <summary>
        /// Get domain.
        /// </summary>
        /// <returns></returns>
        public Domain Domain()
        {
            tiledb_domain_t* domain_p = null;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_domain(ctxHandle, handle, &domain_p));

            return new Domain(_ctx, DomainHandle.CreateUnowned(domain_p));
        }

        /// <summary>
        /// Get tile order.
        /// </summary>
        /// <returns></returns>
        public LayoutType TileOrder()
        {
            tiledb_layout_t tiledb_layout;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_tile_order(ctxHandle, handle, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get number of attributes.
        /// </summary>
        /// <returns></returns>
        public uint AttributeNum()
        {
            uint num = 0;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_num(ctxHandle, handle, &num));
            return num;
        }

        /// <summary>
        /// Get attribute from index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Attribute Attribute(uint i)
        {
            tiledb_attribute_t* attribute_p;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_index(ctxHandle, handle, i, &attribute_p));

            return new Attribute(_ctx, AttributeHandle.CreateUnowned(attribute_p));
        }

        /// <summary>
        /// Get attribute from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Attribute Attribute(string name)
        {
            tiledb_attribute_t* attribute_p;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_name(ctxHandle, handle, ms_name, &attribute_p));

            return new Attribute(_ctx, AttributeHandle.CreateUnowned(attribute_p));
        }

        /// <summary>
        /// Test if an attribute with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAttribute(string name)
        {
            var has_attr = 0;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_array_schema_has_attribute(ctxHandle, handle, ms_name, &has_attr));
            return has_attr>0;
        }

        /// <summary>
        /// Load an array schema from uri.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>

        public static ArraySchema Load(Context ctx, string uri)
        {
            return new ArraySchema(ctx, uri);
        }


        #endregion capi functions

        /// <summary>
        /// Get attributes dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Attribute> Attributes()
        {
            var ret = new SortedDictionary<string, Attribute>();
            var attribute_num = this.AttributeNum();
            for (uint i = 0; i < attribute_num; ++i)
            {
                var attr = Attribute(i);
                ret.Add(attr.Name(), attr);
            }
            return ret;
        }

        /// <summary>
        /// Get dimension dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Dimension> Dimensions()
        {
            var ret = new SortedDictionary<string, Dimension>();
            var domain = this.Domain();
            var ndim = domain.NDim();
            for (uint i = 0; i < ndim; ++i)
            {
                var dim = domain.Dimension(i);
                ret.Add(dim.Name(), dim);
            }
            return ret;
        }

        /// <summary>
        /// Test if an attribute or dimenision is nullable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNullable(string name)
        {
            if (HasAttribute(name))
            {
                var attr = Attribute(name);
                return attr.Nullable();
            }
            return false;
        }

        /// <summary>
        /// Test if an attribute or dimension is variable length.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsVarSize(string name)
        {
            if (string.Equals(name, "coords"))
            {
                return false;
            }
            if (HasAttribute(name))
            {
                var attr = Attribute(name);
                return attr.CellValNum() == (uint)Constants.TILEDB_VAR_NUM;
            }
            else if (Domain().HasDimension(name))
            {
                var dim = Domain().Dimension(name);
                return dim.CellValNum() == (uint)Constants.TILEDB_VAR_NUM;
            }

            return false;
        }
    }
}
