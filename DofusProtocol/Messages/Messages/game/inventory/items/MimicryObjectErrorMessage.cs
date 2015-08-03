

// Generated on 08/04/2015 00:37:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MimicryObjectErrorMessage : SymbioticObjectErrorMessage
    {
        public const uint Id = 6461;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool preview;
        
        public MimicryObjectErrorMessage()
        {
        }
        
        public MimicryObjectErrorMessage(sbyte reason, sbyte errorCode, bool preview)
         : base(reason, errorCode)
        {
            this.preview = preview;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(preview);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            preview = reader.ReadBoolean();
        }
        
    }
    
}