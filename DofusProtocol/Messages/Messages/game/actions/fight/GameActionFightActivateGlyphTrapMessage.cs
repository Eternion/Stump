

// Generated on 08/04/2015 13:24:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightActivateGlyphTrapMessage : AbstractGameActionMessage
    {
        public const uint Id = 6545;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short markId;
        public bool active;
        
        public GameActionFightActivateGlyphTrapMessage()
        {
        }
        
        public GameActionFightActivateGlyphTrapMessage(short actionId, int sourceId, short markId, bool active)
         : base(actionId, sourceId)
        {
            this.markId = markId;
            this.active = active;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
            active = reader.ReadBoolean();
        }
        
    }
    
}