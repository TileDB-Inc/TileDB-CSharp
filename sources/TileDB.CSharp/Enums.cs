using System;
using TileDB.Interop;

namespace TileDB.CSharp
{
    // TODO Not sure why this is not automatically wrapped
    public enum Status
    {
        TILEDB_OOM = -2,
        TILEDB_ERR = -1,
        TILEDB_OK = 0
    }

    public enum ObjectType : uint
    {
        TILEDB_INVALID = tiledb_object_t.TILEDB_INVALID,
        TILEDB_GROUP = tiledb_object_t.TILEDB_GROUP,
        TILEDB_ARRAY = tiledb_object_t.TILEDB_ARRAY
    }


    public enum QueryType : uint
    {
        TILEDB_READ = tiledb_query_type_t.TILEDB_READ,
        TILEDB_WRITE = tiledb_query_type_t.TILEDB_WRITE,
    }

    public enum QueryStatus : uint
    {
        TILEDB_FAILED = tiledb_query_status_t.TILEDB_FAILED,
        TILEDB_COMPLETED = tiledb_query_status_t.TILEDB_COMPLETED,
        TILEDB_INPROGRESS = tiledb_query_status_t.TILEDB_INPROGRESS,
        TILEDB_INCOMPLETE = tiledb_query_status_t.TILEDB_INCOMPLETE,
        TILEDB_UNINITIALIZED = tiledb_query_status_t.TILEDB_UNINITIALIZED,
    }

    public enum QueryConditionOperatorType : uint
    {
        TILEDB_LT = tiledb_query_condition_op_t.TILEDB_LT,
        TILEDB_LE = tiledb_query_condition_op_t.TILEDB_LE,
        TILEDB_GT = tiledb_query_condition_op_t.TILEDB_GT,
        TILEDB_GE = tiledb_query_condition_op_t.TILEDB_GE,
        TILEDB_EQ = tiledb_query_condition_op_t.TILEDB_EQ,
        TILEDB_NE = tiledb_query_condition_op_t.TILEDB_NE,
    }

    public enum QueryConditionCombinationOperatorType : uint
    {
        TILEDB_AND = tiledb_query_condition_combination_op_t.TILEDB_AND,
        TILEDB_OR = tiledb_query_condition_combination_op_t.TILEDB_OR,
        TILEDB_NOT = tiledb_query_condition_combination_op_t.TILEDB_NOT,
    }

    public enum FileSystemType : uint
    {
        TILEDB_HDFS = tiledb_filesystem_t.TILEDB_HDFS,
        TILEDB_S3 = tiledb_filesystem_t.TILEDB_S3,
        TILEDB_AZURE = tiledb_filesystem_t.TILEDB_AZURE,
        TILEDB_GCS = tiledb_filesystem_t.TILEDB_GCS,
        TILEDB_MEMFS = tiledb_filesystem_t.TILEDB_MEMFS,
    }

    public enum DataType : uint
    {
        TILEDB_INT32 = tiledb_datatype_t.TILEDB_INT32,
        TILEDB_INT64 = tiledb_datatype_t.TILEDB_INT64,
        TILEDB_FLOAT32 = tiledb_datatype_t.TILEDB_FLOAT32,
        TILEDB_FLOAT64 = tiledb_datatype_t.TILEDB_FLOAT64,
        TILEDB_CHAR = tiledb_datatype_t.TILEDB_CHAR,
        TILEDB_INT8 = tiledb_datatype_t.TILEDB_INT8,
        TILEDB_UINT8 = tiledb_datatype_t.TILEDB_UINT8,
        TILEDB_INT16 = tiledb_datatype_t.TILEDB_INT16,
        TILEDB_UINT16 = tiledb_datatype_t.TILEDB_UINT16,
        TILEDB_UINT32 = tiledb_datatype_t.TILEDB_UINT32,
        TILEDB_UINT64 = tiledb_datatype_t.TILEDB_UINT64,
        TILEDB_STRING_ASCII = tiledb_datatype_t.TILEDB_STRING_ASCII,
        TILEDB_STRING_UTF8 = tiledb_datatype_t.TILEDB_STRING_UTF8,
        TILEDB_STRING_UTF16 = tiledb_datatype_t.TILEDB_STRING_UTF16,
        TILEDB_STRING_UTF32 = tiledb_datatype_t.TILEDB_STRING_UTF32,
        TILEDB_STRING_UCS2 = tiledb_datatype_t.TILEDB_STRING_UCS2,
        TILEDB_STRING_UCS4 = tiledb_datatype_t.TILEDB_STRING_UCS4,
        TILEDB_ANY = tiledb_datatype_t.TILEDB_ANY,
        TILEDB_DATETIME_YEAR = tiledb_datatype_t.TILEDB_DATETIME_YEAR,
        TILEDB_DATETIME_MONTH = tiledb_datatype_t.TILEDB_DATETIME_MONTH,
        TILEDB_DATETIME_WEEK = tiledb_datatype_t.TILEDB_DATETIME_WEEK,
        TILEDB_DATETIME_DAY = tiledb_datatype_t.TILEDB_DATETIME_DAY,
        TILEDB_DATETIME_HR = tiledb_datatype_t.TILEDB_DATETIME_HR,
        TILEDB_DATETIME_MIN = tiledb_datatype_t.TILEDB_DATETIME_MIN,
        TILEDB_DATETIME_SEC = tiledb_datatype_t.TILEDB_DATETIME_SEC,
        TILEDB_DATETIME_MS = tiledb_datatype_t.TILEDB_DATETIME_MS,
        TILEDB_DATETIME_US = tiledb_datatype_t.TILEDB_DATETIME_US,
        TILEDB_DATETIME_NS = tiledb_datatype_t.TILEDB_DATETIME_NS,
        TILEDB_DATETIME_PS = tiledb_datatype_t.TILEDB_DATETIME_PS,
        TILEDB_DATETIME_FS = tiledb_datatype_t.TILEDB_DATETIME_FS,
        TILEDB_DATETIME_AS = tiledb_datatype_t.TILEDB_DATETIME_AS,
        TILEDB_TIME_HR = tiledb_datatype_t.TILEDB_TIME_HR,
        TILEDB_TIME_MIN = tiledb_datatype_t.TILEDB_TIME_MIN,
        TILEDB_TIME_SEC = tiledb_datatype_t.TILEDB_TIME_SEC,
        TILEDB_TIME_MS = tiledb_datatype_t.TILEDB_TIME_MS,
        TILEDB_TIME_US = tiledb_datatype_t.TILEDB_TIME_US,
        TILEDB_TIME_NS = tiledb_datatype_t.TILEDB_TIME_NS,
        TILEDB_TIME_PS = tiledb_datatype_t.TILEDB_TIME_PS,
        TILEDB_TIME_FS = tiledb_datatype_t.TILEDB_TIME_FS,
        TILEDB_TIME_AS = tiledb_datatype_t.TILEDB_TIME_AS,
    }

    public enum ArrayType : uint
    {
        TILEDB_DENSE = tiledb_array_type_t.TILEDB_DENSE,
        TILEDB_SPARSE = tiledb_array_type_t.TILEDB_SPARSE,
    }

    public enum LayoutType : uint
    {
        TILEDB_ROW_MAJOR = tiledb_layout_t.TILEDB_ROW_MAJOR,
        TILEDB_COL_MAJOR = tiledb_layout_t.TILEDB_COL_MAJOR,
        TILEDB_GLOBAL_ORDER = tiledb_layout_t.TILEDB_GLOBAL_ORDER,
        TILEDB_UNORDERED = tiledb_layout_t.TILEDB_UNORDERED,
        TILEDB_HILBERT = tiledb_layout_t.TILEDB_HILBERT,
    }

    public enum FilterType : uint
    {
        TILEDB_FILTER_NONE = tiledb_filter_type_t.TILEDB_FILTER_NONE,
        TILEDB_FILTER_GZIP = tiledb_filter_type_t.TILEDB_FILTER_GZIP,
        TILEDB_FILTER_ZSTD = tiledb_filter_type_t.TILEDB_FILTER_ZSTD,
        TILEDB_FILTER_LZ4 = tiledb_filter_type_t.TILEDB_FILTER_LZ4,
        TILEDB_FILTER_RLE = tiledb_filter_type_t.TILEDB_FILTER_RLE,
        TILEDB_FILTER_BZIP2 = tiledb_filter_type_t.TILEDB_FILTER_BZIP2,
        TILEDB_FILTER_DOUBLE_DELTA = tiledb_filter_type_t.TILEDB_FILTER_DOUBLE_DELTA,
        TILEDB_FILTER_BIT_WIDTH_REDUCTION = tiledb_filter_type_t.TILEDB_FILTER_BIT_WIDTH_REDUCTION,
        TILEDB_FILTER_BITSHUFFLE = tiledb_filter_type_t.TILEDB_FILTER_BITSHUFFLE,
        TILEDB_FILTER_BYTESHUFFLE = tiledb_filter_type_t.TILEDB_FILTER_BYTESHUFFLE,
        TILEDB_FILTER_POSITIVE_DELTA = tiledb_filter_type_t.TILEDB_FILTER_POSITIVE_DELTA,
        TILEDB_FILTER_CHECKSUM_MD5 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_MD5,
        TILEDB_FILTER_CHECKSUM_SHA256 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_SHA256,
    }

    public enum FilterOption : uint
    {
        TILEDB_COMPRESSION_LEVEL = tiledb_filter_option_t.TILEDB_COMPRESSION_LEVEL,
        TILEDB_BIT_WIDTH_MAX_WINDOW = tiledb_filter_option_t.TILEDB_BIT_WIDTH_MAX_WINDOW,
        TILEDB_POSITIVE_DELTA_MAX_WINDOW = tiledb_filter_option_t.TILEDB_POSITIVE_DELTA_MAX_WINDOW,
    }

    public enum EncryptionType : uint
    {
        TILEDB_NO_ENCRYPTION = tiledb_encryption_type_t.TILEDB_NO_ENCRYPTION,
        TILEDB_AES_256_GCM = tiledb_encryption_type_t.TILEDB_AES_256_GCM,
    }

    public enum WalkOrderType : uint
    {
        TILEDB_PREORDER = tiledb_walk_order_t.TILEDB_PREORDER,
        TILEDB_POSTORDER = tiledb_walk_order_t.TILEDB_POSTORDER,
    }

    public enum VfsMode : uint
    {
        TILEDB_VFS_READ = tiledb_vfs_mode_t.TILEDB_VFS_READ,
        TILEDB_VFS_WRITE = tiledb_vfs_mode_t.TILEDB_VFS_WRITE,
        TILEDB_VFS_APPEND = tiledb_vfs_mode_t.TILEDB_VFS_APPEND,
    }

    public enum Constants : uint {
        TILEDB_VAR_NUM = uint.MaxValue,
    }

    public static class EnumUtil 
    {
        public static tiledb_datatype_t to_tiledb_datatype(Type t)
        {
            tiledb_datatype_t tiledb_datatype;

            if (t == typeof(int))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_INT32;
            }
            else if (t == typeof(long))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_INT64;
            }
            else if (t == typeof(float))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_FLOAT32;
            }
            else if (t == typeof(double))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_FLOAT64;
            }
            else if (t == typeof(byte))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_CHAR;
            }
            else if (t == typeof(short))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_INT16;
            }
            else if (t == typeof(ushort))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_UINT16;
            }
            else if (t == typeof(uint))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_UINT32;
            }
            else if (t == typeof(ulong))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_UINT64;
            }
            else if (t == typeof(string))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_STRING_ASCII;
            }
            else
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_ANY;
            }

            return tiledb_datatype;
        }

        public static DataType to_DataType(Type t) 
        {
            var tiledb_datatype = to_tiledb_datatype(t);
            return (DataType)tiledb_datatype;
        }

        private static Type ToType(tiledb_datatype_t tiledbDatatype)
        {
            switch (tiledbDatatype)
            {
                case tiledb_datatype_t.TILEDB_ANY:
                    return typeof(sbyte);
                case tiledb_datatype_t.TILEDB_CHAR:
                    return typeof(sbyte);
                case tiledb_datatype_t.TILEDB_DATETIME_AS:
                case tiledb_datatype_t.TILEDB_DATETIME_DAY:
                case tiledb_datatype_t.TILEDB_DATETIME_FS:
                case tiledb_datatype_t.TILEDB_DATETIME_HR:
                case tiledb_datatype_t.TILEDB_DATETIME_MIN:
                case tiledb_datatype_t.TILEDB_DATETIME_MONTH:
                case tiledb_datatype_t.TILEDB_DATETIME_MS:
                case tiledb_datatype_t.TILEDB_DATETIME_NS:
                case tiledb_datatype_t.TILEDB_DATETIME_PS:
                case tiledb_datatype_t.TILEDB_DATETIME_SEC:
                case tiledb_datatype_t.TILEDB_DATETIME_US:
                case tiledb_datatype_t.TILEDB_DATETIME_WEEK:
                case tiledb_datatype_t.TILEDB_DATETIME_YEAR:
                    return typeof(long);
                case tiledb_datatype_t.TILEDB_FLOAT32:
                    return typeof(float);
                case tiledb_datatype_t.TILEDB_FLOAT64:
                    return typeof(double);
                case tiledb_datatype_t.TILEDB_INT16:
                    return typeof(short);
                case tiledb_datatype_t.TILEDB_INT32:
                    return typeof(int);
                case tiledb_datatype_t.TILEDB_INT64:
                    return typeof(long);
                case tiledb_datatype_t.TILEDB_INT8:
                    return typeof(sbyte);
                case tiledb_datatype_t.TILEDB_STRING_ASCII:
                case tiledb_datatype_t.TILEDB_STRING_UCS2:
                case tiledb_datatype_t.TILEDB_STRING_UCS4:
                case tiledb_datatype_t.TILEDB_STRING_UTF16:
                case tiledb_datatype_t.TILEDB_STRING_UTF32:
                case tiledb_datatype_t.TILEDB_STRING_UTF8:
                    return typeof(sbyte);
                case tiledb_datatype_t.TILEDB_TIME_AS:
                case tiledb_datatype_t.TILEDB_TIME_FS:
                case tiledb_datatype_t.TILEDB_TIME_HR:
                case tiledb_datatype_t.TILEDB_TIME_MIN:
                case tiledb_datatype_t.TILEDB_TIME_MS:
                case tiledb_datatype_t.TILEDB_TIME_NS:
                case tiledb_datatype_t.TILEDB_TIME_PS:
                case tiledb_datatype_t.TILEDB_TIME_SEC:
                case tiledb_datatype_t.TILEDB_TIME_US:
                    return typeof(long);
                case tiledb_datatype_t.TILEDB_UINT16:
                    return typeof(ushort);
                case tiledb_datatype_t.TILEDB_UINT32:
                    return typeof(uint);
                case tiledb_datatype_t.TILEDB_UINT64:
                    return typeof(ulong);
                case tiledb_datatype_t.TILEDB_UINT8:
                    return typeof(sbyte);
                default:
                    return typeof(byte);
            }
        }

        public static Type to_Type(DataType datatype)
        {
            var tiledb_datatype = (tiledb_datatype_t)datatype;
            return ToType(tiledb_datatype);
        }

        public static bool is_string_type(tiledb_datatype_t tiledbDatatype)
        {
            return tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_ASCII
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UCS2
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UCS4
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF16
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF32
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF8;

        }

        public static bool is_string_type(DataType datatype)
        {
            return datatype == DataType.TILEDB_STRING_ASCII
                || datatype == DataType.TILEDB_STRING_UCS2
                || datatype == DataType.TILEDB_STRING_UCS4
                || datatype == DataType.TILEDB_STRING_UTF16
                || datatype == DataType.TILEDB_STRING_UTF32
                || datatype == DataType.TILEDB_STRING_UTF8;
        }


        public static ulong tiledb_datatype_size(tiledb_datatype_t tiledbDatatype)
        {
            return Methods.tiledb_datatype_size(tiledbDatatype);
        }

        public static ulong datatype_size(DataType datatype) 
        {
            var tiledb_datatype = (tiledb_datatype_t)datatype;
            return Methods.tiledb_datatype_size(tiledb_datatype);
        }




    }//class

}
