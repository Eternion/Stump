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
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Npc = Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs.Npc;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs")]
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcTemplate : WorldBaseRecord<NpcTemplate>
    {
        public delegate void NpcSpawnedEventHandler(NpcTemplate template, Npc npc);
        public event NpcSpawnedEventHandler NpcSpawned;

        public void OnNpcSpawned(Npc npc)
        {
            NpcSpawnedEventHandler handler = NpcSpawned;
            if (handler != null) handler(this, npc);
        }

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
        [Property("DialogMessagesId", ColumnType = "Serializable")]
        public List<List<int>> DialogMessagesId
        {
            get;
            set;
        }

        [D2OField("dialogReplies")]
        [Property("DialogRepliesId", ColumnType = "Serializable")]
        public List<List<int>> DialogRepliesId
        {
            get;
            set;
        }

        [D2OField("actions")]
        [Property("ActionsId", ColumnType = "Serializable")]
        public List<uint> ActionsIds
        {
            get;
            set;
        }

        private List<NpcAction> m_actions;
        public List<NpcAction> Actions
        {
            get
            {
                return m_actions ?? ( m_actions = NpcManager.Instance.GetNpcActions(Id) );
            }
        }

        public NpcAction[] GetNpcActions(NpcActionTypeEnum actionType)
        {
            return Actions.Where(entry => entry.ActionType == actionType).ToArray();
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
                if (m_entityLook == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(m_lookAsString))
                    m_lookAsString = Look.ConvertToString();

                return m_lookAsString;
            }
            set
            {
                m_lookAsString = value;

                if (value != null)
                    m_entityLook = m_lookAsString.ToEntityLook();
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

        [Property]
        [D2OField("tokenShop")]
        public bool TokenShop
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}