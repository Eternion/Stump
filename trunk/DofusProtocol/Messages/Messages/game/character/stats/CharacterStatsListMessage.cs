

// Generated on 03/02/2014 20:42:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterStatsListMessage : Message
    {
        public const uint Id = 500;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterCharacteristicsInformations stats;
        
        public CharacterStatsListMessage()
        {
        }
        
        public CharacterStatsListMessage(Types.CharacterCharacteristicsInformations stats)
        {
            this.stats = stats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            stats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            stats = new Types.CharacterCharacteristicsInformations();
            stats.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return stats.GetSerializationSize();
        }
        
    }
    
}