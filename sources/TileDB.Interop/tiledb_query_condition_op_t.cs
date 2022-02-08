namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_query_condition_op_t : uint
    {
        TILEDB_LT = 0,
        TILEDB_LE = 1,
        TILEDB_GT = 2,
        TILEDB_GE = 3,
        TILEDB_EQ = 4,
        TILEDB_NE = 5,
    }
}
