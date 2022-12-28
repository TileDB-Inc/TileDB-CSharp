# Obsoletion warnings in `TileDB.CSharp`

## <span id="TILEDB0001">`TILEDB0001` - Enum value names that start with `TILEDB_` were replaced with C#-friendly names.</span>

Prior to version 5.3.0, the library's various enum values had names identical with the C API, meaning that they were in all-caps and started with `TILEDB_`.

Version 5.3.0 introduced new values with shorter names that comply with .NET's naming guidelines. The old names were obsoleted and hidden from IntelliSense, and future enum values will not be available in the old naming scheme.

### Recommended action

Switching to the new names is as simple as a find-replace operation. The obsoletion messages will point you to the correct replacement.

> Some already obsolete enum values (like `DataType.TILEDB_ANY`) did not receive new names. Users are strongly encouraged to move away from them.

### Existing code

```csharp
var dim1_data_buffer = new int[3] { 1, 2, 3 };
var dim2_data_buffer = new int[3] { 1, 3, 4 };
var attr_data_buffer = new int[3] { 1, 2, 3 };

using (Array array_write = new Array(Context.GetDefault(), "my_array"))
{
    array_write.Open(QueryType.TILEDB_WRITE);
    var query_write = new Query(Ctx, array_write);
    query_write.SetLayout(LayoutType.TILEDB_UNORDERED);
    query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
    query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
    query_write.SetDataBuffer<int>("a", attr_data_buffer);
    query_write.Submit();
    array_write.Close();
}
```

### New code

```csharp
var dim1_data_buffer = new int[3] { 1, 2, 3 };
var dim2_data_buffer = new int[3] { 1, 3, 4 };
var attr_data_buffer = new int[3] { 1, 2, 3 };

using (Array array_write = new Array(Context.GetDefault(), "my_array"))
{
    array_write.Open(QueryType.Write);
    var query_write = new Query(Ctx, array_write);
    query_write.SetLayout(LayoutType.Unordered);
    query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
    query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
    query_write.SetDataBuffer<int>("a", attr_data_buffer);
    query_write.Submit();
    array_write.Close();
}
```

## <span id="TILEDB0002">`TILEDB0002` - Data type is obsolete.</span>

In version 5.3.0 some members of the `DataType` enum became obsolete and should not be used.

### Recommended action

The following table lists the obsolete types, along with recommended replacements:

|Obsolete type|Replacement|Reason|
|-------------|-----------|------|
|`StringUcs2`|`StringUtf16`|UCS-2 does not support characters outside the Basic Multilingual Plane.|
|`StringUcs4`|`StringUtf32`|UTF-32 is a more established synonym of UCS-4.|
|`Char`|`Byte` or `StringAscii`|The meaning of `Char` is ambiguous.|
|`TILEDB_ANY`|_a more specific type, depending on the situation_||

## <span id="TILEDB0003">`TILEDB0003` - Members of the `TileDB.Interop` namespace will become internal in a future version and should not be used by user code.</span>

To reduce the library's public API surface and improve its ability to evolve, the low-level interop APIs in the `TileDB.Interop` namespace will become internal in a future release. In version 5.3.0 they were marked as obsolete to warn any 3rd-party users of them.

### Version introduced

5.3.0

### Recommended action

Use the high-level APIs in the `TileDB.CSharp` namespace; they are easier and safer to use.

> The eventual goal is to expose all features of TileDB in a high-level API. Until that happens, please contact us if you need to use a particular feature to help us prioritize.

## <span id="TILEDB0004">`TILEDB0004` - The `Context.ErrorHappened` event is obsolete.</span>

Due to a programming mistake the `Context.ErrorHappened` event never worked as expected. Given its limited utility it was marked as obsolete in version 5.3.0.

### Version introduced

5.3.0

### Recommended action

Stop subscribing to `Context.ErrorHappened`. If you want to react to errors, you can now catch exceptions of type `TileDBException` which was introduced in version 5.3.0.

## <span id="TILEDB0005">`TILEDB0005` - The `ErrorException` type is obsolete.</span>

The `ErrorException` type is badly named and was thrown only in limited circumstances. It was marked as obsolete in version 5.3.0 and replaced by the `TileDBException` type, which is thrown in any error reported by TileDB Embedded.

### Version introduced

5.3.0

### Recommended action

Catch exceptions of type `TileDBException` instead of `ErrorException`.

### Existing code

```csharp
try
{
    // �
}
catch (ErrorException e)
{
    Console.WriteLine(e.Message);
}
```

### New code

```csharp
try
{
    // �
}
catch (TileDBException e)
{
    Console.WriteLine(e.Message);
}
```
