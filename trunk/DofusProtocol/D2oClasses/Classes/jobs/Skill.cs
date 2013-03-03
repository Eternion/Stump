
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Skills")]
    [Serializable]
    public class Skill : IDataObject, IIndexedData
    {
        private const String MODULE = "Skills";
        public int id;
        public uint nameId;
        public int parentJobId;
        public Boolean isForgemagus;
        public int modifiableItemType;
        public int gatheredRessourceItem;
        public List<int> craftableItemIds;
        public int interactiveId;
        public String useAnimation;
        public Boolean isRepair;
        public int cursor;
        public Boolean availableInHouse;
        public uint levelMin;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int ParentJobId
        {
            get { return parentJobId; }
            set { parentJobId = value; }
        }

        public Boolean IsForgemagus
        {
            get { return isForgemagus; }
            set { isForgemagus = value; }
        }

        public int ModifiableItemType
        {
            get { return modifiableItemType; }
            set { modifiableItemType = value; }
        }

        public int GatheredRessourceItem
        {
            get { return gatheredRessourceItem; }
            set { gatheredRessourceItem = value; }
        }

        public List<int> CraftableItemIds
        {
            get { return craftableItemIds; }
            set { craftableItemIds = value; }
        }

        public int InteractiveId
        {
            get { return interactiveId; }
            set { interactiveId = value; }
        }

        public String UseAnimation
        {
            get { return useAnimation; }
            set { useAnimation = value; }
        }

        public Boolean IsRepair
        {
            get { return isRepair; }
            set { isRepair = value; }
        }

        public int Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        public Boolean AvailableInHouse
        {
            get { return availableInHouse; }
            set { availableInHouse = value; }
        }

        public uint LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }

    }
}