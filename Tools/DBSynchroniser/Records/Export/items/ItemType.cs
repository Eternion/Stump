 


// Generated on 01/04/2015 01:23:46
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
    [TableName("ItemTypes")]
    [D2OClass("ItemType", "com.ankamagames.dofus.datacenter.items")]
    public class ItemTypeRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "ItemTypes";
        public int id;
        [I18NField]
        public uint nameId;
        public uint superTypeId;
        public Boolean plural;
        public uint gender;
        public String rawZone;
        public Boolean needUseConfirm;
        public Boolean mimickable;

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
        public uint SuperTypeId
        {
            get { return superTypeId; }
            set { superTypeId = value; }
        }

        [D2OIgnore]
        public Boolean Plural
        {
            get { return plural; }
            set { plural = value; }
        }

        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [D2OIgnore]
        [NullString]
        public String RawZone
        {
            get { return rawZone; }
            set { rawZone = value; }
        }

        [D2OIgnore]
        public Boolean NeedUseConfirm
        {
            get { return needUseConfirm; }
            set { needUseConfirm = value; }
        }

        [D2OIgnore]
        public Boolean Mimickable
        {
            get { return mimickable; }
            set { mimickable = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (ItemType)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SuperTypeId = castedObj.superTypeId;
            Plural = castedObj.plural;
            Gender = castedObj.gender;
            RawZone = castedObj.rawZone;
            NeedUseConfirm = castedObj.needUseConfirm;
            Mimickable = castedObj.mimickable;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (ItemType)parent : new ItemType();
            obj.id = Id;
            obj.nameId = NameId;
            obj.superTypeId = SuperTypeId;
            obj.plural = Plural;
            obj.gender = Gender;
            obj.rawZone = RawZone;
            obj.needUseConfirm = NeedUseConfirm;
            obj.mimickable = Mimickable;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}