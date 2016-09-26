

// Generated on 09/26/2016 01:50:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveUseRequestMessage : Message
    {
        public const uint Id = 5001;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int elemId;
        public int skillInstanceUid;
        
        public InteractiveUseRequestMessage()
        {
        }
        
        public InteractiveUseRequestMessage(int elemId, int skillInstanceUid)
        {
            this.elemId = elemId;
            this.skillInstanceUid = skillInstanceUid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(elemId);
            writer.WriteVarInt(skillInstanceUid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            elemId = reader.ReadVarInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillInstanceUid = reader.ReadVarInt();
            if (skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
        
    }
    
}