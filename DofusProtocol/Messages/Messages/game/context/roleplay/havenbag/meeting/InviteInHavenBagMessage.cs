

// Generated on 09/26/2016 01:50:01
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InviteInHavenBagMessage : Message
    {
        public const uint Id = 6642;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterMinimalInformations guestInformations;
        public bool accept;
        
        public InviteInHavenBagMessage()
        {
        }
        
        public InviteInHavenBagMessage(Types.CharacterMinimalInformations guestInformations, bool accept)
        {
            this.guestInformations = guestInformations;
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            guestInformations.Serialize(writer);
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guestInformations = new Types.CharacterMinimalInformations();
            guestInformations.Deserialize(reader);
            accept = reader.ReadBoolean();
        }
        
    }
    
}