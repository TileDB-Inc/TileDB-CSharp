using System;
using System.ComponentModel;
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
        Invalid = tiledb_object_t.TILEDB_INVALID,
        Group = tiledb_object_t.TILEDB_GROUP,
        Array = tiledb_object_t.TILEDB_ARRAY,
        [Obsolete("Use Invalid instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INVALID = Invalid,
        [Obsolete("Use Group instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GROUP = Group,
        [Obsolete("Use Array instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_ARRAY = Array
    }


    public enum QueryType : uint
    {
        Read = tiledb_query_type_t.TILEDB_READ,
        Write = tiledb_query_type_t.TILEDB_WRITE,
        [Obsolete("Use Read instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_READ = Read,
        [Obsolete("Use Write instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_WRITE = Write
    }

    public enum QueryStatus : uint
    {
        Failed = tiledb_query_status_t.TILEDB_FAILED,
        Completed = tiledb_query_status_t.TILEDB_COMPLETED,
        InProgress = tiledb_query_status_t.TILEDB_INPROGRESS,
        Incomplete = tiledb_query_status_t.TILEDB_INCOMPLETE,
        Uninitialized = tiledb_query_status_t.TILEDB_UNINITIALIZED,
        [Obsolete("Use Failed instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FAILED = Failed,
        [Obsolete("Use Completed instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_COMPLETED = Completed,
        [Obsolete("Use InProgress instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INPROGRESS = InProgress,
        [Obsolete("Use Incomplete instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INCOMPLETE = Incomplete,
        [Obsolete("Use Uninitialized instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UNINITIALIZED = Uninitialized
    }

    public enum QueryConditionOperatorType : uint
    {
        LessThan = tiledb_query_condition_op_t.TILEDB_LT,
        LessThanOrEqual = tiledb_query_condition_op_t.TILEDB_LE,
        GreaterThan = tiledb_query_condition_op_t.TILEDB_GT,
        GreaterThanOrEqual = tiledb_query_condition_op_t.TILEDB_GE,
        Equal = tiledb_query_condition_op_t.TILEDB_EQ,
        NotEqual = tiledb_query_condition_op_t.TILEDB_NE,
        [Obsolete("Use LessThan instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_LT = LessThan,
        [Obsolete("Use LessThanOrEqual instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_LE = LessThanOrEqual,
        [Obsolete("Use GreaterThan instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GT = GreaterThan,
        [Obsolete("Use GreaterThanOrEqual instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GE = GreaterThanOrEqual,
        [Obsolete("Use Equal instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_EQ = Equal,
        [Obsolete("Use NotEqual instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_NE = NotEqual
    }

    public enum QueryConditionCombinationOperatorType : uint
    {
        And = tiledb_query_condition_combination_op_t.TILEDB_AND,
        Or = tiledb_query_condition_combination_op_t.TILEDB_OR,
        Not = tiledb_query_condition_combination_op_t.TILEDB_NOT,
        [Obsolete("Use And instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_AND = And,
        [Obsolete("Use Or instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_OR = Or,
        [Obsolete("Use Not instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_NOT = Not
    }

    public enum FileSystemType : uint
    {
        Hdfs = tiledb_filesystem_t.TILEDB_HDFS,
        S3 = tiledb_filesystem_t.TILEDB_S3,
        Azure = tiledb_filesystem_t.TILEDB_AZURE,
        Gcs = tiledb_filesystem_t.TILEDB_GCS,
        Memfs = tiledb_filesystem_t.TILEDB_MEMFS,
        [Obsolete("Use Hdfs instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_HDFS = Hdfs,
        [Obsolete("Use S3 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_S3 = S3,
        [Obsolete("Use Azure instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_AZURE = Azure,
        [Obsolete("Use Gcs instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GCS = Gcs,
        [Obsolete("Use Memfs instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MEMFS = Memfs
    }

    public enum DataType : uint
    {
        Int32 = tiledb_datatype_t.TILEDB_INT32,
        Int64 = tiledb_datatype_t.TILEDB_INT64,
        Float32 = tiledb_datatype_t.TILEDB_FLOAT32,
        Float64 = tiledb_datatype_t.TILEDB_FLOAT64,
        Char = tiledb_datatype_t.TILEDB_CHAR,
        Int8 = tiledb_datatype_t.TILEDB_INT8,
        UInt8 = tiledb_datatype_t.TILEDB_UINT8,
        Int16 = tiledb_datatype_t.TILEDB_INT16,
        UInt16 = tiledb_datatype_t.TILEDB_UINT16,
        UInt32 = tiledb_datatype_t.TILEDB_UINT32,
        UInt64 = tiledb_datatype_t.TILEDB_UINT64,
        StringAscii = tiledb_datatype_t.TILEDB_STRING_ASCII,
        StringUtf8 = tiledb_datatype_t.TILEDB_STRING_UTF8,
        StringUtf16 = tiledb_datatype_t.TILEDB_STRING_UTF16,
        StringUtf32 = tiledb_datatype_t.TILEDB_STRING_UTF32,
        StringUcs2 = tiledb_datatype_t.TILEDB_STRING_UCS2,
        StringUcs4 = tiledb_datatype_t.TILEDB_STRING_UCS4,
        Any = tiledb_datatype_t.TILEDB_ANY,
        DateTimeYear = tiledb_datatype_t.TILEDB_DATETIME_YEAR,
        DateTimeMonth = tiledb_datatype_t.TILEDB_DATETIME_MONTH,
        DateTimeWeek = tiledb_datatype_t.TILEDB_DATETIME_WEEK,
        DateTimeDay = tiledb_datatype_t.TILEDB_DATETIME_DAY,
        DateTimeHour = tiledb_datatype_t.TILEDB_DATETIME_HR,
        DateTimeMinute = tiledb_datatype_t.TILEDB_DATETIME_MIN,
        DateTimeSecond = tiledb_datatype_t.TILEDB_DATETIME_SEC,
        DateTimeMillisecond = tiledb_datatype_t.TILEDB_DATETIME_MS,
        DateTimeMicrosecond = tiledb_datatype_t.TILEDB_DATETIME_US,
        DateTimeNanosecond = tiledb_datatype_t.TILEDB_DATETIME_NS,
        DateTimePicosecond = tiledb_datatype_t.TILEDB_DATETIME_PS,
        DateTimeFemtosecond = tiledb_datatype_t.TILEDB_DATETIME_FS,
        DateTimeAttosecond = tiledb_datatype_t.TILEDB_DATETIME_AS,
        TimeHour = tiledb_datatype_t.TILEDB_TIME_HR,
        TimeMinute = tiledb_datatype_t.TILEDB_TIME_MIN,
        TimeSecond = tiledb_datatype_t.TILEDB_TIME_SEC,
        TimeMillisecond = tiledb_datatype_t.TILEDB_TIME_MS,
        TimeMicrosecond = tiledb_datatype_t.TILEDB_TIME_US,
        TimeNanosecond = tiledb_datatype_t.TILEDB_TIME_NS,
        TimePicosecond = tiledb_datatype_t.TILEDB_TIME_PS,
        TimeFemtosecond = tiledb_datatype_t.TILEDB_TIME_FS,
        TimeAttosecond = tiledb_datatype_t.TILEDB_TIME_AS,
        Blob = tiledb_datatype_t.TILEDB_BLOB,
        Boolean = tiledb_datatype_t.TILEDB_BOOL,
        [Obsolete("Use Int32 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INT32 = Int32,
        [Obsolete("Use Int64 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INT64 = Int64,
        [Obsolete("Use Float32 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FLOAT32 = Float32,
        [Obsolete("Use Float64 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FLOAT64 = Float64,
        [Obsolete("Use Char instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_CHAR = Char,
        [Obsolete("Use Int8 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INT8 = Int8,
        [Obsolete("Use UInt8 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UINT8 = UInt8,
        [Obsolete("Use Int16 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INT16 = Int16,
        [Obsolete("Use UInt16 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UINT16 = UInt16,
        [Obsolete("Use UInt32 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UINT32 = UInt32,
        [Obsolete("Use UInt64 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UINT64 = UInt64,
        [Obsolete("Use StringAscii instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_ASCII = StringAscii,
        [Obsolete("Use StringUtf8 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_UTF8 = StringUtf8,
        [Obsolete("Use StringUtf16 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_UTF16 = StringUtf16,
        [Obsolete("Use StringUtf32 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_UTF32 = StringUtf32,
        [Obsolete("Use StringUcs2 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_UCS2 = StringUcs2,
        [Obsolete("Use StringUcs4 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_STRING_UCS4 = StringUcs4,
        [Obsolete("Use Any instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_ANY = Any,
        [Obsolete("Use DateTimeYear instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_YEAR = DateTimeYear,
        [Obsolete("Use DateTimeMonth instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_MONTH = DateTimeMonth,
        [Obsolete("Use DateTimeWeek instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_WEEK = DateTimeWeek,
        [Obsolete("Use DateTimeDay instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_DAY = DateTimeDay,
        [Obsolete("Use DateTimeHour instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_HR = DateTimeHour,
        [Obsolete("Use DateTimeMinute instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_MIN = DateTimeMinute,
        [Obsolete("Use DateTimeSecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_SEC = DateTimeSecond,
        [Obsolete("Use DateTimeMillisecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_MS = DateTimeMillisecond,
        [Obsolete("Use DateTimeMicrosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_US = DateTimeMicrosecond,
        [Obsolete("Use DateTimeNanosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_NS = DateTimeNanosecond,
        [Obsolete("Use DateTimePicosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_PS = DateTimePicosecond,
        [Obsolete("Use DateTimeFemtosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_FS = DateTimeFemtosecond,
        [Obsolete("Use DateTimeAttosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DATETIME_AS = DateTimeAttosecond,
        [Obsolete("Use TimeHour instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_HR = TimeHour,
        [Obsolete("Use TimeMinute instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_MIN = TimeMinute,
        [Obsolete("Use TimeSecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_SEC = TimeSecond,
        [Obsolete("Use TimeMillisecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_MS = TimeMillisecond,
        [Obsolete("Use TimeMicrosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_US = TimeMicrosecond,
        [Obsolete("Use TimeNanosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_NS = TimeNanosecond,
        [Obsolete("Use TimePicosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_PS = TimePicosecond,
        [Obsolete("Use TimeFemtosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_FS = TimeFemtosecond,
        [Obsolete("Use TimeAttosecond instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_TIME_AS = TimeAttosecond,
        [Obsolete("Use Blob instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_BLOB = Blob,
        [Obsolete("Use Boolean instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_BOOL = Boolean
    }

    public enum ArrayType : uint
    {
        Dense = tiledb_array_type_t.TILEDB_DENSE,
        Sparse = tiledb_array_type_t.TILEDB_SPARSE,
        [Obsolete("Use Dense instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DENSE = Dense,
        [Obsolete("Use Sparse instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_SPARSE = Sparse
    }

    public enum LayoutType : uint
    {
        RowMajor = tiledb_layout_t.TILEDB_ROW_MAJOR,
        ColumnMajor = tiledb_layout_t.TILEDB_COL_MAJOR,
        GlobalOrder = tiledb_layout_t.TILEDB_GLOBAL_ORDER,
        Unordered = tiledb_layout_t.TILEDB_UNORDERED,
        Hilbert = tiledb_layout_t.TILEDB_HILBERT,
        [Obsolete("Use RowMajor instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_ROW_MAJOR = RowMajor,
        [Obsolete("Use ColumnMajor instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_COL_MAJOR = ColumnMajor,
        [Obsolete("Use GlobalOrder instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GLOBAL_ORDER = GlobalOrder,
        [Obsolete("Use Unordered instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_UNORDERED = Unordered,
        [Obsolete("Use Hilbert instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_HILBERT = Hilbert
    }

    public enum FilterType : uint
    {
        None = tiledb_filter_type_t.TILEDB_FILTER_NONE,
        Gzip = tiledb_filter_type_t.TILEDB_FILTER_GZIP,
        Zstandard = tiledb_filter_type_t.TILEDB_FILTER_ZSTD,
        Lz4 = tiledb_filter_type_t.TILEDB_FILTER_LZ4,
        RunLengthEncoding = tiledb_filter_type_t.TILEDB_FILTER_RLE,
        Bzip2 = tiledb_filter_type_t.TILEDB_FILTER_BZIP2,
        DoubleDelta = tiledb_filter_type_t.TILEDB_FILTER_DOUBLE_DELTA,
        BitWidthReduction = tiledb_filter_type_t.TILEDB_FILTER_BIT_WIDTH_REDUCTION,
        BitShuffle = tiledb_filter_type_t.TILEDB_FILTER_BITSHUFFLE,
        ByteShuffle = tiledb_filter_type_t.TILEDB_FILTER_BYTESHUFFLE,
        PositiveDelta = tiledb_filter_type_t.TILEDB_FILTER_POSITIVE_DELTA,
        ChecksumMd5 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_MD5,
        ChecksumSha256 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_SHA256,
        Dictionary = tiledb_filter_type_t.TILEDB_FILTER_DICTIONARY,
        ScaleFloat = tiledb_filter_type_t.TILEDB_FILTER_SCALE_FLOAT,
        [Obsolete("Use None instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_NONE = None,
        [Obsolete("Use Gzip instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_GZIP = Gzip,
        [Obsolete("Use Zstandard instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_ZSTD = Zstandard,
        [Obsolete("Use Lz4 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_LZ4 = Lz4,
        [Obsolete("Use RunLengthEncoding instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_RLE = RunLengthEncoding,
        [Obsolete("Use Bzip2 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_BZIP2 = Bzip2,
        [Obsolete("Use DoubleDelta instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_DOUBLE_DELTA = DoubleDelta,
        [Obsolete("Use BitWidthReduction instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_BIT_WIDTH_REDUCTION = BitWidthReduction,
        [Obsolete("Use BitShuffle instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_BITSHUFFLE = BitShuffle,
        [Obsolete("Use ByteShuffle instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_BYTESHUFFLE = ByteShuffle,
        [Obsolete("Use PositiveDelta instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_POSITIVE_DELTA = PositiveDelta,
        [Obsolete("Use ChecksumMd5 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_CHECKSUM_MD5 = ChecksumMd5,
        [Obsolete("Use ChecksumSha256 instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_CHECKSUM_SHA256 = ChecksumSha256,
        [Obsolete("Use Dictionary instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_DICTIONARY = Dictionary,
        [Obsolete("Use ScaleFloat instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_FILTER_SCALE_FLOAT = ScaleFloat
    }

    public enum FilterOption : uint
    {
        CompressionLevel = tiledb_filter_option_t.TILEDB_COMPRESSION_LEVEL,
        BitWidthMaxWindow = tiledb_filter_option_t.TILEDB_BIT_WIDTH_MAX_WINDOW,
        PositiveDeltaMaxWindow = tiledb_filter_option_t.TILEDB_POSITIVE_DELTA_MAX_WINDOW,
        ScaleFloatByteWidth = tiledb_filter_option_t.TILEDB_SCALE_FLOAT_BYTEWIDTH,
        ScaleFloatFactor = tiledb_filter_option_t.TILEDB_SCALE_FLOAT_FACTOR,
        ScaleFloatOffset = tiledb_filter_option_t.TILEDB_SCALE_FLOAT_OFFSET,
        [Obsolete("Use CompressionLevel instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_COMPRESSION_LEVEL = CompressionLevel,
        [Obsolete("Use BitWidthMaxWindow instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_BIT_WIDTH_MAX_WINDOW = BitWidthMaxWindow,
        [Obsolete("Use PositiveDeltaMaxWindow instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_POSITIVE_DELTA_MAX_WINDOW = PositiveDeltaMaxWindow,
        [Obsolete("Use ScaleFloatByteWidth instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_SCALE_FLOAT_BYTEWIDTH = ScaleFloatByteWidth,
        [Obsolete("Use ScaleFloatFactor instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_SCALE_FLOAT_FACTOR = ScaleFloatFactor,
        [Obsolete("Use ScaleFloatOffset instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_SCALE_FLOAT_OFFSET = ScaleFloatOffset
    }

    public enum EncryptionType : uint
    {
        NoEncryption = tiledb_encryption_type_t.TILEDB_NO_ENCRYPTION,
        Aes256Gcm = tiledb_encryption_type_t.TILEDB_AES_256_GCM,
        [Obsolete("Use NoEncryption instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_NO_ENCRYPTION = NoEncryption,
        [Obsolete("Use Aes256Gcm instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_AES_256_GCM = Aes256Gcm
    }

    public enum WalkOrderType : uint
    {
        PreOrder = tiledb_walk_order_t.TILEDB_PREORDER,
        PostOrder = tiledb_walk_order_t.TILEDB_POSTORDER,
        [Obsolete("Use PreOrder instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_PREORDER = PreOrder,
        [Obsolete("Use PostOrder instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_POSTORDER = PostOrder
    }

    public enum VfsMode : uint
    {
        Read = tiledb_vfs_mode_t.TILEDB_VFS_READ,
        Write = tiledb_vfs_mode_t.TILEDB_VFS_WRITE,
        Append = tiledb_vfs_mode_t.TILEDB_VFS_APPEND,
        [Obsolete("Use Read instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_READ = Read,
        [Obsolete("Use Write instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_WRITE = Write,
        [Obsolete("Use Append instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_APPEND = Append
    }

    public enum MIMEType : uint
    {
        Pdf = tiledb_mime_type_t.TILEDB_MIME_PDF,
        Tiff = tiledb_mime_type_t.TILEDB_MIME_TIFF,
        AutoDetect = tiledb_mime_type_t.TILEDB_MIME_AUTODETECT,
        [Obsolete("Use Pdf instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_PDF = Pdf,
        [Obsolete("Use Tiff instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_TIFF = Tiff,
        [Obsolete("Use AutoDetect instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_AUTODETECT = AutoDetect
    }

    public class Constants
    {
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
            sbyte* result;
            Methods.tiledb_query_type_to_str(tiledb_query_type, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Get QueryType from string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static QueryType QueryTypeFromStr(string str)
        {
            tiledb_query_type_t tiledb_query_type;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_query_type_from_str(ms_str, &tiledb_query_type);
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
            sbyte* result;
            Methods.tiledb_object_type_to_str(tiledb_object, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static ObjectType ObjectTypeFromStr(string str)
        {
            tiledb_object_t tiledb_object;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_object_type_from_str(ms_str, &tiledb_object);
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
            sbyte* result;
            Methods.tiledb_filesystem_to_str(tiledb_filesystem, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static FileSystemType FileSystemTypeFromStr(string str)
        {
            tiledb_filesystem_t tiledb_filesystem;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_filesystem_from_str(ms_str, &tiledb_filesystem);
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
            sbyte* result;
            Methods.tiledb_datatype_to_str(tiledb_datatype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static DataType DataTypeFromStr(string str)
        {
            tiledb_datatype_t tiledb_datatype;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_datatype_from_str(ms_str, &tiledb_datatype);
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
            sbyte* result;
            Methods.tiledb_array_type_to_str(tiledb_arraytype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static ArrayType ArrayTypeFromStr(string str)
        {
            tiledb_array_type_t tiledb_arraytype;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_array_type_from_str(ms_str, &tiledb_arraytype);
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
            sbyte* result;
            Methods.tiledb_layout_to_str(tiledb_layout, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static LayoutType LayoutTypeFromStr(string str)
        {
            tiledb_layout_t tiledb_layout;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_layout_from_str(ms_str, &tiledb_layout);
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
            sbyte* result;
            Methods.tiledb_filter_type_to_str(tiledb_filtertype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static FilterType FilterTypeFromStr(string str)
        {
            tiledb_filter_type_t tiledb_filtertype;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_filter_type_from_str(ms_str, &tiledb_filtertype);
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
            sbyte* result;
            Methods.tiledb_filter_option_to_str(tiledb_filteroption, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static FilterOption FilterOptionFromStr(string str)
        {
            tiledb_filter_option_t tiledb_filteroption;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_filter_option_from_str(ms_str, &tiledb_filteroption);
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
            sbyte* result;
            Methods.tiledb_encryption_type_to_str(tiledb_encryptiontype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static EncryptionType EncryptionTypeFromStr(string str)
        {
            tiledb_encryption_type_t tiledb_encryptiontype;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_encryption_type_from_str(ms_str, &tiledb_encryptiontype);
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
            sbyte* result;
            Methods.tiledb_query_status_to_str(tiledb_querystatus, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static QueryStatus QueryStatusFromStr(string str)
        {
            tiledb_query_status_t tiledb_querystatus;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_query_status_from_str(ms_str, &tiledb_querystatus);
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
            sbyte* result;
            Methods.tiledb_walk_order_to_str(tiledb_walkorder, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static WalkOrderType WalkOrderTypeFromStr(string str)
        {
            tiledb_walk_order_t tiledb_walkorder;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_walk_order_from_str(ms_str, &tiledb_walkorder);
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
            sbyte* result;
            Methods.tiledb_vfs_mode_to_str(tiledb_vfsmode, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        public static VfsMode VfsModeFromStr(string str)
        {
            tiledb_vfs_mode_t tiledb_vfsmode;
            using var ms_str = new MarshaledString(str);
            unsafe
            {
                int status = Methods.tiledb_vfs_mode_from_str(ms_str, &tiledb_vfsmode);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new ArgumentException("EnumUtil.VfsModeFromStr, Invalid string:" + str);
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
            else if (t == typeof(bool))
            {
                tiledb_datatype = tiledb_datatype_t.TILEDB_BOOL;
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
                case tiledb_datatype_t.TILEDB_BOOL:
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
            return datatype == DataType.StringAscii
                || datatype == DataType.StringUcs2
                || datatype == DataType.StringUcs4
                || datatype == DataType.StringUtf16
                || datatype == DataType.StringUtf32
                || datatype == DataType.StringUtf8;
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
    }
}
