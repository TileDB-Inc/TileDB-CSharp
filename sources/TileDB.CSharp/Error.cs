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

    public class ErrorException: Exception 
    {
        public ErrorException()
        {
        }

        public ErrorException(string message): base(message) {
 
        }
    }
}
