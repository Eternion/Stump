using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs")]
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcTemplate : WorldBaseRecord<NpcTemplate>
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
        public List<uint> ActionsIds
        {
            get;
            set;
        }

        private IList<NpcAction> m_actions;

        [HasMany(typeof(NpcAction))]
        public IList<NpcAction> Actions
        {
            get
            {
                return m_actions ?? ( m_actions = new List<NpcAction>() );
            }
            set { m_actions = value; }
        }

        public NpcAction GetNpcAction(NpcActionTypeEnum actionType)
        {
            return Actions.Where(entry => entry.ActionType == actionType).FirstOrDefault();
        }

        [D2OField("gender")]
        [Property("Gender")]
        public uint Gender
        {
            get;
            set;
        }

        private string m_lookAsString;
        private EntityLook m_entityLook;

        [D2OField("look")]
        [Property("Look")]
        private string LookAsString
        {
            get
            {
                if (Look == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(m_lookAsString))
                    m_lookAsString = Look.ConvertToString();

                return m_lookAsString;
            }
            set
            {
                m_lookAsString = value;

                if (value != null)
                    Look = m_lookAsString.ToEntityLook();
            }
        }

        public EntityLook Look
        {
            get { return m_entityLook; }
            set
            {
                m_entityLook = value;

                if (value != null)
                    m_lookAsString = value.ConvertToString();
            }
        }

        [Property]
        public short SpecialArtworkId
        {
            get;
            set;
        }
    }
}