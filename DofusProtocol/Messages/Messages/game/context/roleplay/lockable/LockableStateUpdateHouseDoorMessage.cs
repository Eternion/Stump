

// Generated on 02/11/2015 10:20:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableStateUpdateHouseDoorMessage : LockableStateUpdateAbstractMessage
    {
        public const uint Id = 5668;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        
        public LockableStateUpdateHouseDoorMessage()
        {
        }
        
        public LockableStateUpdateHouseDoorMessage(bool locked, int houseId)
         : base(locked)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(houseId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            houseId = reader.ReadInt();
        }
        
    }
    
}