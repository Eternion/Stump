

// Generated on 10/30/2016 16:20:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatSmileyRequestMessage : Message
    {
        public const uint Id = 800;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short smileyId;
        
        public ChatSmileyRequestMessage()
        {
        }
        
        public ChatSmileyRequestMessage(short smileyId)
        {
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            smileyId = reader.ReadVarShort();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
        
    }
    
}