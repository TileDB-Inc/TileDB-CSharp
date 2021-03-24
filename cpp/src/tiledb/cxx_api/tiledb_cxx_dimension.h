/**
 * @file   tiledb_cpp_api_dimension.h
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
 * This file declares the C++ API for the TileDB Dimension object.
 */

#ifndef TILEDB_CPP_API_DIMENSION_H
#define TILEDB_CPP_API_DIMENSION_H

#include "tiledb_cxx_context.h"
#include "tiledb_cxx_deleter.h"
#include "tiledb_cxx_exception.h"
#include "tiledb_cxx_object.h"
#include "tiledb_cxx_filter_list.h"
#include "tiledb.h"
#include "tiledb_cxx_type.h"

#include <functional>
#include <memory>
#include <sstream>

namespace tiledb {

/**
 * Describes one dimension of an Array. The dimension consists
 * of a type, lower and upper bound, and tile-extent describing
 * the memory ordering. Dimensions are added to a Domain.
 *
 * **Example:**
 * @code{.cpp}
 * tiledb::Context ctx;
 * tiledb::Domain domain(ctx);
 * // Create a dimension with inclusive domain [0,1000] and tile extent 100.
 * domain.add_dimension(Dimension::create<int32_t>(ctx, "d", {{0, 1000}}, 100));
 * @endcode
 **/
class Dimension {
 public:
  /* ********************************* */
  /*     CONSTRUCTORS & DESTRUCTORS    */
  /* ********************************* */

  Dimension(const tiledb::Context& ctx, tiledb_dimension_t* dim)
      : ctx_(ctx) {
    dim_ = std::shared_ptr<tiledb_dimension_t>(dim, deleter_);
  }

  Dimension(const tiledb::Dimension&) = default;
  Dimension(tiledb::Dimension&&) = default;
  tiledb::Dimension& operator=(const tiledb::Dimension&) = default;
  tiledb::Dimension& operator=(tiledb::Dimension&&) = default;

  /* ********************************* */
  /*                API                */
  /* ********************************* */

  /**
   * Returns number of values of one cell on this dimension. For variable-sized
   * dimensions returns TILEDB_VAR_NUM.
   */
  unsigned cell_val_num() const {
    auto& ctx = ctx_.get();
    unsigned num;
    ctx.handle_error(
        tiledb_dimension_get_cell_val_num(ctx.ptr().get(), dim_.get(), &num));
    return num;
  }

  /** Sets the number of values per coordinate. */
  tiledb::Dimension& set_cell_val_num(unsigned num) {
    auto& ctx = ctx_.get();
    ctx.handle_error(
        tiledb_dimension_set_cell_val_num(ctx.ptr().get(), dim_.get(), num));
    return *this;
  }

  /**
   * Returns a copy of the FilterList of the dimemnsion.
   * To change the filter list, use `set_filter_list()`.
   */
  tiledb::FilterList filter_list() const {
    auto& ctx = ctx_.get();
    tiledb_filter_list_t* filter_list;
    ctx.handle_error(tiledb_dimension_get_filter_list(
        ctx.ptr().get(), dim_.get(), &filter_list));
    return FilterList(ctx, filter_list);
  }

  /**
   * Sets the dimension filter list, which is an ordered list of filters that
   * will be used to process and/or transform the coordinate data (such as
   * compression).
   */
  tiledb::Dimension& set_filter_list(const tiledb::FilterList& filter_list) {
    auto& ctx = ctx_.get();
    ctx.handle_error(tiledb_dimension_set_filter_list(
        ctx.ptr().get(), dim_.get(), filter_list.ptr().get()));
    return *this;
  }

  /** Returns the name of the dimension. */
  const std::string name() const {
    const char* name;
    auto& ctx = ctx_.get();
    ctx.handle_error(
        tiledb_dimension_get_name(ctx.ptr().get(), dim_.get(), &name));
    return name;
  }

  /** Returns the dimension datatype. */
  tiledb_datatype_t type() const {
    tiledb_datatype_t type;
    auto& ctx = ctx_.get();
    ctx.handle_error(
        tiledb_dimension_get_type(ctx.ptr().get(), dim_.get(), &type));
    return type;
  }

  /**
   * Returns the domain of the dimension.
   *
   * @tparam T Domain datatype
   * @return Pair of [lower, upper] inclusive bounds.
   */
  template <typename T>
  std::pair<T, T> domain() const {
    impl::type_check<T>(type(), 1);
    auto d = (const T*)_domain();
    return std::pair<T, T>(d[0], d[1]);
  }

  /**
   * Returns a string representation of the domain.
   * @throws TileDBError if the domain cannot be stringified (TILEDB_ANY)
   */
  std::string domain_to_str() const {
    auto domain = _domain();
    auto type = this->type();
    const int8_t* di8;
    const uint8_t* dui8;
    const int16_t* di16;
    const uint16_t* dui16;
    const int32_t* di32;
    const uint32_t* dui32;
    const int64_t* di64;
    const uint64_t* dui64;
    const float* df32;
    const double* df64;

    std::stringstream ss;
    ss << "[";

    switch (type) {
      case TILEDB_INT8:
        di8 = static_cast<const int8_t*>(domain);
        ss << di8[0] << "," << di8[1];
        break;
      case TILEDB_UINT8:
        dui8 = static_cast<const uint8_t*>(domain);
        ss << dui8[0] << "," << dui8[1];
        break;
      case TILEDB_INT16:
        di16 = static_cast<const int16_t*>(domain);
        ss << di16[0] << "," << di16[1];
        break;
      case TILEDB_UINT16:
        dui16 = static_cast<const uint16_t*>(domain);
        ss << dui16[0] << "," << dui16[1];
        break;
      case TILEDB_INT32:
        di32 = static_cast<const int32_t*>(domain);
        ss << di32[0] << "," << di32[1];
        break;
      case TILEDB_UINT32:
        dui32 = static_cast<const uint32_t*>(domain);
        ss << dui32[0] << "," << dui32[1];
        break;
      case TILEDB_INT64:
        di64 = static_cast<const int64_t*>(domain);
        ss << di64[0] << "," << di64[1];
        break;
      case TILEDB_UINT64:
        dui64 = static_cast<const uint64_t*>(domain);
        ss << dui64[0] << "," << dui64[1];
        break;
      case TILEDB_FLOAT32:
        df32 = static_cast<const float*>(domain);
        ss << df32[0] << "," << df32[1];
        break;
      case TILEDB_FLOAT64:
        df64 = static_cast<const double*>(domain);
        ss << df64[0] << "," << df64[1];
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
        di64 = static_cast<const int64_t*>(domain);
        ss << di64[0] << "," << di64[1];
        break;
      case TILEDB_STRING_ASCII:
        // Strings have null domains so let's return an empty string
        // representation
        return "";
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
      case TILEDB_ANY:
        // Not supported domain types
        throw TileDBError("Invalid Dim type");
    }

    ss << "]";

    return ss.str();
  }

  /** Returns the tile extent of the dimension. */
  template <typename T>
  T tile_extent() const {
    impl::type_check<T>(type(), 1);
    return *(const T*)_tile_extent();
  }

  /**
   * Returns a string representation of the extent.
   * @throws TileDBError if the domain cannot be stringified (TILEDB_ANY)
   */
  std::string tile_extent_to_str() const {
    auto tile_extent = _tile_extent();
    auto type = this->type();
    const int8_t* ti8;
    const uint8_t* tui8;
    const int16_t* ti16;
    const uint16_t* tui16;
    const int32_t* ti32;
    const uint32_t* tui32;
    const int64_t* ti64;
    const uint64_t* tui64;
    const float* tf32;
    const double* tf64;

    std::stringstream ss;

    switch (type) {
      case TILEDB_INT8:
        ti8 = static_cast<const int8_t*>(tile_extent);
        ss << *ti8;
        break;
      case TILEDB_UINT8:
        tui8 = static_cast<const uint8_t*>(tile_extent);
        ss << *tui8;
        break;
      case TILEDB_INT16:
        ti16 = static_cast<const int16_t*>(tile_extent);
        ss << *ti16;
        break;
      case TILEDB_UINT16:
        tui16 = static_cast<const uint16_t*>(tile_extent);
        ss << *tui16;
        break;
      case TILEDB_INT32:
        ti32 = static_cast<const int32_t*>(tile_extent);
        ss << *ti32;
        break;
      case TILEDB_UINT32:
        tui32 = static_cast<const uint32_t*>(tile_extent);
        ss << *tui32;
        break;
      case TILEDB_INT64:
        ti64 = static_cast<const int64_t*>(tile_extent);
        ss << *ti64;
        break;
      case TILEDB_UINT64:
        tui64 = static_cast<const uint64_t*>(tile_extent);
        ss << *tui64;
        break;
      case TILEDB_FLOAT32:
        tf32 = static_cast<const float*>(tile_extent);
        ss << *tf32;
        break;
      case TILEDB_FLOAT64:
        tf64 = static_cast<const double*>(tile_extent);
        ss << *tf64;
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
        ti64 = static_cast<const int64_t*>(tile_extent);
        ss << *ti64;
        break;
      case TILEDB_STRING_ASCII:
        // Strings have null tile extents so let's return an empty string
        // representation
        return "";
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
      case TILEDB_ANY:
        throw TileDBError("Invalid Dim type");
    }

    return ss.str();
  }

  /** Returns a shared pointer to the C TileDB dimension object. */
  std::shared_ptr<tiledb_dimension_t> ptr() const {
    return dim_;
  }

  /* ********************************* */
  /*          STATIC FUNCTIONS         */
  /* ********************************* */

  /**
   * Factory function for creating a new dimension with datatype T.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * // Create a dimension with inclusive domain [0,1000] and tile extent 100.
   * auto dim = Dimension::create<int32_t>(ctx, "d", {{0, 1000}}, 100);
   * @endcode
   *
   * @tparam T int, char, etc...
   * @param ctx The TileDB context.
   * @param name The dimension name.
   * @param domain The dimension domain. A pair [lower,upper] of inclusive
   * bounds.
   * @param extent The tile extent on the dimension.
   * @return A new `Dimension` object.
   */
  template <typename T>
  static tiledb::Dimension create(
      const tiledb::Context& ctx,
      const std::string& name,
      const std::array<T, 2>& domain,
      T extent) {
    using DataT = impl::TypeHandler<T>;
    static_assert(
        DataT::tiledb_num == 1,
        "Dimension types cannot be compound, use arithmetic type.");
    return create_impl(ctx, name, DataT::tiledb_type, &domain, &extent);
  }

  /**
   * Factory function for creating a new dimension (non typechecked).
   *
   * @param ctx The TileDB context.
   * @param name The dimension name.
   * @param datatype The dimension datatype.
   * @param domain The dimension domain. A pair [lower,upper] of inclusive
   *    bounds.
   * @param extent The tile extent on the dimension.
   * @return A new `Dimension` object.
   */
  static tiledb::Dimension create(
      const tiledb::Context& ctx,
      const std::string& name,
      tiledb_datatype_t datatype,
      const void* domain,
      const void* extent) {
    return create_impl(ctx, name, datatype, domain, extent);
  }

  static bool is_valid_intdatatype(int intdatatype)
  {
	  if (intdatatype == (int)TILEDB_INT8
		  || intdatatype == (int)TILEDB_UINT8
		  || intdatatype == (int)TILEDB_INT16
		  || intdatatype == (int)TILEDB_UINT16
		  || intdatatype == (int)TILEDB_INT32
		  || intdatatype == (int)TILEDB_UINT32
		  || intdatatype == (int)TILEDB_INT64
		  || intdatatype == (int)TILEDB_UINT64
		  || intdatatype == (int)TILEDB_FLOAT32
		  || intdatatype == (int)TILEDB_FLOAT64
		  || intdatatype == (int)TILEDB_DATETIME_YEAR
		  || intdatatype == (int)TILEDB_DATETIME_MONTH
		  || intdatatype == (int)TILEDB_DATETIME_WEEK
		  || intdatatype == (int)TILEDB_DATETIME_DAY
		  || intdatatype == (int)TILEDB_DATETIME_HR
		  || intdatatype == (int)TILEDB_DATETIME_MIN
		  || intdatatype == (int)TILEDB_DATETIME_SEC
		  || intdatatype == (int)TILEDB_DATETIME_MS
		  || intdatatype == (int)TILEDB_DATETIME_US
		  || intdatatype == (int)TILEDB_DATETIME_NS
		  || intdatatype == (int)TILEDB_DATETIME_PS
		  || intdatatype == (int)TILEDB_DATETIME_FS
		  || intdatatype == (int)TILEDB_DATETIME_AS
		  || intdatatype == (int)TILEDB_STRING_ASCII
		  )
	  {
		  return true;
	  }
	  else
	  {
		  return false;
	  }
  }

  static tiledb::Dimension create_dimension(const tiledb::Context& ctx, const std::string& name, int intdatatype,
	  const std::string& lower_bound_str, const std::string& upper_bound_str, const std::string& extent_str)
  {
	  if (intdatatype == (int)TILEDB_INT8)
	  {
		  std::array<int8_t, 2> bound = { (int8_t)atoi(lower_bound_str.c_str()), (int8_t)atoi(upper_bound_str.c_str()) };
		  int8_t extent = (int8_t)atoi(extent_str.c_str());
		  return create<int8_t>(ctx, name, bound, extent);

	  }
	  else if (intdatatype == (int)TILEDB_UINT8)
	  {
		  std::array<uint8_t, 2> bound = { (uint8_t)atoi(lower_bound_str.c_str()), (uint8_t)atoi(upper_bound_str.c_str()) };
		  uint8_t extent = (uint8_t)atoi(extent_str.c_str());
		  return create<uint8_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_INT16)
	  {
		  std::array<int16_t, 2> bound = { (int16_t)atoi(lower_bound_str.c_str()), (int16_t)atoi(upper_bound_str.c_str()) };
		  int16_t extent = (int16_t)atoi(extent_str.c_str());
		  return create<int16_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_UINT16)
	  {
		  std::array<uint16_t, 2> bound = { (uint16_t)atoi(lower_bound_str.c_str()), (uint16_t)atoi(upper_bound_str.c_str()) };
		  uint16_t extent = (uint16_t)atoi(extent_str.c_str());
		  return create<uint16_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_INT32)
	  {
		  std::array<int32_t, 2> bound = { (int32_t)atoi(lower_bound_str.c_str()), (int32_t)atoi(upper_bound_str.c_str()) };
		  int32_t extent = (int32_t)atoi(extent_str.c_str());
		  return create<int32_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_UINT32)
	  {
		  std::array<uint32_t, 2> bound = { (uint32_t)atoi(lower_bound_str.c_str()), (uint32_t)atoi(upper_bound_str.c_str()) };
		  uint32_t extent = (uint32_t)atoi(extent_str.c_str());
		  return create<uint32_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_INT64)
	  {
		  std::array<int64_t, 2> bound = { (int64_t)atoi(lower_bound_str.c_str()), (int64_t)atoi(upper_bound_str.c_str()) };
		  int64_t extent = (int64_t)atoi(extent_str.c_str());
		  return create<int64_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_UINT64)
	  {
		  std::array<uint64_t, 2> bound = { (uint64_t)atoi(lower_bound_str.c_str()), (uint64_t)atoi(upper_bound_str.c_str()) };
		  uint64_t extent = (uint64_t)atoi(extent_str.c_str());
		  return create<uint64_t>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_FLOAT32)
	  {
		  std::array<float, 2> bound = { (float)atoi(lower_bound_str.c_str()), (float)atoi(upper_bound_str.c_str()) };
		  float extent = (float)atoi(extent_str.c_str());
		  return create<float>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_FLOAT64)
	  {
		  std::array<double, 2> bound = { (double)atoi(lower_bound_str.c_str()), (double)atoi(upper_bound_str.c_str()) };
		  double extent = (double)atoi(extent_str.c_str());
		  return create<double>(ctx, name, bound, extent);
	  }
	  else if (intdatatype == (int)TILEDB_DATETIME_YEAR
		  || intdatatype == (int)TILEDB_DATETIME_MONTH
		  || intdatatype == (int)TILEDB_DATETIME_WEEK
		  || intdatatype == (int)TILEDB_DATETIME_DAY
		  || intdatatype == (int)TILEDB_DATETIME_HR
		  || intdatatype == (int)TILEDB_DATETIME_MIN
		  || intdatatype == (int)TILEDB_DATETIME_SEC
		  || intdatatype == (int)TILEDB_DATETIME_MS
		  || intdatatype == (int)TILEDB_DATETIME_US
		  || intdatatype == (int)TILEDB_DATETIME_NS
		  || intdatatype == (int)TILEDB_DATETIME_PS
		  || intdatatype == (int)TILEDB_DATETIME_FS
		  || intdatatype == (int)TILEDB_DATETIME_AS)
	  {
		  std::array<int64_t, 2> bound = { (int64_t)atoi(lower_bound_str.c_str()), (int64_t)atoi(upper_bound_str.c_str()) };
		  int64_t extent = (int64_t)atoi(extent_str.c_str());
		  tiledb_datatype_t datatype = (tiledb_datatype_t)(intdatatype);
		  return create(ctx, name, datatype,&bound, &extent);
	  }
	  else if (intdatatype == (int)TILEDB_STRING_ASCII)
	  {
		  return create(ctx, name, TILEDB_STRING_ASCII, nullptr, nullptr);
	  }
	  else if (intdatatype == (int)TILEDB_CHAR
		  || intdatatype == (int)TILEDB_STRING_UTF8
		  || intdatatype == (int)TILEDB_STRING_UTF16
		  || intdatatype == (int)TILEDB_STRING_UTF32
		  || intdatatype == (int)TILEDB_STRING_UCS2
		  || intdatatype == (int)TILEDB_STRING_UCS4
		  || intdatatype == (int)TILEDB_ANY
		  ) {
		  std::cout << "tiledb::Dimension create_dimension, unknown type:" << intdatatype << std::endl;
		  return Dimension(ctx,nullptr);
	  }
	  else {
		  std::cout << "tiledb::Dimension create_dimension, unknown type:" << intdatatype << std::endl;
		  return Dimension(ctx, nullptr);
	  }

 
  }

  static tiledb::Dimension create_int32_dimension(const tiledb::Context& ctx, const std::string& name, int bound_lower, int bound_upper, int extent)
  {
	  std::array<int, 2> bound;
	  bound[0] = bound_lower;
	  bound[1] = bound_upper;
	  return Dimension::create<int>(ctx, name, bound, extent);
  }

  static tiledb::Dimension create_int64_dimension(const tiledb::Context& ctx, const std::string& name, int64_t bound_lower, int64_t bound_upper, int64_t extent)
  {
	  std::array<int64_t, 2> bound;
	  bound[0] = bound_lower;
	  bound[1] = bound_upper;
	  return Dimension::create<int64_t>(ctx, name, bound, extent);
  }

  static tiledb::Dimension create_uint64_dimension(const tiledb::Context& ctx, const std::string& name, uint64_t bound_lower, uint64_t bound_upper, uint64_t extent)
  {
	  std::array<uint64_t, 2> bound;
	  bound[0] = bound_lower;
	  bound[1] = bound_upper;
	  return Dimension::create<uint64_t>(ctx, name, bound, extent);
  }

  static tiledb::Dimension create_double_dimension(const tiledb::Context& ctx, const std::string& name, double bound_lower, double bound_upper, double extent)
  {
	  std::array<double, 2> bound;
	  bound[0] = bound_lower;
	  bound[1] = bound_upper;
	  return Dimension::create<double>(ctx, name, bound, extent);
  }

  static tiledb::Dimension create_string_dimension(const tiledb::Context& ctx, const std::string& name)
  {

	  return Dimension::create(ctx, name, tiledb_datatype_t::TILEDB_STRING_ASCII, nullptr, nullptr);
  }



 private:
  /* ********************************* */
  /*         PRIVATE ATTRIBUTES        */
  /* ********************************* */

  /** The TileDB context. */
  std::reference_wrapper<const Context> ctx_;

  /** A deleter wrapper. */
  impl::Deleter deleter_;

  /** The C TileDB dimension object. */
  std::shared_ptr<tiledb_dimension_t> dim_;

  /* ********************************* */
  /*          PRIVATE METHODS          */
  /* ********************************* */

  /** Returns the binary representation of the dimension domain. */
  const void* _domain() const {
    auto& ctx = ctx_.get();
    const void* domain;
    ctx.handle_error(
        tiledb_dimension_get_domain(ctx.ptr().get(), dim_.get(), &domain));
    return domain;
  }

  /** Returns the binary representation of the dimension extent. */
  const void* _tile_extent() const {
    auto& ctx = ctx_.get();
    const void* tile_extent;
    ctx.handle_error(tiledb_dimension_get_tile_extent(
        ctx.ptr().get(), dim_.get(), &tile_extent));
    return tile_extent;
  }

  /* ********************************* */
  /*     PRIVATE STATIC FUNCTIONS      */
  /* ********************************* */

  /**
   * Creates a dimension with the input name, datatype, domain and tile
   * extent.
   */
  static tiledb::Dimension create_impl(
      const tiledb::Context& ctx,
      const std::string& name,
      tiledb_datatype_t type,
      const void* domain,
      const void* tile_extent) {
    tiledb_dimension_t* d;
    ctx.handle_error(tiledb_dimension_alloc(
        ctx.ptr().get(), name.c_str(), type, domain, tile_extent, &d));
    return Dimension(ctx, d);
  }
};

/* ********************************* */
/*               MISC                */
/* ********************************* */

/** Get a string representation of a dimension for an output stream. */
inline std::ostream& operator<<(std::ostream& os, const Dimension& dim) {
  os << "Dim<" << dim.name() << "," << dim.domain_to_str() << ","
     << dim.tile_extent_to_str() << ">";
  return os;
}

}  // namespace tiledb

#endif  // TILEDB_CPP_API_DIMENSION_H
