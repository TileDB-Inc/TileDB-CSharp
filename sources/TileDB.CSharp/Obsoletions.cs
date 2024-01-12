namespace TileDB.CSharp;

internal static class Obsoletions
{
#pragma warning disable S1075 // URIs should not be hardcoded
    public const string SharedUrlFormat = "https://tiledb-inc.github.io/TileDB-CSharp/obsoletions.html#{0}";
#pragma warning restore S1075 // URIs should not be hardcoded

    public const string LegacyEnumNamesMessage = "Enum values that start with TILEDB_ are obsolete.";
    public const string LegacyEnumNamesDiagId = "TILEDB0001";

    public const string ObsoleteDataTypeMessage = "This data type is obsolete.";
    public const string ObsoleteDataTypeDiagId = "TILEDB0002";

    public const string TileDBInteropMessage = "Members of the TileDB.Interop namespace should not be used by user code and will become internal in a future version.";
    public const string TileDBInteropDiagId = "TILEDB0003";

    public const string ContextErrorHappenedMessage = "The Context.ErrorHappened event is obsolete and will not be invoked.";
    public const string ContextErrorHappenedDiagId = "TILEDB0004";

    public const string ErrorExceptionMessage = "The ErrorException type is obsolete and will not be thrown. Catch TileDBException instead.";
    public const string ErrorExceptionDiagId = "TILEDB0005";

    public const string DimensionCreateMessage = "The overload of Dimension.Create that accepts an array is obsolete. Use the overload that explicitly accepts the lower and upper bounds instead.";
    public const string DimensionCreateDiagId = "TILEDB0006";

    public const string QueryConditionInitMessage = "The constructor and Init method of the QueryCondition classes is obsolete. Use the static Create methods instead.";
    public const string QueryConditionInitDiagId = "TILEDB0007";

    public const string QueryConditionCombineMessage = "The QueryCondition.Combine method is obsolete. Use the '&', '|' and '!' operators instead.";
    public const string QueryConditionCombineDiagId = "TILEDB0008";

    public const string ConsolidateMetadataMessage = "The Array.ConsolidateMetadata and ArrayMetadata.ConsolidateMetadata methods are obsolete. Call Array.Consolidate with the config value 'sm.consolidation.mode' set to 'array_meta' instead.";
    public const string ConsolidateMetadataDiagId = "TILEDB0009";

    public const string QuerySubmitAsyncMessage = "The Query.SubmitAsync method is obsolete due to reliability problems and will be removed in a future release";
    public const string QuerySubmitAsyncDiagId = "TILEDB0010";

    public const string QuerySubarrayMessage = "Subarray-related methods of the Query class are obsolete and will become unavailable in a future version.";
    public const string QuerySubarrayDiagId = "TILEDB0011";

    public const string TileDBInterop2Message = "Members of the TileDB.Interop namespace should not be used by user code and will become internal in a future version.";
    public const string TileDBInterop2DiagId = "TILEDB0012";

    public const string DataTypeTypeConversionsMessage = "The EnumUtils.TypeToDataType and EnumUtils.DataTypeToType methods are obsolete and will be removed in a future version.";
    public const string DataTypeTypeConversionsDiagId = "TILEDB0013";

    public const string TileDBInterop3Message = "Members of the TileDB.Interop namespace should not be used by user code and will become internal in a future version.";
    public const string TileDBInterop3DiagId = "TILEDB0014";
}
