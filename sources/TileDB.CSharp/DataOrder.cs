using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Specifies the order of data in dimension labels.
    /// </summary>
    /// <remarks>This API is experimental and subject to breaking changes without advance notice.</remarks>
    public enum DataOrder
    {
        /// <summary>
        /// Data are not ordered.
        /// </summary>
        Unordered = tiledb_data_order_t.TILEDB_UNORDERED_DATA,
        /// <summary>
        /// Data are stored in increasing order.
        /// </summary>
        Increasing = tiledb_data_order_t.TILEDB_INCREASING_DATA,
        /// <summary>
        /// Data are stored in decreasing order.
        /// </summary>
        Decreasing = tiledb_data_order_t.TILEDB_DECREASING_DATA
    }
}
