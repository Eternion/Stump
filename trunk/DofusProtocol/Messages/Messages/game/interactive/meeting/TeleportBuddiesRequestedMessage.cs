
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)invalidBuddiesIds.Count());
            foreach (var entry in invalidBuddiesIds)
            {
                 writer.WriteInt(entry);
            }
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
            invalidBuddiesIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (invalidBuddiesIds as int[])[i] = reader.ReadInt();
            }
        }
        
    }
    
}