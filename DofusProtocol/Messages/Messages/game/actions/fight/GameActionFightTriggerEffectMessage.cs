

// Generated on 10/30/2016 16:20:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightTriggerEffectMessage : GameActionFightDispellEffectMessage
    {
        public const uint Id = 6147;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameActionFightTriggerEffectMessage()
        {
        }
        
        public GameActionFightTriggerEffectMessage(short actionId, double sourceId, double targetId, int boostUID)
         : base(actionId, sourceId, targetId, boostUID)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}