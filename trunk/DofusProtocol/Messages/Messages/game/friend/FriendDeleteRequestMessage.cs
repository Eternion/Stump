

// Generated on 07/26/2013 22:51:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendDeleteRequestMessage : Message
    {
        public const uint Id = 5603;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public FriendDeleteRequestMessage()
        {
        }
        
        public FriendDeleteRequestMessage(string name)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + name.Length;
        }
        
    }
    
}