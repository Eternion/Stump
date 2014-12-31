

// Generated on 12/29/2014 21:12:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpellForgetUIMessage : Message
    {
        public const uint Id = 5565;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool open;
        
        public SpellForgetUIMessage()
        {
        }
        
        public SpellForgetUIMessage(bool open)
        {
            this.open = open;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(open);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            open = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}