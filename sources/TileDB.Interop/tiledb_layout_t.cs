namespace TileDB.Interop
{
    public enum tiledb_layout_t
    {
        TILEDB_ROW_MAJOR = 0,
        TILEDB_COL_MAJOR = 1,
        TILEDB_GLOBAL_ORDER = 2,
        TILEDB_UNORDERED = 3,
        TILEDB_HILBERT = 4,
    }
}
