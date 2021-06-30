/**
 * @file  tiledb_cpp_api_array_schema.h
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
 * This file declares the C++ API for the TileDB ArraySchema object.
 */

#ifndef TILEDB_CPP_API_ARRAY_SCHEMA_H
#define TILEDB_CPP_API_ARRAY_SCHEMA_H

#include "tiledb_cxx_attribute.h"
#include "tiledb_cxx_domain.h"
#include "tiledb_cxx_object.h"
#include "tiledb_cxx_schema_base.h"
#include "tiledb.h"

#include <array>
#include <memory>
#include <string>
#include <map> 
#include <unordered_map>

namespace tiledb {

/**
 * Schema describing an array.
 *
 * @details
 * The schema is an independent description of an array. A schema can be
 * used to create multiple array's, and stores information about its
 * domain, cell types, and compression details. An array schema is composed of:
 *
 * - A Domain
 * - A set of Attributes
 * - Memory layout definitions: tile and cell
 * - Compression details for Array level factors like offsets and coordinates
 *
 * **Example:**
 *
 * @code{.cpp}
 * tiledb::Context ctx;
 * tiledb::ArraySchema schema(c_ctx, TILEDB_SPARSE); // Or
 * TILEDB_DENSE
 *
 * // Create a Domain
 * tiledb::Domain domain(...);
 *
 * // Create Attributes
 * auto a1 = tiledb::Attribute::create(...);
 *
 * schema.set_domain(domain);
 * schema.add_attribute(a1);
 *
 * // Specify tile memory layout
 * schema.set_tile_order(TILEDB_ROW_MAJOR);
 * // Specify cell memory layout within each tile
 * schema.set_cell_order(TILEDB_ROW_MAJOR);
 * schema.set_capacity(10); // For sparse, set capacity of each tile
 *
 * // Create the array on persistent storage with the schema.
 * tiledb::Array::create("my_array", schema);
 *
 * @endcode
 */
class ArraySchema : public Schema {
 public:
  /* ********************************* */
  /*     CONSTRUCTORS & DESTRUCTORS    */
  /* ********************************* */

  /**
   * Creates a new array schema.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(c_ctx, TILEDB_SPARSE);
   * @endcode
   *
   * @param ctx TileDB context
   * @param type Array type, sparse or dense.
   */
  explicit ArraySchema(const std::shared_ptr<tiledb::Context>& ctx, tiledb_array_type_t type)
      : Schema(ctx) {
    tiledb_array_schema_t* schema;
	tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_alloc(c_ctx, type, &schema));
    schema_ = std::shared_ptr<tiledb_array_schema_t>(schema, deleter_);
  }

  /**
   * Loads the schema of an existing array.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx, "s3://bucket-name/array-name");
   * @endcode
   *
   * @param ctx TileDB context
   * @param uri URI of array
   */
  ArraySchema(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri)
      : ArraySchema(ctx, uri, TILEDB_NO_ENCRYPTION, nullptr, 0) {
  }

  /**
   * Loads the schema of an existing encrypted array.
   *
   * **Example:**
   * @code{.cpp}
   * // Load AES-256 key from disk, environment variable, etc.
   * uint8_t key[32] = ...;
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx, "s3://bucket-name/array-name",
   *    TILEDB_AES_256_GCM, key, sizeof(key));
   * @endcode
   *
   * @param ctx TileDB context
   * @param uri URI of array
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   * @param key_length Length in bytes of the encryption key.
   */
  ArraySchema(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb_encryption_type_t encryption_type,
      const void* encryption_key,
      uint32_t key_length)
      : Schema(ctx) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_array_schema_t* schema;
    ctx_->handle_error(tiledb_array_schema_load_with_key(
        c_ctx,
        uri.c_str(),
        encryption_type,
        encryption_key,
        key_length,
        &schema));
    schema_ = std::shared_ptr<tiledb_array_schema_t>(schema, deleter_);
  }

  /**
   * Loads the schema of an existing encrypted array.
   *
   * @param ctx TileDB context
   * @param uri URI of array
   * @param encryption_type The encryption type to use.
   * @param encryption_key The encryption key to use.
   */
  ArraySchema(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& uri,
      tiledb_encryption_type_t encryption_type,
      const std::string& encryption_key)
      : ArraySchema(
            ctx,
            uri,
            encryption_type,
            encryption_key.data(),
            (uint32_t)encryption_key.size()) {
  }

  /**
   * Loads the schema of an existing array with the input C array
   * schema object.
   *
   * @param ctx TileDB context
   * @param schema C API array schema object
   */
  ArraySchema(const std::shared_ptr<tiledb::Context>& ctx, tiledb_array_schema_t* schema)
      : Schema(ctx) {
    schema_ = std::shared_ptr<tiledb_array_schema_t>(schema, deleter_);
  }

  ArraySchema() {} // = default;
  ArraySchema(const tiledb::ArraySchema&) = default;
  ArraySchema(tiledb::ArraySchema&&) = default;
  virtual ~ArraySchema() = default;
  tiledb::ArraySchema& operator=(const tiledb::ArraySchema&) = default;
  tiledb::ArraySchema& operator=(tiledb::ArraySchema&&) = default;

  /* ********************************* */
  /*                API                */
  /* ********************************* */

  /**
   * Dumps the array schema in an ASCII representation to an output.
   *
   * @param out (Optional) File to dump output to. Defaults to `nullptr`
   * which will lead to selection of `stdout`.
   */
  void dump(const std::string& filename) const override { //void dump(FILE* out = nullptr) const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    FILE* pFile = nullptr;
    if(!filename.empty())
    {
      pFile = fopen(filename.c_str(),"w");
    }
    
    ctx_->handle_error(
        tiledb_array_schema_dump(ctx_->ptr().get(), schema_.get(), pFile));
    if(pFile!=NULL)
    {
      fclose(pFile);
    }
  }

  /** Returns the array type. */
  tiledb_array_type_t array_type() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_array_type_t type;
    ctx_->handle_error(tiledb_array_schema_get_array_type(
        ctx_->ptr().get(), schema_.get(), &type));
    return type;
  }

  /** Returns the tile capacity. */
  uint64_t capacity() const {
    //tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    uint64_t capacity;
    ctx_->handle_error(tiledb_array_schema_get_capacity(
        ctx_->ptr().get(), schema_.get(), &capacity));
    return capacity;
  }

  /**
   * Sets the tile capacity.
   *
   * @param capacity The capacity of a sparse data tile. Note that
   * sparse data tiles exist in sparse fragments, which can be created
   * in both sparse and dense arrays. For more details,
   * see [tutorials/tiling-sparse.html](tutorials/tiling-sparse.html).
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_capacity(uint64_t capacity) {
   // tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_capacity(
        ctx_->ptr().get(), schema_.get(), capacity));
    return *this;
  }

  /** Returns `true` if the array allows coordinate duplicates. */
  bool allows_dups() const {
  //  tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int allows_dups;
    ctx_->handle_error(tiledb_array_schema_get_allows_dups(
        ctx_->ptr().get(), schema_.get(), &allows_dups));
    return (bool)allows_dups;
  }

  /**
   * Sets whether the array allows coordinate duplicates. It throws
   * an exception in case it sets `true` to a dense array.
   */
  tiledb::ArraySchema& set_allows_dups(bool allows_dups) {
  //  tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int allows_dups_i = allows_dups ? 1 : 0;
    ctx_->handle_error(tiledb_array_schema_set_allows_dups(
        ctx_->ptr().get(), schema_.get(), allows_dups_i));
    return *this;
  }

  /** Returns the tile order. */
  tiledb_layout_t tile_order() const {
  //  tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_layout_t layout;
    ctx_->handle_error(tiledb_array_schema_get_tile_order(
        ctx_->ptr().get(), schema_.get(), &layout));
    return layout;
  }

  /**
   * Sets the tile order.
   *
   * @param layout Tile order to set.
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_tile_order(tiledb_layout_t layout) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_tile_order(
        ctx_->ptr().get(), schema_.get(), layout));
    return *this;
  }

  /**
   * Sets both the tile and cell orders.
   *
   * @param layout Pair of {tile order, cell order}
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_order(tiledb_layout_t tile_order, tiledb_layout_t cell_order) { //tiledb::ArraySchema& set_order(const std::array<tiledb_layout_t, 2>& p) {
    set_tile_order(tile_order); //set_tile_order(p[0]);
    set_cell_order(cell_order);// set_cell_order(p[1]);
    return *this;
  }

  /** Returns the cell order. */
  tiledb_layout_t cell_order() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_layout_t layout;
    ctx_->handle_error(tiledb_array_schema_get_cell_order(
        ctx_->ptr().get(), schema_.get(), &layout));
    return layout;
  }

  /**
   * Sets the cell order.
   *
   * @param layout Cell order to set.
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_cell_order(tiledb_layout_t layout) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_cell_order(
        ctx_->ptr().get(), schema_.get(), layout));
    return *this;
  }

  /**
   * Returns a copy of the FilterList of the coordinates. To change the
   * coordinate compressor, use `set_coords_filter_list()`.
   *
   * @return Copy of the coordinates FilterList.
   */
  tiledb::FilterList coords_filter_list() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_filter_list_t* filter_list;
    ctx_->handle_error(tiledb_array_schema_get_coords_filter_list(
        ctx_->ptr().get(), schema_.get(), &filter_list));
    return FilterList(ctx_, filter_list);
  }

  /**
   * Sets the FilterList for the coordinates, which is an ordered list of
   * filters that will be used to process and/or transform the coordinate data
   * (such as compression).
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx_->ptr().get(), TILEDB_SPARSE);
   * tiledb::FilterList filter_list(ctx);
   * filter_list.add_filter({ctx, TILEDB_FILTER_BYTESHUFFLE})
   *     .add_filter({ctx, TILEDB_FILTER_BZIP2});
   * schema.set_coords_filter_list(filter_list);
   * @endcode
   *
   * @param filter_list FilterList to use
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_coords_filter_list(const tiledb::FilterList& filter_list) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_coords_filter_list(
        ctx_->ptr().get(), schema_.get(), filter_list.ptr().get()));
    return *this;
  }

  /**
   * Returns a copy of the FilterList of the offsets. To change the
   * offsets compressor, use `set_offsets_filter_list()`.
   *
   * @return Copy of the offsets FilterList.
   */
  tiledb::FilterList offsets_filter_list() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_filter_list_t* filter_list;
    ctx_->handle_error(tiledb_array_schema_get_offsets_filter_list(
        ctx_->ptr().get(), schema_.get(), &filter_list));
    return FilterList(ctx_, filter_list);
  }

  /**
   * Sets the FilterList for the offsets, which is an ordered list of
   * filters that will be used to process and/or transform the offsets data
   * (such as compression).
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx_->ptr().get(), TILEDB_SPARSE);
   * tiledb::FilterList filter_list(ctx);
   * filter_list.add_filter({ctx, TILEDB_FILTER_POSITIVE_DELTA})
   *     .add_filter({ctx, TILEDB_FILTER_LZ4});
   * schema.set_offsets_filter_list(filter_list);
   * @endcode
   *
   * @param filter_list FilterList to use
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_offsets_filter_list(const tiledb::FilterList& filter_list) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_offsets_filter_list(
        ctx_->ptr().get(), schema_.get(), filter_list.ptr().get()));
    return *this;
  }

  /**
   * Returns a copy of the schema's array Domain. To change the domain,
   * use `set_domain()`.
   *
   * @return Copy of the array Domain
   */
  tiledb::Domain domain() const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_domain_t* domain;
    ctx_->handle_error(tiledb_array_schema_get_domain(
        ctx_->ptr().get(), schema_.get(), &domain));
    return Domain(ctx_, domain);
  }

  /**
   * Sets the array domain.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx_->ptr().get(), TILEDB_SPARSE);
   * // Create a Domain
   * tiledb::Domain domain(...);
   * schema.set_domain(domain);
   * @endcode
   *
   * @param domain Domain to use
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& set_domain(const tiledb::Domain& domain) {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_set_domain(
        ctx_->ptr().get(), schema_.get(), domain.ptr().get()));
    return *this;
  }

  /**
   * Adds an Attribute to the array.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx_->ptr().get(), TILEDB_SPARSE);
   * schema.add_attribute(Attribute::create<int32_t>(ctx_->ptr().get(),
   * "attr_name"));
   * @endcode
   *
   * @param attr The Attribute to add
   * @return Reference to this `ArraySchema` instance.
   */
  tiledb::ArraySchema& add_attribute(const tiledb::Attribute& attr) override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_add_attribute(
        ctx_->ptr().get(), schema_.get(), attr.ptr().get()));
    return *this;
  }

  /** Returns a shared pointer to the C TileDB domain object. */
  std::shared_ptr<tiledb_array_schema_t> ptr() const {
    return schema_;
  }

  /**
   * Validates the schema.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::ArraySchema schema(ctx_->ptr().get(), TILEDB_SPARSE);
   * // Add domain, attributes, etc...
   *
   * try {
   *   schema.check();
   * } catch (const tiledb::TileDBError& e) {
   *   std::cout << e.what() << "\n";
   *   exit(1);
   * }
   * @endcode
   *
   * @throws TileDBError if the schema is incorrect or invalid.
   */
  void check() const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_array_schema_check(ctx_->ptr().get(), schema_.get()));
  }

 	/**
	* Gets all attribute name vector.
	*
	* @return vector of attribute names.
	*/
	std::vector<std::string> attribute_names() const override {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
		tiledb_attribute_t* attrptr;
		unsigned int nattr;
 
		ctx_->handle_error(tiledb_array_schema_get_attribute_num(
			c_ctx, schema_.get(), &nattr));
		std::vector<std::string> result;
		result.reserve(nattr);
		for (unsigned int i = 0; i < nattr; ++i) {
			ctx_->handle_error(tiledb_array_schema_get_attribute_from_index(
				c_ctx, schema_.get(), i, &attrptr));
			auto attr = Attribute(ctx_, attrptr);
			result.push_back(attr.name());
			 
		}
		return result;
	} 

  /**
   * Get a copy of an Attribute in the schema by name.
   *
   * @param name Name of attribute
   * @return Attribute
   */
  tiledb::Attribute attribute(const std::string& name) const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_attribute_t* attr;
    ctx_->handle_error(tiledb_array_schema_get_attribute_from_name(
        c_ctx, schema_.get(), name.c_str(), &attr));
    return Attribute(ctx_, attr);
  }

  /** Returns the number of attributes in the schema. **/
  unsigned attribute_num() const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    unsigned num;
    ctx_->handle_error(tiledb_array_schema_get_attribute_num(
        c_ctx, schema_.get(), &num));
    return num;
  }

  /**
   * Get a copy of an Attribute in the schema by index.
   * Attributes are ordered the same way they were defined
   * when constructing the array schema.
   *
   * @param i Index of attribute
   * @return Attribute
   */
  tiledb::Attribute attribute(unsigned int i) const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_attribute_t* attr;
    ctx_->handle_error(tiledb_array_schema_get_attribute_from_index(
        c_ctx, schema_.get(), i, &attr));
    return Attribute(ctx_, attr);
  }



  /**
   * Checks if the schema has an attribute of the given name.
   *
   * @param name Name of attribute to check for
   * @return True if the schema has an attribute of the given name.
   */
  bool has_attribute(const std::string& name) const {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    int32_t has_attr;
    ctx_->handle_error(tiledb_array_schema_has_attribute(
        c_ctx, schema_.get(), name.c_str(), &has_attr));
    return has_attr == 1;
  }

  std::string to_info_str()
  {
          std::stringstream ss;
          ss << "ArraySchema<";
          ss << tiledb::ArraySchema::to_str(array_type());
          ss << ' ' << domain();
          for (const auto& a : attributes()) {
                  ss << ' ' << a.second;
          }
          ss << '>';

          return ss.str();
  }


  /* ********************************* */
  /*         STATIC FUNCTIONS          */
  /* ********************************* */

  /** Returns the input array type in string format. */
  static std::string to_str(tiledb_array_type_t type) {
    return type == TILEDB_DENSE ? "DENSE" : "SPARSE";
  }

  /** Returns the input layout in string format. */
  static std::string to_str(tiledb_layout_t layout) {
    switch (layout) {
      case TILEDB_GLOBAL_ORDER:
        return "GLOBAL";
      case TILEDB_ROW_MAJOR:
        return "ROW-MAJOR";
      case TILEDB_COL_MAJOR:
        return "COL-MAJOR";
      case TILEDB_UNORDERED:
        return "UNORDERED";
    }
    return "";
  }

 private:
  /* ********************************* */
  /*         PRIVATE ATTRIBUTES        */
  /* ********************************* */

  /** The pointer to the C TileDB array schema object. */
  std::shared_ptr<tiledb_array_schema_t> schema_;


//////customized
protected:
  /**
   * Gets all attributes in the array.
   *
   * @return Map of attribute name to copy of Attribute instance.
   */
  std::map<std::string,tiledb::Attribute> attributes() const override { //std::unordered_map<std::string, tiledb::Attribute> attributes() const override {
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_attribute_t* attrptr;
    unsigned int nattr;
    std::map<std::string, Attribute> attrs; //std::unordered_map<std::string, Attribute> attrs;
    ctx_->handle_error(tiledb_array_schema_get_attribute_num(
        ctx_->ptr().get(), schema_.get(), &nattr));
    for (unsigned int i = 0; i < nattr; ++i) {
      ctx_->handle_error(tiledb_array_schema_get_attribute_from_index(
          ctx_->ptr().get(), schema_.get(), i, &attrptr));
      auto attr = Attribute(ctx_, attrptr);
      attrs.emplace(
          std::pair<std::string, Attribute>(attr.name(), std::move(attr)));
    }
    return attrs;
  }



public:


	/**
	* Gets all dimension name vector.
	*
	* @return vector of dimension names.
	*/
	std::vector<std::string> dimension_names() const {
		std::vector<std::string> result;
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();

		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));
	 
		Domain cpp_domain(ctx_, domain);

		return cpp_domain.dimension_names();
		 
	}

	/**
	* Checks if the schema has a dimension of the given name.
	*
	* @param name Name of dimension to check for
	* @return True if the domain has a dimension of the given name.
	*/
	bool has_dimension(const std::string& name) const {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
		int32_t has_dim;
		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));

		ctx_->handle_error(tiledb_domain_has_dimension(
			c_ctx, domain, name.c_str(), &has_dim));
		return has_dim == 1;
	}

	/** Returns the dimensions with the given index. */
	tiledb::Dimension dimension(unsigned idx) const {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
		 
		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));

		tiledb_dimension_t* dimptr;
		ctx_->handle_error(tiledb_domain_get_dimension_from_index(
			c_ctx, domain, idx, &dimptr));
		return Dimension(ctx_, dimptr);
	}

	/** Returns the dimensions with the given name. */
	tiledb::Dimension dimension(const std::string& name) const {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
	 
		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));

		tiledb_dimension_t* dimptr;
		ctx_->handle_error(tiledb_domain_get_dimension_from_name(
			c_ctx, domain, name.c_str(), &dimptr));
		return Dimension(ctx_, dimptr);
	}

	/**
	* Adds a new int dimension to the domain.
	*
	* **Example:**
	* @code{.cpp}
	* tiledb::Context ctx;
	* tiledb::Domain domain;
	* auto d1 = tiledb::Dimension::create<int>(ctx, "d1", {-10, 10});
	* domain.add_dimension(d1);
	* @endcode
	*
	* @param d Dimension to add
	* @return Reference to this Domain
	*/
	void add_int_dimension(const std::string& name,
		int lower,
		int upper,
		int extent) {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
	 
		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));

		std::array<int, 2> bound;
		bound[0] = lower;
		bound[1] = upper;
		tiledb::Dimension d = tiledb::Dimension::create<int>(ctx_, name, bound, extent);
			
		ctx_->handle_error(tiledb_domain_add_dimension(
			c_ctx, domain, d.ptr().get()));
		return;
	}

	/**
	* Adds an Attribute to the array.
	*
	* @param name  The Attribute name
	* @param type datatype
	* @return Reference to this `ArraySchema` instance.
	*/
	void add_attribute(const std::string& name, tiledb_datatype_t type)
	{
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();
		tiledb::Attribute attr(ctx_, name, type);
		ctx_->handle_error(tiledb_array_schema_add_attribute(
			c_ctx, schema_.get(), attr.ptr().get()));

	}

 

	/** Returns in json string format. */
	std::string to_json_str() {
		tiledb_ctx_t* c_ctx = ctx_->ptr().get();

		std::stringstream ss;
		ss << "{";

		tiledb_domain_t* domain;
		ctx_->handle_error(tiledb_array_schema_get_domain(
			c_ctx, schema_.get(), &domain));
		ss << "\"dimensions\":[";
		Domain cpp_domain(ctx_, domain);

		std::vector<std::string> dim_names = cpp_domain.dimension_names();// const std::vector<Dimension> dims = cpp_domain.dimensions();

		int ndim = (int)dim_names.size();
		for (int i = 0; i < ndim; ++i) {
			Dimension dim = cpp_domain.dimension(dim_names[i]);
			if (i > 0)
			{
				ss << ",";
			}
			ss << "{";
			ss << "\"name\":\"" << dim.name() << "\"";
			ss << ",\"type\":" << dim.type() ;
			ss << ",\"domain_bound\":\"" << dim.domain_to_str() << "\"";
			ss << "}";
		}

		 

		ss << "]";

		ss << ",\"attributes\":[";
		
		tiledb_attribute_t* attrptr;
		unsigned int nattr;
 
		ctx_->handle_error(tiledb_array_schema_get_attribute_num(
			c_ctx, schema_.get(), &nattr));
 
		for (unsigned int i = 0; i < nattr; ++i) {
			ctx_->handle_error(tiledb_array_schema_get_attribute_from_index(
				c_ctx, schema_.get(), i, &attrptr));
			auto attr = Attribute(ctx_, attrptr);
			if (i > 0)
			{
				ss << ",";
			}
			ss << "{";
			ss << "\"name\":\"" << attr.name() << "\"";
			ss << ",\"dtype\":" << attr.type();
			ss << "}";
		}
		ss << "]";

		

		tiledb_layout_t tile_order_layout;
		ctx_->handle_error(tiledb_array_schema_get_tile_order(
			c_ctx, schema_.get(), &tile_order_layout));
		ss << ",\"tile_order\":" << tile_order_layout;

		tiledb_layout_t cell_order_layout;
		ctx_->handle_error(tiledb_array_schema_get_cell_order(
			c_ctx, schema_.get(), &cell_order_layout));
		ss << ",\"cell_order\":" << cell_order_layout;

		ss << "}";

		return ss.str();
	}

};

/* ********************************* */
/*               MISC                */
/* ********************************* */

/** Converts the array schema into a string representation. */
inline std::ostream& operator<<(std::ostream& os, const tiledb::ArraySchema& schema) {
  os << "ArraySchema<";
  os << tiledb::ArraySchema::to_str(schema.array_type());
  os << ' ' << schema.domain();
  for (const auto& attr_name : schema.attribute_names()) {
    Attribute attr = schema.attribute(attr_name);
    os << ' ' << attr;
  }
  os << '>';
  return os;
}

}  // namespace tiledb

#endif  // TILEDB_CPP_API_ARRAY_SCHEMA_H
