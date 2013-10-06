

// Generated on 10/06/2013 17:58:55
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
        private const String MODULE = "Achievements";
        public uint id;
        public uint nameId;
        public uint categoryId;
        public uint descriptionId;
        public int iconId;
        public uint points;
        public uint level;
        public uint order;
        public float kamasRatio;
        public float experienceRatio;
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
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
        public float KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }
        [D2OIgnore]
        public float ExperienceRatio
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
        public List<int> ObjectiveIds
        {
            get { return objectiveIds; }
            set { objectiveIds = value; }
        }
        [D2OIgnore]
        public List<int> RewardIds
        {
            get { return rewardIds; }
            set { rewardIds = value; }
        }
    }
}