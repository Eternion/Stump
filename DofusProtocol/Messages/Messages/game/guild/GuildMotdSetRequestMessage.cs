

// Generated on 10/30/2016 16:20:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMotdSetRequestMessage : SocialNoticeSetRequestMessage
    {
        public const uint Id = 6588;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string content;
        
        public GuildMotdSetRequestMessage()
        {
        }
        
        public GuildMotdSetRequestMessage(string content)
        {
            this.content = content;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(content);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            content = reader.ReadUTF();
        }
        
    }
    
}