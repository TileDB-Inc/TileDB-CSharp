namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_array_type_t : uint
    {
        TILEDB_DENSE = 0,
        TILEDB_SPARSE = 1,
    }
}
