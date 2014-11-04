

// Generated on 10/28/2014 16:36:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HelloConnectMessage : Message
    {
        public const uint Id = 3;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string salt;
        public IEnumerable<sbyte> key;
        
        public HelloConnectMessage()
        {
        }
        
        public HelloConnectMessage(string salt, IEnumerable<sbyte> key)
        {
            this.salt = salt;
            this.key = key;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(salt);
            var key_before = writer.Position;
            var key_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
                 key_count++;
            }
            var key_after = writer.Position;
            writer.Seek((int)key_before);
            writer.WriteUShort((ushort)key_count);
            writer.Seek((int)key_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            salt = reader.ReadUTF();
            var limit = reader.ReadUShort();
            var key_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 key_[i] = reader.ReadSByte();
            }
            key = key_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(salt) + sizeof(short) + key.Sum(x => sizeof(sbyte));
        }
        
    }
    
}