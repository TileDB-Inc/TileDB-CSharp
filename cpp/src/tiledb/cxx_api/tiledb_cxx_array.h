/**
 * @file   tiledb_cpp_api_array.h
 *
 *
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
 *
 * @section DESCRIPTION
 *
 * This file declares the C++ API for TileDB array operations.
 */

#ifndef TILEDB_CPP_API_ARRAY_H
#define TILEDB_CPP_API_ARRAY_H

#include "json.h"
#include "tiledb_cxx_array_schema.h"
#include "tiledb_cxx_context.h"
#include "tiledb_cxx_deleter.h"
#include "tiledb_cxx_config.h"
#include "tiledb.h"
#include "tiledb_cxx_type.h"
#include "tiledb_cxx_enum.h"
#include "tiledb_cxx_string_util.h"

#include <map>
#include <unordered_map>
#include <vector>

namespace tiledb {

/**
 * Class representing a TileDB array object.
 *
 * @details
 * An Array object represents array data in TileDB at some persisted location,
 * e.g. on disk, in an S3 bucket, etc. Once an array has been opened for reading
 * or writing, interact with the data through Query objects.
 *
 * **Example:**
 *
 * @code{.cpp}
 * tiledb::Context ctx;
 *
 * // Create an ArraySchema, add attributes, domain, etc.
 * tiledb::ArraySchema schema(...);
 *
 * // Create empty array named "my_array" on persistent storage.
 * tiledb::Array::create("my_array", schema);
 * @endcode
 */
class Array {
 public:
  /**
   * @brief Constructor. This opens the array for the given query type. The
   * destructor calls the `close()` method.
   *
   * **Example:**
   *
   * @code{.cpp}
   * // Open the array for reading
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * @endcode
   *
   * @param ctx TileDB context.
   * @param array_uri The array URI.
   * @param query_type Query type to open the array for.
   */
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType query_type)
      : Array(ctx, array_uri, query_type, TILEDB_NO_ENCRYPTION, nullptr, 0) {
  }

   
  /**
   * @brief Constructor. This opens an encrypted array for the given query type.
   * The destructor calls the `close()` method.
   *
   * **Example:**
   *
   * @code{.cpp}
   * // Open the encrypted array for reading
   * tiledb::Context ctx;
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ,
   *    TILEDB_AES_256_GCM, key, sizeof(key));
   * @endcode
   *
   * @param ctx TileDB context.
   * @param array_uri The array URI.
   * @param querytype Query type to open the array for.
   * @param encryptiontype The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   */
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length)
      : ctx_(ctx)
      , schema_(ArraySchema(ctx, (tiledb_array_schema_t*)nullptr)) {
    tiledb_query_type_t query_type = (tiledb_query_type_t)querytype;
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    tiledb_ctx_t* c_ctx = ctx->ptr().get();
    tiledb_array_t* array;
    ctx->handle_error(tiledb_array_alloc(c_ctx, array_uri.c_str(), &array));
    array_ = std::shared_ptr<tiledb_array_t>(array, deleter_);
    ctx->handle_error(tiledb_array_open_with_key(
        c_ctx, array, query_type, encryption_type, encryption_key, key_length));

    tiledb_array_schema_t* array_schema;
    ctx->handle_error(tiledb_array_get_schema(c_ctx, array, &array_schema));
    schema_ = ArraySchema(ctx, array_schema);
  }

  // clang-format off
  /**
   * @copybrief Array::Array(const Context&,const std::string&,tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t)
   *
   * See @ref Array::Array(const Context&,const std::string&,tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t) "Array::Array"
   *
   * @param ctx TileDB context.
   * @param array_uri The array URI.
   * @param querytype Query type to open the array for.
   * @param encryptiontype The encryption type to use.
   * @param encryption_key The encryption key to use.
   */
  // clang-format on
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key)
      : Array(
            ctx,
            array_uri,
            querytype,
            encryptiontype,
            encryption_key.data(),
            (uint32_t)encryption_key.size()) {
  }

  /**
   * @brief Constructor. This opens the array for the given query type at the
   * given timestamp. The destructor calls the `close()` method.
   *
   * This constructor takes as input a
   * timestamp, representing time in milliseconds ellapsed since
   * 1970-01-01 00:00:00 +0000 (UTC). Opening the array at a
   * timestamp provides a view of the array with all writes/updates that
   * happened at or before `timestamp` (i.e., excluding those that
   * occurred after `timestamp`). This is useful to ensure
   * consistency at a potential distributed setting, where machines
   * need to operate on the same view of the array.
   *
   * **Example:**
   *
   * @code{.cpp}
   * // Open the array for reading
   * tiledb::Context ctx;
   * // Get some `timestamp` here in milliseconds
   * tiledb::Array array(
   *     ctx, "s3://bucket-name/array-name", TILEDB_READ, timestamp);
   * @endcode
   *
   * @param ctx TileDB context.
   * @param array_uri The array URI.
   * @param query_type Query type to open the array for.
   * @param timestamp The timestamp to open the array at.
   */
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType querytype,
      uint64_t timestamp)
      : Array(
            ctx,
            array_uri,
            querytype,
            TILEDB_NO_ENCRYPTION,
            nullptr,
            0,
            timestamp) {
  }

  // clang-format off
  /**
   * @copybrief Array::Array(const Context&,const std::string&,tiledb::QueryType,uint64_t)
   *
   * Same as @ref Array::Array(const Context&,const std::string&,tiledb::QueryType,uint64_t) "Array::Array"
   * but for encrypted arrays.
   *
   * **Example:**
   *
   * @code{.cpp}
   * // Open the encrypted array for reading
   * tiledb::Context ctx;
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * // Get some `timestamp` here in milliseconds
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ,
   *    TILEDB_AES_256_GCM, key, sizeof(key), timestamp);
   * @endcode
   *
   * @param ctx TileDB context.
   * @param array_uri The array URI.
   * @param querytype Query type to open the array for.
   * @param encryptiontype The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   * @param timestamp The timestamp to open the array at.
   */
  // clang-format on
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length,
      uint64_t timestamp)
      : ctx_(ctx)
      , schema_(ArraySchema(ctx, (tiledb_array_schema_t*)nullptr)) {
    tiledb_query_type_t query_type = tiledb_query_type_t(querytype);
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    tiledb_ctx_t* c_ctx = ctx->ptr().get();
    tiledb_array_t* array;
    ctx->handle_error(tiledb_array_alloc(c_ctx, array_uri.c_str(), &array));
    array_ = std::shared_ptr<tiledb_array_t>(array, deleter_);
    ctx->handle_error(tiledb_array_open_at_with_key(
        c_ctx,
        array,
        query_type,
        encryption_type,
        encryption_key,
        key_length,
        timestamp));

    tiledb_array_schema_t* array_schema;
    ctx->handle_error(tiledb_array_get_schema(c_ctx, array, &array_schema));
    schema_ = ArraySchema(ctx, array_schema);
  }

  // clang-format off
  /**
   * @copybrief Array::Array(const Context&,const std::string&,tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t,uint64_t)
   *
   * See @ref Array::Array(const Context&,const std::string&,tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t,uint64_t) "Array::Array"
   */
  // clang-format on
  Array(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& array_uri,
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key,
      uint64_t timestamp)
      : Array(
            ctx,
            array_uri,
            querytype,
            encryptiontype,
            encryption_key.data(),
            (uint32_t)encryption_key.size(),
            timestamp) {
  }

  ///**
  // * Constructor. Creates a TileDB Array instance wrapping the given pointer.
  // * @param ctx tiledb::Context
  // * @param own=true If false, disables underlying cleanup upon destruction.
  // * @throws TileDBError if construction fails
  // */
  //Array(const std::shared_ptr<tiledb::Context>& ctx, tiledb_array_t* carray, bool own = true)
  //    : ctx_(ctx)
  //    , schema_(ArraySchema(ctx, (tiledb_array_schema_t*)nullptr)) {
  //  if (carray == nullptr)
  //    throw TileDBError(
  //        "[TileDB::C++API] Error: Failed to create Array from null pointer");

  //  tiledb_ctx_t* c_ctx = ctx->ptr().get();

  //  tiledb_array_schema_t* array_schema;
  //  ctx->handle_error(tiledb_array_get_schema(c_ctx, carray, &array_schema));
  //  schema_ = ArraySchema(ctx, array_schema);

  //  array_ = std::shared_ptr<tiledb_array_t>(carray, [own](tiledb_array_t* p) {
  //    if (own) {
  //      tiledb_array_free(&p);
  //    }
  //  });
  //}

  Array(const tiledb::Array&) = default;
  Array(tiledb::Array&&) = default;
  tiledb::Array& operator=(const tiledb::Array&) = default;
  tiledb::Array& operator=(tiledb::Array&&) = default;

  /** Destructor; calls `close()`. */
  ~Array() {
    close();
  }

  /** Checks if the array is open. */
  bool is_open() const {
	  tiledb_ctx_t* c_ctx = ctx_->ptr().get(); // 
    int open = 0;
    ctx_->handle_error(
        tiledb_array_is_open(c_ctx, array_.get(), &open));
    return bool(open);
  }

  /** Returns the array URI. */
  std::string uri() const {
	tiledb_ctx_t* c_ctx = ctx_->ptr().get(); // 
    const char* uri = nullptr;
    ctx_->handle_error(tiledb_array_get_uri(c_ctx, array_.get(), &uri));
    return std::string(uri);
  }

  /** Get the ArraySchema for the array. **/
  tiledb::ArraySchema schema() const {
	tiledb_ctx_t* c_ctx = ctx_->ptr().get(); // 
    tiledb_array_schema_t* schema;
    ctx_->handle_error(
        tiledb_array_get_schema(c_ctx, array_.get(), &schema));
    return tiledb::ArraySchema(ctx_, schema);
  }

  /** Returns a shared pointer to the C TileDB array object. */
  std::shared_ptr<tiledb_array_t> ptr() const {
    return array_;
  }

  /**
   * @brief Opens the array. The array is opened using a query type as input.
   *
   * This is to indicate that queries created for this `Array`
   * object will inherit the query type. In other words, `Array`
   * objects are opened to receive only one type of queries.
   * They can always be closed and be re-opened with another query type.
   * Also there may be many different `Array`
   * objects created and opened with different query types. For
   * instance, one may create and open an array object `array_read` for
   * reads and another one `array_write` for writes, and interleave
   * creation and submission of queries for both these array objects.
   *
   * **Example:**
   * @code{.cpp}
   * // Open the array for writing
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_WRITE);
   * // Close and open again for reading.
   * array.close();
   * array.open(TILEDB_READ);
   * @endcode
   *
   * @param querytype The type of queries the array object will be receiving.
   * @throws TileDBError if the array is already open or other error occurred.
   */
  void open(tiledb::QueryType querytype) {
    open(querytype, TILEDB_NO_ENCRYPTION, nullptr, 0);
  }

  /**
   * @brief Opens the array, for encrypted arrays.
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * // Open the encrypted array for writing
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_WRITE,
   *    TILEDB_AES_256_GCM, key, sizeof(key));
   * // Close and open again for reading.
   * array.close();
   * array.open(TILEDB_READ, TILEDB_AES_256_GCM, key, sizeof(key));
   * @endcode
   *
   * @param querytype The type of queries the array object will be receiving.
   * @param encryptiontype The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   */
  void open(
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length) {
    tiledb_query_type_t query_type = tiledb_query_type_t(querytype);
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_open_with_key(
        c_ctx,
        array_.get(),
        query_type,
        encryption_type,
        encryption_key,
        key_length));
    tiledb_array_schema_t* array_schema;
    ctx_->handle_error(
        tiledb_array_get_schema(c_ctx, array_.get(), &array_schema));
    schema_ = tiledb::ArraySchema(ctx_, array_schema);
  }

  // clang-format off
  /**
   * @copybrief Array::open(tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t)
   *
   * See @ref Array::open(tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t) "Array::open"
   */
  // clang-format on
  void open(
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key) {
    open(
        querytype,
        encryptiontype,
        encryption_key.data(),
        (uint32_t)encryption_key.size());
  }

  /**
   * @brief Opens the array for a query type, at the given timestamp.
   *
   * This function takes as input a
   * timestamp, representing time in milliseconds ellapsed since
   * 1970-01-01 00:00:00 +0000 (UTC). Opening the array at a
   * timestamp provides a view of the array with all writes/updates that
   * happened at or before `timestamp` (i.e., excluding those that
   * occurred after `timestamp`). This is useful to ensure
   * consistency at a potential distributed setting, where machines
   * need to operate on the same view of the array.
   *
   * **Example:**
   * @code{.cpp}
   * // Open the array for writing
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_WRITE);
   * // Close and open again for reading.
   * array.close();
   * // Get some `timestamp` in milliseconds here
   * array.open(TILEDB_READ, timestamp);
   * @endcode
   *
   * @param querytype The type of queries the array object will be receiving.
   * @param timestamp The timestamp to open the array at.
   * @throws TileDBError if the array is already open or other error occurred.
   */
  void open(tiledb::QueryType querytype, uint64_t timestamp) {
    open(querytype, TILEDB_NO_ENCRYPTION, nullptr, 0, timestamp);
  }

  /**
   * @copybrief Array::open(tiledb::QueryType,uint64_t)
   *
   * Same as @ref Array::open(tiledb::QueryType,uint64_t) "Array::open"
   * but for encrypted arrays.
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * // Open the encrypted array for writing
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_WRITE,
   *    TILEDB_AES_256_GCM, key, sizeof(key));
   * // Close and open again for reading.
   * array.close();
   * // Get some `timestamp` in milliseconds here
   * array.open(TILEDB_READ, TILEDB_AES_256_GCM, key, sizeof(key), timestamp);
   * @endcode
   *
   * @param query_type The type of queries the array object will be receiving.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   * @param timestamp The timestamp to open the array at.
   */
  void open(
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length,
      uint64_t timestamp) {
    tiledb_query_type_t query_type = tiledb_query_type_t(querytype);
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype; 
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_open_at_with_key(
        c_ctx,
        array_.get(),
        query_type,
        encryption_type,
        encryption_key,
        key_length,
        timestamp));
    tiledb_array_schema_t* array_schema;
    ctx_->handle_error(
        tiledb_array_get_schema(c_ctx, array_.get(), &array_schema));
    schema_ = tiledb::ArraySchema(ctx_, array_schema);
  }

  // clang-format off
  /**
   * @copybrief Array::open(tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t,uint64_t)
   *
   * See @ref Array::open(tiledb::QueryType,tiledb::EncryptionType,const void*,uint32_t,uint64_t) "Array::open"
   */
  // clang-format on
  void open(
      tiledb::QueryType querytype,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key,
      uint64_t timestamp) {
    open(
        querytype,
        encryptiontype,
        encryption_key.data(),
        (uint32_t)encryption_key.size(),
        timestamp);
  }

  /**
   * Reopens the array (the array must be already open). This is useful
   * when the array got updated after it got opened and the `Array`
   * object got created. To sync-up with the updates, the user must either
   * close the array and open with `open()`, or just use
   * `reopen()` without closing. This function will be generally
   * faster than the former alternative.
   *
   * Note: reopening encrypted arrays does not require the encryption key.
   *
   * **Example:**
   * @code{.cpp}
   * // Open the array for reading
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * array.reopen();
   * @endcode
   *
   * @throws TileDBError if the array was not already open or other error
   * occurred.
   */
  void reopen() {
 
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_reopen(c_ctx, array_.get()));
    tiledb_array_schema_t* array_schema;
    ctx_->handle_error(
        tiledb_array_get_schema(c_ctx, array_.get(), &array_schema));
    schema_ = tiledb::ArraySchema(ctx_, array_schema);
  }

  /**
   * Reopens the array at a specific timestamp.
   *
   * **Example:**
   * @code{.cpp}
   * // Open the array for reading
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * uint64_t timestamp = tiledb_timestamp_now_ms();
   * array.reopen_at(timestamp);
   * @endcode
   *
   * @throws TileDBError if the array was not already open or other error
   * occurred.
   */
  void reopen_at(uint64_t timestamp) {
 
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_reopen_at(c_ctx, array_.get(), timestamp));
    tiledb_array_schema_t* array_schema;
    ctx_->handle_error(
        tiledb_array_get_schema(c_ctx, array_.get(), &array_schema));
    schema_ = tiledb::ArraySchema(ctx_, array_schema);
  } 

  /** Returns the timestamp at which the array was opened. */
  uint64_t timestamp() const {
	  tiledb_ctx_t* c_ctx = ctx_->ptr().get(); // 
    uint64_t timestamp;
    ctx_->handle_error(
        tiledb_array_get_timestamp(c_ctx, array_.get(), &timestamp));
    return timestamp;
  }

  /**
   * Closes the array. The destructor calls this automatically.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * array.close();
   * @endcode
   */
  void close() {
	  tiledb_ctx_t* c_ctx = ctx_->ptr().get(); // 
    ctx_->handle_error(tiledb_array_close(c_ctx, array_.get()));
  }

  /**
   * @brief Consolidates the fragments of an array into a single fragment.
   *
   * You must first finalize all queries to the array before consolidation can
   * begin (as consolidation temporarily acquires an exclusive lock on the
   * array).
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Array::consolidate(ctx, "s3://bucket-name/array-name");
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array to be consolidated.
   * @param config Configuration parameters for the consolidation.
   */
  static void consolidate(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::Config* const config = nullptr) {
    consolidate(ctx, uri, TILEDB_NO_ENCRYPTION, nullptr, 0, config);
  }

  /**
   * @brief Consolidates the fragments of an encrypted array into a single
   * fragment.
   *
   * You must first finalize all queries to the array before consolidation can
   * begin (as consolidation temporarily acquires an exclusive lock on the
   * array).
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * tiledb::Array::consolidate(
   *     ctx,
   *     "s3://bucket-name/array-name",
   *     TILEDB_AES_256_GCM,
   *     key,
   *     sizeof(key));
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array to be consolidated.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   * @param config Configuration parameters for the consolidation.
   */
  static void consolidate(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length,
      tiledb::Config* const config = nullptr) {
  tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;      
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(tiledb_array_consolidate_with_key(
        c_ctx,
        uri.c_str(),
        encryption_type,
        encryption_key,
        key_length,
        config ? config->ptr().get() : nullptr));
  }

  /**
   * Cleans up the array, such as consolidated fragments and array metadata.
   * Note that this will coarsen the granularity of time traveling (see docs
   * for more information).
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Array::vacuum(ctx, "s3://bucket-name/array-name");
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array to be vacuumed.
   * @param config Configuration parameters for the vacuuming.
   */
  static void vacuum(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::Config* const config = nullptr) {
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(tiledb_array_vacuum(
        c_ctx, uri.c_str(), config ? config->ptr().get() : nullptr));
  }

  // clang-format off
  /**
   * @copybrief Array::consolidate(const Context&,const std::string&,tiledb_encryption_type_t,const void*,uint32_t,const Config&)
   *
   * See @ref Array::consolidate(
   *     const Context&,
   *     const std::string&,
   *     tiledb_encryption_type_t,
   *     const void*,
   *     uint32_t,const Config&) "Array::consolidate"
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array to be consolidated.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param config Configuration parameters for the consolidation.
   */
  // clang-format on
  static void consolidate(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key,
      tiledb::Config* const config = nullptr) {
 
    return consolidate(
        ctx,
        uri,
        encryptiontype,
        encryption_key.data(),
        (uint32_t)encryption_key.size(),
        config);
  }

  /**
   * Creates a new TileDB array given an input schema.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Array::create("s3://bucket-name/array-name", schema);
   * @endcode
   *
   * @param uri URI where array will be created.
   * @param schema The array schema.
   */
  static void create(const std::string& uri, const std::shared_ptr<tiledb::ArraySchema>& schema) {
    create(uri, schema, TILEDB_NO_ENCRYPTION, nullptr, 0);
  }

  /**
   * Loads the array schema from an array.
   *
   * **Example:**
   * @code{.cpp}
   * auto schema = tiledb::Array::load_schema(ctx,
   * "s3://bucket-name/array-name");
   * @endcode
   *
   * @param ctx The TileDB context.
   * @param uri The array URI.
   * @return The loaded ArraySchema object.
   */
  static std::shared_ptr<tiledb::ArraySchema> load_schema(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri) {
    tiledb_array_schema_t* schema;
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(
        tiledb_array_schema_load(c_ctx, uri.c_str(), &schema));
    return std::shared_ptr<tiledb::ArraySchema>(new tiledb::ArraySchema(ctx, schema));
  }

  /**
   * Loads the array schema from an encrypted array.
   *
   * **Example:**
   * @code{.cpp}
   * auto schema = tiledb::Array::load_schema(ctx,
   * "s3://bucket-name/array-name", key_type, key, key_len);
   * @endcode
   *
   * @param ctx The TileDB context.
   * @param uri The array URI.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   * @return The loaded ArraySchema object.
   */
  static std::shared_ptr<tiledb::ArraySchema> load_schema(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length) {
  tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
  tiledb_array_schema_t* schema;
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(tiledb_array_schema_load_with_key(
        c_ctx,
        uri.c_str(),
        encryption_type,
        encryption_key,
        key_length,
        &schema));
    return std::shared_ptr<tiledb::ArraySchema>(new tiledb::ArraySchema(ctx, schema));
  }

  /**
   * @brief Creates a new encrypted TileDB array given an input schema.
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * tiledb::Array::create("s3://bucket-name/array-name", schema,
   *    TILEDB_AES_256_GCM, key, sizeof(key));
   * @endcode
   *
   * @param uri URI where array will be created.
   * @param schema The array schema.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   */
  static void create(
      const std::string& uri,
      const std::shared_ptr<tiledb::ArraySchema>& schema,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length) {
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    auto& ctx = schema->context();
    tiledb_ctx_t* c_ctx = schema->context()->ptr().get();
    ctx->handle_error(tiledb_array_schema_check(c_ctx, schema->ptr().get()));
    ctx->handle_error(tiledb_array_create_with_key(
        c_ctx,
        uri.c_str(),
        schema->ptr().get(),
        encryption_type,
        encryption_key,
        key_length));
  }

  // clang-format off
  /**
   * @copybrief Array::create(const std::string&,const ArraySchema&,tiledb_encryption_type_t,const void*,uint32_t)
   *
   * See @ref Array::create(const std::string&,const ArraySchema&,tiledb_encryption_type_t,const void*,uint32_t) "Array::create"
   *
   * @param uri URI where array will be created.
   * @param schema The array schema.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   */
  // clang-format on
  static void create(
      const std::string& uri,
      const std::shared_ptr<tiledb::ArraySchema>& schema,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key) {
    return create(
        uri,
        schema,
        encryptiontype,
        encryption_key.data(),
        (uint32_t)encryption_key.size());
  }

  /**
   * Gets the encryption type the given array was created with.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb_encryption_type_t enc_type;
   * tiledb::Array::encryption_type(ctx, "s3://bucket-name/array-name",
   *    &enc_type);
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array to be consolidated.
   * @param encryption_type Set to the encryption type of the array.
   */
  static tiledb::EncryptionType encryption_type(
      const std::shared_ptr<tiledb::Context>& ctx, const std::string& array_uri) {
    tiledb_encryption_type_t encryption_type;
    ctx->handle_error(tiledb_array_encryption_type(
        ctx->ptr().get(), array_uri.c_str(), &encryption_type));
    return (tiledb::EncryptionType)encryption_type;
  }

  /**
   * Retrieves the non-empty domain from the array. This is the union of the
   * non-empty domains of the array fragments.
   *
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * // Specify the domain type (example uint32_t)
   * auto non_empty = array.non_empty_domain<uint32_t>();
   * std::cout << "Dimension named " << non_empty[0].first << " has cells in ["
   *           << non_empty[0].second.first << ", " non_empty[0].second.second
   *           << "]" << std::endl;
   * @endcode
   *
   * @tparam T Domain datatype
   * @return Vector of dim names with a {lower, upper} pair. Inclusive.
   *         Empty vector if the array has no data.
   */
  template <typename T>
  std::vector<std::pair<std::string, std::pair<T, T>>> non_empty_domain() {
    impl::type_check<T>(schema_.domain().type());
    std::vector<std::pair<std::string, std::pair<T, T>>> ret;

	std::vector<std::string> dim_names = schema_.domain().dimension_names();// auto dims = schema_.domain().dimensions();
	std::vector<T> buf(dim_names.size() * 2);  //std::vector<T> buf(dims.size() * 2);
    int empty;

    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_non_empty_domain(
        ctx_->ptr().get(), array_.get(), buf.data(), &empty));

    if (empty)
      return ret;

    for (size_t i = 0; i < dim_names.size(); ++i) { //for (size_t i = 0; i < dims.size(); ++i) {
      auto domain = std::pair<T, T>(buf[i * 2], buf[(i * 2) + 1]);
	  ret.push_back(std::pair<std::string, std::pair<T, T>>(dim_names[i], domain));  //ret.push_back(std::pair<std::string, std::pair<T, T>>(dims[i].name(), domain));
    }

    return ret;
  }

  /**
   * Retrieves the non-empty domain json string information from the array. This is the union of the
   * non-empty domains of the array fragments.
   *
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * std::string non_empty_json = array.non_empty_domain_json_str();
   * std::cout << non_empty_json << std::endl;
   * @endcode
   *
   * @return json string representative of non-empty domain.
   */
  std::string non_empty_domain_json_str() {
    std::stringstream ss;

    ss <<"{";

    std::vector<std::string> dim_names = schema_.domain().dimension_names();
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int empty;
    int count=0;
    for (size_t i = 0; i < dim_names.size(); ++i) {
      std::string dim_name = dim_names[i];
      auto dim = schema_.domain().dimension(dim_name);

      tiledb::DataType dim_datatype = dim.type();
      switch (dim_datatype) {
      case DataType::TILEDB_INT8:
        {
          std::vector<int8_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_UINT8:
        {
          std::vector<uint8_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_INT16:
        {
          std::vector<int16_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_UINT16:
        {
          std::vector<uint16_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_INT32:
        {
          std::vector<int32_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_UINT32:
        {
          std::vector<uint32_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_INT64:
        {
          std::vector<int64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_UINT64:
        {
          std::vector<uint64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_FLOAT32:
        {
          std::vector<float> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case DataType::TILEDB_FLOAT64:
        {
          std::vector<double> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case TILEDB_DATETIME_YEAR:
      case TILEDB_DATETIME_MONTH:
      case TILEDB_DATETIME_WEEK:
      case TILEDB_DATETIME_DAY:
      case TILEDB_DATETIME_HR:
      case TILEDB_DATETIME_MIN:
      case TILEDB_DATETIME_SEC:
      case TILEDB_DATETIME_MS:
      case TILEDB_DATETIME_US:
      case TILEDB_DATETIME_NS:
      case TILEDB_DATETIME_PS:
      case TILEDB_DATETIME_FS:
      case TILEDB_DATETIME_AS:
        {
          std::vector<int64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(empty==0) {
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << buf[0] << "," << buf[1] << "]";
            ++count;
          }
        }
        break;
      case TILEDB_STRING_ASCII:
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
        {
          std::pair<std::string, std::string> result;

          // Get range sizes
          uint64_t start_size, end_size;
 
         ctx_->handle_error(tiledb_array_get_non_empty_domain_var_size_from_name(
          ctx_->ptr().get(), array_.get(), dim_name.c_str(),
          &start_size,&end_size,&empty));
          if (empty==0) 
          {
            // Get ranges
            result.first.resize(start_size);
            result.second.resize(end_size);
            ctx_->handle_error(tiledb_array_get_non_empty_domain_var_from_name(
              ctx_->ptr().get(),array_.get(),dim_name.c_str(),
              &(result.first[0]),&(result.second[0]),&empty));
            if(count>0)
            {
              ss <<",";
            }
            ss <<"\"" << dim_name <<"\":";
            ss <<"[" << result.first << "," << result.second << "]";
            ++count;  
          }
        }
        break;
      case TILEDB_ANY:
        // Not supported domain types
        throw TileDBError("Invalid Dim type");
    }//switch 


 
    }//for (size_t i = 0; i < dim_names.size(); ++i)

    ss << "}";

    return ss.str();
  }

  /**
   * Retrieves the non-empty domain string information[start,end] from the array. This is the union of the
   * non-empty domains of the array fragments.
   *
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * auto non_empty_domain_str_vector = array.non_empty_domain_str_vector_from_name("d1");
   * @endcode
   *
   * @param dim_name The dimension name.
   * @return map of string representative of non-empty domain.
   */
  std::vector<std::string> non_empty_domain_str_vector_from_name(const std::string& dim_name) {
    std::vector<std::string> ret;

    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int empty;
 
    auto dim = schema_.domain().dimension(dim_name);

    tiledb::DataType dim_datatype = dim.type();
    switch (dim_datatype) {
      case DataType::TILEDB_INT8:
        {
          std::vector<int8_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_UINT8:
        {
          std::vector<uint8_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_INT16:
        {
          std::vector<int16_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_UINT16:
        {
          std::vector<uint16_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_INT32:
        {
          std::vector<int32_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_UINT32:
        {
          std::vector<uint32_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_INT64:
        {
          std::vector<int64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_UINT64:
        {
          std::vector<uint64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_FLOAT32:
        {
          std::vector<float> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case DataType::TILEDB_FLOAT64:
        {
          std::vector<double> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case TILEDB_DATETIME_YEAR:
      case TILEDB_DATETIME_MONTH:
      case TILEDB_DATETIME_WEEK:
      case TILEDB_DATETIME_DAY:
      case TILEDB_DATETIME_HR:
      case TILEDB_DATETIME_MIN:
      case TILEDB_DATETIME_SEC:
      case TILEDB_DATETIME_MS:
      case TILEDB_DATETIME_US:
      case TILEDB_DATETIME_NS:
      case TILEDB_DATETIME_PS:
      case TILEDB_DATETIME_FS:
      case TILEDB_DATETIME_AS:
        {
          std::vector<int64_t> buf(2);
          ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
            c_ctx, array_.get(), dim_name.c_str(), buf.data(), &empty));
          if(!empty) {
            std::stringstream ss0;
            ss0 << buf[0];
            std::stringstream ss1;
            ss1 << buf[1];
            ret.push_back(ss0.str());
            ret.push_back(ss1.str());
          }
        }
        break;
      case TILEDB_STRING_ASCII:
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
        {
          std::pair<std::string, std::string> result;

          // Get range sizes
          uint64_t start_size, end_size;
 
         ctx_->handle_error(tiledb_array_get_non_empty_domain_var_size_from_name(
          ctx_->ptr().get(), array_.get(), dim_name.c_str(),
          &start_size,&end_size,&empty));
          if (!empty) 
          {
            // Get ranges
            result.first.resize(start_size);
            result.second.resize(end_size);
            ctx_->handle_error(tiledb_array_get_non_empty_domain_var_from_name(
              ctx_->ptr().get(),array_.get(),dim_name.c_str(),
              &(result.first[0]),&(result.second[0]),&empty));
            ret.push_back(result.first);
            ret.push_back(result.second); 
          }
        }
        break;
      case TILEDB_ANY:
        // Not supported domain types
        throw TileDBError("Invalid Dim type");
    }//switch 
 
   
    
    return ret;
  }


  /**
   * Retrieves the non-empty domain from the array on the given dimension.
   * This is the union of the non-empty domains of the array fragments.
   *
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * // Specify the dimension type (example uint32_t)
   * auto non_empty = array.non_empty_domain<uint32_t>(0);
   * @endcode
   *
   * @tparam T Dimension datatype
   * @param idx The dimension index.
   * @return The {lower, upper} pair of the non-empty domain (inclusive)
   *         on the input dimension.
   */
  template <typename T>
  std::pair<T, T> non_empty_domain(unsigned idx) {
    auto dim = schema_.domain().dimension(idx);
    impl::type_check<T>(dim.type());
    std::pair<T, T> ret;
    std::vector<T> buf(2);
    int empty;

    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_non_empty_domain_from_index(
        ctx_->ptr().get(), array_.get(), idx, buf.data(), &empty));

    if (empty)
      return ret;

    ret = std::pair<T, T>(buf[0], buf[1]);
    return ret;
  }

 

  /**
   * Retrieves the non-empty domain from the array on the given dimension.
   * This is the union of the non-empty domai:uint32_t>("d1");
   * @endcode
   *
   * @tparam T Dimension datatype
   * @param name The dimension name.
   * @return The {lower, upper} pair of the non-empty domain (inclusive)
   *         on the input dimension.
   */
  template <typename T>
  std::pair<T, T> non_empty_domain(const std::string& name) {
    auto dim = schema_.domain().dimension(name);
    impl::type_check<T>(dim.type());
    std::pair<T, T> ret;
    std::vector<T> buf(2);
    int empty;

    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_non_empty_domain_from_name(
        ctx_->ptr().get(), array_.get(), name.c_str(), buf.data(), &empty));

    if (empty)
      return ret;

    ret = std::pair<T, T>(buf[0], buf[1]);
    return ret;
  }

  /**
   * Retrieves the non-empty domain from the array on the given dimension.
   * This is the union of the non-empty domains of the array fragments.
   * Applicable only to var-sized dimensions.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * // Specify the dimension type (example uint32_t)
   * auto non_empty = array.non_empty_domain_var(0);
   * @endcode
   *
   * @param idx The dimension index.
   * @return The {lower, upper} pair of the non-empty domain (inclusive)
   *         on the input dimension.
   */
  std::pair<std::string, std::string> non_empty_domain_var(unsigned idx) {
    auto dim = schema_.domain().dimension(idx);
    impl::type_check<char>(dim.type());
    std::pair<std::string, std::string> ret;

    // Get range sizes
    uint64_t start_size, end_size;
    int empty;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_non_empty_domain_var_size_from_index(
        ctx_->ptr().get(), array_.get(), idx, &start_size, &end_size, &empty));

    if (empty)
      return ret;

    // Get ranges
    ret.first.resize(start_size);
    ret.second.resize(end_size);
    ctx_->handle_error(tiledb_array_get_non_empty_domain_var_from_index(
        ctx_->ptr().get(),
        array_.get(),
        idx,
        &(ret.first[0]),
        &(ret.second[0]),
        &empty));

    return ret;
  }

  /**
   * Retrieves the non-empty domain from the array on the given dimension.
   * This is the union of the non-empty domains of the array fragments.
   * Applicable only to var-sized dimensions.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * // Specify the dimension type (example uint32_t)
   * auto non_empty = array.non_empty_domain_var("d1");
   * @endcode
   *
   * @param name The dimension name.
   * @return The {lower, upper} pair of the non-empty domain (inclusive)
   *         on the input dimension.
   */
  std::pair<std::string, std::string> non_empty_domain_var(
      const std::string& name) {
    auto dim = schema_.domain().dimension(name);
    impl::type_check<char>(dim.type());
    std::pair<std::string, std::string> ret;

    // Get range sizes
    uint64_t start_size, end_size;
    int empty;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_non_empty_domain_var_size_from_name(
        ctx_->ptr().get(),
        array_.get(),
        name.c_str(),
        &start_size,
        &end_size,
        &empty));

    if (empty)
      return ret;

    // Get ranges
    ret.first.resize(start_size);
    ret.second.resize(end_size);
    ctx_->handle_error(tiledb_array_get_non_empty_domain_var_from_name(
        ctx_->ptr().get(),
        array_.get(),
        name.c_str(),
        &(ret.first[0]),
        &(ret.second[0]),
        &empty));

    return ret;
  }

  /**
   * Compute an upper bound on the buffer elements needed to read a subarray.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "s3://bucket-name/array-name", TILEDB_READ);
   * std::vector<int> subarray = {0, 2, 0, 2};
   * auto max_elements = array.max_buffer_elements(subarray);
   *
   * // For fixed-sized attributes, `.second` is the max number of elements
   * // that can be read for the attribute. Use it to size the vector.
   * std::vector<int> data_a1(max_elements["a1"].second);
   *
   * // In sparse reads, coords are also fixed-sized attributes.
   * std::vector<int> coords(max_elements[TILEDB_COORDS].second);
   *
   * // In variable attributes, e.g. std::string type, need two buffers,
   * // one for offsets and one for cell data.
   * std::vector<uint64_t> offsets_a1(max_elements["a2"].first);
   * std::vector<char> data_a1(max_elements["a2"].second);
   * @endcode
   *
   * @tparam T The domain datatype
   * @param subarray Targeted subarray.
   * @return A map of attribute name (including `TILEDB_COORDS`) to
   *     the maximum number of elements that can be read in the given subarray.
   *     For each attribute, a pair of numbers are returned. The first,
   *     for variable-length attributes, is the maximum number of offsets
   *     for that attribute in the given subarray. For fixed-length attributes
   *     and coordinates, the first is always 0. The second is the maximum
   *     number of elements for that attribute in the given subarray.
   */
  template <typename T>
 // TILEDB_DEPRECATED
      std::map<std::string, std::pair<uint64_t, uint64_t>> //std::unordered_map<std::string, std::pair<uint64_t, uint64_t>>
      max_buffer_elements(const std::vector<T>& subarray) {
    auto ctx = ctx_.get();
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    impl::type_check<T>(schema_.domain().type(), 1);

    // Handle attributes
    std::map<std::string, std::pair<uint64_t, uint64_t>> ret; //std::unordered_map<std::string, std::pair<uint64_t, uint64_t>> ret;
    //auto schema_attrs = schema_.attributes();
    std::map<std::string, tiledb::Attribute> schema_attrs;
    std::vector<std::string> attr_names=schema_.attribute_names();
    for(auto& attr_name: attr_names) {
      schema_attrs[attr_name]=schema_.attribute(attr_name);
    }


    uint64_t attr_size, type_size;

    for (const auto& a : schema_attrs) {
      auto var = a.second.cell_val_num() == TILEDB_VAR_NUM;
      auto name = a.second.name();
      type_size = tiledb_datatype_size((tiledb_datatype_t)(a.second.type()));

      if (var) {
        uint64_t size_off, size_val;
        ctx_->handle_error(tiledb_array_max_buffer_size_var(
            c_ctx,
            array_.get(),
            name.c_str(),
            subarray.data(),
            &size_off,
            &size_val));
        ret[a.first] = std::pair<uint64_t, uint64_t>(
            size_off / TILEDB_OFFSET_SIZE, size_val / type_size);
      } else {
        ctx_->handle_error(tiledb_array_max_buffer_size(
            c_ctx, array_.get(), name.c_str(), subarray.data(), &attr_size));
        ret[a.first] = std::pair<uint64_t, uint64_t>(0, attr_size / type_size);
      }
    }

    // Handle coordinates
    type_size = tiledb_datatype_size((tiledb_datatype_t)(schema_.domain().type()));
    ctx_->handle_error(tiledb_array_max_buffer_size(
        c_ctx, array_.get(), TILEDB_COORDS, subarray.data(), &attr_size));
    ret[TILEDB_COORDS] =
        std::pair<uint64_t, uint64_t>(0, attr_size / type_size);

    return ret;
  }

  /** Returns the query type the array was opened with. */
  tiledb::QueryType query_type() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_query_type_t query_type;
    ctx_->handle_error(tiledb_array_get_query_type(
        ctx_->ptr().get(), array_.get(), &query_type));
    return (tiledb::QueryType)query_type;
  }

  /**
   * @brief Consolidates the metadata of an array.
   *
   * You must first finalize all queries to the array before consolidation can
   * begin (as consolidation temporarily acquires an exclusive lock on the
   * array).
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Array::consolidate_metadata(ctx, "s3://bucket-name/array-name");
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array whose
   *     metadata will be consolidated.
   * @param config Configuration parameters for the consolidation.
   */
  static void consolidate_metadata(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::Config* const config = nullptr) {
    Config local_cfg;
    Config* config_aux = config;
    if (!config_aux) {
      config_aux = &local_cfg;
    }

    (*config)["sm.consolidation.mode"] = "array_meta";
    consolidate(ctx, uri, TILEDB_NO_ENCRYPTION, nullptr, 0, config_aux);
  }

  /**
   * @brief Consolidates the metadata of an encrypted array.
   *
   * You must first finalize all queries to the array before consolidation can
   * begin (as consolidation temporarily acquires an exclusive lock on the
   * array).
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * tiledb::Array::consolidate_metadata(
   *     ctx,
   *     "s3://bucket-name/array-name",
   *     TILEDB_AES_256_GCM,
   *     key,
   *     sizeof(key));
   * @endcode
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array whose
   *     metadata will be consolidated.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   * @param config Configuration parameters for the consolidation.
   */
  static void consolidate_metadata(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::EncryptionType encryptiontype,
      const void* encryption_key,
      uint32_t key_length,
      tiledb::Config* const config = nullptr) {
//    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    Config local_cfg;
    Config* config_aux = config;
    if (!config_aux) {
      config_aux = &local_cfg;
    }

    (*config)["sm.consolidation.mode"] = "array_meta";
    consolidate(
        ctx, uri, encryptiontype, encryption_key, key_length, config_aux);
  }

  // clang-format off
  /**
   * @copybrief Array::consolidate_metadata(const Context&, const std::string&, tiledb_encryption_type_t, const void*,uint32_t, const Config&)
   *
   * See @ref Array::consolidate_metadata(
   *     const Context&,
   *     const std::string&,
   *     tiledb_encryption_type_t,
   *     const void*,
   *     uint32_t,const Config&) "Array::consolidate_metadata"
   *
   * @param ctx TileDB context
   * @param array_uri The URI of the TileDB array whose
   *     metadata will be consolidated.
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param config Configuration parameters for the consolidation.
   */
  // clang-format on
  static void consolidate_metadata(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb::EncryptionType encryptiontype,
      const std::string& encryption_key,
      tiledb::Config* const config = nullptr) {
    tiledb_encryption_type_t encryption_type = (tiledb_encryption_type_t)encryptiontype;
    Config local_cfg;
    Config* config_aux = config;
    if (!config_aux) {
      config_aux = &local_cfg;
    }

    (*config)["sm.consolidation.mode"] = "array_meta";
    consolidate(
        ctx,
        uri,
        encryptiontype,
        encryption_key.data(),
        (uint32_t)encryption_key.size(),
        config_aux);
  }

  /**
   * It puts a metadata key-value item to an open array. The array must
   * be opened in WRITE mode, otherwise the function will error out.
   *
   * @param key The key of the metadata item to be added. UTF-8 encodings
   *     are acceptable.
   * @param value_type The datatype of the value.
   * @param value_num The value may consist of more than one items of the
   *     same datatype. This argument indicates the number of items in the
   *     value component of the metadata.
   * @param value The metadata value in binary form.
   *
   * @note The writes will take effect only upon closing the array.
   */
  void put_metadata(
      const std::string& key,
      tiledb::DataType valuetype,
      uint32_t value_num,
      const void* value) {
    tiledb_datatype_t value_type = (tiledb_datatype_t)valuetype;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get(); //tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_put_metadata(
        c_ctx, array_.get(), key.c_str(), value_type, value_num, value));
  }

  void put_metadata_by_json_str_for_key(const std::string& key, const std::string& jsonstr) {
    json::json j;
    std::string metadata_key = key;
    try {
        j = json::json::parse(jsonstr);
        if(j.contains("key")) {
          metadata_key = j["key"].get<std::string>();
        }
        if(j.contains("value_type") && j.contains("value") )  {
            int intdatatype = j["value_type"].get<int>();
            if(intdatatype>=0 && intdatatype<= (int)tiledb::DataType::TILEDB_DATETIME_AS && metadata_key.size()>0) {
              tiledb_datatype_t value_type = (tiledb_datatype_t)intdatatype;
              DataType datatype = (DataType)intdatatype;
              tiledb_ctx_t* c_ctx = ctx_->ptr().get();
              
              switch (datatype) {
               case DataType::TILEDB_INT8:
              {
                std::vector<int8_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<int8_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));
              }
              break;
             case DataType::TILEDB_UINT8:
            {
                 std::vector<uint8_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<uint8_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));          
            }
            break;
            case DataType::TILEDB_INT16:
           {
                 std::vector<int16_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<int16_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));
           }
           break;
          case DataType::TILEDB_UINT16:
          {
                 std::vector<uint16_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<uint16_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));
          }
          break;
         case DataType::TILEDB_INT32:
         {
                  std::vector<int32_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<int32_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));
        }
        break;
        case DataType::TILEDB_UINT32:
        {
                 std::vector<uint32_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<uint32_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));  
        }
        break;
        case DataType::TILEDB_INT64:
        {
                  std::vector<int64_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<int64_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));    
        }
        break;
        case DataType::TILEDB_UINT64:
        {
                  std::vector<uint64_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<uint64_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));       
        }
        break;
        case DataType::TILEDB_FLOAT32:
        {
                  std::vector<float> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<float>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));         
        }
        break;
        case DataType::TILEDB_FLOAT64:
        {
                   std::vector<double> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<double>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));      
        }
        break;
        case TILEDB_DATETIME_YEAR:
        case TILEDB_DATETIME_MONTH:
        case TILEDB_DATETIME_WEEK:
        case TILEDB_DATETIME_DAY:
        case TILEDB_DATETIME_HR:
        case TILEDB_DATETIME_MIN:
        case TILEDB_DATETIME_SEC:
        case TILEDB_DATETIME_MS:
        case TILEDB_DATETIME_US:
        case TILEDB_DATETIME_NS:
        case TILEDB_DATETIME_PS:
        case TILEDB_DATETIME_FS:
        case TILEDB_DATETIME_AS:
        {
                std::vector<int64_t> buff;
                for(auto& v: j["value"].items()) {
                  buff.push_back(v.value().get<int64_t>());
                }
                ctx_->handle_error(tiledb_array_put_metadata(
                  c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));        
        }
        break;
      case TILEDB_STRING_ASCII:
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
      {
        std::string buff = j["value"].get<std::string>();
        ctx_->handle_error(tiledb_array_put_metadata(
          c_ctx, array_.get(), metadata_key.c_str(), value_type, (uint32_t)buff.size(), buff.data()));
      }
      break;
      case TILEDB_ANY:
        // Not supported metadata types
        throw TileDBError("Invalid metadata type");
      }//switch 

              
            }//if
        }//if
        else {
          std::cout<<"json must have key, value_type and values for metadata. invalid jsonstr:" << jsonstr << std::endl;
        }
    }
    catch(json::json::parse_error& json_ex) {
      std::cout<<"caught json parse error at " << json_ex.byte << " for jsonstr: " << jsonstr << std::endl;
    }
    catch(...) {
      std::cout<<"caught unknown exception when parsing " << jsonstr << std::endl;
    }
    

  }//void put_metadata_by_json_str(const std::string& jsonstr) 

  void put_metadata_by_json_str(const std::string& jsonstr) {
    json::json j;
    try {
      j = json::json::parse(jsonstr);
      if(j.contains("metadata")){
        for(auto& item: j["metadata"].items()) {
          std::string item_jsonstr = item.value().dump();
          put_metadata_by_json_str_for_key("",item_jsonstr);
        }
      }
    } 
    catch(json::json::parse_error& json_ex) {
      std::cout<<"caught json parse error at " << json_ex.byte << " for jsonstr: " << jsonstr << std::endl;
    }
    catch(...) {
      std::cout<<"caught unknown exception when parsing " << jsonstr << std::endl;
    }     
  }// void put_metadata_by_json_str(const std::string& jsonstr)

  /**
   * It deletes a metadata key-value item from an open array. The array must
   * be opened in WRITE mode, otherwise the function will error out.
   *
   * @param key The key of the metadata item to be deleted.
   *
   * @note The writes will take effect only upon closing the array.
   *
   * @note If the key does not exist, this will take no effect
   *     (i.e., the function will not error out).
   */
  void delete_metadata(const std::string& key) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(
        tiledb_array_delete_metadata(c_ctx, array_.get(), key.c_str()));
  }

  /**
   * It gets a metadata key-value item from an open array. The array must
   * be opened in READ mode, otherwise the function will error out.
   *
   * @param key The key of the metadata item to be retrieved. UTF-8 encodings
   *     are acceptable.
   * @param valuetype The datatype of the value.
   * @param value_num The value may consist of more than one items of the
   *     same datatype. This argument indicates the number of items in the
   *     value component of the metadata. Keys with empty values are indicated
   *     by value_num == 1 and value == NULL.
   * @param value The metadata value in binary form.
   *
   * @note If the key does not exist, then `value` will be NULL.
   */
  void get_metadata(
      const std::string& key,
      tiledb::DataType* valuetype,
      uint32_t* value_num,
      const void** value) {
    tiledb_datatype_t value_type;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();// 
    ctx_->handle_error(tiledb_array_get_metadata(
        c_ctx, array_.get(), key.c_str(), &value_type, value_num, value));
    *valuetype = (tiledb::DataType)value_type;
  }

  /**
   * It gets a metadata key-value item from an open array. The array must
   * be opened in READ mode, otherwise the function will error out.
   *
   * @param key The key of the metadata item to be retrieved. UTF-8 encodings
   *     are acceptable.
   *
   * @return json string representation of metadata.
   */

 std::string get_metadata_json_str_for_key(const std::string& key) {
    std::stringstream ss;
    ss <<"{";
    
    ss <<"\"key\":\"" << key << "\"";

    tiledb_datatype_t value_type;
    uint32_t value_num;
    const void* value;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();// 
    ctx_->handle_error(tiledb_array_get_metadata(
        c_ctx, array_.get(), key.c_str(), &value_type, &value_num, &value));

    tiledb::DataType datatype = (tiledb::DataType)value_type;
    
    if(value_num == 0) {
        return "{}";
    }
    ss <<",\"value_type\":" << value_type;
    ss <<",\"value_num\":" << value_num;

    if (datatype == TILEDB_STRING_ASCII
      || datatype == TILEDB_CHAR
      || datatype == TILEDB_STRING_UTF8
      || datatype == TILEDB_STRING_UTF16
      || datatype == TILEDB_STRING_UTF32
      || datatype == TILEDB_STRING_UCS2
      || datatype == TILEDB_STRING_UCS4) {
      ss << ",\"value\":\"";
      std::string valuestr;
      valuestr.resize(value_num);
      std::memcpy((void*)valuestr.data(), value, value_num);
      ss << valuestr;
      ss << "\"";
    }
    else {
      ss << ",\"value\":[";

      for (uint32_t i = 0; i < value_num; ++i) {
        if (i > 0) {
          ss << ",";
        }
        switch (datatype) {
        case DataType::TILEDB_INT8:
        {
          ss << ((const int8_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT8:
        {
          ss << ((const uint8_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT16:
        {
          ss << ((const int16_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT16:
        {
          ss << ((const int16_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT32:
        {
          ss << ((const int32_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT32:
        {
          ss << ((const uint32_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT64:
        {
          ss << ((const int64_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT64:
        {
          ss << ((const uint64_t*)value)[i];
        }
        break;
        case DataType::TILEDB_FLOAT32:
        {
          ss << ((const float*)value)[i];
        }
        break;
        case DataType::TILEDB_FLOAT64:
        {
          ss << ((const double*)value)[i];
        }
        break;
        case TILEDB_DATETIME_YEAR:
        case TILEDB_DATETIME_MONTH:
        case TILEDB_DATETIME_WEEK:
        case TILEDB_DATETIME_DAY:
        case TILEDB_DATETIME_HR:
        case TILEDB_DATETIME_MIN:
        case TILEDB_DATETIME_SEC:
        case TILEDB_DATETIME_MS:
        case TILEDB_DATETIME_US:
        case TILEDB_DATETIME_NS:
        case TILEDB_DATETIME_PS:
        case TILEDB_DATETIME_FS:
        case TILEDB_DATETIME_AS:
        {
          ss << ((const int64_t*)value)[i];
        }
        break;
        case TILEDB_STRING_ASCII:
        case TILEDB_CHAR:
        case TILEDB_STRING_UTF8:
        case TILEDB_STRING_UTF16:
        case TILEDB_STRING_UTF32:
        case TILEDB_STRING_UCS2:
        case TILEDB_STRING_UCS4:
        {
        }
        break;
        case TILEDB_ANY:
          // Not supported metadata types
          throw TileDBError("Invalid metadata type");
        }//switch 


      }//for


      ss << "]";

    }//esle
    
    ss <<"}";
    return ss.str();
  }

  /**
   * Checks if key exists in metadata from an open array. The array must
   * be opened in READ mode, otherwise the function will error out.
   *
   * @param key The key of the metadata item to be retrieved. UTF-8 encodings
   *     are acceptable.
   * @param valuetype The datatype of the value associated with the key (if
   * any).
   * @return true if the key exists, else false.
   * @note If the key does not exist, then `value_type` will not be modified.
   */
  bool has_metadata(const std::string& key, tiledb::DataType valuetype) {
 //   tiledb_ctx_t* c_ctx = ctx_->ptr().get();
  tiledb_datatype_t value_type = (tiledb_datatype_t)valuetype; 
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int32_t has_key;
	tiledb_datatype_t datatype = value_type;
    ctx_->handle_error(tiledb_array_has_metadata_key(
        c_ctx, array_.get(), key.c_str(), &datatype, &has_key));
    return has_key == 1;
  }

  /**
   * Returns then number of metadata items in an open array. The array must
   * be opened in READ mode, otherwise the function will error out.
   */
  uint64_t metadata_num() const {
    uint64_t num;
 
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();//   
    ctx_->handle_error(tiledb_array_get_metadata_num(c_ctx, array_.get(), &num));
    return num;
  }

  /**
   * It gets a metadata item from an open array using an index.
   * The array must be opened in READ mode, otherwise the function will
   * error out.
   *
   * @param index The index used to get the metadata.
   * @param key The metadata key.
   * @param valuetype The datatype of the value.
   * @param value_num The value may consist of more than one items of the
   *     same datatype. This argument indicates the number of items in the
   *     value component of the metadata. Keys with empty values are indicated
   *     by value_num == 1 and value == NULL.
   * @param value The metadata value in binary form.
   */
  void get_metadata_from_index(
      uint64_t index,
      std::string* key,
      tiledb::DataType* valuetype,
      uint32_t* value_num,
      const void** value) {
    const char* key_c;
    uint32_t key_len;
   tiledb_datatype_t value_type;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_get_metadata_from_index(
        c_ctx,
        array_.get(),
        index,
        &key_c,
        &key_len,
        &value_type,
        value_num,
        value));
    key->resize(key_len);
    std::memcpy((void*)key->data(), key_c, key_len);
    *valuetype = (tiledb::DataType)value_type;
  }
  
  std::string get_metadata_json_str_from_index(uint64_t index) {
    std::stringstream ss;
    ss <<"{";
    
    std::string key;
    const char* key_c;
    uint32_t key_len;
    tiledb_datatype_t value_type;
    uint32_t value_num;
    const void* value;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();// 
    ctx_->handle_error(tiledb_array_get_metadata_from_index(
        c_ctx,
        array_.get(),
        index,
        &key_c,
        &key_len,
        &value_type,
        &value_num,
        &value));

    key.resize(key_len);
    std::memcpy((void*)key.data(), key_c, key_len);
    tiledb::DataType datatype = (tiledb::DataType)value_type;
    
    if(value_num == 0) {
        return "{}";
    }
    
    ss <<"\"key\":\"" << key << "\"";
    ss << ",\"value_type\":" << value_type;
    ss <<",\"value_num\":" << value_num;

    if (datatype == TILEDB_STRING_ASCII
      || datatype == TILEDB_CHAR
      || datatype == TILEDB_STRING_UTF8
      || datatype == TILEDB_STRING_UTF16
      || datatype == TILEDB_STRING_UTF32
      || datatype == TILEDB_STRING_UCS2
      || datatype == TILEDB_STRING_UCS4) {
      ss << ",\"value\":\"";
      std::string valuestr;
      valuestr.resize(value_num);
      std::memcpy((void*)valuestr.data(), value, value_num);
      ss << valuestr;
      ss << "\"";
    }
    else {
      ss << ",\"value\":[";

      for (uint32_t i = 0; i < value_num; ++i) {
        if (i > 0) {
          ss << ",";
        }
        switch (datatype) {
        case DataType::TILEDB_INT8:
        {
          ss << ((const int8_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT8:
        {
          ss << ((const uint8_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT16:
        {
          ss << ((const int16_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT16:
        {
          ss << ((const int16_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT32:
        {
          ss << ((const int32_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT32:
        {
          ss << ((const uint32_t*)value)[i];
        }
        break;
        case DataType::TILEDB_INT64:
        {
          ss << ((const int64_t*)value)[i];
        }
        break;
        case DataType::TILEDB_UINT64:
        {
          ss << ((const uint64_t*)value)[i];
        }
        break;
        case DataType::TILEDB_FLOAT32:
        {
          ss << ((const float*)value)[i];
        }
        break;
        case DataType::TILEDB_FLOAT64:
        {
          ss << ((const double*)value)[i];
        }
        break;
        case TILEDB_DATETIME_YEAR:
        case TILEDB_DATETIME_MONTH:
        case TILEDB_DATETIME_WEEK:
        case TILEDB_DATETIME_DAY:
        case TILEDB_DATETIME_HR:
        case TILEDB_DATETIME_MIN:
        case TILEDB_DATETIME_SEC:
        case TILEDB_DATETIME_MS:
        case TILEDB_DATETIME_US:
        case TILEDB_DATETIME_NS:
        case TILEDB_DATETIME_PS:
        case TILEDB_DATETIME_FS:
        case TILEDB_DATETIME_AS:
        {
          ss << ((const int64_t*)value)[i];
        }
        break;
        case TILEDB_STRING_ASCII:
        case TILEDB_CHAR:
        case TILEDB_STRING_UTF8:
        case TILEDB_STRING_UTF16:
        case TILEDB_STRING_UTF32:
        case TILEDB_STRING_UCS2:
        case TILEDB_STRING_UCS4:
        {
        }
        break;
        case TILEDB_ANY:
          // Not supported metadata types
          throw TileDBError("Invalid metadata type");
        }//switch 



      }//for


      ss << "]";

    }//else
    
    ss <<"}";
    return ss.str();     
      
      
  }//std::string get_metadata_json_str_from_index
  
  std::string get_metadata_json_str() {
    uint64_t num =  metadata_num();
    if(num==0) {
      return "";
    }
    std::stringstream ss;
    ss <<"{";
    
    ss << "\"metadata_num\":" << num;
    ss <<",\"metadata\":[";
    
    for(uint64_t i = 0; i<num; ++i) {
      if(i>0) {
        ss <<",";
      }
      ss << get_metadata_json_str_from_index(i);
    }
    
    ss <<"]";
    
    ss << "}"; 

    return ss.str();
      
  }//std::string get_metadata_json_str()

 private:
  /* ********************************* */
  /*         PRIVATE ATTRIBUTES        */
  /* ********************************* */

  /** The TileDB context. */
  std::shared_ptr<Context> ctx_; //  

  /** Deleter wrapper. */
  impl::Deleter deleter_;

  /** Pointer to the TileDB C array object. */
  std::shared_ptr<tiledb_array_t> array_;

  /** The array schema. */
  ArraySchema schema_;

  /* ********************************* */
  /*          PRIVATE METHODS          */
  /* ********************************* */
};

}  // namespace tiledb

#endif  // TILEDB_CPP_API_ARRAY_H
