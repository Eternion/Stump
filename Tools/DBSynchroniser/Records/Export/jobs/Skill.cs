 


// Generated on 09/01/2015 10:48:49
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
    public class SkillRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Skills";
        public int id;
        [I18NField]
        public uint nameId;
        public int parentJobId;
        public Boolean isForgemagus;
        public List<int> modifiableItemTypeIds;
        public int gatheredRessourceItem;
        public List<int> craftableItemIds;
        public int interactiveId;
        public String useAnimation;
        public int cursor;
        public int elementActionId;
        public Boolean availableInHouse;
        public uint levelMin;
        public Boolean clientDisplay;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


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
        [Ignore]
        public List<int> ModifiableItemTypeIds
        {
            get { return modifiableItemTypeIds; }
            set
            {
                modifiableItemTypeIds = value;
                m_modifiableItemTypeIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_modifiableItemTypeIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ModifiableItemTypeIdsBin
        {
            get { return m_modifiableItemTypeIdsBin; }
            set
            {
                m_modifiableItemTypeIdsBin = value;
                modifiableItemTypeIds = value == null ? null : value.ToObject<List<int>>();
            }
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
        public int Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        [D2OIgnore]
        public int ElementActionId
        {
            get { return elementActionId; }
            set { elementActionId = value; }
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

        [D2OIgnore]
        public Boolean ClientDisplay
        {
            get { return clientDisplay; }
            set { clientDisplay = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Skill)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ParentJobId = castedObj.parentJobId;
            IsForgemagus = castedObj.isForgemagus;
            ModifiableItemTypeIds = castedObj.modifiableItemTypeIds;
            GatheredRessourceItem = castedObj.gatheredRessourceItem;
            CraftableItemIds = castedObj.craftableItemIds;
            InteractiveId = castedObj.interactiveId;
            UseAnimation = castedObj.useAnimation;
            Cursor = castedObj.cursor;
            ElementActionId = castedObj.elementActionId;
            AvailableInHouse = castedObj.availableInHouse;
            LevelMin = castedObj.levelMin;
            ClientDisplay = castedObj.clientDisplay;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Skill)parent : new Skill();
            obj.id = Id;
            obj.nameId = NameId;
            obj.parentJobId = ParentJobId;
            obj.isForgemagus = IsForgemagus;
            obj.modifiableItemTypeIds = ModifiableItemTypeIds;
            obj.gatheredRessourceItem = GatheredRessourceItem;
            obj.craftableItemIds = CraftableItemIds;
            obj.interactiveId = InteractiveId;
            obj.useAnimation = UseAnimation;
            obj.cursor = Cursor;
            obj.elementActionId = ElementActionId;
            obj.availableInHouse = AvailableInHouse;
            obj.levelMin = LevelMin;
            obj.clientDisplay = ClientDisplay;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_modifiableItemTypeIdsBin = modifiableItemTypeIds == null ? null : modifiableItemTypeIds.ToBinary();
            m_craftableItemIdsBin = craftableItemIds == null ? null : craftableItemIds.ToBinary();
        
        }
    }
}