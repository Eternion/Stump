

// Generated on 03/05/2014 20:34:36
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
        
        public Types.ActorRestrictionsInformations restrictions;
        
        public SetCharacterRestrictionsMessage()
        {
        }
        
        public SetCharacterRestrictionsMessage(Types.ActorRestrictionsInformations restrictions)
        {
            this.restrictions = restrictions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            restrictions.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return restrictions.GetSerializationSize();
        }
        
    }
    
}