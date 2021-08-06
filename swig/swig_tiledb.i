#ifndef SWIG_TILEDB_I
#define SWIG_TILEDB_I

//////include other i files
%include swig_common.i
%include swig_tiledb_exception.i

//////

//////shared_ptr

%shared_ptr(tiledb::Array)

%shared_ptr(tiledb::ArraySchema)


%shared_ptr(tiledb::Attribute)

%shared_ptr(tiledb::Config)

%shared_ptr(tiledb::Context)

%shared_ptr(tiledb::Dimension)

%shared_ptr(tiledb::Domain)


%shared_ptr(tiledb::Filter)

%shared_ptr(tiledb::FilterList)


%shared_ptr(tiledb::Query)

%shared_ptr(tiledb::QueryCondition)

%shared_ptr(tiledb::Stats)


%shared_ptr(tiledb::VFS)

//////end shared_ptr

//////
%{

#include "tiledb_cxx_array.h"

#include "tiledb_cxx_array_schema.h"

#include "tiledb_cxx_array_util.h"

#include "tiledb_cxx_attribute.h"

#include "tiledb_cxx_config.h"

#include "tiledb_cxx_context.h"

#include "tiledb_cxx_dimension.h"

#include "tiledb_cxx_domain.h"

#include "tiledb_cxx_enum.h"

#include "tiledb_cxx_filter.h"

#include "tiledb_cxx_filter_list.h"

#include "tiledb_cxx_group.h"

#include "tiledb_cxx_query.h"

#include "tiledb_cxx_query_condition.h"

#include "tiledb_cxx_stats.h"

#include "tiledb_cxx_version.h"

#include "tiledb_cxx_vfs.h"
%}

//////ignore

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_array.h
%ignore tiledb::Array::Array(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::QueryType,tiledb::EncryptionType,const void *,uint32_t);
%ignore tiledb::Array::Array(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::QueryType,tiledb::EncryptionType,const void *,uint32_t,uint64_t);
%ignore tiledb::Array::operator=(const tiledb::Array);
%ignore tiledb::Array::operator=(tiledb::Array &);
%ignore tiledb::Array::ptr();
%ignore tiledb::Array::open(tiledb::QueryType,tiledb::EncryptionType,const void *,uint32_t);
%ignore tiledb::Array::open(tiledb::QueryType,tiledb::EncryptionType,const void *,uint32_t,uint64_t);
%ignore tiledb::Array::consolidate(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::EncryptionType,const void *,uint32_t,tiledb::Config * const);
%ignore tiledb::Array::load_schema(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::EncryptionType,const void *,uint32_t);
%ignore tiledb::Array::create(const std::string &,const std::shared_ptr<tiledb::ArraySchema> &,tiledb::EncryptionType,const void *,uint32_t);
%ignore tiledb::Array::non_empty_domain();
%ignore tiledb::Array::non_empty_domain(unsigned);
%ignore tiledb::Array::non_empty_domain(const std::string &);
%ignore tiledb::Array::consolidate_metadata(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::EncryptionType,const void *,uint32_t,tiledb::Config * const);
%ignore tiledb::Array::put_metadata(const std::string &,tiledb::DataType,uint32_t,const void *);
%ignore tiledb::Array::get_metadata(const std::string &,tiledb::DataType *,uint32_t *,const void * *);
%ignore tiledb::Array::get_metadata_from_index(uint64_t,std::string *,tiledb::DataType *,uint32_t *,const void * *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_array_schema.h
%ignore tiledb::ArraySchema::ArraySchema(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::EncryptionType,const void *,uint32_t);
%ignore tiledb::ArraySchema::ArraySchema(const std::shared_ptr<tiledb::Context> &,tiledb_array_schema_t *);
%ignore tiledb::ArraySchema::operator=(const tiledb::ArraySchema);
%ignore tiledb::ArraySchema::operator=(tiledb::ArraySchema &);
%ignore tiledb::ArraySchema::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_array_util.h
%ignore tiledb::ArrayUtil::operator=(const tiledb::ArrayUtil &);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_attribute.h
%ignore tiledb::Attribute::Attribute(const std::shared_ptr<tiledb::Context> &,tiledb_attribute_t *);
%ignore tiledb::Attribute::operator=(const tiledb::Attribute);
%ignore tiledb::Attribute::operator=(tiledb::Attribute &);
%ignore tiledb::Attribute::ptr();

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
%ignore tiledb::Context::ptr();
%ignore tiledb::Context::set_error_handler(const std::function<void ( const std::string & )> &);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_dimension.h
%ignore tiledb::Dimension::Dimension(const std::shared_ptr<tiledb::Context> &,tiledb_dimension_t *);
%ignore tiledb::Dimension::operator=(const tiledb::Dimension);
%ignore tiledb::Dimension::operator=(tiledb::Dimension &);
%ignore tiledb::Dimension::domain();
%ignore tiledb::Dimension::ptr();
%ignore tiledb::Dimension::create(const std::shared_ptr<tiledb::Context> &,const std::string &,tiledb::DataType,const void *,const void *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_domain.h
%ignore tiledb::Domain::Domain(const std::shared_ptr<tiledb::Context> &,tiledb_domain_t *);
%ignore tiledb::Domain::operator=(const tiledb::Domain);
%ignore tiledb::Domain::operator=(tiledb::Domain &);
%ignore tiledb::Domain::cell_num();
%ignore tiledb::Domain::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_enum.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_filter.h
%ignore tiledb::Filter::Filter(const std::shared_ptr<tiledb::Context> &,tiledb_filter_t *);
%ignore tiledb::Filter::operator=(const tiledb::Filter);
%ignore tiledb::Filter::operator=(tiledb::Filter &);
%ignore tiledb::Filter::ptr();
%ignore tiledb::Filter::set_option(FilterOption,const void *);
%ignore tiledb::Filter::get_option(FilterOption,void *);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_filter_list.h
%ignore tiledb::FilterList::FilterList(const std::shared_ptr<tiledb::Context> &,tiledb_filter_list_t *);
%ignore tiledb::FilterList::operator=(const tiledb::FilterList);
%ignore tiledb::FilterList::operator=(tiledb::FilterList &);
%ignore tiledb::FilterList::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_group.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_query.h
%ignore tiledb::Query::operator=(const tiledb::Query);
%ignore tiledb::Query::operator=(tiledb::Query &);
%ignore tiledb::Query::ptr();
%ignore tiledb::Query::submit_async(const Fn &);
%ignore tiledb::Query::submit_async();
%ignore tiledb::Query::set_buffer(const std::string &,void *,uint64_t);
%ignore tiledb::Query::set_buffer(const std::string &,uint64_t *,uint64_t,void *,uint64_t);
%ignore tiledb::Query::get_buffer(const std::string &,void * *,uint64_t *,uint64_t *);
%ignore tiledb::Query::get_buffer(const std::string &,uint64_t * *,uint64_t *,void * *,uint64_t *,uint64_t *);
%ignore tiledb::Query::to_status(const tiledb_query_status_t &);

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_query_condition.h
%ignore tiledb::QueryCondition::QueryCondition(const std::shared_ptr<tiledb::Context> &,tiledb_query_condition_t * const);
%ignore tiledb::QueryCondition::operator=(const QueryCondition);
%ignore tiledb::QueryCondition::operator=(QueryCondition &);
%ignore tiledb::QueryCondition::init(const std::string &,const void *,uint64_t,tiledb::QueryConditionOperatorType);
%ignore tiledb::QueryCondition::ptr();

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_stats.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_version.h

//ignore class or methods in file:../cpp/src/tiledb/cxx_api/tiledb_cxx_vfs.h
%ignore VFSFilebuf;
%ignore tiledb::VFS::operator=(const tiledb::VFS);
%ignore tiledb::VFS::operator=(tiledb::VFS &);
%ignore tiledb::VFS::ptr();
%ignore tiledb::VFS::ls_getter(const char *,void *);

//////end ignore

//////headers

%include "tiledb_cxx_array.h"

%include "tiledb_cxx_array_schema.h"

%include "tiledb_cxx_array_util.h"

%include "tiledb_cxx_attribute.h"

%include "tiledb_cxx_config.h"

%include "tiledb_cxx_context.h"

%include "tiledb_cxx_dimension.h"

%include "tiledb_cxx_domain.h"

%include "tiledb_cxx_enum.h"

%include "tiledb_cxx_filter.h"

%include "tiledb_cxx_filter_list.h"

%include "tiledb_cxx_group.h"

%include "tiledb_cxx_query.h"

%include "tiledb_cxx_query_condition.h"

%include "tiledb_cxx_stats.h"

%include "tiledb_cxx_version.h"

%include "tiledb_cxx_vfs.h"

//////end headers

#endif
