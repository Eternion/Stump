

// Generated on 08/04/2015 00:37:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolPartyRefreshMessage : Message
    {
        public const uint Id = 6583;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PartyIdol partyIdol;
        
        public IdolPartyRefreshMessage()
        {
        }
        
        public IdolPartyRefreshMessage(Types.PartyIdol partyIdol)
        {
            this.partyIdol = partyIdol;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            partyIdol.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            partyIdol = new Types.PartyIdol();
            partyIdol.Deserialize(reader);
        }
        
    }
    
}