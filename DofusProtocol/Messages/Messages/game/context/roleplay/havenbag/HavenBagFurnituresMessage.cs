

// Generated on 12/20/2015 16:36:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HavenBagFurnituresMessage : Message
    {
        public const uint Id = 6634;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.HavenBagFurnitureInformation> furnituresInfos;
        
        public HavenBagFurnituresMessage()
        {
        }
        
        public HavenBagFurnituresMessage(IEnumerable<Types.HavenBagFurnitureInformation> furnituresInfos)
        {
            this.furnituresInfos = furnituresInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var furnituresInfos_before = writer.Position;
            var furnituresInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in furnituresInfos)
            {
                 entry.Serialize(writer);
                 furnituresInfos_count++;
            }
            var furnituresInfos_after = writer.Position;
            writer.Seek((int)furnituresInfos_before);
            writer.WriteUShort((ushort)furnituresInfos_count);
            writer.Seek((int)furnituresInfos_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var furnituresInfos_ = new Types.HavenBagFurnitureInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 furnituresInfos_[i] = new Types.HavenBagFurnitureInformation();
                 furnituresInfos_[i].Deserialize(reader);
            }
            furnituresInfos = furnituresInfos_;
        }
        
    }
    
}