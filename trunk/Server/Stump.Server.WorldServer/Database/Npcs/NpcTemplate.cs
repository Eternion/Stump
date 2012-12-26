using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Npc = Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs.Npc;

namespace Stump.Server.WorldServer.Database
{
    public class NpcTemplateConfiguration : EntityTypeConfiguration<NpcTemplate>
    {
        public NpcTemplateConfiguration()
        {
            ToTable("npcs_templates");
        }
    }

    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    public class NpcTemplate : IAssignedByD2O, ISaveIntercepter
    {
        public delegate void NpcSpawnedEventHandler(NpcTemplate template, Npc npc);
        public event NpcSpawnedEventHandler NpcSpawned;

        public void OnNpcSpawned(Npc npc)
        {
            NpcSpawnedEventHandler handler = NpcSpawned;
            if (handler != null) handler(this, npc);
        }

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

        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        private byte[] m_dialogMessagesIdBin;

        public byte[] DialogMessagesIdBin
        {
            get { return m_dialogMessagesIdBin; }
            set
            {
                m_dialogMessagesIdBin = value;
                DialogMessagesId = m_dialogMessagesIdBin.ToObject<List<List<int>>>();
            }
        }

        public List<List<int>> DialogMessagesId
        {
            get;
            set;
        }

        private byte[] m_dialogRepliesIdBin;

        public byte[] DialogRepliesIdBin
        {
            get { return m_dialogRepliesIdBin; }
            set
            {
                m_dialogRepliesIdBin = value;
                DialogRepliesId = m_dialogRepliesIdBin.ToObject<List<List<int>>>();
            }
        }

        public List<List<int>> DialogRepliesId
        {
            get;
            set;
        }

        private byte[] m_actionsIdsBin;

        public byte[] ActionsIdsBin
        {
            get { return m_actionsIdsBin; }
            set
            {
                m_actionsIdsBin = value;
                ActionsIds = m_actionsIdsBin.ToObject<List<uint>>();
            }
        }

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

        public uint Gender
        {
            get;
            set;
        }

        private string m_lookAsString;
        private EntityLook m_entityLook;

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

        public short SpecialArtworkId
        {
            get;
            set;
        }

        public int TokenShop
        {
            get;
            set;
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_dialogMessagesIdBin = DialogMessagesId.ToBinary();
            m_dialogRepliesIdBin = DialogRepliesId.ToBinary();
            m_actionsIdsBin = ActionsIds.ToBinary();
        }

        public void AssignFields(object d2oObject)
        {
            var npc = (DofusProtocol.D2oClasses.Npc)d2oObject;
            Id = npc.id;
            NameId = npc.nameId;
            DialogMessagesId = npc.dialogMessages;
            DialogRepliesId = npc.dialogReplies;
            ActionsIds = npc.actions;
            Gender = npc.gender;
            LookAsString = npc.look;
            TokenShop = npc.tokenShop;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}