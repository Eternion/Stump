

// Generated on 09/01/2015 10:48:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectGroundRemovedMessage : Message
    {
        public const uint Id = 3014;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cell;
        
        public ObjectGroundRemovedMessage()
        {
        }
        
        public ObjectGroundRemovedMessage(short cell)
        {
            this.cell = cell;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(cell);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            cell = reader.ReadVarShort();
            if (cell < 0 || cell > 559)
                throw new Exception("Forbidden value on cell = " + cell + ", it doesn't respect the following condition : cell < 0 || cell > 559");
        }
        
    }
    
}