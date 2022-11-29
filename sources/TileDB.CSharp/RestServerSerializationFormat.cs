namespace TileDB.CSharp
{
    /// <summary>
    /// Specifies the serialization format to be used when communicating with a remote REST server.
    /// </summary>
    /// <seealso cref="Config.RestConfig.ServerSerializationFormat"/>
    public enum RestServerSerializationFormat
    {
        /// <summary>
        /// Cap'n Proto format.
        /// </summary>
        Capnp,
        /// <summary>
        /// JSON format.
        /// </summary>
        Json
    }
}
