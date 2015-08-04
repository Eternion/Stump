

// Generated on 08/04/2015 13:25:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TitleSelectedMessage : Message
    {
        public const uint Id = 6366;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short titleId;
        
        public TitleSelectedMessage()
        {
        }
        
        public TitleSelectedMessage(short titleId)
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