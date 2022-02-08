namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_encryption_type_t : uint
    {
        TILEDB_NO_ENCRYPTION = 0,
        TILEDB_AES_256_GCM = 1,
    }
}
