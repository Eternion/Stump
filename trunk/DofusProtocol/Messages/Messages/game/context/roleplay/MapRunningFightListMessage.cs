

// Generated on 08/11/2013 11:28:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapRunningFightListMessage : Message
    {
        public const uint Id = 5743;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.FightExternalInformations> fights;
        
        public MapRunningFightListMessage()
        {
        }
        
        public MapRunningFightListMessage(IEnumerable<Types.FightExternalInformations> fights)
        {
            this.fights = fights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)fights.Count());
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            fights = new Types.FightExternalInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fights as Types.FightExternalInformations[])[i] = new Types.FightExternalInformations();
                 (fights as Types.FightExternalInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + fights.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}