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

public class Attribute : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnBase;

  internal Attribute(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwnBase = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Attribute obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Attribute() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnBase) {
          swigCMemOwnBase = false;
          tiledbcsPINVOKE.delete_Attribute(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public Attribute(Context ctx, string name, DataType datatype) : this(tiledbcsPINVOKE.new_Attribute__SWIG_0(Context.getCPtr(ctx), name, (int)datatype), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public Attribute(Context ctx, string name, DataType datatype, FilterList filter_list) : this(tiledbcsPINVOKE.new_Attribute__SWIG_1(Context.getCPtr(ctx), name, (int)datatype, FilterList.getCPtr(filter_list)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public Attribute(Attribute arg0) : this(tiledbcsPINVOKE.new_Attribute__SWIG_2(Attribute.getCPtr(arg0)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public string name() {
    string ret = tiledbcsPINVOKE.Attribute_name(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public DataType type() {
    DataType ret = (DataType)tiledbcsPINVOKE.Attribute_type(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ulong cell_size() {
    ulong ret = tiledbcsPINVOKE.Attribute_cell_size(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public uint cell_val_num() {
    uint ret = tiledbcsPINVOKE.Attribute_cell_val_num(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public Attribute set_cell_val_num(uint num) {
    Attribute ret = new Attribute(tiledbcsPINVOKE.Attribute_set_cell_val_num(swigCPtr, num), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool variable_sized() {
    bool ret = tiledbcsPINVOKE.Attribute_variable_sized(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public FilterList filter_list() {
    FilterList ret = new FilterList(tiledbcsPINVOKE.Attribute_filter_list(swigCPtr), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public Attribute set_filter_list(FilterList filter_list) {
    Attribute ret = new Attribute(tiledbcsPINVOKE.Attribute_set_filter_list(swigCPtr, FilterList.getCPtr(filter_list)), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public Attribute set_nullable(bool nullable) {
    Attribute ret = new Attribute(tiledbcsPINVOKE.Attribute_set_nullable(swigCPtr, nullable), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool nullable() {
    bool ret = tiledbcsPINVOKE.Attribute_nullable(swigCPtr);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void dump(string filename) {
    tiledbcsPINVOKE.Attribute_dump(swigCPtr, filename);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public static bool is_valid_intdatatype(int intdatatype) {
    bool ret = tiledbcsPINVOKE.Attribute_is_valid_intdatatype(intdatatype);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static Attribute create_attribute(Context ctx, string name, DataType datatype) {
    Attribute ret = new Attribute(tiledbcsPINVOKE.Attribute_create_attribute(Context.getCPtr(ctx), name, (int)datatype), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static Attribute create_vector_attribute(Context ctx, string name, DataType datatype) {
    Attribute ret = new Attribute(tiledbcsPINVOKE.Attribute_create_vector_attribute(Context.getCPtr(ctx), name, (int)datatype), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
