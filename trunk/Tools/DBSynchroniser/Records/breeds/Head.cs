 


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
    [D2OClass("Heads")]
    public class HeadRecord : ID2ORecord
    {
        private const String MODULE = "Heads";
        public int id;
        public String skins;
        public String assetId;
        public uint breed;
        public uint gender;
        public uint order;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Skins
        {
            get { return skins; }
            set { skins = value; }
        }

        public String AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }

        public uint Breed
        {
            get { return breed; }
            set { breed = value; }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Head)obj;
            
            Id = castedObj.id;
            Skins = castedObj.skins;
            AssetId = castedObj.assetId;
            Breed = castedObj.breed;
            Gender = castedObj.gender;
            Order = castedObj.order;
        }
        
        public object CreateObject()
        {
            var obj = new Head();
            
            obj.id = Id;
            obj.skins = Skins;
            obj.assetId = AssetId;
            obj.breed = Breed;
            obj.gender = Gender;
            obj.order = Order;
            return obj;
        
        }
    }
}