

// Generated on 08/04/2015 00:37:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AreaFightModificatorUpdateMessage : Message
    {
        public const uint Id = 6493;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellPairId;
        
        public AreaFightModificatorUpdateMessage()
        {
        }
        
        public AreaFightModificatorUpdateMessage(int spellPairId)
        {
            this.spellPairId = spellPairId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(spellPairId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellPairId = reader.ReadInt();
        }
        
    }
    
}