

// Generated on 04/19/2016 10:17:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyNameUpdateMessage : AbstractPartyMessage
    {
        public const uint Id = 6502;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string partyName;
        
        public PartyNameUpdateMessage()
        {
        }
        
        public PartyNameUpdateMessage(int partyId, string partyName)
         : base(partyId)
        {
            this.partyName = partyName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(partyName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyName = reader.ReadUTF();
        }
        
    }
    
}