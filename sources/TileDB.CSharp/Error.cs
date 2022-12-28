using System;

namespace TileDB.CSharp
{
    [Obsolete(Obsoletions.ContextErrorHappenedMessage, DiagnosticId = Obsoletions.ContextErrorHappenedDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public int Code { get; set; }
        public string Message {get; set;}
    }

    /// <summary>
    /// Deprecated, use <see cref="TileDBException"/> instead.
    /// </summary>
    [Obsolete(Obsoletions.ErrorExceptionMessage, DiagnosticId = Obsoletions.ErrorExceptionDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    public class ErrorException: Exception 
    {
        /// <summary>
        /// Creates an <see cref="ErrorException"/>.
        /// </summary>
        public ErrorException()
        {
        }

        /// <summary>
        /// Creates an <see cref="ErrorException"/> with a message.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        public ErrorException(string message): base(message) {
 
        }
    }
}
