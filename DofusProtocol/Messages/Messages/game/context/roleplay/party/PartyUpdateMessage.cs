

// Generated on 09/01/2014 15:52:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyUpdateMessage : AbstractPartyEventMessage
    {
        public const uint Id = 5575;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PartyMemberInformations memberInformations;
        
        public PartyUpdateMessage()
        {
        }
        
        public PartyUpdateMessage(int partyId, Types.PartyMemberInformations memberInformations)
         : base(partyId)
        {
            this.memberInformations = memberInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(memberInformations.TypeId);
            memberInformations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            memberInformations = Types.ProtocolTypeManager.GetInstance<Types.PartyMemberInformations>(reader.ReadShort());
            memberInformations.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + memberInformations.GetSerializationSize();
        }
        
    }
    
}