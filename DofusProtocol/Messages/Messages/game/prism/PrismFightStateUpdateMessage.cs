

// Generated on 10/26/2014 23:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightStateUpdateMessage : Message
    {
        public const uint Id = 6040;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte state;
        
        public PrismFightStateUpdateMessage()
        {
        }
        
        public PrismFightStateUpdateMessage(sbyte state)
        {
            this.state = state;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(state);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}