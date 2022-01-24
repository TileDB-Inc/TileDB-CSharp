using System;
using System.Linq;
namespace TileDB.CSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            string array_uri = "test_doubledimension_array";

            CreateArray(array_uri,-10.0,10.0,-10.0,10.0,10.0,10.0);

            int xNumGrids = 250;
            int yNumGrids = 200;
            double minX = -8.08;
            double maxX = 5.96;
            double minY = -5.32;
            double maxY = 5.28;

            var simdata = GetSimulatedDataBuffers(xNumGrids, yNumGrids, minX, maxX, minY, maxY);
            double minX_sim = simdata.xbuffer.Data.Min();
            double maxX_sim = simdata.xbuffer.Data.Max();
            double minY_sim = simdata.ybuffer.Data.Min();
            double maxY_sim = simdata.ybuffer.Data.Max();
            System.Console.WriteLine("simulated data, xmin:{0},xmax:{1},ymin:{2},ymax:{3}", minX_sim, maxX_sim, minY_sim, maxY_sim);

            WriteArray(array_uri,simdata.xbuffer,simdata.ybuffer,simdata.databuffer);

            var readresult = ReadArray(array_uri, xNumGrids * yNumGrids, minX_sim, maxX_sim, minY_sim, maxY_sim);
            var bufferelements = readresult.bufferelements;
            UInt64 num_results = 0;
            UInt64 buffer_size = 0;
            if (bufferelements.ContainsKey("a"))
            {
                var a_bufferelements = bufferelements["a"];
                num_results = a_bufferelements[0];
                buffer_size = a_bufferelements[1];
            }
            System.Console.WriteLine("number of reading results:{0}, buffer_size:{1}", num_results,buffer_size);

            var readresult2 = ReadArray(array_uri, xNumGrids * yNumGrids, minX_sim+0.01, maxX_sim-0.01, minY_sim+0.01, maxY_sim-0.01);
            var bufferelements2 = readresult2.bufferelements;
            UInt64 num_results2 = 0;
            UInt64 buffer_size2 = 0;
            if (bufferelements2.ContainsKey("a"))
            {
                var a_bufferelements2 = bufferelements2["a"];
                num_results2 = a_bufferelements2[0];
                buffer_size2 = a_bufferelements2[1];
            }
            System.Console.WriteLine("number of reading results2:{0}, buffer_size2:{1}", num_results2, buffer_size2);
        }

        static void CreateArray(string array_uri, double minX,double maxX,double minY,double maxY,double xTileSzie,double yTileSize)
        {
            var ctx = new TileDB.Context();
            var dom = new TileDB.Domain(ctx);

            //add dimensions
            dom.add_double_dimension("x", minX, maxX, xTileSzie);
            dom.add_double_dimension("y", minY, maxY, yTileSize);
            var schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_SPARSE);
            schema.set_domain(dom);
            //add attribute
            var attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_STRING_ASCII);
            schema.add_attribute(attr1);
          
            var vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri))
            {
                vfs.remove_dir(array_uri);

            }

            TileDB.Array.create(array_uri, schema);

        }


        static (TileDBBuffer<double> xbuffer,TileDBBuffer<double> ybuffer, TileDBBuffer<string> databuffer) GetSimulatedDataBuffers(int xNumGrids, int yNumGrids, double minX, double maxX, double minY, double maxY)
        {
            int num_data = xNumGrids * yNumGrids;
            TileDB.TileDBBuffer<double> xbuffer = new TileDB.TileDBBuffer<double>();
            xbuffer.Init(num_data, false, false);
            TileDB.TileDBBuffer<double> ybuffer = new TileDB.TileDBBuffer<double>();
            ybuffer.Init(num_data, false, false);
            TileDB.TileDBBuffer<string> databuffer = new TileDB.TileDBBuffer<string>();
            databuffer.Init(num_data, true, false, num_data * 12);


            string[] string_array = new string[num_data];

            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            Random rng = new Random(2022); //using 2022 as starting feed 
            char[] chars = new char[12];

            int count = 0;
            for (int i = 0; i < xNumGrids; ++i)
            {
                double x = minX + (maxX - minX) * rng.NextDouble();
             
                for (int j = 0; j < yNumGrids; ++j)
                {
                    double y = minY + (maxY - minY) * rng.NextDouble();
                    

                    xbuffer.Data[count] = x;
                    ybuffer.Data[count] = y;

                    for (int k = 0; k < 12; ++k)
                    {
                        chars[k] = allowedChars[rng.Next(0, allowedChars.Length)];
                    }
                    string_array[count] = new string(chars);

                    ++count;
                }
            }//for
            databuffer.PackStringArray(string_array);

            return (xbuffer, ybuffer, databuffer);

        }
        static void WriteArray(string array_uri, TileDB.TileDBBuffer<double> xbuffer, TileDB.TileDBBuffer<double> ybuffer, TileDB.TileDBBuffer<string> strbuffer )
        {
            var ctx = new TileDB.Context();
 

            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_WRITE);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
            query.set_buffer("x", xbuffer.DataIntPtr, xbuffer.BufferSize, xbuffer.ElementDataSize);
            query.set_buffer("y", ybuffer.DataIntPtr, ybuffer.BufferSize, ybuffer.ElementDataSize);
            query.set_buffer_with_offsets("a", strbuffer.DataIntPtr, strbuffer.BufferSize, strbuffer.ElementDataSize, strbuffer.Offsets);


            query.submit();
            query.finalize();
            array.close();

        }

        static (TileDB.MapStringVectorUInt64 bufferelements, TileDBBuffer<double> xbuffer,TileDBBuffer<double> ybuffer, TileDBBuffer<string> databuffer) ReadArray(string array_uri, int elementCount, double minX, double maxX, double minY, double maxY)
        {
      
            var ctx = new TileDB.Context();

            TileDB.TileDBBuffer<double> xbuffer = new TileDB.TileDBBuffer<double>();
            xbuffer.Init(elementCount, false, false);
            TileDB.TileDBBuffer<double> ybuffer = new TileDBBuffer<double>();
            ybuffer.Init(elementCount, false, false);
            TileDB.TileDBBuffer<string> databuffer = new TileDBBuffer<string>();
            databuffer.Init(elementCount, true, false, elementCount * 12);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
            query.set_buffer("x", xbuffer.DataIntPtr, xbuffer.BufferSize, xbuffer.ElementDataSize);
            query.set_buffer("y", ybuffer.DataIntPtr, ybuffer.BufferSize, ybuffer.ElementDataSize);
            query.set_buffer_with_offsets("a", databuffer.DataIntPtr, databuffer.BufferSize, databuffer.ElementDataSize, databuffer.Offsets);
            TileDB.VectorString x_range = new TileDB.VectorString();
            x_range.Add(minX.ToString());
            x_range.Add(maxX.ToString());
            query.add_range_from_str_vector(0, x_range);

            TileDB.VectorString y_range = new TileDB.VectorString();
            y_range.Add(minY.ToString());
            y_range.Add(maxY.ToString());
            query.add_range_from_str_vector(1, y_range);

            query.submit();
            query.finalize();

            var resultbufferelements = query.result_buffer_elements();
            array.close();

            return (resultbufferelements, xbuffer,ybuffer,databuffer);
        }

    }
}


 
  
