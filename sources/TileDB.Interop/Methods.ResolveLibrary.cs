using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public static partial class Methods
    {
        static Methods()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), Resolver);
        }

        private static IntPtr Resolver(string libName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;

            if (!NativeLibrary.TryLoad(libName, assembly, searchPath, out libHandle))
            {
                if (libName.Equals("libc"))
                {
                    TryResolveLibC(assembly, searchPath, out libHandle);
                }
                else if (libName.Equals("libtiledb"))
                {
                    TryResolveTileDB(assembly, searchPath, out libHandle);
                }
            }

            if (libHandle == IntPtr.Zero)
            {
                throw new DllNotFoundException($"Error: Unable to load dll: {libName}");
            }
            return libHandle;
        }

       private static bool TryResolveTileDB(Assembly assembly, DllImportSearchPath? searchPath, out IntPtr libHandle)
       {
           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
           {
               return NativeLibrary.TryLoad("tiledb.dll", assembly, searchPath, out libHandle);
           }

           if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
           {
               return NativeLibrary.TryLoad("libtiledb.dylib", assembly, searchPath, out libHandle);
           }

           return NativeLibrary.TryLoad("libtiledb.so", assembly, searchPath, out libHandle);
       }

        private static bool TryResolveLibC(Assembly assembly, DllImportSearchPath? searchPath, out IntPtr libHandle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return NativeLibrary.TryLoad("msvcrt.dll", assembly, searchPath, out libHandle);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return NativeLibrary.TryLoad("libSystem.dylib", assembly, searchPath, out libHandle);
            }

            return NativeLibrary.TryLoad("libc.so.6", assembly, searchPath, out libHandle);
        }
    }
}
