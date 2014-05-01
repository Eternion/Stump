

// Generated on 03/02/2014 20:42:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyJoinMessage : AbstractPartyMessage
    {
        public const uint Id = 5576;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte partyType;
        public int partyLeaderId;
        public sbyte maxParticipants;
        public IEnumerable<Types.PartyMemberInformations> members;
        public IEnumerable<Types.PartyGuestInformations> guests;
        public bool restricted;
        
        public PartyJoinMessage()
        {
        }
        
        public PartyJoinMessage(int partyId, sbyte partyType, int partyLeaderId, sbyte maxParticipants, IEnumerable<Types.PartyMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests, bool restricted)
         : base(partyId)
        {
            this.partyType = partyType;
            this.partyLeaderId = partyLeaderId;
            this.maxParticipants = maxParticipants;
            this.members = members;
            this.guests = guests;
            this.restricted = restricted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(partyType);
            writer.WriteInt(partyLeaderId);
            writer.WriteSByte(maxParticipants);
            var members_before = writer.Position;
            var members_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in members)
            {
                 writer.WriteShort(entry.TypeId);
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

            writer.WriteBoolean(restricted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyType = reader.ReadSByte();
            if (partyType < 0)
                throw new Exception("Forbidden value on partyType = " + partyType + ", it doesn't respect the following condition : partyType < 0");
            partyLeaderId = reader.ReadInt();
            if (partyLeaderId < 0)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0");
            maxParticipants = reader.ReadSByte();
            if (maxParticipants < 0)
                throw new Exception("Forbidden value on maxParticipants = " + maxParticipants + ", it doesn't respect the following condition : maxParticipants < 0");
            var limit = reader.ReadUShort();
            var members_ = new Types.PartyMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 members_[i] = Types.ProtocolTypeManager.GetInstance<Types.PartyMemberInformations>(reader.ReadShort());
                 members_[i].Deserialize(reader);
            }
            members = members_;
            limit = reader.ReadUShort();
            var guests_ = new Types.PartyGuestInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guests_[i] = new Types.PartyGuestInformations();
                 guests_[i].Deserialize(reader);
            }
            guests = guests_;
            restricted = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(int) + sizeof(sbyte) + sizeof(short) + members.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + guests.Sum(x => x.GetSerializationSize()) + sizeof(bool);
        }
        
    }
    
}