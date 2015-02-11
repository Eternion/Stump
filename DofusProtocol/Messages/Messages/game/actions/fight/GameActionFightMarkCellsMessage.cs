

// Generated on 02/11/2015 10:20:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightMarkCellsMessage : AbstractGameActionMessage
    {
        public const uint Id = 5540;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameActionMark mark;
        
        public GameActionFightMarkCellsMessage()
        {
        }
        
        public GameActionFightMarkCellsMessage(short actionId, int sourceId, Types.GameActionMark mark)
         : base(actionId, sourceId)
        {
            this.mark = mark;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            mark.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            mark = new Types.GameActionMark();
            mark.Deserialize(reader);
        }
        
    }
    
}