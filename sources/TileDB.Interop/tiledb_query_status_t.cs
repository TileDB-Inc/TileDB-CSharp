namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_query_status_t : uint
    {
        TILEDB_FAILED = 0,
        TILEDB_COMPLETED = 1,
        TILEDB_INPROGRESS = 2,
        TILEDB_INCOMPLETE = 3,
        TILEDB_UNINITIALIZED = 4,
    }
}
