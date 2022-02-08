namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_filter_type_t : uint
    {
        TILEDB_FILTER_NONE = 0,
        TILEDB_FILTER_GZIP = 1,
        TILEDB_FILTER_ZSTD = 2,
        TILEDB_FILTER_LZ4 = 3,
        TILEDB_FILTER_RLE = 4,
        TILEDB_FILTER_BZIP2 = 5,
        TILEDB_FILTER_DOUBLE_DELTA = 6,
        TILEDB_FILTER_BIT_WIDTH_REDUCTION = 7,
        TILEDB_FILTER_BITSHUFFLE = 8,
        TILEDB_FILTER_BYTESHUFFLE = 9,
        TILEDB_FILTER_POSITIVE_DELTA = 10,
        TILEDB_FILTER_CHECKSUM_MD5 = 12,
        TILEDB_FILTER_CHECKSUM_SHA256 = 13,
    }
}
