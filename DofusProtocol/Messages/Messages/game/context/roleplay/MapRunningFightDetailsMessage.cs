

// Generated on 12/29/2014 21:12:28
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
            var attackers_before = writer.Position;
            var attackers_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in attackers)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 attackers_count++;
            }
            var attackers_after = writer.Position;
            writer.Seek((int)attackers_before);
            writer.WriteUShort((ushort)attackers_count);
            writer.Seek((int)attackers_after);

            var defenders_before = writer.Position;
            var defenders_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in defenders)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 defenders_count++;
            }
            var defenders_after = writer.Position;
            writer.Seek((int)defenders_before);
            writer.WriteUShort((ushort)defenders_count);
            writer.Seek((int)defenders_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            var attackers_ = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 attackers_[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterLightInformations>(reader.ReadShort());
                 attackers_[i].Deserialize(reader);
            }
            attackers = attackers_;
            limit = reader.ReadUShort();
            var defenders_ = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 defenders_[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterLightInformations>(reader.ReadShort());
                 defenders_[i].Deserialize(reader);
            }
            defenders = defenders_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + attackers.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + defenders.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}