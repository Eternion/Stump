 


// Generated on 04/19/2016 10:18:08
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
    [TableName("VeteranRewards")]
    [D2OClass("VeteranReward", "com.ankamagames.dofus.datacenter.items")]
    public class VeteranRewardRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "VeteranRewards";
        public int id;
        public uint requiredSubDays;
        public uint itemGID;
        public uint itemQuantity;

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
        public uint RequiredSubDays
        {
            get { return requiredSubDays; }
            set { requiredSubDays = value; }
        }

        [D2OIgnore]
        public uint ItemGID
        {
            get { return itemGID; }
            set { itemGID = value; }
        }

        [D2OIgnore]
        public uint ItemQuantity
        {
            get { return itemQuantity; }
            set { itemQuantity = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (VeteranReward)obj;
            
            Id = castedObj.id;
            RequiredSubDays = castedObj.requiredSubDays;
            ItemGID = castedObj.itemGID;
            ItemQuantity = castedObj.itemQuantity;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (VeteranReward)parent : new VeteranReward();
            obj.id = Id;
            obj.requiredSubDays = RequiredSubDays;
            obj.itemGID = ItemGID;
            obj.itemQuantity = ItemQuantity;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}