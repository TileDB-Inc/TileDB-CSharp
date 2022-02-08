namespace TileDB.Interop
{
    [NativeTypeName("unsigned int")]
    public enum tiledb_vfs_mode_t : uint
    {
        TILEDB_VFS_READ = 0,
        TILEDB_VFS_WRITE = 1,
        TILEDB_VFS_APPEND = 2,
    }
}
