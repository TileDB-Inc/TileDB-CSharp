/**
 * @file   tiledb_cpp_api_object.h
 *
 * @author Ravi Gaddipati
 *
 * @section LICENSE
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
 * This file declares the C++ API for the TileDB Object object.
 */

#ifndef TILEDB_CPP_API_OBJECT_H
#define TILEDB_CPP_API_OBJECT_H

#include "tiledb_cxx_context.h"
#include "tiledb_enum.h"
#include "tiledb.h"
#include "tiledb_cxx_enum.h"

#include <functional>
#include <iostream>
#include <string>
#include <type_traits>

namespace tiledb {

/**
 * Represents a TileDB object: array, group, key-value (map), or none
 * (invalid).
 */
class Object {
 public:
  // /* ********************************* */
  // /*           TYPE DEFINITIONS        */
  // /* ********************************* */

  // /** The object type. */
  // enum class Type {
  //   /** TileDB array object. */
  //   Array,
  //   /** TileDB group object. */
  //   Group,
  //   /** Invalid or unknown object type. */
  //   Invalid
  // };

  /* ********************************* */
  /*     CONSTRUCTORS & DESTRUCTORS    */
  /* ********************************* */

  explicit Object(const tiledb::ObjectType& type, const std::string& uri = "")
      : type_(type)
      , uri_(uri) {
  }

  explicit Object(tiledb_object_t  type, const std::string& uri = "")
      : uri_(uri) {
    switch (type) {
      case TILEDB_ARRAY:
        type_ = tiledb::ObjectType::TILEDB_ARRAY;
        break;
      case TILEDB_GROUP:
        type_ = tiledb::ObjectType::TILEDB_GROUP;
        break;
      case TILEDB_INVALID:
        type_ = tiledb::ObjectType::TILEDB_INVALID;
        break;
    }
  }

  Object() = default;
  Object(const tiledb::Object&) = default;
  Object(tiledb::Object&&) = default;
  tiledb::Object& operator=(const tiledb::Object&) = default;
  tiledb::Object& operator=(tiledb::Object&&) = default;

  /* ********************************* */
  /*                API                */
  /* ********************************* */

  /**
   * Returns a string representation of the object, including its type
   * and URI.
   */
  std::string to_str() const {
    std::string ret = "Obj<";
    switch (type_) {
      case tiledb::ObjectType::TILEDB_ARRAY:
        ret += "ARRAY";
        break;
      case tiledb::ObjectType::TILEDB_GROUP:
        ret += "GROUP";
        break;
      case tiledb::ObjectType::TILEDB_INVALID:
        ret += "INVALID";
        break;
    }
    ret += " \"" + uri_ + "\">";
    return ret;
  }

  /** Returns the object type. */
  tiledb::ObjectType type() const {
    return type_;
  }

  /** Returns the object URI. */
  std::string uri() const {
    return uri_;
  }

  /* ********************************* */
  /*          STATIC FUNCTIONS         */
  /* ********************************* */

  /**
   * Gets an Object object that encapsulates the object type of the given path.
   *
   * @param ctx The TileDB context
   * @param uri The path to the object.
   * @return An object that contains the type along with the URI.
   */
  static tiledb::Object object(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri) {
    tiledb_object_t type;
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(tiledb_object_type(c_ctx, uri.c_str(), &type));
    Object ret(type, uri);
    return ret;
  }

  /**
   * Deletes a TileDB object at the given URI from disk/persistent storage.
   *
   * @param ctx The TileDB context
   * @param uri The path to the object to be removed.
   */
  static void remove(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri) {
	  tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(tiledb_object_remove(c_ctx, uri.c_str()));
  }

  /**
   * Moves/renames a TileDB object.
   *
   * @param old_uri The path to the old object.
   * @param new_uri The path to the new object.
   */
  static void move(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& old_uri,
      const std::string& new_uri) {
	tiledb_ctx_t* c_ctx = ctx->ptr().get();
    ctx->handle_error(
        tiledb_object_move(c_ctx, old_uri.c_str(), new_uri.c_str()));
  }

 private:
  /* ********************************* */
  /*         PRIVATE ATTRIBUTES        */
  /* ********************************* */

  /** The object type. */
  ObjectType type_; // Type type_;

  /** The obkect uri. */
  std::string uri_;
};

/* ********************************* */
/*               MISC                */
/* ********************************* */

/** Writes object in string format to an output stream. */
inline std::ostream& operator<<(std::ostream& os, const tiledb::Object& obj) {
  os << obj.to_str();
  return os;
}

}  // namespace tiledb

#endif  // TILEDB_CPP_API_OBJECT_H
