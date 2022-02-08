namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_layout_t : uint
    {
        TILEDB_ROW_MAJOR = 0,
        TILEDB_COL_MAJOR = 1,
        TILEDB_GLOBAL_ORDER = 2,
        TILEDB_UNORDERED = 3,
        TILEDB_HILBERT = 4,
    }
}
