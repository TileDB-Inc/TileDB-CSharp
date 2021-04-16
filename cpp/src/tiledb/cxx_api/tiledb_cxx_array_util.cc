#include "tiledb_cxx_array_util.h"
//#include "tiledb_cxx_json.hpp"
//#include "tiledb_cxx_string_util.h"

namespace tiledb {

	std::string ArrayUtil::get_tiledb_version()
	{
		std::vector<int> t = tiledb::version();
		return std::to_string(t[0]) + "." + std::to_string(t[1]) + "." + std::to_string(t[2]);
	}//std::string ArrayUtil::get_tiledb_version()


 


}//namespace