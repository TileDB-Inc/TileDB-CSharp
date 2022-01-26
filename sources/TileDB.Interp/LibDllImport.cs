using System;
namespace TileDB.Interop
{
    internal class LibDllImport
    {
#if __OSX__
        public const string Path = "libtiledb.dylib";
#elif __LINUX__
        public const string Path = "libtiledb.so.2.5";
#else
        public const string Path = "tiledb.dll";
#endif
   
    }
}
