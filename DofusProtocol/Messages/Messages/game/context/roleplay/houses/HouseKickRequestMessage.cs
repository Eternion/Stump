

// Generated on 02/18/2015 10:46:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseKickRequestMessage : Message
    {
        public const uint Id = 5698;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        
        public HouseKickRequestMessage()
        {
        }
        
        public HouseKickRequestMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}