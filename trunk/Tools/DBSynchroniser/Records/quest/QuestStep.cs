 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("QuestSteps")]
    public class QuestStepRecord : ID2ORecord
    {
        private const String MODULE = "QuestSteps";
        public uint id;
        public uint questId;
        public uint nameId;
        public uint descriptionId;
        public int dialogId;
        public uint optimalLevel;
        public float duration;
        public Boolean kamasScaleWithPlayerLevel;
        public float kamasRatio;
        public float xpRatio;
        public List<uint> objectiveIds;
        public List<uint> rewardsIds;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint QuestId
        {
            get { return questId; }
            set { questId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int DialogId
        {
            get { return dialogId; }
            set { dialogId = value; }
        }

        public uint OptimalLevel
        {
            get { return optimalLevel; }
            set { optimalLevel = value; }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public Boolean KamasScaleWithPlayerLevel
        {
            get { return kamasScaleWithPlayerLevel; }
            set { kamasScaleWithPlayerLevel = value; }
        }

        public float KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }

        public float XpRatio
        {
            get { return xpRatio; }
            set { xpRatio = value; }
        }

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
        public byte[] ObjectiveIdsBin
        {
            get { return m_objectiveIdsBin; }
            set
            {
                m_objectiveIdsBin = value;
                objectiveIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

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
        public byte[] RewardsIdsBin
        {
            get { return m_rewardsIdsBin; }
            set
            {
                m_rewardsIdsBin = value;
                rewardsIds = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public void AssignFields(object obj)
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
        
        public object CreateObject()
        {
            var obj = new QuestStep();
            
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