

// Generated on 11/16/2015 14:26:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MoodSmileyResultMessage : Message
    {
        public const uint Id = 6196;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte resultCode;
        public short smileyId;
        
        public MoodSmileyResultMessage()
        {
        }
        
        public MoodSmileyResultMessage(sbyte resultCode, short smileyId)
        {
            this.resultCode = resultCode;
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(resultCode);
            writer.WriteVarShort(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            resultCode = reader.ReadSByte();
            if (resultCode < 0)
                throw new Exception("Forbidden value on resultCode = " + resultCode + ", it doesn't respect the following condition : resultCode < 0");
            smileyId = reader.ReadVarShort();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
        
    }
    
}