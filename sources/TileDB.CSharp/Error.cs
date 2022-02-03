using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public int Code { get; set; }
        public string Message {get; set;}
    }

    public class ErrorException: System.Exception 
    {

        public ErrorException():base() 
        {
        }

        public ErrorException(string message): base(message) {
 
        }
    }
 
}
