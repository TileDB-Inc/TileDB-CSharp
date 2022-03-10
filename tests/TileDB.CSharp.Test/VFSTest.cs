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
                if(vfs.IsDir(dirname)) 
                {
                    vfs.RemoveDir(dirname);
                }

                vfs.CreateDir(dirname);
                Assert.IsTrue(vfs.IsDir(dirname));

                vfs.RemoveDir(dirname);
            }
        }
    }

}//namespace