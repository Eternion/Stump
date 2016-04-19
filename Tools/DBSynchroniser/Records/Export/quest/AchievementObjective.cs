 


// Generated on 04/19/2016 10:18:10
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
    [TableName("AchievementObjectives")]
    [D2OClass("AchievementObjective", "com.ankamagames.dofus.datacenter.quest")]
    public class AchievementObjectiveRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "AchievementObjectives";
        public uint id;
        public uint achievementId;
        public uint order;
        [I18NField]
        public uint nameId;
        public String criterion;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint AchievementId
        {
            get { return achievementId; }
            set { achievementId = value; }
        }

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Criterion
        {
            get { return criterion; }
            set { criterion = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AchievementObjective)obj;
            
            Id = castedObj.id;
            AchievementId = castedObj.achievementId;
            Order = castedObj.order;
            NameId = castedObj.nameId;
            Criterion = castedObj.criterion;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AchievementObjective)parent : new AchievementObjective();
            obj.id = Id;
            obj.achievementId = AchievementId;
            obj.order = Order;
            obj.nameId = NameId;
            obj.criterion = Criterion;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}