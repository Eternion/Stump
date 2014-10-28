

// Generated on 10/28/2014 16:37:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class WrapperObjectAssociatedMessage : SymbioticObjectAssociatedMessage
    {
        public const uint Id = 6523;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public WrapperObjectAssociatedMessage()
        {
        }
        
        public WrapperObjectAssociatedMessage(int hostUID)
         : base(hostUID)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}