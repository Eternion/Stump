

// Generated on 03/06/2014 18:50:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeTypesExchangerDescriptionForUserMessage : Message
    {
        public const uint Id = 5765;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> typeDescription;
        
        public ExchangeTypesExchangerDescriptionForUserMessage()
        {
        }
        
        public ExchangeTypesExchangerDescriptionForUserMessage(IEnumerable<int> typeDescription)
        {
            this.typeDescription = typeDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var typeDescription_before = writer.Position;
            var typeDescription_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in typeDescription)
            {
                 writer.WriteInt(entry);
                 typeDescription_count++;
            }
            var typeDescription_after = writer.Position;
            writer.Seek((int)typeDescription_before);
            writer.WriteUShort((ushort)typeDescription_count);
            writer.Seek((int)typeDescription_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var typeDescription_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 typeDescription_[i] = reader.ReadInt();
            }
            typeDescription = typeDescription_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + typeDescription.Sum(x => sizeof(int));
        }
        
    }
    
}