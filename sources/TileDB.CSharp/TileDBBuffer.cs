/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2021 TileDB, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Text;
using System.Collections.Generic;

namespace TileDB.CSharp
{
    [Obsolete("Will be deprecated, Please use helper functions in CoreUtil.",false)]
    public class TileDBBuffer<T> where T: struct
    {
        #region Fields

        // _data, _offsets and _validites need to pass their addresses to c-api functions, here we don't make them private and access them through properties.
        public T[] _data;
        public ulong[]? _offsets = null;
        public byte[]? _validities = null;

        protected System.Runtime.InteropServices.GCHandle _dataGCHandle;
        protected System.IntPtr _intptr;

        protected System.Type _type;
        protected DataType _dataType;
        #endregion Fields

        #region Constructors and Destructors

        public TileDBBuffer()
        {
            _data = new T[0];
            _offsets = null;
            _validities = null;

            _type = typeof(byte);
            _dataType = EnumUtil.TypeToDataType(_type);
        }

        public TileDBBuffer(int data_size, int offsets_size = 0, int validities_size = 0)
        {
            _data = new T[data_size];
            _offsets = offsets_size > 0 ? new ulong[offsets_size] : null;
            _validities = validities_size > 0 ? new byte[validities_size] : null;

            _type = typeof(T);
            _dataType = EnumUtil.TypeToDataType(_type);

            _dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            _intptr = _dataGCHandle.AddrOfPinnedObject();
        }

        public TileDBBuffer(T[] data, ulong[]? offsets = null, byte[]? validities = null)
        {
            _data = data;
            _offsets = offsets;
            _validities = validities;

            _type = typeof(T);
            _dataType = EnumUtil.TypeToDataType(_type);

            _dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            _intptr = _dataGCHandle.AddrOfPinnedObject();
        }

        ~TileDBBuffer()
        {
            Release();
        }

        protected void Release()
        {
            if (_intptr != System.IntPtr.Zero)
            {
                _dataGCHandle.Free();
            }

            _intptr = System.IntPtr.Zero;
        }

        #endregion


        #region Properties


        public System.UInt64 DataSize
        {
            get { return _data == null ? 0 : (System.UInt64)_data.Length; }
        }

        public System.IntPtr DataIntPtr
        {
            get { return _intptr; }
        }

        public System.Type Type
        {
            get { return _type; }
        }

        public DataType DataType
        {
            get { return _dataType; }
        }

        #endregion Properties



    }//class TileDBBuffer

#pragma warning disable 612, 618
    public class TileDBStringBuffer : TileDBBuffer<byte>
    {
#pragma warning restore 612, 618

        #region Fields

        #endregion Fields

        #region Constructors and Destructors
        public TileDBStringBuffer(int bytes_size, int offsets_size, int validities_size = 0) : base(bytes_size,offsets_size,validities_size)
        {
        }

        public TileDBStringBuffer(string[] strarray, byte[]? validities = null, string encodingmethod = "ASCII")
        {
            (_data, _offsets) = CoreUtil.PackStringArray(strarray, encodingmethod);
            _validities = validities;

            _type = typeof(byte);
            _dataType = EnumUtil.TypeToDataType(_type);

            _dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            _intptr = _dataGCHandle.AddrOfPinnedObject();
        }

        #endregion Constructors and Destructors



    }


}//namespace