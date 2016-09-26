

// Generated on 09/26/2016 01:49:52
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
        
        public IEnumerable<Types.GameFightFighterInformations> summons;
        
        public GameActionFightSummonMessage()
        {
        }
        
        public GameActionFightSummonMessage(short actionId, double sourceId, IEnumerable<Types.GameFightFighterInformations> summons)
         : base(actionId, sourceId)
        {
            this.summons = summons;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var summons_before = writer.Position;
            var summons_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in summons)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 summons_count++;
            }
            var summons_after = writer.Position;
            writer.Seek((int)summons_before);
            writer.WriteUShort((ushort)summons_count);
            writer.Seek((int)summons_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var summons_ = new Types.GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 summons_[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
                 summons_[i].Deserialize(reader);
            }
            summons = summons_;
        }
        
    }
    
}