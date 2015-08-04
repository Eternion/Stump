

// Generated on 08/04/2015 13:25:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NpcDialogReplyMessage : Message
    {
        public const uint Id = 5616;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short replyId;
        
        public NpcDialogReplyMessage()
        {
        }
        
        public NpcDialogReplyMessage(short replyId)
        {
            this.replyId = replyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(replyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            replyId = reader.ReadVarShort();
            if (replyId < 0)
                throw new Exception("Forbidden value on replyId = " + replyId + ", it doesn't respect the following condition : replyId < 0");
        }
        
    }
    
}