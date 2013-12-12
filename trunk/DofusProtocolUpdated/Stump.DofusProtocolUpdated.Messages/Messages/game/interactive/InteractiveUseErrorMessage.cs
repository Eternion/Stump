

// Generated on 12/12/2013 16:57:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveUseErrorMessage : Message
    {
        public const uint Id = 6384;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int elemId;
        public int skillInstanceUid;
        
        public InteractiveUseErrorMessage()
        {
        }
        
        public InteractiveUseErrorMessage(int elemId, int skillInstanceUid)
        {
            this.elemId = elemId;
            this.skillInstanceUid = skillInstanceUid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(elemId);
            writer.WriteInt(skillInstanceUid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            elemId = reader.ReadInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillInstanceUid = reader.ReadInt();
            if (skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int);
        }
        
    }
    
}