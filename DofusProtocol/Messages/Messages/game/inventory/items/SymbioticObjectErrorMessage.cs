

// Generated on 10/30/2016 16:20:47
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SymbioticObjectErrorMessage : ObjectErrorMessage
    {
        public const uint Id = 6526;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte errorCode;
        
        public SymbioticObjectErrorMessage()
        {
        }
        
        public SymbioticObjectErrorMessage(sbyte reason, sbyte errorCode)
         : base(reason)
        {
            this.errorCode = errorCode;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(errorCode);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            errorCode = reader.ReadSByte();
        }
        
    }
    
}