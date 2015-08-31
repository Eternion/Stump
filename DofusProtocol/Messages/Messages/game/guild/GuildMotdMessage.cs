

// Generated on 08/04/2015 13:25:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMotdMessage : Message
    {
        public const uint Id = 6590;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string content;
        public int timestamp;
        
        public GuildMotdMessage()
        {
        }
        
        public GuildMotdMessage(string content, int timestamp)
        {
            this.content = content;
            this.timestamp = timestamp;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(content);
            writer.WriteInt(timestamp);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            content = reader.ReadUTF();
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
        }
        
    }
    
}