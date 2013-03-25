
// Generated on 03/25/2013 19:24:08
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightSynchronizeMessage : Message
    {
        public const uint Id = 5921;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GameFightFighterInformations> fighters;
        
        public GameFightSynchronizeMessage()
        {
        }
        
        public GameFightSynchronizeMessage(IEnumerable<Types.GameFightFighterInformations> fighters)
        {
            this.fighters = fighters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)fighters.Count());
            foreach (var entry in fighters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            fighters = new Types.GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fighters as Types.GameFightFighterInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
                 (fighters as Types.GameFightFighterInformations[])[i].Deserialize(reader);
            }
        }
        
    }
    
}