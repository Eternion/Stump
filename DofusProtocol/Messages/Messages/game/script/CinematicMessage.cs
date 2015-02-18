

// Generated on 02/18/2015 10:46:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CinematicMessage : Message
    {
        public const uint Id = 6053;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cinematicId;
        
        public CinematicMessage()
        {
        }
        
        public CinematicMessage(short cinematicId)
        {
            this.cinematicId = cinematicId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(cinematicId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            cinematicId = reader.ReadVarShort();
            if (cinematicId < 0)
                throw new Exception("Forbidden value on cinematicId = " + cinematicId + ", it doesn't respect the following condition : cinematicId < 0");
        }
        
    }
    
}