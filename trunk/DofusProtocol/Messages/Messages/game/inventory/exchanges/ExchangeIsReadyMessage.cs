

// Generated on 08/11/2013 11:28:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeIsReadyMessage : Message
    {
        public const uint Id = 5509;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        public bool ready;
        
        public ExchangeIsReadyMessage()
        {
        }
        
        public ExchangeIsReadyMessage(int id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteBoolean(ready);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            ready = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(bool);
        }
        
    }
    
}