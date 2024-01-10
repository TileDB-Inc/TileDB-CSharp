using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Specifies additional details about the status of a <see cref="Query"/>.
/// </summary>
public struct QueryStatusDetails
{
    internal tiledb_query_status_details_t _details;

    /// <summary>
    /// The reason the <see cref="Query"/> cannot continue.
    /// </summary>
    public QueryStatusDetailsReason Reason
    {
        readonly get => (QueryStatusDetailsReason)_details.incomplete_reason;
        set => _details.incomplete_reason = (tiledb_query_status_details_reason_t)value;
    }
}
