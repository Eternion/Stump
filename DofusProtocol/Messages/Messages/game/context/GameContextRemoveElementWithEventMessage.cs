

// Generated on 09/26/2016 01:49:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextRemoveElementWithEventMessage : GameContextRemoveElementMessage
    {
        public const uint Id = 6412;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte elementEventId;
        
        public GameContextRemoveElementWithEventMessage()
        {
        }
        
        public GameContextRemoveElementWithEventMessage(double id, sbyte elementEventId)
         : base(id)
        {
            this.elementEventId = elementEventId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(elementEventId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            elementEventId = reader.ReadSByte();
            if (elementEventId < 0)
                throw new Exception("Forbidden value on elementEventId = " + elementEventId + ", it doesn't respect the following condition : elementEventId < 0");
        }
        
    }
    
}