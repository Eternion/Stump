

// Generated on 10/26/2014 23:29:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportBuddiesRequestedMessage : Message
    {
        public const uint Id = 6302;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public int inviterId;
        public IEnumerable<int> invalidBuddiesIds;
        
        public TeleportBuddiesRequestedMessage()
        {
        }
        
        public TeleportBuddiesRequestedMessage(short dungeonId, int inviterId, IEnumerable<int> invalidBuddiesIds)
        {
            this.dungeonId = dungeonId;
            this.inviterId = inviterId;
            this.invalidBuddiesIds = invalidBuddiesIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(dungeonId);
            writer.WriteInt(inviterId);
            var invalidBuddiesIds_before = writer.Position;
            var invalidBuddiesIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in invalidBuddiesIds)
            {
                 writer.WriteInt(entry);
                 invalidBuddiesIds_count++;
            }
            var invalidBuddiesIds_after = writer.Position;
            writer.Seek((int)invalidBuddiesIds_before);
            writer.WriteUShort((ushort)invalidBuddiesIds_count);
            writer.Seek((int)invalidBuddiesIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            inviterId = reader.ReadInt();
            if (inviterId < 0)
                throw new Exception("Forbidden value on inviterId = " + inviterId + ", it doesn't respect the following condition : inviterId < 0");
            var limit = reader.ReadUShort();
            var invalidBuddiesIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 invalidBuddiesIds_[i] = reader.ReadInt();
            }
            invalidBuddiesIds = invalidBuddiesIds_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(short) + invalidBuddiesIds.Sum(x => sizeof(int));
        }
        
    }
    
}