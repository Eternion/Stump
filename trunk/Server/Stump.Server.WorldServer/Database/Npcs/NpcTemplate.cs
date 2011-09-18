using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs")]
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcTemplate
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

        [D2OField("dialogMessages")]
        [Property("DialogMessages", ColumnType = "Serializable")]
        public List<List<int>> DialogMessages
        {
            get;
            set;
        }

        [D2OField("dialogReplies")]
        [Property("DialogReplies", ColumnType = "Serializable")]
        public List<List<int>> DialogReplies
        {
            get;
            set;
        }

        [D2OField("actions")]
        [Property("Actions", ColumnType = "Serializable")]
        public List<uint> Actions
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

        [D2OField("look")]
        [Property("Look")]
        public String Look
        {
            get;
            set;
        }

    }
}