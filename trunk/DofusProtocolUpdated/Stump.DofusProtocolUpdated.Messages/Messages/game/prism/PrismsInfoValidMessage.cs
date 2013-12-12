

// Generated on 12/12/2013 16:57:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismsInfoValidMessage : Message
    {
        public const uint Id = 6451;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.PrismFightersInformation> fights;
        
        public PrismsInfoValidMessage()
        {
        }
        
        public PrismsInfoValidMessage(IEnumerable<Types.PrismFightersInformation> fights)
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
            fights = new Types.PrismFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fights as Types.PrismFightersInformation[])[i] = new Types.PrismFightersInformation();
                 (fights as Types.PrismFightersInformation[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + fights.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}