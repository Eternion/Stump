
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Achievements")]
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

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public uint Points
        {
            get { return points; }
            set { points = value; }
        }

        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public float KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }

        public float ExperienceRatio
        {
            get { return experienceRatio; }
            set { experienceRatio = value; }
        }

        public Boolean KamasScaleWithPlayerLevel
        {
            get { return kamasScaleWithPlayerLevel; }
            set { kamasScaleWithPlayerLevel = value; }
        }

        public List<int> ObjectiveIds
        {
            get { return objectiveIds; }
            set { objectiveIds = value; }
        }

        public List<int> RewardIds
        {
            get { return rewardIds; }
            set { rewardIds = value; }
        }

    }
}