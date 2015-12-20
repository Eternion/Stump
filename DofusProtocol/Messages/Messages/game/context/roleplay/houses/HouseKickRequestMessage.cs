

// Generated on 12/20/2015 16:36:53
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
        
        public long id;
        
        public HouseKickRequestMessage()
        {
        }
        
        public HouseKickRequestMessage(long id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarLong();
            if (id < 0 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 9.007199254740992E15");
        }
        
    }
    
}