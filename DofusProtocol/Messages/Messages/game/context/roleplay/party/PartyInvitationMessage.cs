

// Generated on 09/26/2016 01:50:04
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
        public long fromId;
        public string fromName;
        public long toId;
        
        public PartyInvitationMessage()
        {
        }
        
        public PartyInvitationMessage(int partyId, sbyte partyType, string partyName, sbyte maxParticipants, long fromId, string fromName, long toId)
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
            writer.WriteVarLong(fromId);
            writer.WriteUTF(fromName);
            writer.WriteVarLong(toId);
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
            fromId = reader.ReadVarLong();
            if (fromId < 0 || fromId > 9007199254740990)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0 || fromId > 9007199254740990");
            fromName = reader.ReadUTF();
            toId = reader.ReadVarLong();
            if (toId < 0 || toId > 9007199254740990)
                throw new Exception("Forbidden value on toId = " + toId + ", it doesn't respect the following condition : toId < 0 || toId > 9007199254740990");
        }
        
    }
    
}