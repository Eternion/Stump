 


// Generated on 11/16/2015 14:26:38
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
    [TableName("Heads")]
    [D2OClass("Head", "com.ankamagames.dofus.datacenter.breeds")]
    public class HeadRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Heads";
        public int id;
        public String skins;
        public String assetId;
        public uint breed;
        public uint gender;
        public String label;
        public uint order;

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
        [NullString]
        public String Skins
        {
            get { return skins; }
            set { skins = value; }
        }

        [D2OIgnore]
        [NullString]
        public String AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        [D2OIgnore]
        public uint Breed
        {
            get { return breed; }
            set { breed = value; }
        }

        [D2OIgnore]
        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Label
        {
            get { return label; }
            set { label = value; }
        }

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Head)obj;
            
            Id = castedObj.id;
            Skins = castedObj.skins;
            AssetId = castedObj.assetId;
            Breed = castedObj.breed;
            Gender = castedObj.gender;
            Label = castedObj.label;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Head)parent : new Head();
            obj.id = Id;
            obj.skins = Skins;
            obj.assetId = AssetId;
            obj.breed = Breed;
            obj.gender = Gender;
            obj.label = Label;
            obj.order = Order;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}