#include "tiledb_cxx_array_util.h"


namespace tiledb {

std::string ArrayUtil::get_tiledb_version()
{
  std::vector<int> t = tiledb::version();
  return std::to_string(t[0]) + "." + std::to_string(t[1]) + "." + std::to_string(t[2]);
}//std::string ArrayUtil::get_tiledb_version()

int ArrayUtil::export_file_to_path(const std::string& file_uri, const std::string& output_path, uint64_t buffer_size, const std::shared_ptr<tiledb::Context>& ctx)
{
  const std::string METADATA_SIZE_KEY = "file_size";
  const std::string FILE_ATTRIBUTE_NAME = "contents";
  const std::string METADATA_ORIGINAL_FILE_NAME = "original_file_name";
  try {

    std::shared_ptr<Array> array = std::shared_ptr<Array>(new Array(ctx, file_uri, tiledb::QueryType::TILEDB_READ));

    //get file_size from metadata
    uint32_t v_num;
    const void* v_r;
    tiledb::DataType file_size_datatype = tiledb::DataType::TILEDB_UINT64;
    array->get_metadata(METADATA_SIZE_KEY, &file_size_datatype, &v_num, &v_r);
    uint64_t file_size = *((const int64_t*)v_r);
    if(buffer_size==0) {
      buffer_size = file_size;
    }

    std::string output_file = output_path;
    if(output_file.empty()) {
      tiledb::DataType file_name_datatype = tiledb::DataType::TILEDB_STRING_ASCII;
      uint32_t name_size = 0;
      char original_name[1024] = "";
      array->get_metadata(METADATA_ORIGINAL_FILE_NAME,&file_name_datatype,&name_size,reinterpret_cast<const void**>(&original_name));
      output_file = std::string(original_name);
    }
    
    std::shared_ptr<tiledb::VFS> vfs = std::shared_ptr<tiledb::VFS>(new tiledb::VFS(ctx));
    tiledb::impl::VFSFilebuf filebuf = tiledb::impl::VFSFilebuf(vfs);
    filebuf.open(output_file, std::ios::out);
    std::ostream os(&filebuf);   
    
    uint64_t offset = 0;
    std::array<uint64_t, 2> subarray = { offset, offset + buffer_size - 1 };

    Query query(ctx, array, tiledb::QueryType::TILEDB_READ);
    query.set_layout(tiledb::LayoutType::TILEDB_ROW_MAJOR)
      .set_subarray(subarray);
    std::vector<uint8_t> data_buffer;
    data_buffer.resize(buffer_size);
    query.set_buffer(FILE_ATTRIBUTE_NAME, data_buffer);

    while (query.query_status() != QueryStatus::TILEDB_COMPLETED) {
      query.submit();
      auto result_buffer_elements = query.result_buffer_elements();
      if (result_buffer_elements.find(FILE_ATTRIBUTE_NAME) != result_buffer_elements.end()) {
        uint64_t read_size = result_buffer_elements[FILE_ATTRIBUTE_NAME][1];
        os.write((const char*)data_buffer.data(), read_size);
      }
      offset += buffer_size;
      subarray[0] = offset;
      subarray[1] = (file_size > (offset + buffer_size)) ? (offset + buffer_size -1) : (file_size -1);
      query.set_subarray(subarray);
    }

    array->close();
    os.flush();
    filebuf.close();
  }
  catch (const std::exception& e) {
    std::cout << "ArrayUtil::export_file_to_path, caught error:" << e.what() << std::endl;
    return -1;
  }
  catch (...) {
    std::cout << "ArrayUtil::export_file_to_path, caught unknown error!" << std::endl;
    return -2;
  }

  return 0;
}
 


}//namespace