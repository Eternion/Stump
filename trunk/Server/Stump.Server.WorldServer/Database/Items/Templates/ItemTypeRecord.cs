using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class ItemTypeRecordConfiguration : EntityTypeConfiguration<ItemTypeRecord>
    {
        public ItemTypeRecordConfiguration()
        {
            ToTable("items_types");
            Ignore(x => x.ZoneShape);
            Ignore(x => x.ZoneSize);
        }
    }

    [D2OClass("ItemType", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemTypeRecord : IAssignedByD2O, ISaveIntercepter
    {
        private string m_name;

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

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public uint SuperTypeId
        {
            get;
            set;
        }

        public ItemSuperTypeEnum SuperType
        {
            get { return (ItemSuperTypeEnum) SuperTypeId; }
        }

        public Boolean Plural
        {
            get;
            set;
        }

        public uint Gender
        {
            get;
            set;
        }

        private string m_rawZone;

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

        public Boolean NeedUseConfirm
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

        private void BuildRawZone()
        {
            m_rawZone = ZoneShape.ToString() + ZoneSize;
        }

        public void AssignFields(object d2oObject)
        {
            var type = (DofusProtocol.D2oClasses.ItemType)d2oObject;
            Id = type.id;
            NameId = type.nameId;
            SuperTypeId = type.superTypeId;
            Plural = type.plural;
            Gender = type.gender;
            RawZone = type.rawZone;
            NeedUseConfirm = type.needUseConfirm;
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            BuildRawZone();   
        }
    }
}