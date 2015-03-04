

// Generated on 02/19/2015 12:09:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendDeleteResultMessage : Message
    {
        public const uint Id = 5601;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool success;
        public string name;
        
        public FriendDeleteResultMessage()
        {
        }
        
        public FriendDeleteResultMessage(bool success, string name)
        {
            this.success = success;
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(success);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            success = reader.ReadBoolean();
            name = reader.ReadUTF();
        }
        
    }
    
}