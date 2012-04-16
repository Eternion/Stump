using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_messages")]
    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class NpcMessage : WorldBaseRecord<NpcMessage>
    {
        private IList<string> m_parameters;
        private string m_parametersAsString;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("messageId")]
        [Property("MessageId")]
        public uint MessageId
        {
            get;
            set;
        }

        [D2OField("messageParams")]
        [Property("MessageParams")]
        internal string ParametersAsString
        {
            get { return m_parametersAsString; }
            set
            {
                m_parametersAsString = value;

                if (!string.IsNullOrEmpty(m_parametersAsString))
                    m_parameters = value.Split('|');
                else
                    m_parameters = new List<string>();
            }
        }

        public IList<string> Parameters
        {
            get { return m_parameters; }
            set
            {
                m_parameters = value;
                ParametersAsString = string.Join("|", value);
            }
        }

        private List<NpcReply> m_replies;
        public List<NpcReply> Replies
        {
            get
            {
                return m_replies ?? ( m_replies = NpcManager.Instance.GetMessageReplies(Id) );
            }
        }
    }
}