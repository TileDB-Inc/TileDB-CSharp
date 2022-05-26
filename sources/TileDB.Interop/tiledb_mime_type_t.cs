namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_mime_type_t : uint
    {
        TILEDB_MIME_AUTODETECT = 0,
        TILEDB_MIME_TIFF = 1,
        TILEDB_MIME_PDF = 2,
    }
}
