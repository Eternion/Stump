

// Generated on 10/27/2014 19:57:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var friendsList_before = writer.Position;
            var friendsList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in friendsList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 friendsList_count++;
            }
            var friendsList_after = writer.Position;
            writer.Seek((int)friendsList_before);
            writer.WriteUShort((ushort)friendsList_count);
            writer.Seek((int)friendsList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var friendsList_ = new Types.FriendInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 friendsList_[i] = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
                 friendsList_[i].Deserialize(reader);
            }
            friendsList = friendsList_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + friendsList.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}