

// Generated on 12/20/2015 16:37:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportToBuddyCloseMessage : Message
    {
        public const uint Id = 6303;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public long buddyId;
        
        public TeleportToBuddyCloseMessage()
        {
        }
        
        public TeleportToBuddyCloseMessage(short dungeonId, long buddyId)
        {
            this.dungeonId = dungeonId;
            this.buddyId = buddyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(dungeonId);
            writer.WriteVarLong(buddyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            buddyId = reader.ReadVarLong();
            if (buddyId < 0 || buddyId > 9.007199254740992E15)
                throw new Exception("Forbidden value on buddyId = " + buddyId + ", it doesn't respect the following condition : buddyId < 0 || buddyId > 9.007199254740992E15");
        }
        
    }
    
}