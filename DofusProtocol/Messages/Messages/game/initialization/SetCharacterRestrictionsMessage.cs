

// Generated on 12/20/2015 16:37:01
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
        
        public double actorId;
        public Types.ActorRestrictionsInformations restrictions;
        
        public SetCharacterRestrictionsMessage()
        {
        }
        
        public SetCharacterRestrictionsMessage(double actorId, Types.ActorRestrictionsInformations restrictions)
        {
            this.actorId = actorId;
            this.restrictions = restrictions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(actorId);
            restrictions.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actorId = reader.ReadDouble();
            if (actorId < -9.007199254740992E15 || actorId > 9.007199254740992E15)
                throw new Exception("Forbidden value on actorId = " + actorId + ", it doesn't respect the following condition : actorId < -9.007199254740992E15 || actorId > 9.007199254740992E15");
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
        }
        
    }
    
}