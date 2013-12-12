

// Generated on 12/12/2013 16:57:40
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Skill", "com.ankamagames.dofus.datacenter.jobs")]
    [Serializable]
    public class Skill : IDataObject, IIndexedData
    {
        public const String MODULE = "Skills";
        public int id;
        [I18NField]
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
        [D2OIgnore]
        public int Id
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
        public int ParentJobId
        {
            get { return parentJobId; }
            set { parentJobId = value; }
        }
        [D2OIgnore]
        public Boolean IsForgemagus
        {
            get { return isForgemagus; }
            set { isForgemagus = value; }
        }
        [D2OIgnore]
        public int ModifiableItemType
        {
            get { return modifiableItemType; }
            set { modifiableItemType = value; }
        }
        [D2OIgnore]
        public int GatheredRessourceItem
        {
            get { return gatheredRessourceItem; }
            set { gatheredRessourceItem = value; }
        }
        [D2OIgnore]
        public List<int> CraftableItemIds
        {
            get { return craftableItemIds; }
            set { craftableItemIds = value; }
        }
        [D2OIgnore]
        public int InteractiveId
        {
            get { return interactiveId; }
            set { interactiveId = value; }
        }
        [D2OIgnore]
        public String UseAnimation
        {
            get { return useAnimation; }
            set { useAnimation = value; }
        }
        [D2OIgnore]
        public Boolean IsRepair
        {
            get { return isRepair; }
            set { isRepair = value; }
        }
        [D2OIgnore]
        public int Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }
        [D2OIgnore]
        public Boolean AvailableInHouse
        {
            get { return availableInHouse; }
            set { availableInHouse = value; }
        }
        [D2OIgnore]
        public uint LevelMin
        {
            get { return levelMin; }
            set { levelMin = value; }
        }
    }
}