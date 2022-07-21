using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    internal class LibDllImport
    {
        public const string Path = "tiledb";

        static LibDllImport()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), Resolver);
        }

        public static void Initialize() { }

        private static IntPtr Resolver(string libName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libName += ".dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libName += ".dylib";
            }
            else
            {
                libName += ".so";
            }

            if (!NativeLibrary.TryLoad(libName, assembly, searchPath, out libHandle))
            {
                throw new DllNotFoundException($"Error: Unable to load dll: {libName}");
            }

            return libHandle;
        }
// #if __OSX__
//         public const string LibCPath = "libc.dylib";
// #elif __LINUX__
//         public const string LibCPath = "libc.so";
// #else
//         public const string LibCPath = "msvcrt.dll";
// #endif
    }
}
