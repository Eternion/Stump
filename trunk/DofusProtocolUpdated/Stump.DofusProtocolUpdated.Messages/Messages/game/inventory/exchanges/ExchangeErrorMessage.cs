

// Generated on 12/12/2013 16:57:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeErrorMessage : Message
    {
        public const uint Id = 5513;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte errorType;
        
        public ExchangeErrorMessage()
        {
        }
        
        public ExchangeErrorMessage(sbyte errorType)
        {
            this.errorType = errorType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(errorType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            errorType = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}