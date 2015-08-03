

// Generated on 08/04/2015 00:37:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMultiCraftCrafterCanUseHisRessourcesMessage : Message
    {
        public const uint Id = 6020;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool allowed;
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage()
        {
        }
        
        public ExchangeMultiCraftCrafterCanUseHisRessourcesMessage(bool allowed)
        {
            this.allowed = allowed;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(allowed);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allowed = reader.ReadBoolean();
        }
        
    }
    
}