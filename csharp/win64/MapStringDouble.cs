//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace tiledb {

public class MapStringDouble : global::System.IDisposable 
    , global::System.Collections.Generic.IDictionary<string, double>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal MapStringDouble(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(MapStringDouble obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~MapStringDouble() {
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
          tiledbPINVOKE.delete_MapStringDouble(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }


  public double this[string key] {
    get {
      return getitem(key);
    }

    set {
      setitem(key, value);
    }
  }

  public bool TryGetValue(string key, out double value) {
    if (this.ContainsKey(key)) {
      value = this[key];
      return true;
    }
    value = default(double);
    return false;
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public global::System.Collections.Generic.ICollection<string> Keys {
    get {
      global::System.Collections.Generic.ICollection<string> keys = new global::System.Collections.Generic.List<string>();
      int size = this.Count;
      if (size > 0) {
        global::System.IntPtr iter = create_iterator_begin();
        for (int i = 0; i < size; i++) {
          keys.Add(get_next_key(iter));
        }
        destroy_iterator(iter);
      }
      return keys;
    }
  }

  public global::System.Collections.Generic.ICollection<double> Values {
    get {
      global::System.Collections.Generic.ICollection<double> vals = new global::System.Collections.Generic.List<double>();
      foreach (global::System.Collections.Generic.KeyValuePair<string, double> pair in this) {
        vals.Add(pair.Value);
      }
      return vals;
    }
  }

  public void Add(global::System.Collections.Generic.KeyValuePair<string, double> item) {
    Add(item.Key, item.Value);
  }

  public bool Remove(global::System.Collections.Generic.KeyValuePair<string, double> item) {
    if (Contains(item)) {
      return Remove(item.Key);
    } else {
      return false;
    }
  }

  public bool Contains(global::System.Collections.Generic.KeyValuePair<string, double> item) {
    if (this[item.Key] == item.Value) {
      return true;
    } else {
      return false;
    }
  }

  public void CopyTo(global::System.Collections.Generic.KeyValuePair<string, double>[] array) {
    CopyTo(array, 0);
  }

  public void CopyTo(global::System.Collections.Generic.KeyValuePair<string, double>[] array, int arrayIndex) {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (arrayIndex+this.Count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");

    global::System.Collections.Generic.IList<string> keyList = new global::System.Collections.Generic.List<string>(this.Keys);
    for (int i = 0; i < keyList.Count; i++) {
      string currentKey = keyList[i];
      array.SetValue(new global::System.Collections.Generic.KeyValuePair<string, double>(currentKey, this[currentKey]), arrayIndex+i);
    }
  }

  global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<string, double>> global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, double>>.GetEnumerator() {
    return new MapStringDoubleEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new MapStringDoubleEnumerator(this);
  }

  public MapStringDoubleEnumerator GetEnumerator() {
    return new MapStringDoubleEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class MapStringDoubleEnumerator : global::System.Collections.IEnumerator,
      global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<string, double>>
  {
    private MapStringDouble collectionRef;
    private global::System.Collections.Generic.IList<string> keyCollection;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public MapStringDoubleEnumerator(MapStringDouble collection) {
      collectionRef = collection;
      keyCollection = new global::System.Collections.Generic.List<string>(collection.Keys);
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public global::System.Collections.Generic.KeyValuePair<string, double> Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (global::System.Collections.Generic.KeyValuePair<string, double>)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object global::System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        string currentKey = keyCollection[currentIndex];
        currentObject = new global::System.Collections.Generic.KeyValuePair<string, double>(currentKey, collectionRef[currentKey]);
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new global::System.InvalidOperationException("Collection modified.");
      }
    }

    public void Dispose() {
      currentIndex = -1;
      currentObject = null;
    }
  }


  public MapStringDouble() : this(tiledbPINVOKE.new_MapStringDouble__SWIG_0(), true) {
  }

  public MapStringDouble(MapStringDouble other) : this(tiledbPINVOKE.new_MapStringDouble__SWIG_1(MapStringDouble.getCPtr(other)), true) {
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = tiledbPINVOKE.MapStringDouble_size(swigCPtr);
    return ret;
  }

  public bool empty() {
    bool ret = tiledbPINVOKE.MapStringDouble_empty(swigCPtr);
    return ret;
  }

  public void Clear() {
    tiledbPINVOKE.MapStringDouble_Clear(swigCPtr);
  }

  private double getitem(string key) {
    double ret = tiledbPINVOKE.MapStringDouble_getitem(swigCPtr, key);
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(string key, double x) {
    tiledbPINVOKE.MapStringDouble_setitem(swigCPtr, key, x);
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool ContainsKey(string key) {
    bool ret = tiledbPINVOKE.MapStringDouble_ContainsKey(swigCPtr, key);
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Add(string key, double value) {
    tiledbPINVOKE.MapStringDouble_Add(swigCPtr, key, value);
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Remove(string key) {
    bool ret = tiledbPINVOKE.MapStringDouble_Remove(swigCPtr, key);
    if (tiledbPINVOKE.SWIGPendingException.Pending) throw tiledbPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private global::System.IntPtr create_iterator_begin() {
    global::System.IntPtr ret = tiledbPINVOKE.MapStringDouble_create_iterator_begin(swigCPtr);
    return ret;
  }

  private string get_next_key(global::System.IntPtr swigiterator) {
    string ret = tiledbPINVOKE.MapStringDouble_get_next_key(swigCPtr, swigiterator);
    return ret;
  }

  private void destroy_iterator(global::System.IntPtr swigiterator) {
    tiledbPINVOKE.MapStringDouble_destroy_iterator(swigCPtr, swigiterator);
  }

}

}
