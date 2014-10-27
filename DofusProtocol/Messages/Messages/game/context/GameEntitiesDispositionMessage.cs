

// Generated on 10/27/2014 19:57:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameEntitiesDispositionMessage : Message
    {
        public const uint Id = 5696;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions;
        
        public GameEntitiesDispositionMessage()
        {
        }
        
        public GameEntitiesDispositionMessage(IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions)
        {
            this.dispositions = dispositions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var dispositions_before = writer.Position;
            var dispositions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
                 dispositions_count++;
            }
            var dispositions_after = writer.Position;
            writer.Seek((int)dispositions_before);
            writer.WriteUShort((ushort)dispositions_count);
            writer.Seek((int)dispositions_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var dispositions_ = new Types.IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
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