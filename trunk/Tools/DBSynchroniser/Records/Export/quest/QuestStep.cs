 


// Generated on 10/28/2013 14:03:26
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
    [TableName("QuestSteps")]
    [D2OClass("QuestStep", "com.ankamagames.dofus.datacenter.quest")]
    public class QuestStepRecord : ID2ORecord
    {
        private const String MODULE = "QuestSteps";
        public uint id;
        public uint questId;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public int dialogId;
        public uint optimalLevel;
        public double duration;
        public Boolean kamasScaleWithPlayerLevel;
        public double kamasRatio;
        public double xpRatio;
        public List<uint> objectiveIds;
        public List<uint> rewardsIds;

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
        public uint QuestId
        {
            get { return questId; }
            set { questId = value; }
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
        public int DialogId
        {
            get { return dialogId; }
            set { dialogId = value; }
        }

        [D2OIgnore]
        public uint OptimalLevel
        {
            get { return optimalLevel; }
            set { optimalLevel = value; }
        }

        [D2OIgnore]
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [D2OIgnore]
        public Boolean KamasScaleWithPlayerLevel
        {
            get { return kamasScaleWithPlayerLevel; }
            set { kamasScaleWithPlayerLevel = value; }
        }

        [D2OIgnore]
        public double KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }

        [D2OIgnore]
        public double XpRatio
        {
            get { return xpRatio; }
            set { xpRatio = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> ObjectiveIds
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
                objectiveIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> RewardsIds
        {
            get { return rewardsIds; }
            set
            {
                rewardsIds = value;
                m_rewardsIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_rewardsIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] RewardsIdsBin
        {
            get { return m_rewardsIdsBin; }
            set
            {
                m_rewardsIdsBin = value;
                rewardsIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (QuestStep)obj;
            
            Id = castedObj.id;
            QuestId = castedObj.questId;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            DialogId = castedObj.dialogId;
            OptimalLevel = castedObj.optimalLevel;
            Duration = castedObj.duration;
            KamasScaleWithPlayerLevel = castedObj.kamasScaleWithPlayerLevel;
            KamasRatio = castedObj.kamasRatio;
            XpRatio = castedObj.xpRatio;
            ObjectiveIds = castedObj.objectiveIds;
            RewardsIds = castedObj.rewardsIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (QuestStep)parent : new QuestStep();
            obj.id = Id;
            obj.questId = QuestId;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.dialogId = DialogId;
            obj.optimalLevel = OptimalLevel;
            obj.duration = Duration;
            obj.kamasScaleWithPlayerLevel = KamasScaleWithPlayerLevel;
            obj.kamasRatio = KamasRatio;
            obj.xpRatio = XpRatio;
            obj.objectiveIds = ObjectiveIds;
            obj.rewardsIds = RewardsIds;
            return obj;
        
        }
    }
}