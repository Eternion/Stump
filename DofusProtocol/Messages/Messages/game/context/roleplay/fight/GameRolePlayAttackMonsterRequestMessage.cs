

// Generated on 10/27/2014 19:57:43
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
        
        public int monsterGroupId;
        
        public GameRolePlayAttackMonsterRequestMessage()
        {
        }
        
        public GameRolePlayAttackMonsterRequestMessage(int monsterGroupId)
        {
            this.monsterGroupId = monsterGroupId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(monsterGroupId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            monsterGroupId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}