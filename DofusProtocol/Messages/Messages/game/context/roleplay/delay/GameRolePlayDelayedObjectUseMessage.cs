

// Generated on 02/02/2016 14:14:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayDelayedObjectUseMessage : GameRolePlayDelayedActionMessage
    {
        public const uint Id = 6425;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short objectGID;
        
        public GameRolePlayDelayedObjectUseMessage()
        {
        }
        
        public GameRolePlayDelayedObjectUseMessage(double delayedCharacterId, sbyte delayTypeId, double delayEndTime, short objectGID)
         : base(delayedCharacterId, delayTypeId, delayEndTime)
        {
            this.objectGID = objectGID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(objectGID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGID = reader.ReadVarShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
    }
    
}