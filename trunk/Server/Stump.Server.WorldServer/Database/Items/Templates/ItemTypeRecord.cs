using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    [Serializable]
    [ActiveRecord("items_type")]
    [D2OClass("ItemType", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemTypeRecord : WorldBaseRecord<ItemTypeRecord>
    {
        private string m_name;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        public ItemTypeEnum ItemType
        {
            get
            {
                return (ItemTypeEnum)Id;
            }
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        [D2OField("superTypeId")]
        [Property("SuperTypeId")]
        public uint SuperTypeId
        {
            get;
            set;
        }

        public ItemSuperTypeEnum SuperType
        {
            get { return (ItemSuperTypeEnum) SuperTypeId; }
        }

        [D2OField("plural")]
        [Property("Plural")]
        public Boolean Plural
        {
            get;
            set;
        }

        [D2OField("gender")]
        [Property("Gender")]
        public uint Gender
        {
            get;
            set;
        }

        private string m_rawZone;

        [D2OField("rawZone")]
        [Property]
        public string RawZone
        {
            get { return m_rawZone; }
            set
            {
                m_rawZone = value; 
                ParseRawZone();
            }
        }

        public uint ZoneSize
        {
            get;
            set;
        }

        public SpellShapeEnum ZoneShape
        {
            get;
            set;
        }

        private void ParseRawZone()
        {
            if (string.IsNullOrEmpty(RawZone) || RawZone == "null")
            {
                ZoneSize = 0;
                ZoneShape = 0;
                return;
            }

            var type = (SpellShapeEnum)Enum.Parse(typeof(SpellShapeEnum), RawZone[0].ToString());
            var size = 1u;

            if (RawZone.Length > 1)
                size = uint.Parse(RawZone[1].ToString());

            ZoneSize = size;
            ZoneShape = type;
        }

        [D2OField("needUseConfirm")]
        [Property("NeedUseConfirm")]
        public Boolean NeedUseConfirm
        {
            get;
            set;
        }
    }
}