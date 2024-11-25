# Obsoletion warnings in `TileDB.CSharp`

This document lists the obsolete APIs in the `TileDB.CSharp` library by their diagnostic code, along with migration instructions.

Following [the deprecation policy of TileDB Embedded][core-deprecation], obsolete APIs will be removed after two minor releases of `TileDB.CSharp`. This is better illustrated in the following table:

|Diagnostic codes|Deprecated in version|Removed in version|
|----------------|---------------------|------------------|
|[`TILEDB0001`](#TILEDB0001) …[`TILEDB0011`](#TILEDB0011)|5.3.0|5.5.0|
|[`TILEDB0012`](#TILEDB0012) …[`TILEDB0013`](#TILEDB0013)|5.7.0|5.9.0|
|[`TILEDB0014`](#TILEDB0014) …[`TILEDB0014`](#TILEDB0014)|5.8.0|5.10.0|
|[`TILEDB0015`](#TILEDB0015) …[`TILEDB0015`](#TILEDB0015)|5.13.0|5.15.0|
|[`TILEDB0015`](#TILEDB0016) …[`TILEDB0015`](#TILEDB0016)|5.17.0|5.19.0|

## `TILEDB0001` - Enum value names that start with `TILEDB_` were replaced with C#-friendly names.

<a name="TILEDB0001"></a>

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
    using var query_write = new Query(Ctx, array_write);
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
    using var query_write = new Query(Ctx, array_write);
    query_write.SetLayout(LayoutType.Unordered);
    query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
    query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
    query_write.SetDataBuffer<int>("a", attr_data_buffer);
    query_write.Submit();
    array_write.Close();
}
```

## `TILEDB0002` - Data type is obsolete.

<a name="TILEDB0002"></a>

In version 5.3.0 some members of the `DataType` enum became obsolete and should not be used.

### Recommended action

The following table lists the obsolete types, along with recommended replacements:

|Obsolete type|Replacement|Reason|
|-------------|-----------|------|
|`StringUcs2`|`StringUtf16`|UCS-2 does not support characters outside the Basic Multilingual Plane.|
|`StringUcs4`|`StringUtf32`|UTF-32 is a more established synonym of UCS-4.|
|`Char`|`Byte` or `StringAscii`|The meaning of `Char` is ambiguous.|
|`TILEDB_ANY`|_a more specific type, depending on the situation_||

## `TILEDB0003` - Members of the `TileDB.Interop` namespace will become internal in a future version and should not be used by user code.

<a name="TILEDB0003"></a>

To reduce the library's public API surface and improve its ability to evolve, the low-level interop APIs in the `TileDB.Interop` namespace will become internal in a future release. In version 5.3.0 they were marked as obsolete to warn any 3rd-party users of them.

### Version introduced

5.3.0

### Recommended action

Use the high-level APIs in the `TileDB.CSharp` namespace; they are easier and safer to use.

> The eventual goal is to expose all features of TileDB in a high-level API. Until that happens, please contact us if you need to use a particular feature to help us prioritize.

## `TILEDB0004` - The `Context.ErrorHappened` event is obsolete.

<a name="TILEDB0004"></a>

Due to a programming mistake the `Context.ErrorHappened` event never worked as expected. Given its limited utility it was marked as obsolete in version 5.3.0.

### Version introduced

5.3.0

### Recommended action

Stop subscribing to `Context.ErrorHappened`. If you want to react to errors, you can now catch exceptions of type `TileDBException` which was introduced in version 5.3.0.

## `TILEDB0005` - The `ErrorException` type is obsolete.

<a name="TILEDB0005"></a>

The `ErrorException` type is badly named and was thrown only in limited circumstances. It was marked as obsolete in version 5.3.0 and replaced by the `TileDBException` type, which is thrown in any error reported by TileDB Embedded.

### Version introduced

5.3.0

### Recommended action

Catch exceptions of type `TileDBException` instead of `ErrorException`.

### Existing code

```csharp
try
{
    // ...
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
    // ...
}
catch (TileDBException e)
{
    Console.WriteLine(e.Message);
}
```

## `TILEDB0006` - The overload of `Dimension.Create` method that takes an array is obsolete.

<a name="TILEDB0006"></a>

The `Dimension.Create<T>(Context, string, T[], T)` method is problematic because only the first two values of the array parameter are actually being used. For this reason it was marked as obsolete in version 5.3.0.

### Version introduced

5.3.0

### Recommended action

Use the `Dimension.Create<T>(Context, string, T, T, T)` overload which explicitly accepts the lower and upper bound and better represents the intended usage.

### Existing code

```csharp
Dimension dimension = Dimension.Create(context, "test", new int[] { 1, 10 }, 5);
```

### New code

```csharp
Dimension dimension = Dimension.Create(context, "test", 1, 10, 5);
```

## `TILEDB0007` - The constructor and `Init` methods of the `QueryCondition` class are obsolete.

<a name="TILEDB0007"></a>

There are two ways to create query conditions: by constructing `QueryCondition` objects and calling the `Init` method, or by calling the `QueryCondition.Create` static methods. The former way is prone to bugs because it unnecessarily requires two methods and so in version 5.3.0 the public constructor and `Init` methods of the `QueryCondition` class were marked as obsolete.

### Version introduced

5.3.0

### Recommended action

Use `QueryCondition.Create` to create `QueryCondition` objects.

### Existing code

```csharp
using QueryCondition qc = new QueryCondition(Context.GetDefault());
qc.Init("attr1", 15, QueryConditionOperatorType.GreaterThan);
```

### New code

```csharp
using QueryCondition qc = QueryCondition.Create(Context.GetDefault(), "attr1", 15, QueryConditionOperatorType.GreaterThan);
```

## `TILEDB0008` - The `QueryCondition.Combine` method is obsolete.

<a name="TILEDB0008"></a>

The `QueryCondition.Combine` method is verbose and does not clearly signify which combination operator types are unary or binary. For these reasons it was marked as obsolete in version 5.3.0 and replaced with operator overloading.

### Version introduced

5.3.0

### Recommended action

Use the `&`, `|` or `!` operators to combine query conditions.

### Existing code

```csharp
using QueryCondition qc1 = QueryCondition.Create(Context.GetDefault(), "attr1", 15, QueryConditionOperatorType.GreaterThan);
using QueryCondition qc2 = QueryCondition.Create(Context.GetDefault(), "attr2", 12, QueryConditionOperatorType.LessThan);
using QueryCondition qc = qc1.Combine(qc2, QueryConditionCombinationOperatorType.And);
```

### New code

```csharp
using QueryCondition qc1 = QueryCondition.Create(Context.GetDefault(), "attr1", 15, QueryConditionOperatorType.GreaterThan);
using QueryCondition qc2 = QueryCondition.Create(Context.GetDefault(), "attr2", 12, QueryConditionOperatorType.LessThan);
using QueryCondition qc = qc1 & qc2;
```

## `TILEDB0009` - The `Array.ConsolidateMetadata` and `ArrayMetadata.ConsolidateMetadata` methods are obsolete.

<a name="TILEDB0009"></a>

Following changes to the native API of TileDB Embedded, the `Array.ConsolidateMetadata` and `ArrayMetadata.ConsolidateMetadata` methods became obsolete.

### Version introduced

5.3.0

### Recommended action

Use the `Array.Consolidate` method with the config value `sm.consolidation.mode` set to `array_meta` instead.

### Existing code

```csharp
Array.ConsolidateMetadata(Context.GetDefault(), "my_array");
```

### New code

```csharp
using Config config = new Config();
config.Set("sm.consolidation.mode", "array_meta");
Array.Consolidate(Context.GetDefault(), "my_array", config);
```

## `TILEDB0010` - The `Query.SubmitAsync` method is obsolete.

<a name="TILEDB0010"></a>

The `Query.SubmitAsync` method uses the legacy [Event Asynchronous Pattern](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/event-based-asynchronous-pattern-eap) and its implementation in TileDB Embedded has several known reliability issues.

### Version introduced

5.3.0

### Recommended action

Use `Query.Submit`. Until a native async API becomes available, you can use the `Task.Run` method to submit the query on a background thread.

## `TILEDB0011` - Subarray-related methods on the `Query` class are obsolete.

<a name="TILEDB0011"></a>

Following changes to the native API of TileDB Embedded, certain methods of the `Query` class such as `AddRange`, `Range` and `RangeVar` are now exposed through the newly introduced `Subarray` class.

Some of these methods were renamed or had their signature changed (for example those who returned tuples now return value tuples). Here is a table with the name changes:

|`Query` method|`Subarray` method|
|--------------|-----------------|
|`RangeNum`|`GetRangeCount(uint)`|
|`RangeNumFromName`|`GetRangeCount(string)`|
|`Range` (all overloads)|`GetRange`\*|
|`RangeVar` (all overloads)|`GetStringRange`|

> \* Because of lack of support by TileDB Embedded, the `Subarray.GetRange` methods will return a tuple with two members instead of three. The deprecated `Query.Range` methods have never actually been usable because of it.

### Version introduced

5.3.0

### Recommended action

Instead of setting ranges and subarrays on the `Query`, create and configure a `Subarray` object, and assign it to the query using the `Query.SetSubarray` method.

## `TILEDB0012` - Members of the `TileDB.Interop` namespace will become internal in a future version and should not be used by user code.

<a name="TILEDB0012"></a>

Some APIs in the `TileDB.Interop` namespace that were inadvertently removed in version 5.3.0 were reintroduced in version 5.7.0. They are marked as obsolete and hidden from IntelliSense and will be removed from the public API for good in version 5.9.0. They were also reintroduced in patch releases 5.3.1 and 5.4.1, obsoleted under the [`TILEDB0003`](#TILEDB0003) code.

### Version introduced

5.7.0

### Recommended action

The obsoleted APIs fall into the following categories:

- The `MarshaledString` and `MarshaledStringOut` types are used to help convert strings from and to ASCII and pass them to native code. If you are using them in user code for native code interop, you should use .NET's [built-in P/Invoke](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/charset) marshaling instead. For other kinds of encoding conversions, you should use APIs from the [`System.Text.Encoding`](https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding) class.
- The `__sFile` and `LibC` types have no members and there is no reason to use them anyway.
- The types that derive from `SafeHandle` are used to safely manage the lifetime of native TileDB objects. With the APIs in the `TileDB.CSharp` namespace providing broad coverage of TileDB's functionalities while also being safer and easier to sue, these types provide limited utility. You should use the APIs in the `TileDB.CSharp` namespace instead.
- Types with the name `tiledb_***_t` were made public again only to support the APIs of the safe handles above. They have little other use on their own. You should use APIs in the `TileDB.CSharp` namespace instead.

## `TILEDB0013` - The `EnumUtils.TypeToDataType` and `EnumUtils.DataTypeToType` methods are obsolete and will be removed in a future version.

<a name="TILEDB0013"></a>

The `EnumUtils.TypeToDataType` and `EnumUtils.DataTypeToType` methods convert between TileDB data types and .NET types. Given that there is no one-to-one correspondence between these two and for legacy reasons, these methods sometimes return wrong results and were obsoleted.

### Version introduced

5.7.0

### Recommended action

If you are performing queries on arrays of unknown schema, you can use the `Query.UnsafeSetDataBuffer` and `Query.UnsafeSetWriteDataBuffer` methods to set a data buffer to a query without type validation.

## `TILEDB0014` - Members of the `TileDB.Interop` namespace will become internal in a future version and should not be used by user code.

<a name="TILEDB0014"></a>

Some APIs in the `TileDB.Interop` namespace that were inadvertently removed in version 5.3.0 were reintroduced in version 5.7.0. They are marked as obsolete in version 5.8.0 and hidden from IntelliSense and will be removed from the public API for good in version 5.10.0. They were also reintroduced in patch releases 5.3.1 and 5.4.1, obsoleted under the [`TILEDB0003`](#TILEDB0003) code.

### Version introduced

5.8.0

### Recommended action

Stop using the obsoleted APIs. No other public API of `TileDB.CSharp` depends on them.

## `TILEDB0015` - `ConfigIterator` is obsolete.

<a name="TILEDB0015"></a>

The `ConfigIterator` class is unintuitive to use. In version 5.13.0 it was marked as obsolete and replaced by `Config` implementing `IEnumerable<KeyValuePair<string,string>>`.

### Version introduced

5.13.0

### Recommended action

Replace uses of `ConfigIterator` with enumerating the `Config` object directly using a `foreach` loop or LINQ. To get only the config options that start with a specific prefix, call the `Config.EnumerateOptions` method and enumerate its returned object.

## `TILEDB0016` - `File` is obsolete.

<a name="TILEDB0016"></a>

The TileDB filestore APIs, exposed by the `TileDB.CSharp.File` class are obsolete and will be removed in a future version.

### Version introduced

5.17.0

### Recommended action

There is no direct replacement. You can manually store files in TileDB by representing them as one-dimensional dense arrays of bytes.

[core-deprecation]: https://github.com/TileDB-Inc/TileDB/blob/dev/doc/policy/api_changes.md
