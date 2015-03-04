

// Generated on 02/19/2015 12:09:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationDetailsMessage : AbstractPartyMessage
    {
        public const uint Id = 6263;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte partyType;
        public string partyName;
        public int fromId;
        public string fromName;
        public int leaderId;
        public IEnumerable<Types.PartyInvitationMemberInformations> members;
        public IEnumerable<Types.PartyGuestInformations> guests;
        
        public PartyInvitationDetailsMessage()
        {
        }
        
        public PartyInvitationDetailsMessage(int partyId, sbyte partyType, string partyName, int fromId, string fromName, int leaderId, IEnumerable<Types.PartyInvitationMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyName = partyName;
            this.fromId = fromId;
            this.fromName = fromName;
            this.leaderId = leaderId;
            this.members = members;
            this.guests = guests;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteUTF(partyName);
            writer.WriteVarInt(fromId);
            writer.WriteUTF(fromName);
            writer.WriteVarInt(leaderId);
            var members_before = writer.Position;
            var members_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
                 members_count++;
            }
            var members_after = writer.Position;
            writer.Seek((int)members_before);
            writer.WriteUShort((ushort)members_count);
            writer.Seek((int)members_after);

            var guests_before = writer.Position;
            var guests_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in guests)
            {
                 entry.Serialize(writer);
                 guests_count++;
            }
            var guests_after = writer.Position;
            writer.Seek((int)guests_before);
            writer.WriteUShort((ushort)guests_count);
            writer.Seek((int)guests_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyName = reader.ReadUTF();
            fromId = reader.ReadVarInt();
            if (fromId < 0)
                throw new Exception("Forbidden value on fromId = " + fromId + ", it doesn't respect the following condition : fromId < 0");
            fromName = reader.ReadUTF();
            leaderId = reader.ReadVarInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            var limit = reader.ReadVarInt();
            var members_ = new Types.PartyInvitationMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 members_[i] = new Types.PartyInvitationMemberInformations();
                 members_[i].Deserialize(reader);
            }
            members = members_;
            limit = reader.ReadVarInt();
            var guests_ = new Types.PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guests_[i] = new Types.PartyGuestInformations();
                 guests_[i].Deserialize(reader);
            }
            guests = guests_;
        }
        
    }
    
}