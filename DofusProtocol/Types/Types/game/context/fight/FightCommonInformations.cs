

// Generated on 03/02/2014 20:42:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightCommonInformations
    {
        public const short Id = 43;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int fightId;
        public sbyte fightType;
        public IEnumerable<Types.FightTeamInformations> fightTeams;
        public IEnumerable<short> fightTeamsPositions;
        public IEnumerable<Types.FightOptionsInformations> fightTeamsOptions;
        
        public FightCommonInformations()
        {
        }
        
        public FightCommonInformations(int fightId, sbyte fightType, IEnumerable<Types.FightTeamInformations> fightTeams, IEnumerable<short> fightTeamsPositions, IEnumerable<Types.FightOptionsInformations> fightTeamsOptions)
        {
            this.fightId = fightId;
            this.fightType = fightType;
            this.fightTeams = fightTeams;
            this.fightTeamsPositions = fightTeamsPositions;
            this.fightTeamsOptions = fightTeamsOptions;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteSByte(fightType);
            var fightTeams_before = writer.Position;
            var fightTeams_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fightTeams)
            {
                 entry.Serialize(writer);
                 fightTeams_count++;
            }
            var fightTeams_after = writer.Position;
            writer.Seek((int)fightTeams_before);
            writer.WriteUShort((ushort)fightTeams_count);
            writer.Seek((int)fightTeams_after);

            var fightTeamsPositions_before = writer.Position;
            var fightTeamsPositions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fightTeamsPositions)
            {
                 writer.WriteShort(entry);
                 fightTeamsPositions_count++;
            }
            var fightTeamsPositions_after = writer.Position;
            writer.Seek((int)fightTeamsPositions_before);
            writer.WriteUShort((ushort)fightTeamsPositions_count);
            writer.Seek((int)fightTeamsPositions_after);

            var fightTeamsOptions_before = writer.Position;
            var fightTeamsOptions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fightTeamsOptions)
            {
                 entry.Serialize(writer);
                 fightTeamsOptions_count++;
            }
            var fightTeamsOptions_after = writer.Position;
            writer.Seek((int)fightTeamsOptions_before);
            writer.WriteUShort((ushort)fightTeamsOptions_count);
            writer.Seek((int)fightTeamsOptions_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
            var limit = reader.ReadUShort();
            var fightTeams_ = new Types.FightTeamInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightTeams_[i] = new Types.FightTeamInformations();
                 fightTeams_[i].Deserialize(reader);
            }
            fightTeams = fightTeams_;
            limit = reader.ReadUShort();
            var fightTeamsPositions_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightTeamsPositions_[i] = reader.ReadShort();
            }
            fightTeamsPositions = fightTeamsPositions_;
            limit = reader.ReadUShort();
            var fightTeamsOptions_ = new Types.FightOptionsInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightTeamsOptions_[i] = new Types.FightOptionsInformations();
                 fightTeamsOptions_[i].Deserialize(reader);
            }
            fightTeamsOptions = fightTeamsOptions_;
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(sbyte) + sizeof(short) + fightTeams.Sum(x => x.GetSerializationSize()) + sizeof(short) + fightTeamsPositions.Sum(x => sizeof(short)) + sizeof(short) + fightTeamsOptions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}