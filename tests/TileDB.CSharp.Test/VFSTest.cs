using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB.CSharp;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class VFSTest
    {
        [TestMethod]
        public void TestDir()
        {
            var ctx = Context.GetDefault();
            using (var vfs = new VFS(ctx)) 
            {
                string dirname = "tempdir";
                if(vfs.is_dir(dirname)) 
                {
                    vfs.remove_dir(dirname);
                }

                vfs.create_dir(dirname);
                Assert.IsTrue(vfs.is_dir(dirname));

                vfs.remove_dir(dirname);
            }
        }
    }

}//namespace