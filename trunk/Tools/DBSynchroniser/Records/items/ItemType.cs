 


// Generated on 10/06/2013 14:21:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("ItemTypes")]
    [D2OClass("ItemType")]
    public class ItemTypeRecord : ID2ORecord
    {
        private const String MODULE = "ItemTypes";
        public int id;
        public uint nameId;
        public uint superTypeId;
        public Boolean plural;
        public uint gender;
        public String rawZone;
        public Boolean needUseConfirm;

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

        public uint SuperTypeId
        {
            get { return superTypeId; }
            set { superTypeId = value; }
        }

        public Boolean Plural
        {
            get { return plural; }
            set { plural = value; }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [NullString]
        public String RawZone
        {
            get { return rawZone; }
            set { rawZone = value; }
        }

        public Boolean NeedUseConfirm
        {
            get { return needUseConfirm; }
            set { needUseConfirm = value; }
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
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new ItemType();
            obj.id = Id;
            obj.nameId = NameId;
            obj.superTypeId = SuperTypeId;
            obj.plural = Plural;
            obj.gender = Gender;
            obj.rawZone = RawZone;
            obj.needUseConfirm = NeedUseConfirm;
            return obj;
        
        }
    }
}