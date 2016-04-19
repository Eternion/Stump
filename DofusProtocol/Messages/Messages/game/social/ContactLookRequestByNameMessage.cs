

// Generated on 04/19/2016 10:17:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ContactLookRequestByNameMessage : ContactLookRequestMessage
    {
        public const uint Id = 5933;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string playerName;
        
        public ContactLookRequestByNameMessage()
        {
        }
        
        public ContactLookRequestByNameMessage(byte requestId, sbyte contactType, string playerName)
         : base(requestId, contactType)
        {
            this.playerName = playerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(playerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerName = reader.ReadUTF();
        }
        
    }
    
}