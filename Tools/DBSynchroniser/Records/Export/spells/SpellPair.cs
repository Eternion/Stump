 


// Generated on 09/01/2015 10:48:50
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
    [TableName("SpellPairs")]
    [D2OClass("SpellPair", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellPairRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SpellPairs";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public int iconId;

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
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellPair)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            IconId = castedObj.iconId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SpellPair)parent : new SpellPair();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.iconId = IconId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}