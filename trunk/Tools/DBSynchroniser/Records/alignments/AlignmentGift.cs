 


// Generated on 10/06/2013 01:10:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("AlignmentGift")]
    public class AlignmentGiftRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentGift";
        public int id;
        public uint nameId;
        public int effectId;
        public uint gfxId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int EffectId
        {
            get { return effectId; }
            set { effectId = value; }
        }

        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (AlignmentGift)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            EffectId = castedObj.effectId;
            GfxId = castedObj.gfxId;
        }
        
        public object CreateObject()
        {
            var obj = new AlignmentGift();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.effectId = EffectId;
            obj.gfxId = GfxId;
            return obj;
        
        }
    }
}