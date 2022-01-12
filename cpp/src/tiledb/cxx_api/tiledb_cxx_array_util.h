#pragma once
#ifndef TILEDB_ARRAY_UTIL_H
#define TILEDB_ARRAY_UTIL_H

/**
* \file array_util.h
*
* \author Bin Deng (bin.deng@tiledb.com)
*
* \brief  Provide utility functions for tiledb array.
*
* \description
*	This component provides utility functions for tiledb array.
*/

#include <limits>
#include <memory>

#include "json.h" 
#include "tiledb_cxx_array.h"
#include "tiledb_cxx_array_schema.h"
#include "tiledb_cxx_attribute.h"
#include "tiledb_cxx_config.h"
#include "tiledb_cxx_context.h"
#include "tiledb_cxx_deleter.h"
#include "tiledb_cxx_dimension.h"
#include "tiledb_cxx_domain.h"
#include "tiledb_cxx_exception.h"
#include "tiledb_cxx_filter.h"
#include "tiledb_cxx_filter_list.h"
#include "tiledb_cxx_group.h"
#include "tiledb_cxx_object.h"
#include "tiledb_cxx_object_iter.h"
#include "tiledb_cxx_query.h"
#include "tiledb_cxx_schema_base.h"
#include "tiledb_cxx_stats.h"
#include "tiledb.h"
#include "tiledb_cxx_utils.h"
#include "tiledb_cxx_version.h"
#include "tiledb_cxx_vfs.h"

namespace tiledb {

class ArrayUtil {
public:
	/**
	* @name Constructor and Destructor
	*/
	///@{

	/**
	* @brief constructor
	*/
	ArrayUtil() {}

	/**
	* @brief copy constructor
	*/
	ArrayUtil(const tiledb::ArrayUtil& from) {} // = delete;

    /**
	 * @brief copy assignment
	*/
	ArrayUtil& operator=(const tiledb::ArrayUtil& from) { return *this; } // = delete;

	/**
	* @brief destructor
	*/
	virtual ~ArrayUtil() {}

	///@}

public:
	/**
	* @brief get version
	*
	* @return version string
	*/
	static std::string get_tiledb_version();
    
	/**
	* @brief get array schema json string
	* 
	* @param ctx
	* @param uri
	* 
	* @return json string representation of array schema
	*/
    static std::string get_array_schema_json_str(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri);
    
	/**
	* @brief get array metadata json string
	*
	* @param ctx
	* @param uri
	*
	* @return json string representation of array metadata
	*/
    static std::string get_array_metadata_json_str(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri);
    
	/**
	* @brief get array metadata json string for a key
	*
	* @param ctx
	* @param uri
	* @param key
	*
	* @return json string representation of array metadata
	*/
    static std::string get_array_metadata_json_str_for_key(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri, const std::string& key);
    
	/**
	* @brief get array metadata json string from an index
	*
	* @param ctx
	* @param uri
	* @param index
	*
	* @return json string representation of array metadata
	*/
    static std::string get_array_metadata_json_str_from_index(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri, uint64_t index);
       
	/**
	* @brief add array metadata from json string
	*
	* @param ctx
	* @param uri
	* @param jsonstr
	*
	* @return json string representation of array metadata
	*/
    static void add_array_metadata_by_json_str(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri, const std::string& jsonstr);
    
	/**
	* @brief add array metadata from json string
	*
	* @param ctx
	* @param uri
	* @param key
	* @param jsonstr
	*
	* @return json string representation of array metadata
	*/
    static void add_array_metadata_by_json_str_for_key(const std::shared_ptr<tiledb::Context>& ctx, const std::string& uri, const std::string& key, const std::string& jsonstr);
    
    
	/**
	* @brief export file to a path
	*/
	static int export_file_to_path(const std::shared_ptr<tiledb::Context>& ctx, const std::string& file_uri, const std::string& output_path, uint64_t buffer_size);

	/**
	* @brief save file from path
	*/
	static int save_file_from_path(const std::shared_ptr<tiledb::Context>& ctx, const std::string& file_uri, const std::string& input_path, const std::string& mime_type, const std::string& mime_coding);
	////TODO add more help functions
 

};//class ArrayUtil

}//namespace 

#endif
