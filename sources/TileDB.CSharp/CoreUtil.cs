using System;
using System.Text;
using System.Collections.Generic;
using TileDB.Interop;
using TileDB.CSharp.Marshalling;

namespace TileDB.CSharp;

/// <summary>
/// Contains general utility functions.
/// </summary>
public static class CoreUtil
{
    private static Version? _coreLibVersion;

    private static string? _buildConfiguration;

    /// <summary>
    /// Returns the version of the TileDB Embedded binary being used.
    /// </summary>
    public static Version GetCoreLibVersion()
    {
        return _coreLibVersion ??= GetVersion();

        static unsafe Version GetVersion()
        {
            int major, minor, rev;
            Methods.tiledb_version(&major, &minor, &rev);
            return new Version(major, minor, rev);
        }
    }

    /// <summary>
    /// Returns a string describing the build configuration of the TileDB Embedded binary being used.
    /// </summary>
    /// <remarks>
    /// This method exposes the <c>tiledb_as_built_dump</c> function of the TileDB Embedded C API.
    /// </remarks>
    public static string GetBuildConfiguration()
    {
        return _buildConfiguration ??= Create();

        static unsafe string Create()
        {
            using var resultHolder = new StringHandleHolder();
            ErrorHandling.ThrowOnError(Methods.tiledb_as_built_dump(&resultHolder._handle));
            return resultHolder.ToString();
        }
    }

    #region  Buffer

    /// <summary>
    /// Convert element offsets to byte offsets.
    /// </summary>
    /// <param name="element_offsets"></param>
    /// <returns></returns>
    public static ulong[] ElementOffsetsToByteOffsets(ulong[] element_offsets, DataType dataType)
    {
        ulong[] ret = new ulong[element_offsets.Length];
        for (var i = 0; i < element_offsets.Length; ++i)
        {
            ret[i] = element_offsets[i] * EnumUtil.DataTypeSize(dataType);
        }
        return ret;
    }

    /// <summary>
    /// Convert byte offsets to element offsets
    /// </summary>
    /// <param name="offsets"></param>
    /// <returns></returns>
    public static ulong[] ByteOffsetsToElementOffsets(ulong[] offsets, DataType dataType)
    {
        ulong[] ret = new ulong[offsets.Length];
        for (var i = 0; i < offsets.Length; ++i)
        {
            ret[i] = offsets[i] / EnumUtil.DataTypeSize(dataType);
        }
        return ret;
    }


    /// <summary>
    /// Pack string array.
    /// </summary>
    /// <param name="strarray"></param>
    /// <param name="encodingmethod"></param>
    /// <returns></returns>
    public static (byte[] data, ulong[] offsets) PackStringArray(string[] strarray, string encodingmethod = "ASCII")
    {
        if (strarray.Length == 0)
        {
            return (new byte[0], new ulong[0]);
        }

        int totsize = 0;
        for (int i = 0; i < strarray.Length; ++i)
        {
            int bytes_size = 0;
            if (encodingmethod == "ASCII")
            {
                bytes_size = Encoding.ASCII.GetByteCount(strarray[i]);
            }
            else if (encodingmethod == "UTF16")
            {
                bytes_size = Encoding.Unicode.GetByteCount(strarray[i]);
            }
            else if (encodingmethod == "UTF32")
            {
                bytes_size = Encoding.UTF32.GetByteCount(strarray[i]);
            }
            else
            {
                bytes_size = Encoding.UTF8.GetByteCount(strarray[i]);
            }
            totsize += bytes_size;
        }

        byte[] data = new byte[totsize];
        ulong[] offsets = new ulong[strarray.Length];
        offsets[0] = 0;

        int idx = 0;
        for (int i = 0; i < strarray.Length; ++i)
        {
            byte[] bytes;
            if (encodingmethod == "ASCII")
            {
                bytes = Encoding.ASCII.GetBytes(strarray[i]);

            }
            else if (encodingmethod == "UTF16")
            {
                bytes = Encoding.Unicode.GetBytes(strarray[i]);
            }
            else if (encodingmethod == "UTF32")
            {
                bytes = Encoding.UTF32.GetBytes(strarray[i]);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(strarray[i]);
            }
            bytes.CopyTo(data, idx);
            idx += bytes.Length;


            if (i < (strarray.Length - 1))
            {
                offsets[i + 1] = offsets[i] + (ulong)bytes.Length;
            }

        }
        return (data, offsets);
    }


    /// <summary>
    /// Unpack string array.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offsets"></param>
    /// <param name="validities"></param>
    /// <param name="encodingmethod"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static List<string> UnPackStringArray(byte[] data, ulong[] offsets, byte[]? validities = null, string encodingmethod = "ASCII")
    {
        List<string> result = new List<string>();
        int tot_bytes_size = data.Length;
        int length = offsets.Length;

        bool is_nullable = (validities != null);
        if (is_nullable && (validities?.Length != offsets.Length))
        {
            throw new System.ArgumentException("CoreUtil.UnpackStringArray, the lengths of offsets and validities are not equal!");
        }

        int totsize = 0;
        int indexFrom = 0;
        for (int i = 0; i < length; ++i)
        {
            if (is_nullable && validities?[i] == 0)
            {
                continue;
            }
            int bytesize = 0;
            if (i < (length - 1))
            {
                bytesize = (int)(offsets[i + 1] - offsets[i]);

            }
            else
            {
                bytesize = tot_bytes_size - (int)offsets[length - 1];
            }

            totsize += bytesize;
            if (totsize <= tot_bytes_size && bytesize > 0)
            {
                byte[] bytes = new byte[bytesize];
                System.Array.Copy(data, indexFrom, bytes, 0, bytesize);
                if (encodingmethod == "ASCII")
                {
                    result.Add(Encoding.ASCII.GetString(bytes));
                }
                else if (encodingmethod == "UTF16")
                {
                    result.Add(Encoding.Unicode.GetString(bytes));
                }
                else if (encodingmethod == "UTF32")
                {
                    result.Add(Encoding.UTF32.GetString(bytes));
                }
                else
                {
                    result.Add(Encoding.UTF8.GetString(bytes));
                }

                indexFrom = totsize;
            }

        } //for

        return result;
    }
    #endregion Buffer
}
