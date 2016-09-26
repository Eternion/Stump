

// Generated on 09/26/2016 01:49:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var fighters_before = writer.Position;
            var fighters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fighters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 fighters_count++;
            }
            var fighters_after = writer.Position;
            writer.Seek((int)fighters_before);
            writer.WriteUShort((ushort)fighters_count);
            writer.Seek((int)fighters_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var fighters_ = new Types.GameFightFighterInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fighters_[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
                 fighters_[i].Deserialize(reader);
            }
            fighters = fighters_;
        }
        
    }
    
}