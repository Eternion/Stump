

// Generated on 12/12/2013 16:57:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeReplayMessage : Message
    {
        public const uint Id = 6002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeReplayMessage()
        {
        }
        
        public ExchangeReplayMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}