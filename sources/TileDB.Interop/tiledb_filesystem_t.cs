namespace TileDB.Interop
{
    public enum tiledb_filesystem_t
    {
        TILEDB_HDFS = 0,
        TILEDB_S3 = 1,
        TILEDB_AZURE = 2,
        TILEDB_GCS = 3,
        TILEDB_MEMFS = 4,
    }
}
