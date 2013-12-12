

// Generated on 12/12/2013 16:56:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapRunningFightDetailsMessage : Message
    {
        public const uint Id = 5751;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public IEnumerable<Types.GameFightFighterLightInformations> attackers;
        public IEnumerable<Types.GameFightFighterLightInformations> defenders;
        
        public MapRunningFightDetailsMessage()
        {
        }
        
        public MapRunningFightDetailsMessage(int fightId, IEnumerable<Types.GameFightFighterLightInformations> attackers, IEnumerable<Types.GameFightFighterLightInformations> defenders)
        {
            this.fightId = fightId;
            this.attackers = attackers;
            this.defenders = defenders;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteUShort((ushort)attackers.Count());
            foreach (var entry in attackers)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)defenders.Count());
            foreach (var entry in defenders)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            attackers = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (attackers as Types.GameFightFighterLightInformations[])[i] = new Types.GameFightFighterLightInformations();
                 (attackers as Types.GameFightFighterLightInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            defenders = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (defenders as Types.GameFightFighterLightInformations[])[i] = new Types.GameFightFighterLightInformations();
                 (defenders as Types.GameFightFighterLightInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + attackers.Sum(x => x.GetSerializationSize()) + sizeof(short) + defenders.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}