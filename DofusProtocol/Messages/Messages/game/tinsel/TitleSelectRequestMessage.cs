

// Generated on 04/24/2015 03:38:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TitleSelectRequestMessage : Message
    {
        public const uint Id = 6365;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short titleId;
        
        public TitleSelectRequestMessage()
        {
        }
        
        public TitleSelectRequestMessage(short titleId)
        {
            this.titleId = titleId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(titleId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            titleId = reader.ReadVarShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
        }
        
    }
    
}