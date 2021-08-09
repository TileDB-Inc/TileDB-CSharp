using System.Runtime.InteropServices;

namespace TileDB {
    public static class QueryExtensions {
        public static void set_vector_buffer<T>(this Query q, string attr, T buf, HandleRef bufHandle) {
            switch (buf.GetType().Name)
            {
                case "VectorInt32":
                    VectorInt32 v1 = new VectorInt32(bufHandle.Handle, true);
                    q.set_int32_vector_buffer(attr, v1);
                    break;
                case "VectorInt64":
                    VectorInt64 v2 = new VectorInt64(bufHandle.Handle, true);
                    q.set_int64_vector_buffer(attr, v2);
                    break;
                case "VectorUInt32":
                    VectorUInt32 v3 = new VectorUInt32(bufHandle.Handle, true);
                    q.set_uint32_vector_buffer(attr, v3);
                break;
                case "VectorUInt64":
                    VectorUInt64 v4 = new VectorUInt64(bufHandle.Handle, true);
                    q.set_uint64_vector_buffer(attr, v4);
                break;
                case "VectorDouble":
                    VectorDouble v5 = new VectorDouble(bufHandle.Handle, true);
                    q.set_double_vector_buffer(attr, v5);
                break;
                default:
                    break;
            }
        }
    }
}