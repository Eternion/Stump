using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database
{
    public class NpcMessageConfiguration : EntityTypeConfiguration<NpcMessage>
    {
        public NpcMessageConfiguration()
        {
            ToTable("npcs_messages");
        }
    }

    [D2OClass("NpcMessage", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class NpcMessage : IAssignedByD2O
    {
        private IList<string> m_parameters;
        private string m_parametersAsString;

        public int Id
        {
            get;
            set;
        }

        public uint MessageId
        {
            get;
            set;
        }

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

        private List<Npcs.NpcReply> m_replies;
        public List<Npcs.NpcReply> Replies
        {
            get
            {
                return m_replies ?? ( m_replies = NpcManager.Instance.GetMessageReplies(Id) );
            }
        }


        public void AssignFields(object d2oObject)
        {
            var message = (DofusProtocol.D2oClasses.NpcMessage)d2oObject;
            Id = message.id;
            MessageId = message.messageId;
            ParametersAsString = message.messageParams;
        }
    }
}