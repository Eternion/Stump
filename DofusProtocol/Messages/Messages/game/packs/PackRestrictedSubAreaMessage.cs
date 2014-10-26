

// Generated on 10/26/2014 23:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PackRestrictedSubAreaMessage : Message
    {
        public const uint Id = 6186;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int subAreaId;
        
        public PackRestrictedSubAreaMessage()
        {
        }
        
        public PackRestrictedSubAreaMessage(int subAreaId)
        {
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadInt();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}