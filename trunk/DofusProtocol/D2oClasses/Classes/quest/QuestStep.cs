

// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestStep", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class QuestStep : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
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
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
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
        public List<uint> ObjectiveIds
        {
            get { return objectiveIds; }
            set { objectiveIds = value; }
        }
        [D2OIgnore]
        public List<uint> RewardsIds
        {
            get { return rewardsIds; }
            set { rewardsIds = value; }
        }
    }
}