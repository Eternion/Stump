

// Generated on 10/27/2014 19:57:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMemberLeavingMessage : Message
    {
        public const uint Id = 5923;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool kicked;
        public int memberId;
        
        public GuildMemberLeavingMessage()
        {
        }
        
        public GuildMemberLeavingMessage(bool kicked, int memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteInt(memberId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kicked = reader.ReadBoolean();
            memberId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(int);
        }
        
    }
    
}