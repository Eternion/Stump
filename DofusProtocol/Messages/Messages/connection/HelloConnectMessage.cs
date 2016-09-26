

// Generated on 09/26/2016 01:49:50
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
            writer.WriteVarInt((int)key.Count());
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
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