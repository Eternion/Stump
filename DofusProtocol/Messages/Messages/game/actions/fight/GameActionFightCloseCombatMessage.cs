

// Generated on 12/29/2014 21:11:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage
    {
        public const uint Id = 6116;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short weaponGenericId;
        
        public GameActionFightCloseCombatMessage()
        {
        }
        
        public GameActionFightCloseCombatMessage(short actionId, int sourceId, int targetId, short destinationCellId, sbyte critical, bool silentCast, short weaponGenericId)
         : base(actionId, sourceId, targetId, destinationCellId, critical, silentCast)
        {
            this.weaponGenericId = weaponGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(weaponGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponGenericId = reader.ReadShort();
            if (weaponGenericId < 0)
                throw new Exception("Forbidden value on weaponGenericId = " + weaponGenericId + ", it doesn't respect the following condition : weaponGenericId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}