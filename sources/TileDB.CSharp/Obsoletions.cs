namespace TileDB.CSharp
{
    internal static class Obsoletions
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        public const string SharedUrlFormat = "https://github.com/TileDB/TileDB-CSharp/blob/main/docs/obsoletions.md#{0}";
#pragma warning restore S1075 // URIs should not be hardcoded

        public const string LegacyEnumNamesMessage = "Enum values that start with TILEDB_ are obsolete.";
        public const string LegacyEnumNamesDiagId = "TILEDB0001";
    }
}
