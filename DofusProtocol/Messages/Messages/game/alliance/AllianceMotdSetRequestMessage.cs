

// Generated on 10/30/2016 16:20:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceMotdSetRequestMessage : SocialNoticeSetRequestMessage
    {
        public const uint Id = 6687;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string content;
        
        public AllianceMotdSetRequestMessage()
        {
        }
        
        public AllianceMotdSetRequestMessage(string content)
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