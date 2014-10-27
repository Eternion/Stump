

// Generated on 10/27/2014 19:57:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightEndMessage : Message
    {
        public const uint Id = 720;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int duration;
        public short ageBonus;
        public short lootShareLimitMalus;
        public IEnumerable<Types.FightResultListEntry> results;
        public IEnumerable<Types.NamedPartyTeamWithOutcome> namedPartyTeamsOutcomes;
        
        public GameFightEndMessage()
        {
        }
        
        public GameFightEndMessage(int duration, short ageBonus, short lootShareLimitMalus, IEnumerable<Types.FightResultListEntry> results, IEnumerable<Types.NamedPartyTeamWithOutcome> namedPartyTeamsOutcomes)
        {
            this.duration = duration;
            this.ageBonus = ageBonus;
            this.lootShareLimitMalus = lootShareLimitMalus;
            this.results = results;
            this.namedPartyTeamsOutcomes = namedPartyTeamsOutcomes;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(duration);
            writer.WriteShort(ageBonus);
            writer.WriteShort(lootShareLimitMalus);
            var results_before = writer.Position;
            var results_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in results)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 results_count++;
            }
            var results_after = writer.Position;
            writer.Seek((int)results_before);
            writer.WriteUShort((ushort)results_count);
            writer.Seek((int)results_after);

            var namedPartyTeamsOutcomes_before = writer.Position;
            var namedPartyTeamsOutcomes_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in namedPartyTeamsOutcomes)
            {
                 entry.Serialize(writer);
                 namedPartyTeamsOutcomes_count++;
            }
            var namedPartyTeamsOutcomes_after = writer.Position;
            writer.Seek((int)namedPartyTeamsOutcomes_before);
            writer.WriteUShort((ushort)namedPartyTeamsOutcomes_count);
            writer.Seek((int)namedPartyTeamsOutcomes_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            duration = reader.ReadInt();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
            ageBonus = reader.ReadShort();
            lootShareLimitMalus = reader.ReadShort();
            var limit = reader.ReadUShort();
            var results_ = new Types.FightResultListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 results_[i] = Types.ProtocolTypeManager.GetInstance<Types.FightResultListEntry>(reader.ReadShort());
                 results_[i].Deserialize(reader);
            }
            results = results_;
            limit = reader.ReadUShort();
            var namedPartyTeamsOutcomes_ = new Types.NamedPartyTeamWithOutcome[limit];
            for (int i = 0; i < limit; i++)
            {
                 namedPartyTeamsOutcomes_[i] = new Types.NamedPartyTeamWithOutcome();
                 namedPartyTeamsOutcomes_[i].Deserialize(reader);
            }
            namedPartyTeamsOutcomes = namedPartyTeamsOutcomes_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + sizeof(short) + sizeof(short) + results.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + namedPartyTeamsOutcomes.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}