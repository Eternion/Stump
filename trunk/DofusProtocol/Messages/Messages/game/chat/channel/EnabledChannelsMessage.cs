

// Generated on 08/11/2013 11:28:19
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
            writer.WriteUShort((ushort)channels.Count());
            foreach (var entry in channels)
            {
                 writer.WriteSByte(entry);
            }
            writer.WriteUShort((ushort)disallowed.Count());
            foreach (var entry in disallowed)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            channels = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (channels as sbyte[])[i] = reader.ReadSByte();
            }
            limit = reader.ReadUShort();
            disallowed = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (disallowed as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + channels.Sum(x => sizeof(sbyte)) + sizeof(short) + disallowed.Sum(x => sizeof(sbyte));
        }
        
    }
    
}