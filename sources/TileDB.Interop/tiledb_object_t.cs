namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_object_t : uint
    {
        TILEDB_INVALID = 0,
        TILEDB_GROUP = 1,
        TILEDB_ARRAY = 2,
    }
}
