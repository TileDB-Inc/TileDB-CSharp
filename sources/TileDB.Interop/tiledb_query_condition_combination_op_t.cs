namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_query_condition_combination_op_t : uint
    {
        TILEDB_AND = 0,
        TILEDB_OR = 1,
        TILEDB_NOT = 2,
    }
}
