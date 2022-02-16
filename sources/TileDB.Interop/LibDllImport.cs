namespace TileDB.Interop
{
    internal class LibDllImport
    {
#if __OSX__
        public const string Path = "libtiledb.dylib";
        public const string LibCPath = "libc.dylib";
#elif __LINUX__
        public const string Path = "libtiledb.so.2.6";
        public const string LibCPath = "libc.so";
#else
        public const string Path = "tiledb.dll";
        public const string LibCPath = "msvcrt.dll";
#endif
   
    }
}
