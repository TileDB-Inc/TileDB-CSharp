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
        TILEDB_BLOB = tiledb_datatype_t.TILEDB_BLOB
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
    
    public enum MIMEType : uint
    {
        TILEDB_MIME_PDF = tiledb_mime_type_t.TILEDB_MIME_PDF,
        TILEDB_MIME_TIFF = tiledb_mime_type_t.TILEDB_MIME_TIFF,
        TILEDB_MIME_AUTODETECT = tiledb_mime_type_t.TILEDB_MIME_AUTODETECT,
    }

   public class Constants {
        public const uint TILEDB_VAR_NUM = uint.MaxValue;

        #region File Api
        public const string METADATA_SIZE_KEY = "file_size";
        public const string FILE_DIMENSION_NAME = "position";
        public const string FILE_ATTRIBUTE_NAME = "contents";
        public const string FILE_METADATA_MIME_TYPE_KEY = "mime";
        public const string FILE_METADATA_MIME_ENCODING_KEY = "mime_encoding";
        public const string METADATA_ORIGINAL_FILE_NAME = "original_file_name";
        #endregion File Api

    }



    public static unsafe class EnumUtil 
    {
        /// <summary>
        /// Get string of QueryType.
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        public static string QueryTypeToStr(QueryType queryType)
        {
            tiledb_query_type_t tiledb_query_type = (tiledb_query_type_t)queryType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_query_type_to_str(tiledb_query_type, p_result);
            }
            return ms_result;
        }

        /// <summary>
        /// Get QueryType from string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static QueryType QueryTypeFromStr(string str)
        {
            tiledb_query_type_t tiledb_query_type;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_query_type_from_str(ms_str, &tiledb_query_type);
                if (status != (int)Status.TILEDB_OK) 
                {
                    throw new System.ArgumentException("EnumUtil.QueryTypeFromStr, Invalid string:" + str);
                }
            }
            return (QueryType)tiledb_query_type;
        }

        /// <summary>
        /// Get string of ObjectType.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static string ObjectTypeToStr(ObjectType objectType)
        {
            tiledb_object_t tiledb_object = (tiledb_object_t)objectType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_object_type_to_str(tiledb_object, p_result);
            }
            return ms_result;
        }

        public static ObjectType ObjectTypeFromStr(string str)
        {
            tiledb_object_t tiledb_object;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_object_type_from_str(ms_str, &tiledb_object);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.ObjectTypeFromStr, Invalid string:" + str);
                }
            }
            return (ObjectType)tiledb_object;
        }

        public static string FileSystemTypeToStr(FileSystemType fileSystemType)
        {
            tiledb_filesystem_t tiledb_filesystem = (tiledb_filesystem_t)fileSystemType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_filesystem_to_str(tiledb_filesystem, p_result);
            }
            return ms_result;
        }

        public static FileSystemType FileSystemTypeFromStr(string str)
        {
            tiledb_filesystem_t tiledb_filesystem;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_filesystem_from_str(ms_str, &tiledb_filesystem);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.FileSystemTypeFromStr, Invalid string:" + str);
                }
            }
            return (FileSystemType)tiledb_filesystem;
        }

        public static string DataTypeToStr(DataType dataType)
        {
            tiledb_datatype_t tiledb_datatype = (tiledb_datatype_t)dataType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_datatype_to_str(tiledb_datatype, p_result);
            }
            return ms_result;
        }

        public static DataType DataTypeFromStr(string str)
        {
            tiledb_datatype_t tiledb_datatype;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_datatype_from_str(ms_str, &tiledb_datatype); 
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.DataTypeFromStr, Invalid string:" + str);
                }
            }
            return (DataType)tiledb_datatype;
        }

        public static string ArrayTypeToStr(ArrayType arrayType)
        {
            tiledb_array_type_t tiledb_arraytype = (tiledb_array_type_t)arrayType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_array_type_to_str(tiledb_arraytype, p_result);
            }
            return ms_result;
        }

        public static ArrayType ArrayTypeFromStr(string str)
        {
            tiledb_array_type_t tiledb_arraytype;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_array_type_from_str(ms_str, &tiledb_arraytype);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.ArrayTypeFromStr, Invalid string:" + str);
                }
            }
            return (ArrayType)tiledb_arraytype;
        }

        public static string LayoutTypeToStr(LayoutType layoutType)
        {
            tiledb_layout_t tiledb_layout = (tiledb_layout_t)layoutType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_layout_to_str(tiledb_layout, p_result);
            }
            return ms_result;
        }

        public static LayoutType LayoutTypeFromStr(string str)
        {
            tiledb_layout_t tiledb_layout;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_layout_from_str(ms_str, &tiledb_layout);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.LayoutTypeFromStr, Invalid string:" + str);
                }
            }
            return (LayoutType)tiledb_layout;
        }

        public static string FilterTypeToStr(FilterType filterType)
        {
            tiledb_filter_type_t tiledb_filtertype = (tiledb_filter_type_t)filterType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_filter_type_to_str(tiledb_filtertype, p_result);
            }
            return ms_result;
        }

        public static FilterType FilterTypeFromStr(string str)
        {
            tiledb_filter_type_t tiledb_filtertype;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_filter_type_from_str(ms_str, &tiledb_filtertype);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.FilterTypeFromStr, Invalid string:" + str);
                }
            }
            return (FilterType)tiledb_filtertype;
        }

        public static string FilterOptionToStr(FilterOption filterOption)
        {
            tiledb_filter_option_t tiledb_filteroption = (tiledb_filter_option_t)filterOption;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_filter_option_to_str(tiledb_filteroption, p_result);
            }
            return ms_result;
        }

        public static FilterOption FilterOptionFromStr(string str)
        {
            tiledb_filter_option_t tiledb_filteroption;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_filter_option_from_str(ms_str, &tiledb_filteroption);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.FilterOptionFromStr, Invalid string:" + str);
                }
            }
            return (FilterOption)tiledb_filteroption;
        }

        public static string EncryptionTypeToStr(EncryptionType encryptionType)
        {
            tiledb_encryption_type_t tiledb_encryptiontype = (tiledb_encryption_type_t)encryptionType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_encryption_type_to_str(tiledb_encryptiontype, p_result);
            }
            return ms_result;
        }

        public static EncryptionType EncryptionTypeFromStr(string str)
        {
            tiledb_encryption_type_t tiledb_encryptiontype;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_encryption_type_from_str(ms_str, &tiledb_encryptiontype);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.EncryptionTypeFromStr, Invalid string:" + str);
                }
            }
            return (EncryptionType)tiledb_encryptiontype;
        }

        public static string QueryStatusToStr(QueryStatus queryStatus)
        {
            tiledb_query_status_t tiledb_querystatus = (tiledb_query_status_t)queryStatus;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_query_status_to_str(tiledb_querystatus, p_result);
            }
            return ms_result;
        }

        public static QueryStatus QueryStatusFromStr(string str)
        {
            tiledb_query_status_t tiledb_querystatus;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_query_status_from_str(ms_str, &tiledb_querystatus);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.QueryStatusFromStr, Invalid string:" + str);
                }
            }
            return (QueryStatus)tiledb_querystatus;
        }

        public static string WalkOrderTypeToStr(WalkOrderType walkOrderType)
        {
            tiledb_walk_order_t tiledb_walkorder = (tiledb_walk_order_t)walkOrderType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_walk_order_to_str(tiledb_walkorder, p_result);
            }
            return ms_result;
        }

        public static WalkOrderType WalkOrderTypeFromStr(string str)
        {
            tiledb_walk_order_t tiledb_walkorder;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_walk_order_from_str(ms_str, &tiledb_walkorder);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.WalkOrderFromStr, Invalid string:" + str);
                }
            }
            return (WalkOrderType)tiledb_walkorder;
        }

        public static string VfsModeToStr(VfsMode vfsMode)
        {
            tiledb_vfs_mode_t tiledb_vfsmode = (tiledb_vfs_mode_t)vfsMode;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                TileDB.Interop.Methods.tiledb_vfs_mode_to_str(tiledb_vfsmode, p_result);
            }
            return ms_result;
        }

        public static VfsMode VfsModeFromStr(string str)
        {
            tiledb_vfs_mode_t tiledb_vfsmode;
            var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_vfs_mode_from_str(ms_str, &tiledb_vfsmode);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("EnumUtil.VfsModeFromStr, Invalid string:" + str);
                }
            }
            return (VfsMode)tiledb_vfsmode;
        }

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
                tiledb_datatype = tiledb_datatype_t.TILEDB_UINT8;
            }
            else if (t == typeof(sbyte))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_INT8;
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

        public static DataType TypeToDataType(Type t) 
        {
            var tiledb_datatype = to_tiledb_datatype(t);
            return (DataType)tiledb_datatype;
        }

        private static Type TileDBDataTypeToType(tiledb_datatype_t tiledbDatatype)
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
                    return typeof(byte);
                case tiledb_datatype_t.TILEDB_BLOB:
                    return typeof(byte);
                default:
                    return typeof(byte);
            }
        }

        public static Type DataTypeToType(DataType datatype)
        {
            var tiledb_datatype = (tiledb_datatype_t)datatype;
            return TileDBDataTypeToType(tiledb_datatype);
        }

        public static bool IsStringType(tiledb_datatype_t tiledbDatatype)
        {
            return tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_ASCII
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UCS2
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UCS4
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF16
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF32
                || tiledbDatatype == tiledb_datatype_t.TILEDB_STRING_UTF8;

        }

        public static bool IsStringType(DataType datatype)
        {
            return datatype == DataType.TILEDB_STRING_ASCII
                || datatype == DataType.TILEDB_STRING_UCS2
                || datatype == DataType.TILEDB_STRING_UCS4
                || datatype == DataType.TILEDB_STRING_UTF16
                || datatype == DataType.TILEDB_STRING_UTF32
                || datatype == DataType.TILEDB_STRING_UTF8;
        }


        public static ulong TileDBDataTypeSize(tiledb_datatype_t tiledbDatatype)
        {
            return Methods.tiledb_datatype_size(tiledbDatatype);
        }

        public static ulong DataTypeSize(DataType datatype) 
        {
            var tiledb_datatype = (tiledb_datatype_t)datatype;
            return Methods.tiledb_datatype_size(tiledb_datatype);
        }




    }//class

}
