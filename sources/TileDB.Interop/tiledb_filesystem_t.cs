namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_filesystem_t : uint
    {
        TILEDB_HDFS = 0,
        TILEDB_S3 = 1,
        TILEDB_AZURE = 2,
        TILEDB_GCS = 3,
        TILEDB_MEMFS = 4,
    }
}
