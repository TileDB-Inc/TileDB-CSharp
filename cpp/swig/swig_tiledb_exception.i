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

 //////
 %insert(runtime) %{
  // Code to handle throwing of C# TileDBErrorApplicationException from C/C++ code.
  // The equivalent delegate to the callback, CSharpExceptionCallback_t, is TileDBErrorExceptionDelegate
  // and the equivalent tileDBErrorExceptionCallback instance is tileDBErrorDelegate
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);
  CSharpExceptionCallback_t tileDBErrorExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL TileDBErrorExceptionRegisterCallback(CSharpExceptionCallback_t tileDBErrorCallback) {
    tileDBErrorExceptionCallback = tileDBErrorCallback;
  }

  // Note that SWIG detects any method calls named starting with
  // SWIG_CSharpSetPendingException for warning 845
  static void SWIG_CSharpSetPendingExceptionTileDBError(const char *msg) {
    tileDBErrorExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class TileDBErrorExceptionHelper {
    // C# delegate for the C/C++ tileDBErrorExceptionCallback
    public delegate void TileDBErrorExceptionDelegate(string message);
    static TileDBErrorExceptionDelegate tileDBErrorDelegate =
                                   new TileDBErrorExceptionDelegate(SetPendingTileDBErrorException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="TileDBErrorExceptionRegisterCallback")]
    public static extern
           void TileDBErrorExceptionRegisterCallback(TileDBErrorExceptionDelegate tileDBErrorCallback);

    static void SetPendingTileDBErrorException(string message) {
      SWIGPendingException.Set(new TileDB.TileDBError(message));
    }

    static TileDBErrorExceptionHelper() {
      TileDBErrorExceptionRegisterCallback(tileDBErrorDelegate);
    }
  }
  static TileDBErrorExceptionHelper tileDBErrorExceptionHelper = new TileDBErrorExceptionHelper();
%}


%typemap(throws, canthrow=1) tiledb::TileDBError {
  SWIG_CSharpSetPendingExceptionTileDBError($1.what());
  return $null;  
}
 //////

%insert(runtime) %{
  // Code to handle throwing of C# TypeErrorApplicationException from C/C++ code.
  // The equivalent delegate to the callback, CSharpExceptionCallback_t, is TypeErrorExceptionDelegate
  // and the equivalent typeErrorExceptionCallback instance is typeErrorDelegate
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);
  CSharpExceptionCallback_t typeErrorExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL TypeErrorExceptionRegisterCallback(CSharpExceptionCallback_t typeErrorCallback) {
    typeErrorExceptionCallback = typeErrorCallback;
  }

  // Note that SWIG detects any method calls named starting with
  // SWIG_CSharpSetPendingException for warning 845
  static void SWIG_CSharpSetPendingExceptionTypeError(const char *msg) {
    typeErrorExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class TypeErrorExceptionHelper {
    // C# delegate for the C/C++ typeErrorExceptionCallback
    public delegate void TypeErrorExceptionDelegate(string message);
    static TypeErrorExceptionDelegate typeErrorDelegate =
                                   new TypeErrorExceptionDelegate(SetPendingTypeErrorException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="TypeErrorExceptionRegisterCallback")]
    public static extern
           void TypeErrorExceptionRegisterCallback(TypeErrorExceptionDelegate typeErrorCallback);

    static void SetPendingTypeErrorException(string message) {
      SWIGPendingException.Set(new TileDB.TypeError(message));
    }

    static TypeErrorExceptionHelper() {
      TypeErrorExceptionRegisterCallback(typeErrorDelegate);
    }
  }
  static TypeErrorExceptionHelper typeErrorExceptionHelper = new TypeErrorExceptionHelper();
%}


%typemap(throws, canthrow=1) tiledb::TypeError {
  SWIG_CSharpSetPendingExceptionTypeError($1.what());
  return $null;  
}
//////

%insert(runtime) %{
  // Code to handle throwing of C# SchemaMismatchApplicationException from C/C++ code.
  // The equivalent delegate to the callback, CSharpExceptionCallback_t, is SchemaMismatchExceptionDelegate
  // and the equivalent schemaMismatchExceptionCallback instance is schemaMismatchDelegate
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);
  CSharpExceptionCallback_t schemaMismatchExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL SchemaMismatchExceptionRegisterCallback(CSharpExceptionCallback_t schemaMismatchCallback) {
    schemaMismatchExceptionCallback = schemaMismatchCallback;
  }

  // Note that SWIG detects any method calls named starting with
  // SWIG_CSharpSetPendingException for warning 845
  static void SWIG_CSharpSetPendingExceptionSchemaMismatch(const char *msg) {
    schemaMismatchExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class SchemaMismatchExceptionHelper {
    // C# delegate for the C/C++ schemaMismatchExceptionCallback
    public delegate void SchemaMismatchExceptionDelegate(string message);
    static SchemaMismatchExceptionDelegate schemaMismatchDelegate =
                                   new SchemaMismatchExceptionDelegate(SetPendingSchemaMismatchException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="SchemaMismatchExceptionRegisterCallback")]
    public static extern
           void SchemaMismatchExceptionRegisterCallback(SchemaMismatchExceptionDelegate schemaMismatchCallback);

    static void SetPendingSchemaMismatchException(string message) {
      SWIGPendingException.Set(new TileDB.SchemaMismatch(message));
    }

    static SchemaMismatchExceptionHelper() {
      SchemaMismatchExceptionRegisterCallback(schemaMismatchDelegate);
    }
  }
  static SchemaMismatchExceptionHelper schemaMismatchExceptionHelper = new SchemaMismatchExceptionHelper();
%}


%typemap(throws, canthrow=1) tiledb::SchemaMismatch {
  SWIG_CSharpSetPendingExceptionSchemaMismatch($1.what());
  return $null;  
}

//////

%insert(runtime) %{
  // Code to handle throwing of C# AttributeErrorApplicationException from C/C++ code.
  // The equivalent delegate to the callback, CSharpExceptionCallback_t, is AttributeErrorExceptionDelegate
  // and the equivalent attributeErrorExceptionCallback instance is attributeErrorDelegate
  typedef void (SWIGSTDCALL* CSharpExceptionCallback_t)(const char *);
  CSharpExceptionCallback_t attributeErrorExceptionCallback = NULL;

  extern "C" SWIGEXPORT
  void SWIGSTDCALL AttributeErrorExceptionRegisterCallback(CSharpExceptionCallback_t attributeErrorCallback) {
    attributeErrorExceptionCallback = attributeErrorCallback;
  }

  // Note that SWIG detects any method calls named starting with
  // SWIG_CSharpSetPendingException for warning 845
  static void SWIG_CSharpSetPendingExceptionAttributeError(const char *msg) {
    attributeErrorExceptionCallback(msg);
  }
%}

%pragma(csharp) imclasscode=%{
  class AttributeErrorExceptionHelper {
    // C# delegate for the C/C++ attributeErrorExceptionCallback
    public delegate void AttributeErrorExceptionDelegate(string message);
    static AttributeErrorExceptionDelegate attributeErrorDelegate =
                                   new AttributeErrorExceptionDelegate(SetPendingAttributeErrorException);

    [global::System.Runtime.InteropServices.DllImport("$dllimport", EntryPoint="AttributeErrorExceptionRegisterCallback")]
    public static extern
           void AttributeErrorExceptionRegisterCallback(AttributeErrorExceptionDelegate attributeErrorCallback);

    static void SetPendingAttributeErrorException(string message) {
      SWIGPendingException.Set(new TileDB.AttributeError(message));
    }

    static AttributeErrorExceptionHelper() {
      AttributeErrorExceptionRegisterCallback(attributeErrorDelegate);
    }
  }
  static AttributeErrorExceptionHelper attributeErrorExceptionHelper = new AttributeErrorExceptionHelper();
%}


%typemap(throws, canthrow=1) tiledb::AttributeError {
  SWIG_CSharpSetPendingExceptionAttributeError($1.what());
  return $null;  
}

//////


%exception {
  try {
    $action
  } catch(const tiledb::TileDBError& e) {
    SWIG_CSharpSetPendingExceptionTileDBError(e.what());//SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(const tiledb::TypeError& e) {
    SWIG_CSharpSetPendingExceptionTypeError(e.what());//SWIG_exception(SWIG_RuntimeError, e.what());  
  } catch(const tiledb::SchemaMismatch& e) {
     SWIG_CSharpSetPendingExceptionSchemaMismatch(e.what());//SWIG_exception(SWIG_RuntimeError, e.what());   
  } catch(const tiledb::AttributeError& e) {
     SWIG_CSharpSetPendingExceptionAttributeError(e.what());// SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(const std::exception& e) {
      SWIG_exception(SWIG_RuntimeError, e.what());
  } catch(...) {
    SWIG_exception(SWIG_UnknownError, "");
  }
}

//////
%exceptionclass tiledb::TileDBError;
%exceptionclass tiledb::TypeError;
%exceptionclass tiledb::SchemaMismatch;
%exceptionclass tiledb::AttributeError;


%include "tiledb_cxx_exception.h" 



#endif