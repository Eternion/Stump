

// Generated on 02/19/2015 12:09:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectUseOnCellMessage : ObjectUseMessage
    {
        public const uint Id = 3013;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cells;
        
        public ObjectUseOnCellMessage()
        {
        }
        
        public ObjectUseOnCellMessage(int objectUID, short cells)
         : base(objectUID)
        {
            this.cells = cells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(cells);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cells = reader.ReadVarShort();
            if (cells < 0 || cells > 559)
                throw new Exception("Forbidden value on cells = " + cells + ", it doesn't respect the following condition : cells < 0 || cells > 559");
        }
        
    }
    
}