

// Generated on 07/29/2013 23:08:17
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteShort(dungeonId);
            writer.WriteInt(buddyId);
            writer.WriteInt(timeLeft);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            buddyId = reader.ReadInt();
            if (buddyId < 0)
                throw new Exception("Forbidden value on buddyId = " + buddyId + ", it doesn't respect the following condition : buddyId < 0");
            timeLeft = reader.ReadInt();
            if (timeLeft < 0)
                throw new Exception("Forbidden value on timeLeft = " + timeLeft + ", it doesn't respect the following condition : timeLeft < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(int);
        }
        
    }
    
}