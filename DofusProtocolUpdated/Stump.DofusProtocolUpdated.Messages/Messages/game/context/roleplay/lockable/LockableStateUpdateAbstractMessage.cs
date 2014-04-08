

// Generated on 03/06/2014 18:50:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableStateUpdateAbstractMessage : Message
    {
        public const uint Id = 5671;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool locked;
        
        public LockableStateUpdateAbstractMessage()
        {
        }
        
        public LockableStateUpdateAbstractMessage(bool locked)
        {
            this.locked = locked;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(locked);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            locked = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}