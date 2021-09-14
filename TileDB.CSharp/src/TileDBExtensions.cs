
namespace TileDB
{

public class ColumnData
{
        #region Constructors and Destructors
        ColumnData(string name,bool isdim = false)
        {
            m_name = name;
            m_isdim = isdim;

            m_data = null;
            m_offsets = null;
            m_validities = null;

        }
        ~ColumnData()
        {
            m_dataHandle.Free();
        }
        #endregion

        public void init(System.Type t, int length,bool isvar=false,bool isnullable=false)
        {
            m_data = System.Array.CreateInstance(t, length);
            m_dataHandle = System.Runtime.InteropServices.GCHandle.Alloc(m_data, System.Runtime.InteropServices.GCHandleType.Pinned);
            if(isvar)
            {
                m_offsets = TileDB.VectorUInt64.Repeat(0, length);
            }
            if(isnullable)
            {
                m_validities = TileDB.VectorUInt8.Repeat(0, length);
            }
        }

        #region Properties

        public string Name
        {
            get { return m_name; }
        }

        public bool IsDim
        {
            get { return m_isdim; }
        }

        public System.Array Data
        {
            get { return m_data; }
        }
        public System.IntPtr DataIntPtr 
        {
            get { return m_dataIntPtr; }
        }

 

        #endregion Properties

        #region Protected members
        string m_name;
        bool m_isdim;

        System.Array m_data;
        TileDB.VectorUInt64 m_offsets;
        TileDB.VectorUInt8 m_validities;
 
        System.Runtime.InteropServices.GCHandle m_dataHandle;
  

        System.IntPtr m_dataIntPtr;
   
        #endregion Protected members

    }

public static class TileDBExtensions
{

  public static void SetBuffer(this Query query, ColumnData bufferdata)
  {
     System.Type element_type = bufferdata.Data.GetType().GetElementType();
     ulong nelements = (ulong)bufferdata.Data.Length;
     uint element_size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(element_type);

     System.IntPtr intptr = bufferdata.DataIntPtr;
     query.set_buffer(bufferdata.Name, intptr, nelements, element_size);

  }



}//class TileDBExtensions

}//namespace TileDB