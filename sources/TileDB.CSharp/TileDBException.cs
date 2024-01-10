using System;

namespace TileDB.CSharp;

/// <summary>
/// This exception is thrown when a TileDB Embedded method fails with an error.
/// </summary>
/// <remarks>
/// Almost all methods of this library may throw it.
/// </remarks>
public sealed class TileDBException : Exception
{
    /// <summary>
    /// The exception's status code, as reported by <c>tiledb_status_code</c> function.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Creates a <see cref="TileDBException"/>.
    /// </summary>
    public TileDBException() { }

    /// <summary>
    /// Creates a <see cref="TileDBException"/> with a message.
    /// </summary>
    /// <param name="message">The exception's message.</param>
    public TileDBException(string? message) : base(message) { }

    /// <summary>
    /// Creates a <see cref="TileDBException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">The exception's message.</param>
    /// <param name="inner">The exception's inner exception.</param>
    public TileDBException(string? message, Exception? inner) : base(message, inner) { }
}
