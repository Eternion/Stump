
// Generated on 01/04/2013 14:35:42
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicSwitchModeRequestMessage : Message
    {
        public const uint Id = 6101;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte mode;
        
        public BasicSwitchModeRequestMessage()
        {
        }
        
        public BasicSwitchModeRequestMessage(sbyte mode)
        {
            this.mode = mode;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(mode);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mode = reader.ReadSByte();
        }
        
    }
    
}