

// Generated on 02/18/2015 10:46:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportToBuddyOfferMessage : Message
    {
        public const uint Id = 6287;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public int buddyId;
        public int timeLeft;
        
        public TeleportToBuddyOfferMessage()
        {
        }
        
        public TeleportToBuddyOfferMessage(short dungeonId, int buddyId, int timeLeft)
        {
            this.dungeonId = dungeonId;
            this.buddyId = buddyId;
            this.timeLeft = timeLeft;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(dungeonId);
            writer.WriteVarInt(buddyId);
            writer.WriteVarInt(timeLeft);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            buddyId = reader.ReadVarInt();
            if (buddyId < 0)
                throw new Exception("Forbidden value on buddyId = " + buddyId + ", it doesn't respect the following condition : buddyId < 0");
            timeLeft = reader.ReadVarInt();
            if (timeLeft < 0)
                throw new Exception("Forbidden value on timeLeft = " + timeLeft + ", it doesn't respect the following condition : timeLeft < 0");
        }
        
    }
    
}