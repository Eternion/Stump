

// Generated on 09/26/2016 01:50:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendJoinRequestMessage : Message
    {
        public const uint Id = 5605;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public FriendJoinRequestMessage()
        {
        }
        
        public FriendJoinRequestMessage(string name)
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