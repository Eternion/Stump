 


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
    [TableName("Achievements")]
    [D2OClass("Achievement", "com.ankamagames.dofus.datacenter.quest")]
    public class AchievementRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Achievements";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint categoryId;
        [I18NField]
        public uint descriptionId;
        public int iconId;
        public uint points;
        public uint level;
        public uint order;
        public double kamasRatio;
        public double experienceRatio;
        public Boolean kamasScaleWithPlayerLevel;
        public List<int> objectiveIds;
        public List<int> rewardIds;

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
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
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

        [D2OIgnore]
        public uint Points
        {
            get { return points; }
            set { points = value; }
        }

        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        public double KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }

        [D2OIgnore]
        public double ExperienceRatio
        {
            get { return experienceRatio; }
            set { experienceRatio = value; }
        }

        [D2OIgnore]
        public Boolean KamasScaleWithPlayerLevel
        {
            get { return kamasScaleWithPlayerLevel; }
            set { kamasScaleWithPlayerLevel = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> ObjectiveIds
        {
            get { return objectiveIds; }
            set
            {
                objectiveIds = value;
                m_objectiveIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_objectiveIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ObjectiveIdsBin
        {
            get { return m_objectiveIdsBin; }
            set
            {
                m_objectiveIdsBin = value;
                objectiveIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> RewardIds
        {
            get { return rewardIds; }
            set
            {
                rewardIds = value;
                m_rewardIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_rewardIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] RewardIdsBin
        {
            get { return m_rewardIdsBin; }
            set
            {
                m_rewardIdsBin = value;
                rewardIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Achievement)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            CategoryId = castedObj.categoryId;
            DescriptionId = castedObj.descriptionId;
            IconId = castedObj.iconId;
            Points = castedObj.points;
            Level = castedObj.level;
            Order = castedObj.order;
            KamasRatio = castedObj.kamasRatio;
            ExperienceRatio = castedObj.experienceRatio;
            KamasScaleWithPlayerLevel = castedObj.kamasScaleWithPlayerLevel;
            ObjectiveIds = castedObj.objectiveIds;
            RewardIds = castedObj.rewardIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Achievement)parent : new Achievement();
            obj.id = Id;
            obj.nameId = NameId;
            obj.categoryId = CategoryId;
            obj.descriptionId = DescriptionId;
            obj.iconId = IconId;
            obj.points = Points;
            obj.level = Level;
            obj.order = Order;
            obj.kamasRatio = KamasRatio;
            obj.experienceRatio = ExperienceRatio;
            obj.kamasScaleWithPlayerLevel = KamasScaleWithPlayerLevel;
            obj.objectiveIds = ObjectiveIds;
            obj.rewardIds = RewardIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_objectiveIdsBin = objectiveIds == null ? null : objectiveIds.ToBinary();
            m_rewardIdsBin = rewardIds == null ? null : rewardIds.ToBinary();
        
        }
    }
}