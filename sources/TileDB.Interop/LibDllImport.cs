namespace TileDB.Interop
{
    internal class LibDllImport
    {
        public const string Path = "tiledb";
// #if __OSX__
//         public const string LibCPath = "libc.dylib";
// #elif __LINUX__
//         public const string LibCPath = "libc.so";
// #else
//         public const string LibCPath = "msvcrt.dll";
// #endif
    }
}
