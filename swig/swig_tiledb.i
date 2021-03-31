#ifndef SWIG_TILEDB_I
#define SWIG_TILEDB_I

//////include other i files
%include swig_common.i
//%include protobuf.i

//////

//////shared_ptr

%shared_ptr(tiledb::Array)

%shared_ptr(tiledb::ArraySchema)

%shared_ptr(tiledb::Attribute)

%shared_ptr(tiledb::Config)

%shared_ptr(tiledb::Context)


%shared_ptr(tiledb::Dimension)

%shared_ptr(tiledb::Domain)

%shared_ptr(tiledb::TileDBError)
%shared_ptr(tiledb::TypeError)
%shared_ptr(tiledb::SchemaMismatch)
%shared_ptr(tiledb::AttributeError)

%shared_ptr(tiledb::Filter)

%shared_ptr(tiledb::FilterList)


%shared_ptr(tiledb::Object)

%shared_ptr(tiledb::ObjectIter)

%shared_ptr(tiledb::Query)

%shared_ptr(tiledb::Stats)



%shared_ptr(tiledb::VFS)


//////end shared_ptr

//////
%{

#include "tiledb_cxx_array.h"

#include "tiledb_cxx_array_schema.h"

#include "tiledb_cxx_attribute.h"

#include "tiledb_cxx_config.h"

#include "tiledb_cxx_context.h"

#include "tiledb_cxx_core_interface.h"

#include "tiledb_cxx_dimension.h"

#include "tiledb_cxx_domain.h"

#include "tiledb_cxx_exception.h"

#include "tiledb_cxx_filter.h"

#include "tiledb_cxx_filter_list.h"

#include "tiledb_cxx_group.h"

#include "tiledb_cxx_object.h"

#include "tiledb_cxx_object_iter.h"

#include "tiledb_cxx_query.h"

#include "tiledb_cxx_stats.h"

#include "tiledb_cxx_utils.h"

#include "tiledb_cxx_version.h"

#include "tiledb_cxx_vfs.h"

#include "tiledb_enum.h"
%}

//////ignore

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_array.h
%ignore tiledb::Array::Array(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_query_type_t,tiledb_encryption_type_t,const void *,uint32_t);
%ignore tiledb::Array::Array(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_query_type_t,tiledb_encryption_type_t,const void *,uint32_t,uint64_t);
%ignore tiledb::Array::Array(const std::shared_ptr<tiledb::Context> &,tiledb_array_t *,bool);
%ignore tiledb::Array::operator=(const tiledb::Array);
%ignore tiledb::Array::operator=(tiledb::Array &);
%ignore tiledb::Array::ptr();
%ignore tiledb::Array::open(tiledb_query_type_t,tiledb_encryption_type_t,const void *,uint32_t);
%ignore tiledb::Array::open(tiledb_query_type_t,tiledb_encryption_type_t,const void *,uint32_t,uint64_t);
%ignore tiledb::Array::consolidate(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_encryption_type_t,const void *,uint32_t,tiledb::Config * const);
%ignore tiledb::Array::load_schema(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_encryption_type_t,const void *,uint32_t);
%ignore tiledb::Array::create(const std::string &,const std::shared_ptr<tiledb::ArraySchema> &,tiledb_encryption_type_t,const void *,uint32_t);
%ignore tiledb::Array::non_empty_domain();
%ignore tiledb::Array::non_empty_domain(unsigned);
%ignore tiledb::Array::non_empty_domain(const std::string &);
%ignore tiledb::Array::max_buffer_elements(const std::vector<T> &);
%ignore tiledb::Array::consolidate_metadata(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_encryption_type_t,const void *,uint32_t,tiledb::Config * const);
%ignore tiledb::Array::put_metadata(const std::string &,tiledb_datatype_t,uint32_t,const void *);
%ignore tiledb::Array::get_metadata(const std::string &,tiledb_datatype_t *,uint32_t *,const void * *);
%ignore tiledb::Array::get_metadata_from_index(uint64_t,std::string *,tiledb_datatype_t *,uint32_t *,const void * *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_array_schema.h
%ignore tiledb::ArraySchema::ArraySchema(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_encryption_type_t,const void *,uint32_t);
%ignore tiledb::ArraySchema::ArraySchema(const std::shared_ptr<tiledb::Context> &,tiledb_array_schema_t *);
%ignore tiledb::ArraySchema::operator=(const tiledb::ArraySchema);
%ignore tiledb::ArraySchema::operator=(tiledb::ArraySchema &);
%ignore tiledb::ArraySchema::dump(FILE *);
%ignore tiledb::ArraySchema::ptr();
%ignore tiledb::ArraySchema::attributes();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_attribute.h
%ignore tiledb::Attribute::Attribute(const std::shared_ptr<tiledb::Context> &,tiledb_attribute_t *);
%ignore tiledb::Attribute::operator=(const tiledb::Attribute);
%ignore tiledb::Attribute::operator=(tiledb::Attribute &);
%ignore tiledb::Attribute::ptr();
%ignore tiledb::Attribute::dump(FILE *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_config.h
%ignore ConfigIter;
%ignore ConfigProxy;
%ignore tiledb::Config::Config(tiledb_config_t * *);
%ignore tiledb::Config::ptr();
%ignore tiledb::Config::operator[](const std::string &);
%ignore tiledb::Config::begin(const std::string &);
%ignore tiledb::Config::begin();
%ignore tiledb::Config::end();
%ignore tiledb::Config::free(tiledb_config_t *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_context.h
%ignore tiledb::Context::Context(tiledb_ctx_t *,bool);
%ignore tiledb::Context::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_core_interface.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_dimension.h
%ignore tiledb::Dimension::Dimension(const std::shared_ptr<tiledb::Context> &,tiledb_dimension_t *);
%ignore tiledb::Dimension::operator=(const tiledb::Dimension);
%ignore tiledb::Dimension::operator=(tiledb::Dimension &);
%ignore tiledb::Dimension::domain();
%ignore tiledb::Dimension::ptr();
%ignore tiledb::Dimension::create(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb_datatype_t,const void *,const void *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_domain.h
%ignore tiledb::Domain::Domain(const std::shared_ptr<tiledb::Context> &,tiledb_domain_t *);
%ignore tiledb::Domain::operator=(const tiledb::Domain);
%ignore tiledb::Domain::operator=(tiledb::Domain &);
%ignore tiledb::Domain::cell_num();
%ignore tiledb::Domain::dump(FILE *);
%ignore tiledb::Domain::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_exception.h
%ignore tiledb::TypeError::TypeError(const std::string &);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_filter.h
%ignore tiledb::Filter::Filter(const std::shared_ptr<tiledb::Context> &,tiledb_filter_t *);
%ignore tiledb::Filter::operator=(const tiledb::Filter);
%ignore tiledb::Filter::operator=(tiledb::Filter &);
%ignore tiledb::Filter::ptr();
%ignore tiledb::Filter::set_option(tiledb_filter_option_t,const void *);
%ignore tiledb::Filter::get_option(tiledb_filter_option_t,void *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_filter_list.h
%ignore tiledb::FilterList::FilterList(const std::shared_ptr<tiledb::Context> &,tiledb_filter_list_t *);
%ignore tiledb::FilterList::operator=(const tiledb::FilterList);
%ignore tiledb::FilterList::operator=(tiledb::FilterList &);
%ignore tiledb::FilterList::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_group.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_object.h
%ignore tiledb::Object::Object(Type,const std::string &);
%ignore tiledb::Object::operator=(const tiledb::Object);
%ignore tiledb::Object::operator=(tiledb::Object &);
%ignore tiledb::Object::type();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_object_iter.h
%ignore tiledb::ObjectIter::obj_getter(const char *,tiledb_object_t,void *);
%ignore ObjGetterData;
%ignore iterator;

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_query.h
%ignore tiledb::Query::operator=(const tiledb::Query);
%ignore tiledb::Query::operator=(tiledb::Query &);
%ignore tiledb::Query::ptr();
%ignore tiledb::Query::result_buffer_elements();
%ignore tiledb::Query::set_buffer(const std::string &,void *,uint64_t);
%ignore tiledb::Query::set_buffer(const std::string &,uint64_t *,uint64_t,void *,uint64_t);
%ignore tiledb::Query::get_buffer(const std::string &,void * *,uint64_t *,uint64_t *);
%ignore tiledb::Query::get_buffer(const std::string &,uint64_t * *,uint64_t *,void * *,uint64_t *,uint64_t *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_stats.h
%ignore tiledb::Stats::dump(FILE *);
%ignore tiledb::Stats::raw_dump(FILE *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_utils.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_version.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_vfs.h
%ignore VFSFilebuf;
%ignore tiledb::VFS::operator=(const tiledb::VFS);
%ignore tiledb::VFS::operator=(tiledb::VFS &);
%ignore tiledb::VFS::ptr();
%ignore tiledb::VFS::ls_getter(const char *,void *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_enum.h

//////end ignore

//////headers

%include "tiledb_cxx_array.h"

%include "tiledb_cxx_array_schema.h"

%include "tiledb_cxx_attribute.h"

%include "tiledb_cxx_config.h"

%include "tiledb_cxx_context.h"

%include "tiledb_cxx_core_interface.h"

%include "tiledb_cxx_dimension.h"

%include "tiledb_cxx_domain.h"

%include "tiledb_cxx_exception.h"

%include "tiledb_cxx_filter.h"

%include "tiledb_cxx_filter_list.h"

%include "tiledb_cxx_group.h"

%include "tiledb_cxx_object.h"

%include "tiledb_cxx_object_iter.h"

%include "tiledb_cxx_query.h"

%include "tiledb_cxx_stats.h"

%include "tiledb_cxx_utils.h"

%include "tiledb_cxx_version.h"

%include "tiledb_cxx_vfs.h"

%include "tiledb_enum.h"

//////end headers

#endif
