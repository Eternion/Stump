

// Generated on 11/16/2015 14:25:58
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
        
        public IEnumerable<sbyte> features;
        
        public ServerOptionalFeaturesMessage()
        {
        }
        
        public ServerOptionalFeaturesMessage(IEnumerable<sbyte> features)
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
                 writer.WriteSByte(entry);
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
            var features_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 features_[i] = reader.ReadSByte();
            }
            features = features_;
        }
        
    }
    
}