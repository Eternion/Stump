

// Generated on 08/04/2015 00:37:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInformationsMembersMessage : Message
    {
        public const uint Id = 5558;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GuildMember> members;
        
        public GuildInformationsMembersMessage()
        {
        }
        
        public GuildInformationsMembersMessage(IEnumerable<Types.GuildMember> members)
        {
            this.members = members;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var members_before = writer.Position;
            var members_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
                 members_count++;
            }
            var members_after = writer.Position;
            writer.Seek((int)members_before);
            writer.WriteUShort((ushort)members_count);
            writer.Seek((int)members_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var members_ = new Types.GuildMember[limit];
            for (int i = 0; i < limit; i++)
            {
                 members_[i] = new Types.GuildMember();
                 members_[i].Deserialize(reader);
            }
            members = members_;
        }
        
    }
    
}