 


// Generated on 10/06/2013 14:22:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Skills")]
    [D2OClass("Skill")]
    public class SkillRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        [Ignore]
        public List<int> CraftableItemIds
        {
            get { return craftableItemIds; }
            set
            {
                craftableItemIds = value;
                m_craftableItemIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_craftableItemIdsBin;
        public byte[] CraftableItemIdsBin
        {
            get { return m_craftableItemIdsBin; }
            set
            {
                m_craftableItemIdsBin = value;
                craftableItemIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public int InteractiveId
        {
            get { return interactiveId; }
            set { interactiveId = value; }
        }

        [NullString]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Skill)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ParentJobId = castedObj.parentJobId;
            IsForgemagus = castedObj.isForgemagus;
            ModifiableItemType = castedObj.modifiableItemType;
            GatheredRessourceItem = castedObj.gatheredRessourceItem;
            CraftableItemIds = castedObj.craftableItemIds;
            InteractiveId = castedObj.interactiveId;
            UseAnimation = castedObj.useAnimation;
            IsRepair = castedObj.isRepair;
            Cursor = castedObj.cursor;
            AvailableInHouse = castedObj.availableInHouse;
            LevelMin = castedObj.levelMin;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Skill();
            obj.id = Id;
            obj.nameId = NameId;
            obj.parentJobId = ParentJobId;
            obj.isForgemagus = IsForgemagus;
            obj.modifiableItemType = ModifiableItemType;
            obj.gatheredRessourceItem = GatheredRessourceItem;
            obj.craftableItemIds = CraftableItemIds;
            obj.interactiveId = InteractiveId;
            obj.useAnimation = UseAnimation;
            obj.isRepair = IsRepair;
            obj.cursor = Cursor;
            obj.availableInHouse = AvailableInHouse;
            obj.levelMin = LevelMin;
            return obj;
        
        }
    }
}