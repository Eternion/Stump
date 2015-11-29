

// Generated on 11/16/2015 14:26:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportToBuddyAnswerMessage : Message
    {
        public const uint Id = 6293;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public int buddyId;
        public bool accept;
        
        public TeleportToBuddyAnswerMessage()
        {
        }
        
        public TeleportToBuddyAnswerMessage(short dungeonId, int buddyId, bool accept)
        {
            this.dungeonId = dungeonId;
            this.buddyId = buddyId;
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(dungeonId);
            writer.WriteVarInt(buddyId);
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            buddyId = reader.ReadVarInt();
            if (buddyId < 0)
                throw new Exception("Forbidden value on buddyId = " + buddyId + ", it doesn't respect the following condition : buddyId < 0");
            accept = reader.ReadBoolean();
        }
        
    }
    
}