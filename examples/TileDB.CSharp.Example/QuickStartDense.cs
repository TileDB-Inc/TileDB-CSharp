using TileDB;
namespace TileDB_CSharp_Examples
{
    class QuickStartDense
    {
        static void Main(string[] args)
        {

            var config = new Config();
            var context = new Context(config);

            var attrName = "a";
            var attribute = new Attribute(context, attrName, DataType.TILEDB_STRING_ASCII);


            attribute.set_fill_value("testfill");
            var fill_value = attribute.fill_value();
            System.Console.WriteLine(fill_value);



        }
    }
}
