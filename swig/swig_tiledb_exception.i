#ifndef SWIG_TILEDB_EXCEPTION_I
#define SWIG_TILEDB_EXCEPTION_I

%{
#include "tiledb_cxx_exception.h"
%}

%include "std_string.i"
%include "exception.i"
%include "std_shared_ptr.i"

%shared_ptr(tiledb::TileDBError)
%shared_ptr(tiledb::TypeError)
%shared_ptr(tiledb::SchemaMismatch)
%shared_ptr(tiledb::AttributeError)

%exception {
  try {
    $action
  } catch(const tiledb::TileDBError& e) {
    SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(const tiledb::TypeError& e) {
    SWIG_exception(SWIG_RuntimeError, e.what());  
  } catch(const tiledb::SchemaMismatch& e) {
     SWIG_exception(SWIG_RuntimeError, e.what());   
  } catch(const tiledb::AttributeError& e) {
      SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(const std::exception& e) {
      SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(...) {
    SWIG_exception(SWIG_UnknownError, "");
  }
}

%exceptionclass tiledb::TileDBError;
%exceptionclass tiledb::TypeError;
%exceptionclass tiledb::SchemaMismatch;
%exceptionclass tiledb::AttributeError;

%include "tiledb_cxx_exception.h"

#endif