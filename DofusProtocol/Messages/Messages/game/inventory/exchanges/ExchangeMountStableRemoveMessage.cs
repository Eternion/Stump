

// Generated on 10/26/2014 23:29:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountStableRemoveMessage : Message
    {
        public const uint Id = 5964;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        
        public ExchangeMountStableRemoveMessage()
        {
        }
        
        public ExchangeMountStableRemoveMessage(double mountId)
        {
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadDouble();
            if (mountId < -9.007199254740992E15 || mountId > 9.007199254740992E15)
                throw new Exception("Forbidden value on mountId = " + mountId + ", it doesn't respect the following condition : mountId < -9.007199254740992E15 || mountId > 9.007199254740992E15");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double);
        }
        
    }
    
}