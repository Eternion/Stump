

// Generated on 02/18/2015 10:46:29
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
            var fights_before = writer.Position;
            var fights_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
                 fights_count++;
            }
            var fights_after = writer.Position;
            writer.Seek((int)fights_before);
            writer.WriteUShort((ushort)fights_count);
            writer.Seek((int)fights_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var fights_ = new Types.PrismFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 fights_[i] = new Types.PrismFightersInformation();
                 fights_[i].Deserialize(reader);
            }
            fights = fights_;
        }
        
    }
    
}