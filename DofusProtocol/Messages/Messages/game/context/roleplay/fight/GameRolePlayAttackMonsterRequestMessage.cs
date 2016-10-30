

// Generated on 10/30/2016 16:20:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayAttackMonsterRequestMessage : Message
    {
        public const uint Id = 6191;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double monsterGroupId;
        
        public GameRolePlayAttackMonsterRequestMessage()
        {
        }
        
        public GameRolePlayAttackMonsterRequestMessage(double monsterGroupId)
        {
            this.monsterGroupId = monsterGroupId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(monsterGroupId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            monsterGroupId = reader.ReadDouble();
            if (monsterGroupId < -9007199254740990 || monsterGroupId > 9007199254740990)
                throw new Exception("Forbidden value on monsterGroupId = " + monsterGroupId + ", it doesn't respect the following condition : monsterGroupId < -9007199254740990 || monsterGroupId > 9007199254740990");
        }
        
    }
    
}