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
            
            if(m_elementType == typeof(System.Int32))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT32;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.Int64))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT64;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(float))
            {
                m_elementDataType = TileDB.DataType.TILEDB_FLOAT32;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.Double))
            {
                m_elementDataType = TileDB.DataType.TILEDB_FLOAT64;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.Byte))
            {
                m_elementDataType = TileDB.DataType.TILEDB_CHAR;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.Int16))
            {
                m_elementDataType = TileDB.DataType.TILEDB_INT16;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.UInt16))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT16;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.UInt32))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT32;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.UInt64))
            {
                m_elementDataType = TileDB.DataType.TILEDB_UINT64;
                m_elementDataSize = (System.UInt32)(System.Runtime.InteropServices.Marshal.SizeOf(m_elementType));
            }
            else if(m_elementType == typeof(System.String))
            {
                m_elementDataType = TileDB.DataType.TILEDB_STRING_ASCII;
                m_elementDataSize = sizeof(byte);
            }
            else
            {
                m_elementDataType = TileDB.DataType.TILEDB_ANY;
                m_elementDataSize = sizeof(byte);
            }

            m_intptr = System.IntPtr.Zero;
        }
        ~TileDBBuffer()
        {
            Release();
        }
        #endregion

        #region 
        public void Init(int length, bool isVarSize, bool isNullable=false, int buffersize=0)
        {
 
            if (buffersize < length)
            {
                buffersize = length;
            }

            Release();
            m_data = new T[length];
            if(isVarSize || m_elementType == typeof(System.String))
            {
                m_offsets = TileDB.VectorUInt64.Repeat(0, length);
            }

            if (isNullable)
            {
                m_validities = TileDB.VectorUInt8.Repeat(0, length);
            }

            if(m_elementType == typeof(System.String))
            {
                m_bytes = new byte[buffersize];
                m_dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(m_bytes, System.Runtime.InteropServices.GCHandleType.Pinned);

            }
            else
            {
                m_dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(m_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            }

            
            m_intptr = m_dataGCHandle.AddrOfPinnedObject();
 

        }

        protected void Release()
        {
            if(m_intptr != System.IntPtr.Zero)
            {
                m_dataGCHandle.Free();
            }
            if(m_offsets!=null)
            {
                m_offsets.Clear();
            }
            if(m_validities !=null)
            {
                m_validities.Clear();
            }

    
           
            m_intptr = System.IntPtr.Zero;
        }
        #endregion

        #region string
        public void PackStringArray(string[] strarray)
        {
            Release();
            if(strarray.Length==0)
            {
                return;
            }
            m_data = new T[strarray.Length];
            m_offsets = TileDB.VectorUInt64.Repeat(0, strarray.Length);

            List<byte[]> bytesList = new List<byte[]>();

            int totsize = 0;
            for(int i=0; i<strarray.Length; ++i)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(strarray[i]);
                totsize += bytes.Length;
                bytesList.Add(bytes);
            }

            m_bytes = new byte[totsize];

            int idx = 0;
            for (int i = 0; i < bytesList.Count; ++i)
            {
                bytesList[i].CopyTo(m_bytes, idx);
                idx += bytesList[i].Length;

                if(i<(bytesList.Count-1))
                {
                    m_offsets[i + 1] = m_offsets[i] + (ulong)bytesList[i].Length;
                }
                   
            }

            m_dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(m_bytes, System.Runtime.InteropServices.GCHandleType.Pinned);
            m_intptr = m_dataGCHandle.AddrOfPinnedObject();


        }

        public string[] UnPackStringArray(int totbytesize)
        {
            string[] data = new string[m_offsets.Count];
            if(totbytesize > m_bytes.Length)
            {
                totbytesize = m_bytes.Length;
            }
            if (m_offsets.Count == 0 || totbytesize == 0)
            {
                return data;
            }

            int totsize = 0;
            int indexFrom = 0;
            for(int i=0; i<(m_offsets.Count); ++i)
            {
                int bytesize = 0;
                if(i<(m_offsets.Count-1))
                {
                    bytesize = (int)(m_offsets[i + 1] - m_offsets[i]);

                }
                else
                {
                    bytesize = totbytesize - (int)m_offsets[m_offsets.Count - 1];

                }
                
                totsize += bytesize;
                if (totsize <= totbytesize && bytesize>0)
                {
                    byte[] bytes = new byte[bytesize];// 
                    System.Array.Copy(m_bytes, indexFrom, bytes, 0, bytesize);
                    data[i] = Encoding.ASCII.GetString(bytes);
                    indexFrom = totsize;
                }
                
            }

            return data;
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

        public System.UInt64 BufferSize
        {
            get { return m_bytes.Length > 0 ? (System.UInt64)m_bytes.Length : (System.UInt64)m_data.Length; }
        }

        public TileDB.VectorUInt64 Offsets
        {
            get { return m_offsets; }
        }

        public TileDB.VectorUInt8 Validities
        {
            get { return m_validities; }
        }

        public byte[] BufferBytes
        {
            get { return m_bytes; }
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
        protected byte[] m_bytes;
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