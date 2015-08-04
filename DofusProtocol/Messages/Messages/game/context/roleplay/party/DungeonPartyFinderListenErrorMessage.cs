

// Generated on 08/04/2015 13:25:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderListenErrorMessage : Message
    {
        public const uint Id = 6248;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        
        public DungeonPartyFinderListenErrorMessage()
        {
        }
        
        public DungeonPartyFinderListenErrorMessage(short dungeonId)
        {
            this.dungeonId = dungeonId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(dungeonId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
        }
        
    }
    
}