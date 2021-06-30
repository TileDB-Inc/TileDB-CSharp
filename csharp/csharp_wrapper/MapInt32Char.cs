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

public class MapInt32Char : global::System.IDisposable 
    , global::System.Collections.Generic.IDictionary<int, char>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal MapInt32Char(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(MapInt32Char obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~MapInt32Char() {
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
          tiledbcsPINVOKE.delete_MapInt32Char(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }


  public char this[int key] {
    get {
      return getitem(key);
    }

    set {
      setitem(key, value);
    }
  }

  public bool TryGetValue(int key, out char value) {
    if (this.ContainsKey(key)) {
      value = this[key];
      return true;
    }
    value = default(char);
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

  public global::System.Collections.Generic.ICollection<int> Keys {
    get {
      global::System.Collections.Generic.ICollection<int> keys = new global::System.Collections.Generic.List<int>();
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

  public global::System.Collections.Generic.ICollection<char> Values {
    get {
      global::System.Collections.Generic.ICollection<char> vals = new global::System.Collections.Generic.List<char>();
      foreach (global::System.Collections.Generic.KeyValuePair<int, char> pair in this) {
        vals.Add(pair.Value);
      }
      return vals;
    }
  }

  public void Add(global::System.Collections.Generic.KeyValuePair<int, char> item) {
    Add(item.Key, item.Value);
  }

  public bool Remove(global::System.Collections.Generic.KeyValuePair<int, char> item) {
    if (Contains(item)) {
      return Remove(item.Key);
    } else {
      return false;
    }
  }

  public bool Contains(global::System.Collections.Generic.KeyValuePair<int, char> item) {
    if (this[item.Key] == item.Value) {
      return true;
    } else {
      return false;
    }
  }

  public void CopyTo(global::System.Collections.Generic.KeyValuePair<int, char>[] array) {
    CopyTo(array, 0);
  }

  public void CopyTo(global::System.Collections.Generic.KeyValuePair<int, char>[] array, int arrayIndex) {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (arrayIndex+this.Count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");

    global::System.Collections.Generic.IList<int> keyList = new global::System.Collections.Generic.List<int>(this.Keys);
    for (int i = 0; i < keyList.Count; i++) {
      int currentKey = keyList[i];
      array.SetValue(new global::System.Collections.Generic.KeyValuePair<int, char>(currentKey, this[currentKey]), arrayIndex+i);
    }
  }

  global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, char>> global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<int, char>>.GetEnumerator() {
    return new MapInt32CharEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new MapInt32CharEnumerator(this);
  }

  public MapInt32CharEnumerator GetEnumerator() {
    return new MapInt32CharEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class MapInt32CharEnumerator : global::System.Collections.IEnumerator,
      global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, char>>
  {
    private MapInt32Char collectionRef;
    private global::System.Collections.Generic.IList<int> keyCollection;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public MapInt32CharEnumerator(MapInt32Char collection) {
      collectionRef = collection;
      keyCollection = new global::System.Collections.Generic.List<int>(collection.Keys);
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public global::System.Collections.Generic.KeyValuePair<int, char> Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (global::System.Collections.Generic.KeyValuePair<int, char>)currentObject;
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
        int currentKey = keyCollection[currentIndex];
        currentObject = new global::System.Collections.Generic.KeyValuePair<int, char>(currentKey, collectionRef[currentKey]);
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


  public MapInt32Char() : this(tiledbcsPINVOKE.new_MapInt32Char__SWIG_0(), true) {
  }

  public MapInt32Char(MapInt32Char other) : this(tiledbcsPINVOKE.new_MapInt32Char__SWIG_1(MapInt32Char.getCPtr(other)), true) {
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  private uint size() {
    uint ret = tiledbcsPINVOKE.MapInt32Char_size(swigCPtr);
    return ret;
  }

  public bool empty() {
    bool ret = tiledbcsPINVOKE.MapInt32Char_empty(swigCPtr);
    return ret;
  }

  public void Clear() {
    tiledbcsPINVOKE.MapInt32Char_Clear(swigCPtr);
  }

  private char getitem(int key) {
    char ret = tiledbcsPINVOKE.MapInt32Char_getitem(swigCPtr, key);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int key, char x) {
    tiledbcsPINVOKE.MapInt32Char_setitem(swigCPtr, key, x);
  }

  public bool ContainsKey(int key) {
    bool ret = tiledbcsPINVOKE.MapInt32Char_ContainsKey(swigCPtr, key);
    return ret;
  }

  public void Add(int key, char value) {
    tiledbcsPINVOKE.MapInt32Char_Add(swigCPtr, key, value);
    if (tiledbcsPINVOKE.SWIGPendingException.Pending) throw tiledbcsPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Remove(int key) {
    bool ret = tiledbcsPINVOKE.MapInt32Char_Remove(swigCPtr, key);
    return ret;
  }

  private global::System.IntPtr create_iterator_begin() {
    global::System.IntPtr ret = tiledbcsPINVOKE.MapInt32Char_create_iterator_begin(swigCPtr);
    return ret;
  }

  private int get_next_key(global::System.IntPtr swigiterator) {
    int ret = tiledbcsPINVOKE.MapInt32Char_get_next_key(swigCPtr, swigiterator);
    return ret;
  }

  private void destroy_iterator(global::System.IntPtr swigiterator) {
    tiledbcsPINVOKE.MapInt32Char_destroy_iterator(swigCPtr, swigiterator);
  }

}

}
