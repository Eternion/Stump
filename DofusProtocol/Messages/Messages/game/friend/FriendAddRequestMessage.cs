

// Generated on 04/24/2015 03:38:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendAddRequestMessage : Message
    {
        public const uint Id = 4004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public FriendAddRequestMessage()
        {
        }
        
        public FriendAddRequestMessage(string name)
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
        
    }
    
}