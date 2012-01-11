using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;

namespace UnitTest
{
    [TestClass]
    public class EntitLookParserTest
    {
        [TestMethod]
        public void SimpleEntity()
        {
            var entity = new EntityLook(123, new short[0], new int[0], new short[0], new SubEntity[0]);

            var str = entity.ConvertToString();
            var obj = str.ToEntityLook();

            Assert.AreEqual(entity.ConvertToString(), obj.ConvertToString());
        }

        [TestMethod]
        public void ComplexEntity()
        {
            var entity = new EntityLook()
            {
                bonesId = 123,
                skins = new short[0],
                scales = new short[0],
                indexedColors = new [] { 0x1433421 },
                subentities = new [] { new SubEntity(0, 1, new EntityLook(100, new short[] { 120 }, new int[0], new short[0], new SubEntity[0]))}
            };

            var str = entity.ConvertToString();
            var obj = str.ToEntityLook();

            Assert.AreEqual(entity.ConvertToString(), obj.ConvertToString());
        }
    }
}
