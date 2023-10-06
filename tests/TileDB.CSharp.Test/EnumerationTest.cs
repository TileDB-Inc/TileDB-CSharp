using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class EnumerationTest
    {
        [TestMethod]
        public void TestString()
        {
            string[] expectedValues = new[] { "Hello", "World", "👋" };
            using Enumeration e = Enumeration.Create(Context.GetDefault(), "test_string", true, expectedValues);

            System.Array values = e.GetValues();
            byte[] rawData = e.GetRawData();
            ulong[] rawOffsets = e.GetRawOffsets();

            Assert.AreEqual("test_string", e.GetName());
            Assert.IsTrue(e.IsOrdered);
            Assert.AreEqual(DataType.StringUtf8, e.DataType);
            Assert.AreEqual(Enumeration.VariableSized, e.ValuesPerMember);
            CollectionAssert.AreEqual(expectedValues, values);
            CollectionAssert.AreEqual("HelloWorld👋"u8.ToArray(), rawData);
            CollectionAssert.AreEqual(new ulong[] { 0, 5, 10 }, rawOffsets);
        }

        [TestMethod]
        [DataRow(1u)]
        [DataRow(2u)]
        [DataRow(4u)]
        public void TestFixedSize(uint cellValNum)
        {
            int[] expectedValues = new[] { 1, 2, 3, 4 };

            using Enumeration e = Enumeration.Create<int>(Context.GetDefault(), "test_fixed", false, expectedValues, cellValNum);

            System.Array values = e.GetValues();
            byte[] rawData = e.GetRawData();
            ulong[] rawOffsets = e.GetRawOffsets();

            Assert.AreEqual("test_fixed", e.GetName());
            Assert.IsFalse(e.IsOrdered);
            Assert.AreEqual(DataType.Int32, e.DataType);
            Assert.AreEqual(cellValNum, e.ValuesPerMember);
            CollectionAssert.AreEqual(expectedValues, values);
            CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0 }, rawData);
            Assert.AreEqual(0, rawOffsets.Length);
        }

        [TestMethod]
        public void TestVarSize()
        {
            int[] expectedValues = new int[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };
            ulong[] expectedOffsets = new ulong[] { 0, 4, 12, 24, 40 };

            using Enumeration e = Enumeration.Create<int>(Context.GetDefault(), "test_var", false, expectedValues, expectedOffsets);

            System.Array values = e.GetValues();
            byte[] rawData = e.GetRawData();
            ulong[] rawOffsets = e.GetRawOffsets();

            Assert.AreEqual("test_var", e.GetName());
            Assert.IsFalse(e.IsOrdered);
            Assert.AreEqual(DataType.Int32, e.DataType);
            Assert.AreEqual(Enumeration.VariableSized, e.ValuesPerMember);
            CollectionAssert.AreEqual(expectedValues, values);
            CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0 }, rawData);
            Assert.AreEqual(0, rawOffsets.Length);
        }
    }
}
