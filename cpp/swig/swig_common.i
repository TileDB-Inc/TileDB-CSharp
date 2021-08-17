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
%include "std_pair.i"
%include "std_set.i"
%include "std_shared_ptr.i"
//%include "std_unordered_map.i"

%include "exception.i"

namespace std {


 

  %template(VectorInt32) vector<int>;
  %template(VectorUInt32) vector<unsigned int>;
  %template(VectorInt64) vector<int64_t>;
  %template(VectorUInt64) vector<uint64_t>;
  %template(VectorChar) vector<char>;
  %template(VectorUInt8) vector<uint8_t>;
  %template(VectorFloat32) vector<float>;
  %template(VectorDouble) vector<double>;
  %template(VectorString) vector<string>;
  %template(VectorConstChar) vector<const char*>;

  %template(VectorOfVectorInt32) vector<vector<int> >;
  %template(VectorOfVectorUInt32) vector<vector<unsigned int> >;
  %template(VectorOfVectorInt64) vector<vector<int64_t> >;
  %template(VectorOfVectorUInt64) vector<vector<uint64_t> >;
  %template(VectorOfVectorChar) vector<vector<char> >;
  %template(VectorOfVectorUChar) vector<vector<unsigned char> >;
  %template(VectorOfVectorFloat32) vector<vector<float> >;  
  %template(VectorOfVectorDouble) vector<vector<double> >;  
  %template(VectorOfVectorString) vector<vector<string> >;


  %template(MapStringString) map<string,string>;
  %template(MapStringInt32) map<string,int>;
  %template(MapStringUInt32) map<string,uint32_t>;
  %template(MapStringInt64) map<string,int64_t>;
  %template(MapStringUInt64) map<string,uint64_t>;
  %template(MapStringFloat32) map<string,float>;  
  %template(MapStringDouble) map<string,double>;
  %template(MapStringChar) map<string,char>;
  %template(MapStringUChar) map<string,unsigned char>;

  %template(MapInt32String) map<int,string>;
  %template(MapInt32Int32) map<int,int>;
  %template(MapInt32UInt32) map<int,uint32_t>;
  %template(MapInt32Int64) map<int,int64_t>;
  %template(MapInt32UInt64) map<int,uint64_t>;
  %template(MapInt32Float32) map<int,float>;  
  %template(MapInt32Double) map<int,double>;
  %template(MapInt32Char) map<int,char>;
  %template(MapInt32UChar) map<int,unsigned char>;

  %template(MapInt64String) map<int64_t,string>;
  %template(MapInt64Int32) map<int64_t,int>;
  %template(MapInt64UInt32) map<int64_t,uint32_t>;
  %template(MapInt64Int64) map<int64_t,int64_t>;
  %template(MapInt64UInt64) map<int64_t,uint64_t>;
  %template(MapInt64Float32) map<int64_t,float>;  
  %template(MapInt64Double) map<int64_t,double>;
  %template(MapInt64Char) map<int64_t,char>;
  %template(MapInt64UChar) map<int64_t,unsigned char>;    

  %template(PairStringString) pair<string,string>;
  %template(PairInt64Int64) pair<int64_t,int64_t>;
  %template(PairUInt64UInt64) pair<uint64_t,uint64_t>;
  %template(PairInt32Int32) pair<int32_t, int32_t>;
  %template(PairUInt32UInt32) pair<uint32_t, uint32_t>;
  %template(PairFloat32Float32) pair<float, float>;  
  %template(PairDoubleDouble) pair<double, double>;

  %template(MapStringPairUInt64UInt64) map<string, pair<uint64_t,uint64_t> >;
  %template(MapStringPairInt64Int64) map<string, pair<int64_t,int64_t> >;

  %template(MapStringVectorUInt8) map<string, vector<uint8_t> >;
  %template(MapStringVectorUInt32) map<string, vector<uint32_t> >;
  %template(MapStringVectorInt32) map<string, vector<int> >;
  %template(MapStringVectorUInt64) map<string, vector<uint64_t> >;
  %template(MapStringVectorInt64) map<string, vector<int64_t> >;

  %template(MapStringVectorFloat32) map<string, vector<float> >;
  %template(MapStringVectorDouble) map<string, vector<double> >;

  %template(MapStringVectorString) map<string, vector<string> >;
  %template(MapStringVectorChar) map<string, vector<char> >;   



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
 %ignore *::ptr;

  

#endif
