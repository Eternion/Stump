 


// Generated on 10/26/2014 23:31:12
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentGift")]
    [D2OClass("AlignmentGift", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentGiftRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "AlignmentGift";
        public int id;
        [I18NField]
        public uint nameId;
        public int effectId;
        public uint gfxId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int EffectId
        {
            get { return effectId; }
            set { effectId = value; }
        }

        [D2OIgnore]
        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentGift)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            EffectId = castedObj.effectId;
            GfxId = castedObj.gfxId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlignmentGift)parent : new AlignmentGift();
            obj.id = Id;
            obj.nameId = NameId;
            obj.effectId = EffectId;
            obj.gfxId = GfxId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}