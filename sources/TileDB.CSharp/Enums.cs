using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public enum Status
    {
        TILEDB_OOM = -2,
        TILEDB_ERR = -1,
        TILEDB_OK = 0
    }

    public enum ObjectType
    {
        TILEDB_INVALID = TileDB.Interop.tiledb_object_t.TILEDB_INVALID,
        TILEDB_GROUP = TileDB.Interop.tiledb_object_t.TILEDB_GROUP,
        TILEDB_ARRAY = TileDB.Interop.tiledb_object_t.TILEDB_ARRAY
    }


    public enum QueryType
    {
        TILEDB_READ = TileDB.Interop.tiledb_query_type_t.TILEDB_READ,
        TILEDB_WRITE = TileDB.Interop.tiledb_query_type_t.TILEDB_WRITE,
    }

    public enum QueryStatus
    {
        TILEDB_FAILED = TileDB.Interop.tiledb_query_status_t.TILEDB_FAILED,
        TILEDB_COMPLETED = TileDB.Interop.tiledb_query_status_t.TILEDB_COMPLETED,
        TILEDB_INPROGRESS = TileDB.Interop.tiledb_query_status_t.TILEDB_INPROGRESS,
        TILEDB_INCOMPLETE = TileDB.Interop.tiledb_query_status_t.TILEDB_INCOMPLETE,
        TILEDB_UNINITIALIZED = TileDB.Interop.tiledb_query_status_t.TILEDB_UNINITIALIZED,
    }

    public enum QueryConditionOperatorType
    {
        TILEDB_LT = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_LT,
        TILEDB_LE = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_LE,
        TILEDB_GT = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_GT,
        TILEDB_GE = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_GE,
        TILEDB_EQ = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_EQ,
        TILEDB_NE = TileDB.Interop.tiledb_query_condition_op_t.TILEDB_NE,
    }

    public enum QueryConditionCombinationOperatorType
    {
        TILEDB_AND = TileDB.Interop.tiledb_query_condition_combination_op_t.TILEDB_AND,
        TILEDB_OR = TileDB.Interop.tiledb_query_condition_combination_op_t.TILEDB_OR,
        TILEDB_NOT = TileDB.Interop.tiledb_query_condition_combination_op_t.TILEDB_NOT,
    }

    public enum tiledb_filesystem_t
    {
        TILEDB_HDFS = TileDB.Interop.tiledb_filesystem_t.TILEDB_HDFS,
        TILEDB_S3 = TileDB.Interop.tiledb_filesystem_t.TILEDB_S3,
        TILEDB_AZURE = TileDB.Interop.tiledb_filesystem_t.TILEDB_AZURE,
        TILEDB_GCS = TileDB.Interop.tiledb_filesystem_t.TILEDB_GCS,
        TILEDB_MEMFS = TileDB.Interop.tiledb_filesystem_t.TILEDB_MEMFS,
    }

    public enum tiledb_datatype_t
    {
        TILEDB_INT32 = TileDB.Interop.tiledb_datatype_t.TILEDB_INT32,
        TILEDB_INT64 = TileDB.Interop.tiledb_datatype_t.TILEDB_INT64,
        TILEDB_FLOAT32 = TileDB.Interop.tiledb_datatype_t.TILEDB_FLOAT32,
        TILEDB_FLOAT64 = TileDB.Interop.tiledb_datatype_t.TILEDB_FLOAT64,
        TILEDB_CHAR = TileDB.Interop.tiledb_datatype_t.TILEDB_CHAR,
        TILEDB_INT8 = TileDB.Interop.tiledb_datatype_t.TILEDB_INT8,
        TILEDB_UINT8 = TileDB.Interop.tiledb_datatype_t.TILEDB_UINT8,
        TILEDB_INT16 = TileDB.Interop.tiledb_datatype_t.TILEDB_INT16,
        TILEDB_UINT16 = TileDB.Interop.tiledb_datatype_t.TILEDB_UINT16,
        TILEDB_UINT32 = TileDB.Interop.tiledb_datatype_t.TILEDB_UINT32,
        TILEDB_UINT64 = TileDB.Interop.tiledb_datatype_t.TILEDB_UINT64,
        TILEDB_STRING_ASCII = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_ASCII,
        TILEDB_STRING_UTF8 = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_UTF8,
        TILEDB_STRING_UTF16 = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_UTF16,
        TILEDB_STRING_UTF32 = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_UTF32,
        TILEDB_STRING_UCS2 = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_UCS2,
        TILEDB_STRING_UCS4 = TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_UCS4,
        TILEDB_ANY = TileDB.Interop.tiledb_datatype_t.TILEDB_ANY,
        TILEDB_DATETIME_YEAR = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_YEAR,
        TILEDB_DATETIME_MONTH = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_MONTH,
        TILEDB_DATETIME_WEEK = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_WEEK,
        TILEDB_DATETIME_DAY = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_DAY,
        TILEDB_DATETIME_HR = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_HR,
        TILEDB_DATETIME_MIN = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_MIN,
        TILEDB_DATETIME_SEC = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_SEC,
        TILEDB_DATETIME_MS = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_MS,
        TILEDB_DATETIME_US = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_US,
        TILEDB_DATETIME_NS = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_NS,
        TILEDB_DATETIME_PS = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_PS,
        TILEDB_DATETIME_FS = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_FS,
        TILEDB_DATETIME_AS = TileDB.Interop.tiledb_datatype_t.TILEDB_DATETIME_AS,
        TILEDB_TIME_HR = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_HR,
        TILEDB_TIME_MIN = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_MIN,
        TILEDB_TIME_SEC = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_SEC,
        TILEDB_TIME_MS = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_MS,
        TILEDB_TIME_US = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_US,
        TILEDB_TIME_NS = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_NS,
        TILEDB_TIME_PS = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_PS,
        TILEDB_TIME_FS = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_FS,
        TILEDB_TIME_AS = TileDB.Interop.tiledb_datatype_t.TILEDB_TIME_AS,
    }

    public enum ArrayType
    {
        TILEDB_DENSE = TileDB.Interop.tiledb_array_type_t.TILEDB_DENSE,
        TILEDB_SPARSE = TileDB.Interop.tiledb_array_type_t.TILEDB_SPARSE,
    }

    public enum LayoutType
    {
        TILEDB_ROW_MAJOR = TileDB.Interop.tiledb_layout_t.TILEDB_ROW_MAJOR,
        TILEDB_COL_MAJOR = TileDB.Interop.tiledb_layout_t.TILEDB_COL_MAJOR,
        TILEDB_GLOBAL_ORDER = TileDB.Interop.tiledb_layout_t.TILEDB_GLOBAL_ORDER,
        TILEDB_UNORDERED = TileDB.Interop.tiledb_layout_t.TILEDB_UNORDERED,
        TILEDB_HILBERT = TileDB.Interop.tiledb_layout_t.TILEDB_HILBERT,
    }

    public enum FilterType
    {
        TILEDB_FILTER_NONE = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_NONE,
        TILEDB_FILTER_GZIP = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_GZIP,
        TILEDB_FILTER_ZSTD = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_ZSTD,
        TILEDB_FILTER_LZ4 = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_LZ4,
        TILEDB_FILTER_RLE = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_RLE,
        TILEDB_FILTER_BZIP2 = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_BZIP2,
        TILEDB_FILTER_DOUBLE_DELTA = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_DOUBLE_DELTA,
        TILEDB_FILTER_BIT_WIDTH_REDUCTION = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_BIT_WIDTH_REDUCTION,
        TILEDB_FILTER_BITSHUFFLE = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_BITSHUFFLE,
        TILEDB_FILTER_BYTESHUFFLE = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_BYTESHUFFLE,
        TILEDB_FILTER_POSITIVE_DELTA = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_POSITIVE_DELTA,
        TILEDB_FILTER_CHECKSUM_MD5 = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_MD5,
        TILEDB_FILTER_CHECKSUM_SHA256 = TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_SHA256,
    }

    public enum FilterOption
    {
        TILEDB_COMPRESSION_LEVEL = TileDB.Interop.tiledb_filter_option_t.TILEDB_COMPRESSION_LEVEL,
        TILEDB_BIT_WIDTH_MAX_WINDOW = TileDB.Interop.tiledb_filter_option_t.TILEDB_BIT_WIDTH_MAX_WINDOW,
        TILEDB_POSITIVE_DELTA_MAX_WINDOW = TileDB.Interop.tiledb_filter_option_t.TILEDB_POSITIVE_DELTA_MAX_WINDOW,
    }

    public enum EncryptionType
    {
        TILEDB_NO_ENCRYPTION = TileDB.Interop.tiledb_encryption_type_t.TILEDB_NO_ENCRYPTION,
        TILEDB_AES_256_GCM = TileDB.Interop.tiledb_encryption_type_t.TILEDB_AES_256_GCM,
    }

    public enum WalkOrderType
    {
        TILEDB_PREORDER = TileDB.Interop.tiledb_walk_order_t.TILEDB_PREORDER,
        TILEDB_POSTORDER = TileDB.Interop.tiledb_walk_order_t.TILEDB_POSTORDER,
    }

    public enum VFSMode
    {
        TILEDB_VFS_READ = TileDB.Interop.tiledb_vfs_mode_t.TILEDB_VFS_READ,
        TILEDB_VFS_WRITE = TileDB.Interop.tiledb_vfs_mode_t.TILEDB_VFS_WRITE,
        TILEDB_VFS_APPEND = TileDB.Interop.tiledb_vfs_mode_t.TILEDB_VFS_APPEND,
    }

}
