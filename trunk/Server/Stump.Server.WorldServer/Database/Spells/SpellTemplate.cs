using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class SpellTemplateConfiguration : EntityTypeConfiguration<SpellTemplate>
    {
        public SpellTemplateConfiguration()
        {
            ToTable("spells_templates");
        }
    }
    [D2OClass("Spell", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellTemplate : IAssignedByD2O, ISaveIntercepter
    {
        private string m_description;
        private string m_name;

        public int Id
        {
            get;
            set;
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

        public uint DescriptionId
        {
            get;
            set;
        }


        public string Description
        {
            get { return m_description ?? (m_description = TextManager.Instance.GetText(NameId)); }
        }

        public uint TypeId
        {
            get;
            set;
        }

        public String ScriptParams
        {
            get;
            set;
        }

        public String ScriptParamsCritical
        {
            get;
            set;
        }

        public int ScriptId
        {
            get;
            set;
        }

        public int ScriptIdCritical
        {
            get;
            set;
        }

        public int IconId
        {
            get;
            set;
        }

        private byte[] m_spellLevelsIdsBin;

        public byte[] SpellLevelsIdsBin
        {
            get { return m_spellLevelsIdsBin; }
            set { m_spellLevelsIdsBin = value;
            SpellLevelsIds = value.ToObject<List<uint>>();
            }
        }

        public List<uint> SpellLevelsIds
        {
            get;
            set;
        }

        public Boolean UseParamCache
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Name);
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_spellLevelsIdsBin = SpellLevelsIds.ToBinary();
        }

        public void AssignFields(object d2oObject)
        {
            var spell = (DofusProtocol.D2oClasses.Spell)d2oObject;
            Id = spell.id;
            NameId = spell.nameId;
            DescriptionId = spell.descriptionId;
            TypeId = spell.typeId;
            ScriptParams = spell.scriptParams;
            ScriptParamsCritical = spell.scriptParamsCritical;
            ScriptId = spell.scriptId;
            ScriptIdCritical = spell.scriptIdCritical;
            IconId = spell.iconId;
            SpellLevelsIds = spell.spellLevels;
            UseParamCache = spell.useParamCache;
        }
    }
}