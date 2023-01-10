namespace TileDB.CSharp
{
    internal static class Obsoletions
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        public const string SharedUrlFormat = "https://github.com/TileDB/TileDB-CSharp/blob/main/docs/obsoletions.md#{0}";
#pragma warning restore S1075 // URIs should not be hardcoded

        public const string LegacyEnumNamesMessage = "Enum values that start with TILEDB_ are obsolete.";
        public const string LegacyEnumNamesDiagId = "TILEDB0001";

        public const string ObsoleteDataTypeMessage = "This data type is obsolete.";
        public const string ObsoleteDataTypeDiagId = "TILEDB0002";

        public const string TileDBInteropMessage = "Members of the TileDB.Interop namespace should not be used by user code will become internal in a future version.";
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
    }
}
