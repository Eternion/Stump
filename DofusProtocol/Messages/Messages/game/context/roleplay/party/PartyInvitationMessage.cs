

// Generated on 09/01/2014 15:52:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationMessage : AbstractPartyMessage
    {
        public const uint Id = 5586;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte partyType;
        public string partyName;
        public sbyte maxParticipants;
        public int fromId;
        public string fromName;
        public int toId;
        
        public PartyInvitationMessage()
        {
        }
        
        public PartyInvitationMessage(int partyId, sbyte partyType, string partyName, sbyte maxParticipants, int fromId, string fromName, int toId)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyName = partyName;
            this.maxParticipants = maxParticipants;
            this.fromId = fromId;
            this.fromName = fromName;
            this.toId = toId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteUTF(partyName);
            writer.WriteSByte(maxParticipants);
            writer.WriteInt(fromId);
            writer.WriteUTF(fromName);
            writer.WriteInt(toId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyName = reader.ReadUTF();
            maxParticipants = reader.ReadSByte();
            if (maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            fromId = reader.ReadInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadUTF();
            toId = reader.ReadInt();
            if (toId < 0)
                throw new Exception("Forbidden value on toId = " + toId + ", it doesn't respect the following condition : toId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(short) + Encoding.UTF8.GetByteCount(partyName) + sizeof(sbyte) + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(fromName) + sizeof(int);
        }
        
    }
    
}