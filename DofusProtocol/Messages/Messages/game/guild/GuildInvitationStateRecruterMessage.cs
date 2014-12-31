

// Generated on 12/29/2014 21:13:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInvitationStateRecruterMessage : Message
    {
        public const uint Id = 5563;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string recrutedName;
        public sbyte invitationState;
        
        public GuildInvitationStateRecruterMessage()
        {
        }
        
        public GuildInvitationStateRecruterMessage(string recrutedName, sbyte invitationState)
        {
            this.recrutedName = recrutedName;
            this.invitationState = invitationState;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(recrutedName);
            writer.WriteSByte(invitationState);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            recrutedName = reader.ReadUTF();
            invitationState = reader.ReadSByte();
            if (invitationState < 0)
                throw new Exception("Forbidden value on invitationState = " + invitationState + ", it doesn't respect the following condition : invitationState < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(recrutedName) + sizeof(sbyte);
        }
        
    }
    
}