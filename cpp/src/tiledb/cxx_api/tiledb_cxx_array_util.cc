#include "tiledb_cxx_array_util.h"
//#include "tiledb_cxx_json.hpp"
//#include "tiledb_cxx_string_util.h"

namespace tiledb {

	std::string ArrayUtil::get_tiledb_version()
	{
		std::tuple<int, int, int> t = tiledb::version();
		return std::to_string(std::get<0>(t)) + "." + std::to_string(std::get<1>(t)) + "." + std::to_string(std::get<2>(t));
	}//std::string ArrayUtil::get_tiledb_version()


 


}//namespace