

// Generated on 11/16/2015 14:26:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Achievement", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class Achievement : IDataObject, IIndexedData
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public uint Points
        {
            get { return this.points; }
            set { this.points = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public double KamasRatio
        {
            get { return this.kamasRatio; }
            set { this.kamasRatio = value; }
        }
        [D2OIgnore]
        public double ExperienceRatio
        {
            get { return this.experienceRatio; }
            set { this.experienceRatio = value; }
        }
        [D2OIgnore]
        public Boolean KamasScaleWithPlayerLevel
        {
            get { return this.kamasScaleWithPlayerLevel; }
            set { this.kamasScaleWithPlayerLevel = value; }
        }
        [D2OIgnore]
        public List<int> ObjectiveIds
        {
            get { return this.objectiveIds; }
            set { this.objectiveIds = value; }
        }
        [D2OIgnore]
        public List<int> RewardIds
        {
            get { return this.rewardIds; }
            set { this.rewardIds = value; }
        }
    }
}