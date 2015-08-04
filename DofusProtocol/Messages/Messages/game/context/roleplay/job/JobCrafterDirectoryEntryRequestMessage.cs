

// Generated on 08/04/2015 13:25:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryEntryRequestMessage : Message
    {
        public const uint Id = 6043;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        
        public JobCrafterDirectoryEntryRequestMessage()
        {
        }
        
        public JobCrafterDirectoryEntryRequestMessage(int playerId)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}