

// Generated on 03/05/2014 20:34:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EnabledChannelsMessage : Message
    {
        public const uint Id = 892;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<sbyte> channels;
        public IEnumerable<sbyte> disallowed;
        
        public EnabledChannelsMessage()
        {
        }
        
        public EnabledChannelsMessage(IEnumerable<sbyte> channels, IEnumerable<sbyte> disallowed)
        {
            this.channels = channels;
            this.disallowed = disallowed;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var channels_before = writer.Position;
            var channels_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in channels)
            {
                 writer.WriteSByte(entry);
                 channels_count++;
            }
            var channels_after = writer.Position;
            writer.Seek((int)channels_before);
            writer.WriteUShort((ushort)channels_count);
            writer.Seek((int)channels_after);

            var disallowed_before = writer.Position;
            var disallowed_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in disallowed)
            {
                 writer.WriteSByte(entry);
                 disallowed_count++;
            }
            var disallowed_after = writer.Position;
            writer.Seek((int)disallowed_before);
            writer.WriteUShort((ushort)disallowed_count);
            writer.Seek((int)disallowed_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var channels_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 channels_[i] = reader.ReadSByte();
            }
            channels = channels_;
            limit = reader.ReadUShort();
            var disallowed_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 disallowed_[i] = reader.ReadSByte();
            }
            disallowed = disallowed_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + channels.Sum(x => sizeof(sbyte)) + sizeof(short) + disallowed.Sum(x => sizeof(sbyte));
        }
        
    }
    
}