

// Generated on 07/26/2013 22:51:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendsListMessage : Message
    {
        public const uint Id = 4002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.FriendInformations> friendsList;
        
        public FriendsListMessage()
        {
        }
        
        public FriendsListMessage(IEnumerable<Types.FriendInformations> friendsList)
        {
            this.friendsList = friendsList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)friendsList.Count());
            foreach (var entry in friendsList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            friendsList = new Types.FriendInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (friendsList as Types.FriendInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
                 (friendsList as Types.FriendInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + friendsList.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}