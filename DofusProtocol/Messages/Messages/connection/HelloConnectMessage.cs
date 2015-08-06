

// Generated on 08/04/2015 13:24:43
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
            writer.WriteVarInt(key.Count());
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
                 key_count++;
            }

        }
        
        public override void Deserialize(IDataReader reader)
        {
            salt = reader.ReadUTF();
            var limit = reader.ReadVarInt();
            var key_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 key_[i] = reader.ReadSByte();
            }
            key = key_;
        }
        
    }
    
}