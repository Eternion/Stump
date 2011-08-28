using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Spells
{
    [Serializable]
    [ActiveRecord("spells_type")]
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellType : WorldBaseRecord<SpellType>
    {
        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("longNameId")]
        [Property("LongNameId")]
        public uint LongNameId
        {
            get;
            set;
        }

        private string m_name;

        public string LongName
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(LongNameId) );
            }
        }

        [D2OField("shortNameId")]
        [Property("ShortNameId")]
        public uint ShortNameId
        {
            get;
            set;
        }

        private string m_shortName;

        public string ShortName
        {
            get
            {
                return m_shortName ?? ( m_shortName = TextManager.Instance.GetText(ShortNameId) );
            }
        }
    }
}