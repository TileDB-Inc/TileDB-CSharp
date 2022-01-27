// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Text;

namespace TileDB.Interop
{
    /// <summary>
    /// Marshal sbyte** as output string
    /// </summary>
    public unsafe struct MarshaledStringOut :IDisposable
    {
        private sbyte** _values;
        public MarshaledStringOut(int max_num_chars)
        {
            _values = (sbyte**)System.Runtime.InteropServices.Marshal.AllocHGlobal(IntPtr.Size).ToPointer();
            int max_num = max_num_chars;
            if(max_num<1)
            {
                max_num = 1;
            }
            _values[0] = (sbyte*)System.Runtime.InteropServices.Marshal.AllocHGlobal(max_num_chars).ToPointer();
        }

        public sbyte** Values 
        { 
            get  { return _values; }
            set { _values = Values; } 
        }

        public void Dispose()
        {
            if (_values != null) 
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)(_values[0]));
                System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)(_values));
                _values = null;
            }
        }

        public static implicit operator sbyte**(in MarshaledStringOut value) => value.Values;

        public override string ToString()
        {
            if(_values == null) 
            {
                return string.Empty;
            }
            return new string(_values[0]);
        }
    }
 
}