using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB dimension label object.
    /// </summary>
    /// <remarks>This API is experimental and susceptible to breaking changes without advance notice.</remarks>
    /// <seealso cref="ArraySchema.DimensionLabel"/>
    public sealed unsafe class DimensionLabel : IDisposable
    {
        private readonly Context _ctx;

        private readonly DimensionLabelHandle _handle;

        internal DimensionLabel(Context ctx, DimensionLabelHandle handle)
        {
            _ctx = ctx;
            _handle = handle;
        }

        /// <summary>
        /// This value indicates a variable-sized dimension label.
        /// It may be returned from <see cref="ValuesPerCell"/>.
        /// </summary>
        public const uint VariableSized = Constants.VariableSizedImpl;

        /// <summary>
        /// The <see cref="DimensionLabel"/>'s data type.
        /// </summary>
        public DataType DataType
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                tiledb_datatype_t result;
                _ctx.handle_error(Methods.tiledb_dimension_label_get_label_type(ctxHandle, handle, &result));
                return (DataType)result;
            }
        }

        /// <summary>
        /// The order of the <see cref="DimensionLabel"/>'s data.
        /// </summary>
        public DataOrder DataOrder
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                tiledb_data_order_t result;
                _ctx.handle_error(Methods.tiledb_dimension_label_get_label_order(ctxHandle, handle, &result));
                return (DataOrder)result;
            }
        }

        /// <summary>
        /// The index of the <see cref="DimensionLabel"/>'s underlying dimension.
        /// </summary>
        public uint DimensionIndex
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                uint result;
                _ctx.handle_error(Methods.tiledb_dimension_label_get_dimension_index(ctxHandle, handle, &result));
                return result;
            }
        }

        /// <summary>
        /// Gets the number of values per cell for this <see cref="DimensionLabel"/>.
        /// </summary>
        /// <remarks>
        /// For variable-sized dimension labels the result is <see cref="VariableSized"/>.
        /// </remarks>
        public uint ValuesPerCell
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                uint result;
                _ctx.handle_error(Methods.tiledb_dimension_label_get_label_cell_val_num(ctxHandle, handle, &result));
                return result;
            }
        }

        /// <summary>
        /// Gets the name of the attrbiute the <see cref="DimensionLabel"/>'s data are stored under.
        /// </summary>
        public string GetAttributeName()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_dimension_label_get_label_attr_name(ctxHandle, handle, &result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Gets the name of the <see cref="DimensionLabel"/>.
        /// </summary>
        public string GetName()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_dimension_label_get_name(ctxHandle, handle, &result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Gets the URI of the <see cref="DimensionLabel"/>'s array.
        /// </summary>
        public string GetUri()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_dimension_label_get_uri(ctxHandle, handle, &result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
