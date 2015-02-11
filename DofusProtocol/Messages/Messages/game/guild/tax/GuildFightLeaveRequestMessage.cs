

// Generated on 02/11/2015 10:20:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightLeaveRequestMessage : Message
    {
        public const uint Id = 5715;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int taxCollectorId;
        public int characterId;
        
        public GuildFightLeaveRequestMessage()
        {
        }
        
        public GuildFightLeaveRequestMessage(int taxCollectorId, int characterId)
        {
            this.taxCollectorId = taxCollectorId;
            this.characterId = characterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(taxCollectorId);
            writer.WriteVarInt(characterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            taxCollectorId = reader.ReadInt();
            if (taxCollectorId < 0)
                throw new Exception("Forbidden value on taxCollectorId = " + taxCollectorId + ", it doesn't respect the following condition : taxCollectorId < 0");
            characterId = reader.ReadVarInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
    }
    
}