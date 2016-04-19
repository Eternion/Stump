

// Generated on 04/19/2016 10:17:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MoodSmileyRequestMessage : Message
    {
        public const uint Id = 6192;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short smileyId;
        
        public MoodSmileyRequestMessage()
        {
        }
        
        public MoodSmileyRequestMessage(short smileyId)
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