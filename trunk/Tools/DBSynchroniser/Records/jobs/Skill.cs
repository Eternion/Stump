 


// Generated on 10/19/2013 17:17:44
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Skills")]
    [D2OClass("Skill", "com.ankamagames.dofus.datacenter.jobs")]
    public class SkillRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
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

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] CraftableItemIdsBin
        {
            get { return m_craftableItemIdsBin; }
            set
            {
                m_craftableItemIdsBin = value;
                craftableItemIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        public int InteractiveId
        {
            get { return interactiveId; }
            set { interactiveId = value; }
        }

        [D2OIgnore]
        [NullString]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Skill)parent : new Skill();
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