namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_query_type_t : uint
    {
        TILEDB_READ = 0,
        TILEDB_WRITE = 1,
        TILEDB_DELETE = 2,
        TILEDB_MODIFY_EXCLUSIVE = 3,
    }
}
