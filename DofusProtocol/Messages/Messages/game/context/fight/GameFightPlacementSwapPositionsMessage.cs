

// Generated on 12/29/2014 21:12:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightPlacementSwapPositionsMessage : Message
    {
        public const uint Id = 6544;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions;
        
        public GameFightPlacementSwapPositionsMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsMessage(IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions)
        {
            this.dispositions = dispositions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var dispositions_ = new Types.IdentifiedEntityDispositionInformations[2];
            for (int i = 0; i < 2; i++)
            {
                 dispositions_[i] = new Types.IdentifiedEntityDispositionInformations();
                 dispositions_[i].Deserialize(reader);
            }
            dispositions = dispositions_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + dispositions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}