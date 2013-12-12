

// Generated on 12/12/2013 16:56:55
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
            writer.WriteUShort((ushort)dispositions.Count());
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            dispositions = new Types.IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dispositions as Types.IdentifiedEntityDispositionInformations[])[i] = new Types.IdentifiedEntityDispositionInformations();
                 (dispositions as Types.IdentifiedEntityDispositionInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + dispositions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}