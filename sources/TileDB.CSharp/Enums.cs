using System;
using System.ComponentModel;
using TileDB.Interop;

namespace TileDB.CSharp
{
    // TODO Not sure why this is not automatically wrapped
    internal enum Status
    {
        TILEDB_OOM = -2,
        TILEDB_ERR = -1,
        TILEDB_OK = 0
    }

    /// <summary>
    /// Specifies the type of a TileDB object.
    /// </summary>
    public enum ObjectType : uint
    {
        /// <summary>
        /// Invalid object.
        /// </summary>
        Invalid = tiledb_object_t.TILEDB_INVALID,
        /// <summary>
        /// The object is a <see cref="Group"/>.
        /// </summary>
        Group = tiledb_object_t.TILEDB_GROUP,
        /// <summary>
        /// The object is an <see cref="Array"/>.
        /// </summary>
        Array = tiledb_object_t.TILEDB_ARRAY,
        [Obsolete("Use Invalid instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_INVALID = Invalid,
        [Obsolete("Use Group instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_GROUP = Group,
        [Obsolete("Use Array instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_ARRAY = Array
    }

    /// <summary>
    /// Specifies the type of a <see cref="Query"/>.
    /// </summary>
    public enum QueryType : uint
    {
        /// <summary>
        /// Read query.
        /// </summary>
        Read = tiledb_query_type_t.TILEDB_READ,
        /// <summary>
        /// Write query.
        /// </summary>
        Write = tiledb_query_type_t.TILEDB_WRITE,
        Delete = tiledb_query_type_t.TILEDB_DELETE,
        Update = tiledb_query_type_t.TILEDB_UPDATE,
        ModifyExclusive = tiledb_query_type_t.TILEDB_MODIFY_EXCLUSIVE,
        [Obsolete("Use Read instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_READ = Read,
        [Obsolete("Use Write instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_WRITE = Write
    }

    /// <summary>
    /// Specifies the status of a <see cref="Query"/>.
    /// </summary>
    public enum QueryStatus : uint
    {
        /// <summary>
        /// The query failed.
        /// </summary>
        Failed = tiledb_query_status_t.TILEDB_FAILED,
        /// <summary>
        /// The query completed and all data has been read.
        /// </summary>
        Completed = tiledb_query_status_t.TILEDB_COMPLETED,
        /// <summary>
        /// The query is in process.
        /// </summary>
        InProgress = tiledb_query_status_t.TILEDB_INPROGRESS,
        /// <summary>
        /// The query completed but not all data has been read.
        /// </summary>
        Incomplete = tiledb_query_status_t.TILEDB_INCOMPLETE,
        /// <summary>
        /// The query is not initialized.
        /// </summary>
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

    /// <summary>
    /// Specifies the relation type of a <see cref="QueryCondition"/> between its attribute and its value.
    /// </summary>
    public enum QueryConditionOperatorType : uint
    {
        /// <summary>
        /// The attribute is less than the value.
        /// </summary>
        LessThan = tiledb_query_condition_op_t.TILEDB_LT,
        /// <summary>
        /// The attribute is less than or equal to the value.
        /// </summary>
        LessThanOrEqual = tiledb_query_condition_op_t.TILEDB_LE,
        /// <summary>
        /// The attribute is greater than the value.
        /// </summary>
        GreaterThan = tiledb_query_condition_op_t.TILEDB_GT,
        /// <summary>
        /// The attribute is greater than or equal to the value.
        /// </summary>
        GreaterThanOrEqual = tiledb_query_condition_op_t.TILEDB_GE,
        /// <summary>
        /// The attribute is equal to the value.
        /// </summary>
        Equal = tiledb_query_condition_op_t.TILEDB_EQ,
        /// <summary>
        /// The attribute is not equal to the value.
        /// </summary>
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

    /// <summary>
    /// Specifies the combination operator type of one or two <see cref="QueryCondition"/>s.
    /// </summary>
    public enum QueryConditionCombinationOperatorType : uint
    {
        /// <summary>
        /// Both conditions must be satisfied.
        /// </summary>
        And = tiledb_query_condition_combination_op_t.TILEDB_AND,
        /// <summary>
        /// Either of the two conditions must be satisfied.
        /// </summary>
        Or = tiledb_query_condition_combination_op_t.TILEDB_OR,
        /// <summary>
        /// The condition must not be satisfied.
        /// </summary>
        Not = tiledb_query_condition_combination_op_t.TILEDB_NOT,
        [Obsolete("Use And instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_AND = And,
        [Obsolete("Use Or instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_OR = Or,
        [Obsolete("Use Not instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_NOT = Not
    }

    /// <summary>
    /// Specifies the type of a TileDB file system.
    /// </summary>
    public enum FileSystemType : uint
    {
        /// <summary>
        /// HDFS file system.
        /// </summary>
        Hdfs = tiledb_filesystem_t.TILEDB_HDFS,
        /// <summary>
        /// S3 file system.
        /// </summary>
        S3 = tiledb_filesystem_t.TILEDB_S3,
        /// <summary>
        /// Azure filesystem.
        /// </summary>
        Azure = tiledb_filesystem_t.TILEDB_AZURE,
        /// <summary>
        /// Google Cloud Storage file system.
        /// </summary>
        Gcs = tiledb_filesystem_t.TILEDB_GCS,
        /// <summary>
        /// In-memory file system.
        /// </summary>
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

    /// <summary>
    /// Specifies the data type of entities such as <see cref="Dimension"/>s, <see cref="Attribute"/>s,
    /// <see cref="ArrayMetadata"/> and <see cref="GroupMetadata"/>.
    /// </summary>
    public enum DataType : uint
    {
        /// <summary>
        /// A signed 32-bit integer.
        /// </summary>
        Int32 = tiledb_datatype_t.TILEDB_INT32,
        /// <summary>
        /// A signed 64-bit integer.
        /// </summary>
        Int64 = tiledb_datatype_t.TILEDB_INT64,
        /// <summary>
        /// A 32-bit floating-point number.
        /// </summary>
        Float32 = tiledb_datatype_t.TILEDB_FLOAT32,
        /// <summary>
        /// A 64-bit floating-point number.
        /// </summary>
        Float64 = tiledb_datatype_t.TILEDB_FLOAT64,
        /// <summary>
        /// A sequence of characters. Deprecated.
        /// </summary>
        [Obsolete("This data type is deprecated.")]
        // https://github.com/TileDB-Inc/TileDB/pull/2742
        Char = tiledb_datatype_t.TILEDB_CHAR,
        /// <summary>
        /// A signed 8-bit integer.
        /// </summary>
        Int8 = tiledb_datatype_t.TILEDB_INT8,
        /// <summary>
        /// An unsigned 8-bit integer.
        /// </summary>
        UInt8 = tiledb_datatype_t.TILEDB_UINT8,
        /// <summary>
        /// A signed 16-bit integer.
        /// </summary>
        Int16 = tiledb_datatype_t.TILEDB_INT16,
        /// <summary>
        /// An unsigned 16-bit integer.
        /// </summary>
        UInt16 = tiledb_datatype_t.TILEDB_UINT16,
        /// <summary>
        /// An unsigned 32-bit integer.
        /// </summary>
        UInt32 = tiledb_datatype_t.TILEDB_UINT32,
        /// <summary>
        /// An unsigned 64-bit integer.
        /// </summary>
        UInt64 = tiledb_datatype_t.TILEDB_UINT64,
        /// <summary>
        /// An ASCII string.
        /// </summary>
        StringAscii = tiledb_datatype_t.TILEDB_STRING_ASCII,
        /// <summary>
        /// A UTF-8 string.
        /// </summary>
        StringUtf8 = tiledb_datatype_t.TILEDB_STRING_UTF8,
        /// <summary>
        /// A UTF-16 string.
        /// </summary>
        StringUtf16 = tiledb_datatype_t.TILEDB_STRING_UTF16,
        /// <summary>
        /// A UTF-32 string.
        /// </summary>
        StringUtf32 = tiledb_datatype_t.TILEDB_STRING_UTF32,
        /// <summary>
        /// A UCS-2 string. Deprecated.
        /// </summary>
        [Obsolete("This data type is deprecated.")]
        // https://github.com/TileDB-Inc/TileDB/pull/2812
        StringUcs2 = tiledb_datatype_t.TILEDB_STRING_UCS2,
        /// <summary>
        /// A UCS-4 string. Deprecated.
        /// </summary>
        [Obsolete("This data type is deprecated.")]
        // https://github.com/TileDB-Inc/TileDB/pull/2812
        StringUcs4 = tiledb_datatype_t.TILEDB_STRING_UCS4,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// years since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeYear = tiledb_datatype_t.TILEDB_DATETIME_YEAR,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// months since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeMonth = tiledb_datatype_t.TILEDB_DATETIME_MONTH,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// weeks since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeWeek = tiledb_datatype_t.TILEDB_DATETIME_WEEK,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// days since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeDay = tiledb_datatype_t.TILEDB_DATETIME_DAY,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// hours since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeHour = tiledb_datatype_t.TILEDB_DATETIME_HR,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// minutes since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeMinute = tiledb_datatype_t.TILEDB_DATETIME_MIN,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// seconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeSecond = tiledb_datatype_t.TILEDB_DATETIME_SEC,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// milliseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeMillisecond = tiledb_datatype_t.TILEDB_DATETIME_MS,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// microseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeMicrosecond = tiledb_datatype_t.TILEDB_DATETIME_US,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// nanoseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        DateTimeNanosecond = tiledb_datatype_t.TILEDB_DATETIME_NS,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// picoseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        /// <remarks>
        /// One second consists of one trillion picoseconds.
        /// </remarks>
        DateTimePicosecond = tiledb_datatype_t.TILEDB_DATETIME_PS,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// femtoseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        /// <remarks>
        /// One second consists of one quadrillion femtoseconds.
        /// </remarks>
        DateTimeFemtosecond = tiledb_datatype_t.TILEDB_DATETIME_FS,
        /// <summary>
        /// A date and time, counted as the signed 64-bit number of
        /// attoseconds since the Unix epoch (January 1 1970 at midnight).
        /// </summary>
        /// <remarks>
        /// One second consists of one quintillion attoseconds.
        /// </remarks>
        DateTimeAttosecond = tiledb_datatype_t.TILEDB_DATETIME_AS,
        /// <summary>
        /// A time of day counted in hours.
        /// </summary>
        TimeHour = tiledb_datatype_t.TILEDB_TIME_HR,
        /// <summary>
        /// A time of day counted in minutes.
        /// </summary>
        TimeMinute = tiledb_datatype_t.TILEDB_TIME_MIN,
        /// <summary>
        /// A time of day counted in seconds.
        /// </summary>
        TimeSecond = tiledb_datatype_t.TILEDB_TIME_SEC,
        /// <summary>
        /// A time of day counted in milliseconds.
        /// </summary>
        TimeMillisecond = tiledb_datatype_t.TILEDB_TIME_MS,
        /// <summary>
        /// A time of day counted in microseconds.
        /// </summary>
        TimeMicrosecond = tiledb_datatype_t.TILEDB_TIME_US,
        /// <summary>
        /// A time of day counted in nanoseconds.
        /// </summary>
        TimeNanosecond = tiledb_datatype_t.TILEDB_TIME_NS,
        /// <summary>
        /// A time of day counted in picoseconds.
        /// </summary>
        TimePicosecond = tiledb_datatype_t.TILEDB_TIME_PS,
        /// <summary>
        /// A time of day counted in femtoseconds.
        /// </summary>
        TimeFemtosecond = tiledb_datatype_t.TILEDB_TIME_FS,
        /// <summary>
        /// A time of day counted in attoseconds.
        /// </summary>
        TimeAttosecond = tiledb_datatype_t.TILEDB_TIME_AS,
        /// <summary>
        /// A binary blob.
        /// </summary>
        Blob = tiledb_datatype_t.TILEDB_BLOB,
        /// <summary>
        /// A boolean value.
        /// </summary>
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
        [Obsolete("This data type is deprecated."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_ANY = tiledb_datatype_t.TILEDB_ANY,
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

    /// <summary>
    /// Specifies the type of an <see cref="Array"/>.
    /// </summary>
    public enum ArrayType : uint
    {
        /// <summary>
        /// The array is dense.
        /// </summary>
        Dense = tiledb_array_type_t.TILEDB_DENSE,
        /// <summary>
        /// The array is sparse.
        /// </summary>
        Sparse = tiledb_array_type_t.TILEDB_SPARSE,
        [Obsolete("Use Dense instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_DENSE = Dense,
        [Obsolete("Use Sparse instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_SPARSE = Sparse
    }

    /// <summary>
    /// Specifies the order of data stored in an <see cref="Array"/>
    /// or sent or retrieved from a <see cref="Query"/>.
    /// </summary>
    public enum LayoutType : uint
    {
        /// <summary>
        /// Data are in row-major order.
        /// </summary>
        RowMajor = tiledb_layout_t.TILEDB_ROW_MAJOR,
        /// <summary>
        /// Data are in column-major order.
        /// </summary>
        ColumnMajor = tiledb_layout_t.TILEDB_COL_MAJOR,
        /// <summary>
        /// Data are in global order.
        /// </summary>
        GlobalOrder = tiledb_layout_t.TILEDB_GLOBAL_ORDER,
        /// <summary>
        /// Data are unordered.
        /// </summary>
        // TODO: Make them more explanatory.
        Unordered = tiledb_layout_t.TILEDB_UNORDERED,
        /// <summary>
        /// Data are ordered according to a Hilbert curve.
        /// </summary>
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

    /// <summary>
    /// Specifies the type of a <see cref="Filter"/>.
    /// </summary>
    public enum FilterType : uint
    {
        /// <summary>
        /// No-op filter.
        /// </summary>
        None = tiledb_filter_type_t.TILEDB_FILTER_NONE,
        /// <summary>
        /// Gzip compressor.
        /// </summary>
        Gzip = tiledb_filter_type_t.TILEDB_FILTER_GZIP,
        /// <summary>
        /// Zstandard compressor.
        /// </summary>
        Zstandard = tiledb_filter_type_t.TILEDB_FILTER_ZSTD,
        /// <summary>
        /// LZ4 compressor.
        /// </summary>
        Lz4 = tiledb_filter_type_t.TILEDB_FILTER_LZ4,
        /// <summary>
        /// Run-length encoding compressor.
        /// </summary>
        RunLengthEncoding = tiledb_filter_type_t.TILEDB_FILTER_RLE,
        /// <summary>
        /// Bzip2 compressor.
        /// </summary>
        Bzip2 = tiledb_filter_type_t.TILEDB_FILTER_BZIP2,
        /// <summary>
        /// Double-delta compressor.
        /// </summary>
        DoubleDelta = tiledb_filter_type_t.TILEDB_FILTER_DOUBLE_DELTA,
        /// <summary>
        /// Bit width reduction filter.
        /// </summary>
        BitWidthReduction = tiledb_filter_type_t.TILEDB_FILTER_BIT_WIDTH_REDUCTION,
        /// <summary>
        /// Bit shuffle filter.
        /// </summary>
        BitShuffle = tiledb_filter_type_t.TILEDB_FILTER_BITSHUFFLE,
        /// <summary>
        /// Byte shuffle filter.
        /// </summary>
        ByteShuffle = tiledb_filter_type_t.TILEDB_FILTER_BYTESHUFFLE,
        /// <summary>
        /// Positive delta filter.
        /// </summary>
        PositiveDelta = tiledb_filter_type_t.TILEDB_FILTER_POSITIVE_DELTA,
        /// <summary>
        /// MD5 checksum filter.
        /// </summary>
        ChecksumMd5 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_MD5,
        /// <summary>
        /// SHA-256 checksum filter.
        /// </summary>
        ChecksumSha256 = tiledb_filter_type_t.TILEDB_FILTER_CHECKSUM_SHA256,
        /// <summary>
        /// Dictionary encoding filter.
        /// </summary>
        Dictionary = tiledb_filter_type_t.TILEDB_FILTER_DICTIONARY,
        /// <summary>
        /// Float scaling filter.
        /// </summary>
        ScaleFloat = tiledb_filter_type_t.TILEDB_FILTER_SCALE_FLOAT,
        /// <summary>
        /// XOR filter.
        /// </summary>
        Xor = tiledb_filter_type_t.TILEDB_FILTER_XOR,
        /// <summary>
        /// WebP filter.
        /// </summary>
        Webp = tiledb_filter_type_t.TILEDB_FILTER_WEBP,
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
        /// <summary>
        /// The WebP filter's compression quality.
        /// </summary>
        /// <remarks>
        /// Must be a <see cref="float"/> between 1 and 100.
        /// </remarks>
        WebpQuality = tiledb_filter_option_t.TILEDB_WEBP_QUALITY,
        /// <summary>
        /// The WebP filter's input format.
        /// </summary>
        /// <remarks>Must be a <see cref="byte"/>. The allowed values are listed below:
        /// <list type="table">
        /// <listheader>
        /// <term>Allowed value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item><term>0</term><description>Unspecified</description></item>
        /// <item><term>1</term><description>RGB</description></item>
        /// <item><term>2</term><description>BGR</description></item>
        /// <item><term>3</term><description>RGBA</description></item>
        /// <item><term>4</term><description>BGRA</description></item>
        /// </list>
        /// </remarks>
        WebpInputFormat = tiledb_filter_option_t.TILEDB_WEBP_INPUT_FORMAT,
        /// <summary>
        /// Whether the WebP filter should perform lossless compression.
        /// </summary>
        /// <remarks>
        /// Must be a <see cref="bool"/>.
        /// </remarks>
        WebpLossless = tiledb_filter_option_t.TILEDB_WEBP_LOSSLESS,
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

    /// <summary>
    /// Specifies the kind of data encryption in an <see cref="Array"/>.
    /// </summary>
    public enum EncryptionType : uint
    {
        /// <summary>
        /// Data are not encrypted.
        /// </summary>
        NoEncryption = tiledb_encryption_type_t.TILEDB_NO_ENCRYPTION,
        /// <summary>
        /// Data are encrypted using the AES-256 block cipher in Galois Counter Mode (GCM).
        /// </summary>
        Aes256Gcm = tiledb_encryption_type_t.TILEDB_AES_256_GCM,
        [Obsolete("Use NoEncryption instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_NO_ENCRYPTION = NoEncryption,
        [Obsolete("Use Aes256Gcm instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_AES_256_GCM = Aes256Gcm
    }

    /// <summary>
    /// Specifies the order objects are walked.
    /// </summary>
    public enum WalkOrderType : uint
    {
        /// <summary>
        /// The objects are walked in pre-order.
        /// </summary>
        PreOrder = tiledb_walk_order_t.TILEDB_PREORDER,
        /// <summary>
        /// The objects are walked in post-order.
        /// </summary>
        PostOrder = tiledb_walk_order_t.TILEDB_POSTORDER,
        [Obsolete("Use PreOrder instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_PREORDER = PreOrder,
        [Obsolete("Use PostOrder instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_POSTORDER = PostOrder
    }

    /// <summary>
    /// Specifies the VFS mode.
    /// </summary>
    public enum VfsMode : uint
    {
        /// <summary>
        /// Read mode.
        /// </summary>
        Read = tiledb_vfs_mode_t.TILEDB_VFS_READ,
        /// <summary>
        /// Write mode.
        /// </summary>
        Write = tiledb_vfs_mode_t.TILEDB_VFS_WRITE,
        /// <summary>
        /// Append mode.
        /// </summary>
        Append = tiledb_vfs_mode_t.TILEDB_VFS_APPEND,
        [Obsolete("Use Read instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_READ = Read,
        [Obsolete("Use Write instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_WRITE = Write,
        [Obsolete("Use Append instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_VFS_APPEND = Append
    }

    /// <summary>
    /// Specifies a MIME type.
    /// </summary>
    public enum MIMEType : uint
    {
        /// <summary>
        /// <c>application/pdf</c>
        /// </summary>
        Pdf = tiledb_mime_type_t.TILEDB_MIME_PDF,
        /// <summary>
        /// <c>image/tiff</c>
        /// </summary>
        Tiff = tiledb_mime_type_t.TILEDB_MIME_TIFF,
        /// <summary>
        /// Unspecified MIME type.
        /// </summary>
        AutoDetect = tiledb_mime_type_t.TILEDB_MIME_AUTODETECT,
        [Obsolete("Use Pdf instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_PDF = Pdf,
        [Obsolete("Use Tiff instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_TIFF = Tiff,
        [Obsolete("Use AutoDetect instead."), EditorBrowsable(EditorBrowsableState.Never)]
        TILEDB_MIME_AUTODETECT = AutoDetect
    }

    public static class Constants
    {
        /// <summary>
        /// Referenced by <see cref="TILEDB_VAR_NUM"/>, <see cref="Attribute.VariableSized"/>
        /// and <see cref="Dimension.VariableSized"/>.
        /// </summary>
        internal const uint VariableSizedImpl = uint.MaxValue;

        [Obsolete("Use Attribute.VariableSized or Dimension.VariableSized instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public const uint TILEDB_VAR_NUM = VariableSizedImpl;

        #region File Api
        public const string METADATA_SIZE_KEY = "file_size";
        public const string FILE_DIMENSION_NAME = "position";
        public const string FILE_ATTRIBUTE_NAME = "contents";
        public const string FILE_METADATA_MIME_TYPE_KEY = "mime";
        public const string FILE_METADATA_MIME_ENCODING_KEY = "mime_encoding";
        public const string METADATA_ORIGINAL_FILE_NAME = "original_file_name";
        #endregion File Api
    }

    /// <summary>
    /// Contains utility functions that operate on the library's enum types.
    /// </summary>
    public static unsafe class EnumUtil
    {
        /// <summary>
        /// Get string of QueryType.
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string ObjectTypeToStr(ObjectType objectType)
        {
            tiledb_object_t tiledb_object = (tiledb_object_t)objectType;
            sbyte* result;
            Methods.tiledb_object_type_to_str(tiledb_object, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string FileSystemTypeToStr(FileSystemType fileSystemType)
        {
            tiledb_filesystem_t tiledb_filesystem = (tiledb_filesystem_t)fileSystemType;
            sbyte* result;
            Methods.tiledb_filesystem_to_str(tiledb_filesystem, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string DataTypeToStr(DataType dataType)
        {
            tiledb_datatype_t tiledb_datatype = (tiledb_datatype_t)dataType;
            sbyte* result;
            Methods.tiledb_datatype_to_str(tiledb_datatype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string ArrayTypeToStr(ArrayType arrayType)
        {
            tiledb_array_type_t tiledb_arraytype = (tiledb_array_type_t)arrayType;
            sbyte* result;
            Methods.tiledb_array_type_to_str(tiledb_arraytype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string LayoutTypeToStr(LayoutType layoutType)
        {
            tiledb_layout_t tiledb_layout = (tiledb_layout_t)layoutType;
            sbyte* result;
            Methods.tiledb_layout_to_str(tiledb_layout, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string FilterTypeToStr(FilterType filterType)
        {
            tiledb_filter_type_t tiledb_filtertype = (tiledb_filter_type_t)filterType;
            sbyte* result;
            Methods.tiledb_filter_type_to_str(tiledb_filtertype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string FilterOptionToStr(FilterOption filterOption)
        {
            tiledb_filter_option_t tiledb_filteroption = (tiledb_filter_option_t)filterOption;
            sbyte* result;
            Methods.tiledb_filter_option_to_str(tiledb_filteroption, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string EncryptionTypeToStr(EncryptionType encryptionType)
        {
            tiledb_encryption_type_t tiledb_encryptiontype = (tiledb_encryption_type_t)encryptionType;
            sbyte* result;
            Methods.tiledb_encryption_type_to_str(tiledb_encryptiontype, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string QueryStatusToStr(QueryStatus queryStatus)
        {
            tiledb_query_status_t tiledb_querystatus = (tiledb_query_status_t)queryStatus;
            sbyte* result;
            Methods.tiledb_query_status_to_str(tiledb_querystatus, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string WalkOrderTypeToStr(WalkOrderType walkOrderType)
        {
            tiledb_walk_order_t tiledb_walkorder = (tiledb_walk_order_t)walkOrderType;
            sbyte* result;
            Methods.tiledb_walk_order_to_str(tiledb_walkorder, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string VfsModeToStr(VfsMode vfsMode)
        {
            tiledb_vfs_mode_t tiledb_vfsmode = (tiledb_vfs_mode_t)vfsMode;
            sbyte* result;
            Methods.tiledb_vfs_mode_to_str(tiledb_vfsmode, &result);
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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

        [Obsolete("This method will be removed in a future version of the library. Use TypeToDataType and the DataType enum instead.")]
        public static tiledb_datatype_t to_tiledb_datatype(Type t) =>
            (tiledb_datatype_t)TypeToDataType(t);

        /// <summary>
        /// Converts a <see cref="Type"/> to a <see cref="DataType"/> enum value.
        /// </summary>
        /// <param name="t">The type to convert.</param>
        /// <exception cref="NotSupportedException"><paramref name="t"/> is unsupported.</exception>
        public static DataType TypeToDataType(Type t)
        {
            if (t == typeof(int))
            {
                return DataType.Int32;
            }
            else if (t == typeof(long))
            {
                return DataType.Int64;
            }
            else if (t == typeof(float))
            {
                return DataType.Float32;
            }
            else if (t == typeof(double))
            {
                return DataType.Float64;
            }
            else if (t == typeof(byte))
            {
                return DataType.UInt8;
            }
            else if (t == typeof(sbyte))
            {
                return DataType.Int8;
            }
            else if (t == typeof(short))
            {
                return DataType.Int16;
            }
            else if (t == typeof(ushort))
            {
                return DataType.UInt16;
            }
            else if (t == typeof(uint))
            {
                return DataType.UInt32;
            }
            else if (t == typeof(ulong))
            {
                return DataType.UInt64;
            }
            else if (t == typeof(string))
            {
                return DataType.StringAscii;
            }
            else if (t == typeof(bool))
            {
                return DataType.Boolean;
            }
            else
            {
                ThrowHelpers.ThrowTypeNotSupported();
                return default; // unreachable
            }
        }

        public static Type DataTypeToType(DataType datatype)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            switch (datatype)
            {
                case DataType.Char:
                    return typeof(sbyte);
                case DataType.DateTimeAttosecond:
                case DataType.DateTimeDay:
                case DataType.DateTimeFemtosecond:
                case DataType.DateTimeHour:
                case DataType.DateTimeMinute:
                case DataType.DateTimeMonth:
                case DataType.DateTimeMillisecond:
                case DataType.DateTimeNanosecond:
                case DataType.DateTimePicosecond:
                case DataType.DateTimeSecond:
                case DataType.DateTimeMicrosecond:
                case DataType.DateTimeWeek:
                case DataType.DateTimeYear:
                    return typeof(long);
                case DataType.Float32:
                    return typeof(float);
                case DataType.Float64:
                    return typeof(double);
                case DataType.Int16:
                    return typeof(short);
                case DataType.Int32:
                    return typeof(int);
                case DataType.Int64:
                    return typeof(long);
                case DataType.Int8:
                    return typeof(sbyte);
                case DataType.StringAscii:
                case DataType.StringUcs2:
                case DataType.StringUcs4:
                case DataType.StringUtf16:
                case DataType.StringUtf32:
                case DataType.StringUtf8:
                    return typeof(sbyte);
                case DataType.TimeAttosecond:
                case DataType.TimeFemtosecond:
                case DataType.TimeHour:
                case DataType.TimeMinute:
                case DataType.TimeMillisecond:
                case DataType.TimeNanosecond:
                case DataType.TimePicosecond:
                case DataType.TimeSecond:
                case DataType.TimeMicrosecond:
                    return typeof(long);
                case DataType.UInt16:
                    return typeof(ushort);
                case DataType.UInt32:
                    return typeof(uint);
                case DataType.UInt64:
                    return typeof(ulong);
                case DataType.UInt8:
                    return typeof(byte);
                case DataType.Blob:
                    return typeof(byte);
                case DataType.Boolean:
                    return typeof(byte);
                default:
                    return typeof(byte);
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Obsolete("This method will be removed in a future version of the library. Use the overload that accepts the DataType enum instead.")]
        public static bool IsStringType(tiledb_datatype_t tiledbDatatype) =>
            IsStringType((DataType)tiledbDatatype);

        public static bool IsStringType(DataType datatype)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return datatype == DataType.StringAscii
                || datatype == DataType.StringUcs2
                || datatype == DataType.StringUcs4
                || datatype == DataType.StringUtf16
                || datatype == DataType.StringUtf32
                || datatype == DataType.StringUtf8;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Obsolete("This method will be removed in a future version of the library. Use DataTypeSize and the DataType enum instead.")]
        public static ulong TileDBDataTypeSize(tiledb_datatype_t tiledbDatatype) =>
            DataTypeSize((DataType)tiledbDatatype);

        public static ulong DataTypeSize(DataType datatype) =>
            Methods.tiledb_datatype_size((tiledb_datatype_t)datatype);
    }
}
