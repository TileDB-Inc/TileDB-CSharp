#ifndef SWIG_COMMON_I
#define SWIG_COMMON_I

%module tiledb 

#ifdef SWIGPYTHON
//	define something related to python wrapper
//		 
//
%include "cpointer.i"	 
 
#endif

#ifdef SWIGCSHARP
//	define something related to csharp wrapper
//		 
//	 
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

//namespace std {

  //      %template(IntVector) vector<int>;
  //      %template(DoubleVector) vector<double>;
  //      %template(StringVector) vector<string>;
  //      %template(ConstCharVector) vector<const char*>;

//}

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
