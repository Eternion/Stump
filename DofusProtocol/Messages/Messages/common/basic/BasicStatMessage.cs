

// Generated on 12/29/2014 21:11:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicStatMessage : Message
    {
        public const uint Id = 6530;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short statId;
        
        public BasicStatMessage()
        {
        }
        
        public BasicStatMessage(short statId)
        {
            this.statId = statId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(statId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            statId = reader.ReadShort();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}