using System;
using System.Collections;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("superareas")]
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SuperAreaRecord : WorldBaseRecord<SuperAreaRecord>
    {

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

        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        [D2OField("worldmapId")]
        [Property("WorldmapId")]
        public uint WorldmapId
        {
            get;
            set;
        }
    }
}