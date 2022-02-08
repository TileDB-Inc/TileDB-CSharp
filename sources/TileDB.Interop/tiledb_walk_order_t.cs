namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_walk_order_t : uint
    {
        TILEDB_PREORDER = 0,
        TILEDB_POSTORDER = 1,
    }
}
