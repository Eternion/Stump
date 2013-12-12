

// Generated on 12/12/2013 16:57:10
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
            writer.WriteUShort((ushort)members.Count());
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            members = new Types.GuildMember[limit];
            for (int i = 0; i < limit; i++)
            {
                 (members as Types.GuildMember[])[i] = new Types.GuildMember();
                 (members as Types.GuildMember[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + members.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}