

// Generated on 02/19/2015 12:09:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationDungeonDetailsMessage : PartyInvitationDetailsMessage
    {
        public const uint Id = 6262;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public IEnumerable<bool> playersDungeonReady;
        
        public PartyInvitationDungeonDetailsMessage()
        {
        }
        
        public PartyInvitationDungeonDetailsMessage(int partyId, sbyte partyType, string partyName, int fromId, string fromName, int leaderId, IEnumerable<Types.PartyInvitationMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests, short dungeonId, IEnumerable<bool> playersDungeonReady)
         : base(partyId, partyType, partyName, fromId, fromName, leaderId, members, guests)
        {
            this.dungeonId = dungeonId;
            this.playersDungeonReady = playersDungeonReady;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(dungeonId);
            var playersDungeonReady_before = writer.Position;
            var playersDungeonReady_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in playersDungeonReady)
            {
                 writer.WriteBoolean(entry);
                 playersDungeonReady_count++;
            }
            var playersDungeonReady_after = writer.Position;
            writer.Seek((int)playersDungeonReady_before);
            writer.WriteUShort((ushort)playersDungeonReady_count);
            writer.Seek((int)playersDungeonReady_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadVarInt();
            var playersDungeonReady_ = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 playersDungeonReady_[i] = reader.ReadBoolean();
            }
            playersDungeonReady = playersDungeonReady_;
        }
        
    }
    
}