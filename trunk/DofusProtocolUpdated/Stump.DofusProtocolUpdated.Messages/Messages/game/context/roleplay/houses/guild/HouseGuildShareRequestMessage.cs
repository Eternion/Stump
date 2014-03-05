

// Generated on 03/05/2014 20:34:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseGuildShareRequestMessage : Message
    {
        public const uint Id = 5704;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        public uint rights;
        
        public HouseGuildShareRequestMessage()
        {
        }
        
        public HouseGuildShareRequestMessage(bool enable, uint rights)
        {
            this.enable = enable;
            this.rights = rights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enable);
            writer.WriteUInt(rights);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enable = reader.ReadBoolean();
            rights = reader.ReadUInt();
            if (rights < 0 || rights > 4.294967295E9)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0 || rights > 4.294967295E9");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(uint);
        }
        
    }
    
}