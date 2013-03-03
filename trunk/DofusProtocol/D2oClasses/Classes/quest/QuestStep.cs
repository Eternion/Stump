
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestSteps")]
    [Serializable]
    public class QuestStep : IDataObject, IIndexedData
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

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

        public List<uint> ObjectiveIds
        {
            get { return objectiveIds; }
            set { objectiveIds = value; }
        }

        public List<uint> RewardsIds
        {
            get { return rewardsIds; }
            set { rewardsIds = value; }
        }

    }
}