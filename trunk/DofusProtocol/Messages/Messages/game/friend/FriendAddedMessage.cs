

// Generated on 03/02/2014 20:42:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendAddedMessage : Message
    {
        public const uint Id = 5599;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FriendInformations friendAdded;
        
        public FriendAddedMessage()
        {
        }
        
        public FriendAddedMessage(Types.FriendInformations friendAdded)
        {
            this.friendAdded = friendAdded;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(friendAdded.TypeId);
            friendAdded.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            friendAdded = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
            friendAdded.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + friendAdded.GetSerializationSize();
        }
        
    }
    
}