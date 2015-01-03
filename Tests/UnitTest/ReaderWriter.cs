using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stump.Core.IO;

namespace UnitTest
{
    [TestClass]
    public class ReaderWriter
    {
        [TestMethod]
        public void TestVarInt()
        {
            var writer = new BigEndianWriter();
            writer.WriteVarInt(128);
            writer.WriteVarInt(255);
            writer.WriteVarInt(-128);
            writer.WriteVarInt(int.MaxValue);
            writer.WriteVarInt(int.MinValue);
            writer.WriteVarShort(255);
            writer.WriteVarShort(short.MaxValue);
            writer.WriteVarShort(short.MinValue);
            var reader = new BigEndianReader(writer.Data);
            var int1 = reader.ReadVarInt();
            Assert.AreEqual(int1, 128);
            var int2 = reader.ReadVarInt();
            Assert.AreEqual(int2, 255);
            var int3 = reader.ReadVarInt();
            Assert.AreEqual(int3, -128);
            var int4 = reader.ReadVarInt();
            Assert.AreEqual(int4, int.MaxValue);
            var int5 = reader.ReadVarInt();
            Assert.AreEqual(int5, int.MinValue);
            var short1 = reader.ReadVarShort();
            Assert.AreEqual(short1, 255);
            var short2 = reader.ReadVarShort();
            Assert.AreEqual(short2, short.MaxValue);
            var short3 = reader.ReadVarShort();
            Assert.AreEqual(short3, short.MinValue);
        }
    }
}
