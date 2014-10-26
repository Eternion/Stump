

// Generated on 10/26/2014 23:29:12
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
        
        public uint statId;
        
        public BasicStatMessage()
        {
        }
        
        public BasicStatMessage(uint statId)
        {
            this.statId = statId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUInt(statId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            statId = reader.ReadUInt();
            if (statId < 0 || statId > 4.294967295E9)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0 || statId > 4.294967295E9");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(uint);
        }
        
    }
    
}