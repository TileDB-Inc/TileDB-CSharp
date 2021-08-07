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

public class QueryCondition : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnBase;

  internal QueryCondition(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwnBase = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(QueryCondition obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~QueryCondition() {
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
          tiledbcsPINVOKE.delete_QueryCondition(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public QueryCondition(Context ctx) : this(tiledbcsPINVOKE.new_QueryCondition__SWIG_0(Context.getCPtr(ctx)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public QueryCondition(QueryCondition arg0) : this(tiledbcsPINVOKE.new_QueryCondition__SWIG_1(QueryCondition.getCPtr(arg0)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public void init(string attribute_name, string condition_value, QueryConditionOperatorType optype) {
    tiledbcsPINVOKE.QueryCondition_init(swigCPtr, attribute_name, condition_value, (int)optype);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public QueryCondition combine(QueryCondition rhs, QueryConditionCombinationOperatorType combination_optype) {
    QueryCondition ret = new QueryCondition(tiledbcsPINVOKE.QueryCondition_combine(swigCPtr, QueryCondition.getCPtr(rhs), (int)combination_optype), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static QueryCondition create(Context ctx, string attribute_name, string value, QueryConditionOperatorType optype) {
    QueryCondition ret = new QueryCondition(tiledbcsPINVOKE.QueryCondition_create(Context.getCPtr(ctx), attribute_name, value, (int)optype), true);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}