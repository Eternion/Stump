

// Generated on 04/19/2016 10:17:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightUnmarkCellsMessage : AbstractGameActionMessage
    {
        public const uint Id = 5570;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short markId;
        
        public GameActionFightUnmarkCellsMessage()
        {
        }
        
        public GameActionFightUnmarkCellsMessage(short actionId, double sourceId, short markId)
         : base(actionId, sourceId)
        {
            this.markId = markId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
        }
        
    }
    
}