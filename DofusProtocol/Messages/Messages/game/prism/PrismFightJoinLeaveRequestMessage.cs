

// Generated on 02/02/2016 14:14:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightJoinLeaveRequestMessage : Message
    {
        public const uint Id = 5843;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public bool join;
        
        public PrismFightJoinLeaveRequestMessage()
        {
        }
        
        public PrismFightJoinLeaveRequestMessage(short subAreaId, bool join)
        {
            this.subAreaId = subAreaId;
            this.join = join;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(subAreaId);
            writer.WriteBoolean(join);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            join = reader.ReadBoolean();
        }
        
    }
    
}