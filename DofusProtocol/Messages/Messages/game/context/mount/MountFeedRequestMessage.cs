

// Generated on 02/02/2016 14:14:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountFeedRequestMessage : Message
    {
        public const uint Id = 6189;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mountUid;
        public sbyte mountLocation;
        public int mountFoodUid;
        public int quantity;
        
        public MountFeedRequestMessage()
        {
        }
        
        public MountFeedRequestMessage(int mountUid, sbyte mountLocation, int mountFoodUid, int quantity)
        {
            this.mountUid = mountUid;
            this.mountLocation = mountLocation;
            this.mountFoodUid = mountFoodUid;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(mountUid);
            writer.WriteSByte(mountLocation);
            writer.WriteVarInt(mountFoodUid);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountUid = reader.ReadVarInt();
            if (mountUid < 0)
                throw new Exception("Forbidden value on mountUid = " + mountUid + ", it doesn't respect the following condition : mountUid < 0");
            mountLocation = reader.ReadSByte();
            mountFoodUid = reader.ReadVarInt();
            if (mountFoodUid < 0)
                throw new Exception("Forbidden value on mountFoodUid = " + mountFoodUid + ", it doesn't respect the following condition : mountFoodUid < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}