//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace TileDB {

public class PairDoubleDouble : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal PairDoubleDouble(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(PairDoubleDouble obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~PairDoubleDouble() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          tiledbcsPINVOKE.delete_PairDoubleDouble(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public PairDoubleDouble() : this(tiledbcsPINVOKE.new_PairDoubleDouble__SWIG_0(), true) {
  }

  public PairDoubleDouble(double first, double second) : this(tiledbcsPINVOKE.new_PairDoubleDouble__SWIG_1(first, second), true) {
  }

  public PairDoubleDouble(PairDoubleDouble other) : this(tiledbcsPINVOKE.new_PairDoubleDouble__SWIG_2(PairDoubleDouble.getCPtr(other)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public double first {
    set {
      tiledbcsPINVOKE.PairDoubleDouble_first_set(swigCPtr, value);
    } 
    get {
      double ret = tiledbcsPINVOKE.PairDoubleDouble_first_get(swigCPtr);
      return ret;
    } 
  }

  public double second {
    set {
      tiledbcsPINVOKE.PairDoubleDouble_second_set(swigCPtr, value);
    } 
    get {
      double ret = tiledbcsPINVOKE.PairDoubleDouble_second_get(swigCPtr);
      return ret;
    } 
  }

}

}
