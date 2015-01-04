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

            writer.WriteVarLong(0x7FFFFFFF);
            writer.WriteVarLong(0xFFFFFFFF);
            writer.WriteVarLong(0x80000001);
            writer.WriteVarLong(long.MaxValue);
            writer.WriteVarLong(long.MinValue);
            var reader = new BigEndianReader(writer.Data);
            Assert.AreEqual(128, reader.ReadVarInt());
            Assert.AreEqual(255, reader.ReadVarInt());
            Assert.AreEqual(-128, reader.ReadVarInt());
            Assert.AreEqual(int.MaxValue, reader.ReadVarInt());
            Assert.AreEqual(int.MinValue, reader.ReadVarInt());
            Assert.AreEqual(255, reader.ReadVarShort());
            Assert.AreEqual(short.MaxValue, reader.ReadVarShort());
            Assert.AreEqual(short.MinValue, reader.ReadVarShort());
            Assert.AreEqual(0x7FFFFFFFL, reader.ReadVarLong());
            Assert.AreEqual(0xFFFFFFFFL, reader.ReadVarLong());
            Assert.AreEqual(0x80000001L, reader.ReadVarLong());
            Assert.AreEqual(long.MaxValue, reader.ReadVarLong());
            var value = reader.ReadVarLong();
            Assert.AreEqual(long.MinValue, value);
            
        }
    }
}
