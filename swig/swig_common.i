#ifndef SWIG_COMMON_I
#define SWIG_COMMON_I

//%module tiledbswig 

#ifdef SWIGPYTHON
//	define something related to python wrapper
//		 
//
%module tiledbpy
%include "cpointer.i"	 
 
#endif

#ifdef SWIGCSHARP
//	define something related to csharp wrapper
//		 
%module tiledbcs
 
%include "arrays_csharp.i"

//	%pragma(csharp)
//	imclassclassmodifiers="[System.Security.SuppressUnmanagedCodeSecurity]\npublic class"
#endif
/////////////////////////////////////////////////


%include "stdint.i"

%include "typemaps.i"

%include "std_except.i"
%include "std_common.i"
%include "stl.i"
%include "std_string.i"
%include "std_vector.i"
%include "std_map.i"
%include "std_list.i"
%include "std_set.i"
%include "std_shared_ptr.i"
%include "exception.i"

namespace std {

  %template(VectorInt32) vector<int>;
  %template(VectorUInt32) vector<unsigned int>;
  %template(VectorInt64) vector<int64_t>;
  %template(VectorUInt64) vector<uint64_t>;
  %template(VectorChar) vector<char>;
  %template(VectorUChar) vector<unsigned char>;
  %template(VectorDouble) vector<double>;
  %template(VectorString) vector<string>;
  %template(VectorConstChar) vector<const char*>;

  %template(MapStringString) map<string,string>;
  %template(MapStringInt32) map<string,int>;
  %template(MapStringUInt32) map<string,uint32_t>;
  %template(MapStringInt64) map<string,int64_t>;
  %template(MapStringUInt64) map<string,uint64_t>;
  %template(MapStringDouble) map<string,double>;
  %template(MapStringChar) map<string,char>;
  %template(MapStringUChar) map<string,unsigned char>;

  %template(MapInt32String) map<int,string>;
  %template(MapInt32Int32) map<int,int>;
  %template(MapInt32UInt32) map<int,uint32_t>;
  %template(MapInt32Int64) map<int,int64_t>;
  %template(MapInt32UInt64) map<int,uint64_t>;
  %template(MapInt32Double) map<int,double>;
  %template(MapInt32Char) map<int,char>;
  %template(MapInt32UChar) map<int,unsigned char>;

  %template(MapInt64String) map<int64_t,string>;
  %template(MapInt64Int32) map<int64_t,int>;
  %template(MapInt64UInt32) map<int64_t,uint32_t>;
  %template(MapInt64Int64) map<int64_t,int64_t>;
  %template(MapInt64UInt64) map<int64_t,uint64_t>;
  %template(MapInt64Double) map<int64_t,double>;
  %template(MapInt64Char) map<int64_t,char>;
  %template(MapInt64UChar) map<int64_t,unsigned char>;    



}

////wrap void * to System.IntPtr in C#
%typemap(ctype)  void* "void *"
%typemap(imtype) void* "System.IntPtr"
%typemap(cstype) void* "System.IntPtr"
%typemap(csin)   void* "$csinput"
%typemap(in)     void* %{ $1 = $input; %}
%typemap(out)    void* %{ $result = $1; %}
%typemap(csout)  void* { return $imcall; }
//%typemap(csout, excode=SWIGEXCODE)  void* { 
//    System.IntPtr cPtr = $imcall;$excode
//    return cPtr;
//    }
//%typemap(csvarout, excode=SWIGEXCODE2) void* %{ 
//    get {
//        System.IntPtr cPtr = $imcall;$excode 
//        return cPtr; 
//   } 
//%} 
//
 
 //%ignore something

  

#endif
