

// Generated on 12/29/2014 21:12:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatClientPrivateMessage : ChatAbstractClientMessage
    {
        public const uint Id = 851;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string receiver;
        
        public ChatClientPrivateMessage()
        {
        }
        
        public ChatClientPrivateMessage(string content, string receiver)
         : base(content)
        {
            this.receiver = receiver;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(receiver);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            receiver = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(receiver);
        }
        
    }
    
}