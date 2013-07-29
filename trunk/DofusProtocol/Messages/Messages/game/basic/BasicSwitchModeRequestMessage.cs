

// Generated on 07/29/2013 23:07:38
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
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}