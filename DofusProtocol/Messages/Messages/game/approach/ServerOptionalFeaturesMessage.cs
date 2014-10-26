

// Generated on 10/26/2014 23:29:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerOptionalFeaturesMessage : Message
    {
        public const uint Id = 6305;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> features;
        
        public ServerOptionalFeaturesMessage()
        {
        }
        
        public ServerOptionalFeaturesMessage(IEnumerable<short> features)
        {
            this.features = features;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var features_before = writer.Position;
            var features_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in features)
            {
                 writer.WriteShort(entry);
                 features_count++;
            }
            var features_after = writer.Position;
            writer.Seek((int)features_before);
            writer.WriteUShort((ushort)features_count);
            writer.Seek((int)features_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var features_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 features_[i] = reader.ReadShort();
            }
            features = features_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + features.Sum(x => sizeof(short));
        }
        
    }
    
}