

// Generated on 10/26/2014 23:29:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightSummonMessage : AbstractGameActionMessage
    {
        public const uint Id = 5825;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameFightFighterInformations summon;
        
        public GameActionFightSummonMessage()
        {
        }
        
        public GameActionFightSummonMessage(short actionId, int sourceId, Types.GameFightFighterInformations summon)
         : base(actionId, sourceId)
        {
            this.summon = summon;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(summon.TypeId);
            summon.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            summon = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
            summon.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + summon.GetSerializationSize();
        }
        
    }
    
}