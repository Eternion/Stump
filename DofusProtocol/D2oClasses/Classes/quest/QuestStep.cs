

// Generated on 02/18/2015 11:06:01
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
        public const String MODULE = "QuestSteps";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint QuestId
        {
            get { return this.questId; }
            set { this.questId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int DialogId
        {
            get { return this.dialogId; }
            set { this.dialogId = value; }
        }
        [D2OIgnore]
        public uint OptimalLevel
        {
            get { return this.optimalLevel; }
            set { this.optimalLevel = value; }
        }
        [D2OIgnore]
        public double Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }
        [D2OIgnore]
        public Boolean KamasScaleWithPlayerLevel
        {
            get { return this.kamasScaleWithPlayerLevel; }
            set { this.kamasScaleWithPlayerLevel = value; }
        }
        [D2OIgnore]
        public double KamasRatio
        {
            get { return this.kamasRatio; }
            set { this.kamasRatio = value; }
        }
        [D2OIgnore]
        public double XpRatio
        {
            get { return this.xpRatio; }
            set { this.xpRatio = value; }
        }
        [D2OIgnore]
        public List<uint> ObjectiveIds
        {
            get { return this.objectiveIds; }
            set { this.objectiveIds = value; }
        }
        [D2OIgnore]
        public List<uint> RewardsIds
        {
            get { return this.rewardsIds; }
            set { this.rewardsIds = value; }
        }
    }
}