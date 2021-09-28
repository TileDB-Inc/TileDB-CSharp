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

namespace TileDB 
{
    public class TileDBBuffer<T>
    {
        #region Constructors and Destructors
        public TileDBBuffer()
        {
            m_elementType = typeof(T);
            m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            if(m_elementType == typeof(System.Int32))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT32;
            }
            else if(m_elementType == typeof(System.Int64))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT64;
            }
            else if(m_elementType == typeof(float))
            {
                m_elementDataType = TileDB.DataType.TILEDB_FLOAT32;
            }
            else if(m_elementType == typeof(System.Double))
            {
                m_elementDataType = TileDB.DataType.TILEDB_FLOAT64;
            }
            else if(m_elementType == typeof(System.Byte))
            {
                m_elementDataType = TileDB.DataType.TILEDB_CHAR;
            }
            else if(m_elementType == typeof(System.Int16))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT16;
            }
            else if(m_elementType == typeof(System.UInt16))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT16;
            }
            else if(m_elementType == typeof(System.UInt32))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT32;
            }
            else if(m_elementType == typeof(System.UInt64))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT64;
            }
            else if(m_elementType == typeof(System.String))
            {
                m_elementDataType = TileDB.DataType.TILEDB_STRING_ASCII;
            }
            else
            {
                m_elementDataType = TileDB.DataType.TILEDB_ANY;
            }

            m_intptr = System.IntPtr.Zero;
        }
        ~TileDBBuffer()
        {
            Release();
        }
        #endregion

        #region 
        public void Init(int length, bool isVarSize, bool isNullable)
        {
            Release();
            
            m_data = new T[length];
            m_offsets = TileDB.VectorUInt64.Repeat(0, length);
            m_validities = TileDB.VectorUInt8.Repeat(0, length);

            m_dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(m_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            m_intptr = m_dataGCHandle.AddrOfPinnedObject();
        }

        protected void Release()
        {
            if(m_intptr != System.IntPtr.Zero)
            {
                m_dataGCHandle.Free();
            }
            m_intptr = System.IntPtr.Zero;
        }
        #endregion

        #region Properties
        public T[] Data
        {
            get { return m_data; }
        }

        public System.UInt64 DataLength
        {
            get { return (System.UInt64)m_data.Length; }
        }

        public TileDB.VectorUInt64 Offsets
        {
            get { return m_offsets; }
        }

        public TileDB.VectorUInt8 Validities
        {
            get { return m_validities; }
        }

        public System.IntPtr DataIntPtr
        {
            get { return m_intptr; }
        }

        public System.Type ElementType
        {
            get { return m_elementType; }
        }

        public TileDB.DataType ElementDataType
        {
            get { return m_elementDataType; }
        }

        public System.UInt32 ElementDataSize
        {
            get { return m_elementDataSize; }
        }

        #endregion Properties

        #region Fields
        protected T[] m_data;
        protected TileDB.VectorUInt64 m_offsets;
        protected TileDB.VectorUInt8 m_validities;

        protected System.Runtime.InteropServices.GCHandle m_dataGCHandle;
        protected System.IntPtr m_intptr;

        protected System.Type m_elementType;
        protected TileDB.DataType m_elementDataType;
        protected UInt32 m_elementDataSize;

        #endregion Fields

    }//class TileDBBuffer

 
}//namespace 