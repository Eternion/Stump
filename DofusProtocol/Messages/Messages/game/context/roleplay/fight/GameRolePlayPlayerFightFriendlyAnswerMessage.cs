

// Generated on 10/26/2014 23:29:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayPlayerFightFriendlyAnswerMessage : Message
    {
        public const uint Id = 5732;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public bool accept;
        
        public GameRolePlayPlayerFightFriendlyAnswerMessage()
        {
        }
        
        public GameRolePlayPlayerFightFriendlyAnswerMessage(int fightId, bool accept)
        {
            this.fightId = fightId;
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            accept = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(bool);
        }
        
    }
    
}