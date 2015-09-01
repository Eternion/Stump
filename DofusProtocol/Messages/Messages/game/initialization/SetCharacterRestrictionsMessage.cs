

// Generated on 09/01/2015 10:48:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SetCharacterRestrictionsMessage : Message
    {
        public const uint Id = 170;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int actorId;
        public Types.ActorRestrictionsInformations restrictions;
        
        public SetCharacterRestrictionsMessage()
        {
        }
        
        public SetCharacterRestrictionsMessage(int actorId, Types.ActorRestrictionsInformations restrictions)
        {
            this.actorId = actorId;
            this.restrictions = restrictions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(actorId);
            restrictions.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actorId = reader.ReadInt();
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
        }
        
    }
    
}