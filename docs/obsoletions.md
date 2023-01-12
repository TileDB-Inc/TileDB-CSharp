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

## <span id="TILEDB0006">`TILEDB0006` - The overload of `Dimension.Create` method that takes an array is obsolete.</span>

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

## <span id="TILEDB0007">`TILEDB0007` - The constructor and `Init` methods of the `QueryCondition` class are obsolete.</span>

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

## <span id="TILEDB0008">`TILEDB0008` - The `QueryCondition.Combine` method is obsolete.</span>

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

## <span id="TILEDB0009">`TILEDB0009` - The `Array.ConsolidateMetadata` and `ArrayMetadata.ConsolidateMetadata` methods are obsolete.</span>

Following changes to the native API of TileDB Embedded, the `Array.ConsolidateMetadata` and `ArrayMetadata.ConsolidateMetadata` methods became obsolete.

In accordance with [TileDB Embedded's deprecation policy](https://github.com/TileDB-Inc/TileDB/blob/dev/doc/policy/api_changes.md), they will be removed in a future version and calling them will fail.

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

## <span id="TILEDB0010">`TILEDB0010` - The `Query.SubmitAsync` method is obsolete.</span>

The `Query.SubmitAsync` method uses the legacy [Event Asynchronous Pattern](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/event-based-asynchronous-pattern-eap) and its implementation in TileDB Embedded has several known reliability issues.

### Version introduced

5.3.0

### Recommended action

Use `Query.SubmitAsync`. Until a native async API becomes available, you can use the `Task.Run` method to submit the query on a background thread.
