

// Generated on 10/30/2016 16:20:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatAdminServerMessage : ChatServerMessage
    {
        public const uint Id = 6135;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ChatAdminServerMessage()
        {
        }
        
        public ChatAdminServerMessage(sbyte channel, string content, int timestamp, string fingerprint, double senderId, string senderName, int senderAccountId)
         : base(channel, content, timestamp, fingerprint, senderId, senderName, senderAccountId)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}