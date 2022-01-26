namespace TileDB_CSharp_Examples
{
    class QuickStartDense
    {
        static void Main(string[] args)
        {
            TileDB.Config config = new TileDB.Config();
            int status;
            // Set values
            status = config.set("sm.memory_budget", "512000000");
            status = config.set("vfs.s3.connect_timeout_ms", "5000");
            status = config.set("vfs.s3.endpoint_override", "localhost:8888");

            // Get values

            string memory_budget = config.get("sm.memory_budget");
            System.Console.WriteLine("memory_budget:{0}", memory_budget);


            // Save to a file
            config.save_to_file("temp.cfg");

            TileDB.Config config2 = new TileDB.Config();
            config2.load_from_file("temp.cfg");
            string? memory_budget2 = config2.get("sm.memory_budget");
            System.Console.WriteLine("memory_budget2:{0}", memory_budget2);


            TileDB.Context ctx = new TileDB.Context(config);
            ctx.set_tag("key1", "value1");
            string? stats = ctx.stats();
            System.Console.WriteLine("stats:{0}", stats);


        }
    }
}
