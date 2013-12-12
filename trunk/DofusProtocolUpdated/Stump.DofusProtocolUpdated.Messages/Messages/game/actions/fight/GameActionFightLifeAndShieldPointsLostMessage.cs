

// Generated on 12/12/2013 16:56:47
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightLifeAndShieldPointsLostMessage : GameActionFightLifePointsLostMessage
    {
        public const uint Id = 6310;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short shieldLoss;
        
        public GameActionFightLifeAndShieldPointsLostMessage()
        {
        }
        
        public GameActionFightLifeAndShieldPointsLostMessage(short actionId, int sourceId, int targetId, short loss, short permanentDamages, short shieldLoss)
         : base(actionId, sourceId, targetId, loss, permanentDamages)
        {
            this.shieldLoss = shieldLoss;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(shieldLoss);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            shieldLoss = reader.ReadShort();
            if (shieldLoss < 0)
                throw new Exception("Forbidden value on shieldLoss = " + shieldLoss + ", it doesn't respect the following condition : shieldLoss < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}