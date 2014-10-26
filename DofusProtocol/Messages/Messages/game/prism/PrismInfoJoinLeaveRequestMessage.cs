

// Generated on 10/26/2014 23:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismInfoJoinLeaveRequestMessage : Message
    {
        public const uint Id = 5844;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool join;
        
        public PrismInfoJoinLeaveRequestMessage()
        {
        }
        
        public PrismInfoJoinLeaveRequestMessage(bool join)
        {
            this.join = join;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(join);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            join = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}