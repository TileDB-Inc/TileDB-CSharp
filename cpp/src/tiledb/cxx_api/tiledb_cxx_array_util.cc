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
    if (buffer_size <= 0) {
      std::cout << "ArrayUtil::export_file_to_path, wrong buffer_size!" << std::endl;
      return -1;
    }

    std::string output_file = output_path;
    tiledb::DataType file_name_datatype = tiledb::DataType::TILEDB_STRING_ASCII;
    if(output_file.empty() && array->has_metadata(METADATA_ORIGINAL_FILE_NAME, file_name_datatype)) {
      uint32_t name_size = 0;
      const char* original_name;
      array->get_metadata(METADATA_ORIGINAL_FILE_NAME,&file_name_datatype,&name_size,reinterpret_cast<const void**>(&original_name));
      output_file = std::string(original_name, name_size);
    }
    
    std::shared_ptr<tiledb::VFS> vfs = std::shared_ptr<tiledb::VFS>(new tiledb::VFS(ctx));
    tiledb::VFS::filebuf filebuf = tiledb::VFS::filebuf(vfs);
    if (filebuf.open(output_file, std::ios::out) == nullptr) {
      std::cout << "ArrayUtil::export_file_to_path, can not open output_path:" << output_path << " for writing!" << std::endl;
      return -1;
    }
    std::ostream os(&filebuf);   
    
    std::array<uint64_t, 2> subarray = { 0, file_size - 1 };

    Query query(ctx, array, tiledb::QueryType::TILEDB_READ);
    query.set_layout(tiledb::LayoutType::TILEDB_ROW_MAJOR)
      .set_subarray(subarray);
    
    std::vector<uint8_t> data_buffer(buffer_size);
    query.set_buffer(FILE_ATTRIBUTE_NAME, data_buffer);

    int loop_zero_num = 0;
    uint64_t read_size = 0;
    while (query.query_status() != QueryStatus::TILEDB_COMPLETED) {
      query.submit();
      auto result_buffer_elements = query.result_buffer_elements();
      if (result_buffer_elements.find(FILE_ATTRIBUTE_NAME) != result_buffer_elements.end()) {
        read_size = result_buffer_elements[FILE_ATTRIBUTE_NAME][1];
        os.write((const char*)data_buffer.data(), read_size);
      }

      if ( read_size == 0 && (++loop_zero_num) > 10) {
        break;
      }
      else {
        read_size = 0;
      }
    }

    array->close();
    os.flush();
    filebuf.close();
  }
  catch (tiledb::TileDBError& tdbe) {
    std::cout << "ArrayUtil::export_file_to_path, caught error:" << tdbe.what() << std::endl;
  }
  catch (const std::exception& e) {
    std::cout << "ArrayUtil::export_file_to_path, caught error:" << e.what() << std::endl;
    return -2;
  }
  catch (...) {
    std::cout << "ArrayUtil::export_file_to_path, caught unknown error!" << std::endl;
    return -3;
  }

  return 0;
}
 
int ArrayUtil::save_file_from_path(const std::string& file_uri, const std::string& input_path, const std::string& mime_type, const std::string& mime_encoding, const std::shared_ptr<tiledb::Context>& ctx) {
  const std::string METADATA_SIZE_KEY = "file_size";
  const std::string FILE_DIMENSION_NAME = "position";
  const std::string FILE_ATTRIBUTE_NAME = "contents";
  const std::string FILE_METADATA_MIME_TYPE_KEY = "mime";
  const std::string FILE_METADATA_MIME_ENCODING_KEY = "mime_encoding";
  const std::string METADATA_ORIGINAL_FILE_NAME = "original_file_name";
  try {
    if (input_path.empty()) {
      std::cout << "ArrayUtil::save_file_from_path, input_path can not be empty!" << std::endl;
      return -1;
    }
    std::shared_ptr<tiledb::VFS> vfs = std::shared_ptr<tiledb::VFS>(new tiledb::VFS(ctx));
    tiledb::VFS::filebuf filebuf = tiledb::VFS::filebuf(vfs);
    filebuf.open(input_path, std::ios::in);
    std::istream os(&filebuf);
    uint64_t nbytes = vfs->file_size(input_path);


    uint64_t tile_extent = 1024;
    if (nbytes > (1024ULL * 1024ULL * 1024ULL * 10ULL)) {
      tile_extent = 1024ULL * 1024 * 100;
    }
    else if (nbytes > (1024UL * 1024 * 100)) {
      tile_extent = 1024ULL * 1024 * 1;
    }
    else if (nbytes > (1024UL * 1024 * 1)) {
      tile_extent = 1024ULL * 256;
    }

    std::shared_ptr<tiledb::ArraySchema> file_schema = std::shared_ptr<tiledb::ArraySchema>(new tiledb::ArraySchema(ctx, tiledb::ArrayType::TILEDB_DENSE));
    tiledb::Domain dom(ctx);
    std::array<uint64_t, 2> dim_domain = { 0,std::numeric_limits<uint64_t>::max() - tile_extent -2 };
    tiledb::Dimension dim = tiledb::Dimension::create(ctx, FILE_DIMENSION_NAME, dim_domain, tile_extent);
    dom.add_dimension(dim);
    file_schema->set_domain(dom);
    tiledb::Attribute attr = tiledb::Attribute::create_attribute(ctx, "contents", tiledb::DataType::TILEDB_UINT8);
    attr.set_cell_val_num(1);
    file_schema->add_attribute(attr);

    tiledb::Array::create(file_uri, file_schema);

    std::shared_ptr<Array> array = std::shared_ptr<Array>(new Array(ctx, file_uri, tiledb::QueryType::TILEDB_WRITE));
    Query query(ctx, array, tiledb::QueryType::TILEDB_WRITE);

    std::vector<uint8_t> contents;
    contents.resize(nbytes);
    std::array<uint64_t, 2> subarray = { 0,nbytes - 1 };
    query.set_layout(tiledb::LayoutType::TILEDB_ROW_MAJOR);
    query.set_subarray(subarray);
    os.read((char*)contents.data(), nbytes);
      
    query.set_buffer(FILE_ATTRIBUTE_NAME, contents);
    query.submit();
    query.finalize();
    
    //save metadata
    array->put_metadata(METADATA_SIZE_KEY, tiledb::DataType::TILEDB_UINT64, 1, &nbytes);
    array->put_metadata(METADATA_ORIGINAL_FILE_NAME, tiledb::DataType::TILEDB_STRING_ASCII,
      static_cast<uint32_t>(input_path.size()), input_path.c_str());
    std::string mimetype = mime_type.empty() ? "None" : mime_type;
    array->put_metadata(FILE_METADATA_MIME_TYPE_KEY, tiledb::DataType::TILEDB_STRING_ASCII,
      static_cast<uint32_t>(mimetype.size()), mimetype.c_str());
    std::string mimeencoding = mime_encoding.empty() ? "None" : mime_encoding;
    array->put_metadata(FILE_METADATA_MIME_ENCODING_KEY, tiledb::DataType::TILEDB_STRING_ASCII,
      static_cast<uint32_t>(mimeencoding.size()), mimeencoding.c_str());

    array->close();
    filebuf.close();
  }
  catch (tiledb::TileDBError& tdbe) {
    std::cout << "ArrayUtil::save_file_from_path, caught error:" << tdbe.what() << std::endl;
    return -1;
  }
  catch (const std::exception& e) {
    std::cout << "ArrayUtil::save_file_from_path, caught error:" << e.what() << std::endl;
    return -2;
  }
  catch (...) {
    std::cout << "ArrayUtil::save_file_from_path, caught unknown error!" << std::endl;
    return -3;
  }
  return 0;
}

}//namespace