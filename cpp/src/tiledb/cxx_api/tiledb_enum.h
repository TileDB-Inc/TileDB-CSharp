/*
 * The MIT License
 *
 * @copyright Copyright (c) 2017-2020 TileDB, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#pragma once
// clang-format is disabled on the first enum so that we can manually indent it
// properly.
// clang-format off
enum tiledb_query_type_t{ //#ifdef TILEDB_QUERY_TYPE_ENUM
    /** Read query */
    TILEDB_READ =  0, //    TILEDB_QUERY_TYPE_ENUM(READ) = 0,
    /** Write query */
    TILEDB_WRITE =  1, //    TILEDB_QUERY_TYPE_ENUM(WRITE) = 1,
};//#endif
// clang-format on

enum tiledb_object_t{ //#ifdef TILEDB_OBJECT_TYPE_ENUM
    /** Invalid object */
    TILEDB_INVALID =  0, //    TILEDB_OBJECT_TYPE_ENUM(INVALID) = 0,
    /** Group object */
    TILEDB_GROUP =  1, //    TILEDB_OBJECT_TYPE_ENUM(GROUP) = 1,
    /** Array object */
    TILEDB_ARRAY =  2, //    TILEDB_OBJECT_TYPE_ENUM(ARRAY) = 2,
// We remove 3 (KEY_VALUE), so we should probably reserve it
};//#endif

enum tiledb_filesystem_t{ //#ifdef TILEDB_FILESYSTEM_ENUM
    /** HDFS filesystem */
    TILEDB_HDFS =  0, //    TILEDB_FILESYSTEM_ENUM(HDFS) = 0,
    /** S3 filesystem */
    TILEDB_S3 =  1, //    TILEDB_FILESYSTEM_ENUM(S3) = 1,
    /** Azure filesystem */
    TILEDB_AZURE =  2, //    TILEDB_FILESYSTEM_ENUM(AZURE) = 2,
    /** GCS filesystem */
    TILEDB_GCS =  3, //    TILEDB_FILESYSTEM_ENUM(GCS) = 3,
};//#endif

enum tiledb_datatype_t{ //#ifdef TILEDB_DATATYPE_ENUM
    /** 32-bit signed integer */
    TILEDB_INT32 =  0, //    TILEDB_DATATYPE_ENUM(INT32) = 0,
    /** 64-bit signed integer */
    TILEDB_INT64 =  1, //    TILEDB_DATATYPE_ENUM(INT64) = 1,
    /** 32-bit floating point value */
    TILEDB_FLOAT32 =  2, //    TILEDB_DATATYPE_ENUM(FLOAT32) = 2,
    /** 64-bit floating point value */
    TILEDB_FLOAT64 =  3, //    TILEDB_DATATYPE_ENUM(FLOAT64) = 3,
    /** Character */
    TILEDB_CHAR =  4, //    TILEDB_DATATYPE_ENUM(CHAR) = 4,
    /** 8-bit signed integer */
    TILEDB_INT8 =  5, //    TILEDB_DATATYPE_ENUM(INT8) = 5,
    /** 8-bit unsigned integer */
    TILEDB_UINT8 =  6, //    TILEDB_DATATYPE_ENUM(UINT8) = 6,
    /** 16-bit signed integer */
    TILEDB_INT16 =  7, //    TILEDB_DATATYPE_ENUM(INT16) = 7,
    /** 16-bit unsigned integer */
    TILEDB_UINT16 =  8, //    TILEDB_DATATYPE_ENUM(UINT16) = 8,
    /** 32-bit unsigned integer */
    TILEDB_UINT32 =  9, //    TILEDB_DATATYPE_ENUM(UINT32) = 9,
    /** 64-bit unsigned integer */
    TILEDB_UINT64 =  10, //    TILEDB_DATATYPE_ENUM(UINT64) = 10,
    /** ASCII string */
    TILEDB_STRING_ASCII =  11, //    TILEDB_DATATYPE_ENUM(STRING_ASCII) = 11,
    /** UTF-8 string */
    TILEDB_STRING_UTF8 =  12, //    TILEDB_DATATYPE_ENUM(STRING_UTF8) = 12,
    /** UTF-16 string */
    TILEDB_STRING_UTF16 =  13, //    TILEDB_DATATYPE_ENUM(STRING_UTF16) = 13,
    /** UTF-32 string */
    TILEDB_STRING_UTF32 =  14, //    TILEDB_DATATYPE_ENUM(STRING_UTF32) = 14,
    /** UCS2 string */
    TILEDB_STRING_UCS2 =  15, //    TILEDB_DATATYPE_ENUM(STRING_UCS2) = 15,
    /** UCS4 string */
    TILEDB_STRING_UCS4 =  16, //    TILEDB_DATATYPE_ENUM(STRING_UCS4) = 16,
    /** This can be any datatype. Must store (type tag, value) pairs. */
    TILEDB_ANY =  17, //    TILEDB_DATATYPE_ENUM(ANY) = 17,
    /** Datetime with year resolution */
    TILEDB_DATETIME_YEAR =  18, //    TILEDB_DATATYPE_ENUM(DATETIME_YEAR) = 18,
    /** Datetime with month resolution */
    TILEDB_DATETIME_MONTH =  19, //    TILEDB_DATATYPE_ENUM(DATETIME_MONTH) = 19,
    /** Datetime with week resolution */
    TILEDB_DATETIME_WEEK =  20, //    TILEDB_DATATYPE_ENUM(DATETIME_WEEK) = 20,
    /** Datetime with day resolution */
    TILEDB_DATETIME_DAY =  21, //    TILEDB_DATATYPE_ENUM(DATETIME_DAY) = 21,
    /** Datetime with hour resolution */
    TILEDB_DATETIME_HR =  22, //    TILEDB_DATATYPE_ENUM(DATETIME_HR) = 22,
    /** Datetime with minute resolution */
    TILEDB_DATETIME_MIN =  23, //    TILEDB_DATATYPE_ENUM(DATETIME_MIN) = 23,
    /** Datetime with second resolution */
    TILEDB_DATETIME_SEC =  24, //    TILEDB_DATATYPE_ENUM(DATETIME_SEC) = 24,
    /** Datetime with millisecond resolution */
    TILEDB_DATETIME_MS =  25, //    TILEDB_DATATYPE_ENUM(DATETIME_MS) = 25,
    /** Datetime with microsecond resolution */
    TILEDB_DATETIME_US =  26, //    TILEDB_DATATYPE_ENUM(DATETIME_US) = 26,
    /** Datetime with nanosecond resolution */
    TILEDB_DATETIME_NS =  27, //    TILEDB_DATATYPE_ENUM(DATETIME_NS) = 27,
    /** Datetime with picosecond resolution */
    TILEDB_DATETIME_PS =  28, //    TILEDB_DATATYPE_ENUM(DATETIME_PS) = 28,
    /** Datetime with femtosecond resolution */
    TILEDB_DATETIME_FS =  29, //    TILEDB_DATATYPE_ENUM(DATETIME_FS) = 29,
    /** Datetime with attosecond resolution */
    TILEDB_DATETIME_AS =  30, //    TILEDB_DATATYPE_ENUM(DATETIME_AS) = 30,
};//#endif

enum tiledb_array_type_t{ //#ifdef TILEDB_ARRAY_TYPE_ENUM
    /** Dense array */
    TILEDB_DENSE =  0, //    TILEDB_ARRAY_TYPE_ENUM(DENSE) = 0,
    /** Sparse array */
    TILEDB_SPARSE =  1, //    TILEDB_ARRAY_TYPE_ENUM(SPARSE) = 1,
};//#endif

enum tiledb_layout_t{ //#ifdef TILEDB_LAYOUT_ENUM
    /** Row-major layout */
    TILEDB_ROW_MAJOR =  0, //    TILEDB_LAYOUT_ENUM(ROW_MAJOR) = 0,
    /** Column-major layout */
    TILEDB_COL_MAJOR =  1, //    TILEDB_LAYOUT_ENUM(COL_MAJOR) = 1,
    /** Global-order layout */
    TILEDB_GLOBAL_ORDER =  2, //    TILEDB_LAYOUT_ENUM(GLOBAL_ORDER) = 2,
    /** Unordered layout */
    TILEDB_UNORDERED =  3, //    TILEDB_LAYOUT_ENUM(UNORDERED) = 3,
};//#endif

enum tiledb_filter_type_t{ //#ifdef TILEDB_FILTER_TYPE_ENUM
    /** No-op filter */
    TILEDB_FILTER_NONE =  0, //    TILEDB_FILTER_TYPE_ENUM(FILTER_NONE) = 0,
    /** Gzip compressor */
    TILEDB_FILTER_GZIP =  1, //    TILEDB_FILTER_TYPE_ENUM(FILTER_GZIP) = 1,
    /** Zstandard compressor */
    TILEDB_FILTER_ZSTD =  2, //    TILEDB_FILTER_TYPE_ENUM(FILTER_ZSTD) = 2,
    /** LZ4 compressor */
    TILEDB_FILTER_LZ4 =  3, //    TILEDB_FILTER_TYPE_ENUM(FILTER_LZ4) = 3,
    /** Run-length encoding compressor */
    TILEDB_FILTER_RLE =  4, //    TILEDB_FILTER_TYPE_ENUM(FILTER_RLE) = 4,
    /** Bzip2 compressor */
    TILEDB_FILTER_BZIP2 =  5, //    TILEDB_FILTER_TYPE_ENUM(FILTER_BZIP2) = 5,
    /** Double-delta compressor */
    TILEDB_FILTER_DOUBLE_DELTA =  6, //    TILEDB_FILTER_TYPE_ENUM(FILTER_DOUBLE_DELTA) = 6,
    /** Bit width reduction filter. */
    TILEDB_FILTER_BIT_WIDTH_REDUCTION =  7, //    TILEDB_FILTER_TYPE_ENUM(FILTER_BIT_WIDTH_REDUCTION) = 7,
    /** Bitshuffle filter. */
    TILEDB_FILTER_BITSHUFFLE =  8, //    TILEDB_FILTER_TYPE_ENUM(FILTER_BITSHUFFLE) = 8,
    /** Byteshuffle filter. */
    TILEDB_FILTER_BYTESHUFFLE =  9, //    TILEDB_FILTER_TYPE_ENUM(FILTER_BYTESHUFFLE) = 9,
    /** Positive-delta encoding filter. */
    TILEDB_FILTER_POSITIVE_DELTA =  10, //    TILEDB_FILTER_TYPE_ENUM(FILTER_POSITIVE_DELTA) = 10,
    /** MD5 checksum filter. Starts at 12 because 11 is used for encryption, see
       tiledb/sm/enums/filter_type.h */
    TILEDB_FILTER_CHECKSUM_MD5 =  12, //    TILEDB_FILTER_TYPE_ENUM(FILTER_CHECKSUM_MD5) = 12,
    /** SHA256 checksum filter. */
    TILEDB_FILTER_CHECKSUM_SHA256 =  13, //    TILEDB_FILTER_TYPE_ENUM(FILTER_CHECKSUM_SHA256) = 13,
};//#endif

enum tiledb_filter_option_t{ //#ifdef TILEDB_FILTER_OPTION_ENUM
    /** Compression level. Type: `int32_t`. */
    TILEDB_COMPRESSION_LEVEL =  0, //    TILEDB_FILTER_OPTION_ENUM(COMPRESSION_LEVEL) = 0,
    /** Max window length for bit width reduction. Type: `uint32_t`. */
    TILEDB_BIT_WIDTH_MAX_WINDOW =  1, //    TILEDB_FILTER_OPTION_ENUM(BIT_WIDTH_MAX_WINDOW) = 1,
    /** Max window length for positive-delta encoding. Type: `uint32_t`. */
    TILEDB_POSITIVE_DELTA_MAX_WINDOW =  2, //    TILEDB_FILTER_OPTION_ENUM(POSITIVE_DELTA_MAX_WINDOW) = 2,
};//#endif

enum tiledb_encryption_type_t{ //#ifdef TILEDB_ENCRYPTION_TYPE_ENUM
    /** No encryption. */
    TILEDB_NO_ENCRYPTION =  0, //    TILEDB_ENCRYPTION_TYPE_ENUM(NO_ENCRYPTION) = 0,
    /** AES-256-GCM encryption. */
    TILEDB_AES_256_GCM =  1, //    TILEDB_ENCRYPTION_TYPE_ENUM(AES_256_GCM) = 1,
};//#endif

enum tiledb_query_status_t{ //#ifdef TILEDB_QUERY_STATUS_ENUM
    /** Query failed */
    TILEDB_FAILED =  0, //    TILEDB_QUERY_STATUS_ENUM(FAILED) = 0,
    /** Query completed (all data has been read) */
    TILEDB_COMPLETED =  1, //    TILEDB_QUERY_STATUS_ENUM(COMPLETED) = 1,
    /** Query is in progress */
    TILEDB_INPROGRESS =  2, //    TILEDB_QUERY_STATUS_ENUM(INPROGRESS) = 2,
    /** Query completed (but not all data has been read) */
    TILEDB_INCOMPLETE =  3, //    TILEDB_QUERY_STATUS_ENUM(INCOMPLETE) = 3,
    /** Query not initialized.  */
    TILEDB_UNINITIALIZED =  4, //    TILEDB_QUERY_STATUS_ENUM(UNINITIALIZED) = 4,
};//#endif

enum tiledb_serialization_type_t{ //#ifdef TILEDB_SERIALIZATION_TYPE_ENUM
    /** Serialize to json */
    TILEDB_JSON = 0, //    TILEDB_SERIALIZATION_TYPE_ENUM(JSON),
    /** Serialize to capnp */
    TILEDB_CAPNP = 1, //    TILEDB_SERIALIZATION_TYPE_ENUM(CAPNP),
};//#endif

enum tiledb_walk_order_t{ //#ifdef TILEDB_WALK_ORDER_ENUM
    /** Pre-order traversal */
    TILEDB_PREORDER =  0, //    TILEDB_WALK_ORDER_ENUM(PREORDER) = 0,
    /** Post-order traversal */
    TILEDB_POSTORDER =  1, //    TILEDB_WALK_ORDER_ENUM(POSTORDER) = 1,
};//#endif

/** TileDB VFS mode */
enum tiledb_vfs_mode_t{ //#ifdef TILEDB_VFS_MODE_ENUM
    /** Read mode */
    TILEDB_VFS_READ =  0, //    TILEDB_VFS_MODE_ENUM(VFS_READ) = 0,
    /** Write mode */
    TILEDB_VFS_WRITE =  1, //    TILEDB_VFS_MODE_ENUM(VFS_WRITE) = 1,
    /** Append mode */
    TILEDB_VFS_APPEND =  2, //    TILEDB_VFS_MODE_ENUM(VFS_APPEND) = 2,
};//#endif
