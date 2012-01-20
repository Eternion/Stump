using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters")]
    [D2OClass("Monster", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterTemplate : WorldBaseRecord<MonsterTemplate>
    {
        private EntityLook m_entityLook;
        private string m_lookAsString;
        private string m_name;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
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

        [D2OField("gfxId")]
        [Property("GfxId")]
        public uint GfxId
        {
            get;
            set;
        }

        [D2OField("race")]
        [Property("Race")]
        public int Race
        {
            get;
            set;
        }

        [Property]
        public int MinDroppedKamas
        {
            get;
            set;
        }

        [Property]
        public int MaxDroppedKamas
        {
            get;
            set;
        }

        private List<DroppableItem> m_droppableItems;
        public List<DroppableItem> DroppableItems
        {
            get
            {
                return m_droppableItems ?? ( m_droppableItems = MonsterManager.Instance.GetMonsterDroppableItems(Id) );
            }
        }

        private List<MonsterGrade> m_grades;
        public List<MonsterGrade> Grades
        {
            get
            {
                return m_grades ?? ( m_grades = MonsterManager.Instance.GetMonsterGrades(Id) );
            }
        }

        [D2OField("look")]
        [Property("Look")]
        private string LookAsString
        {
            get
            {
                if (EntityLook == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(m_lookAsString))
                    m_lookAsString = EntityLook.ConvertToString();

                return m_lookAsString;
            }
            set
            {
                m_lookAsString = value;

                if (!string.IsNullOrEmpty(value) && value != "null")
                    m_entityLook = m_lookAsString.ToEntityLook();
                else
                    m_entityLook = null;
            }
        }

        public EntityLook EntityLook
        {
            get { return m_entityLook; }
            set
            {
                m_entityLook = value;

                if (value != null)
                    m_lookAsString = value.ConvertToString();
            }
        }

        [D2OField("useSummonSlot")]
        [Property("UseSummonSlot")]
        public Boolean UseSummonSlot
        {
            get;
            set;
        }

        [D2OField("useBombSlot")]
        [Property("UseBombSlot")]
        public Boolean UseBombSlot
        {
            get;
            set;
        }

        [D2OField("canPlay")]
        [Property("CanPlay")]
        public Boolean CanPlay
        {
            get;
            set;
        }

        [D2OField("isBoss")]
        [Property("IsBoss")]
        public Boolean IsBoss
        {
            get;
            set;
        }
    }
}