

// Generated on 03/05/2014 20:34:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftResultMagicWithObjectDescMessage : ExchangeCraftResultWithObjectDescMessage
    {
        public const uint Id = 6188;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte magicPoolStatus;
        
        public ExchangeCraftResultMagicWithObjectDescMessage()
        {
        }
        
        public ExchangeCraftResultMagicWithObjectDescMessage(sbyte craftResult, Types.ObjectItemNotInContainer objectInfo, sbyte magicPoolStatus)
         : base(craftResult, objectInfo)
        {
            this.magicPoolStatus = magicPoolStatus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(magicPoolStatus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            magicPoolStatus = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}