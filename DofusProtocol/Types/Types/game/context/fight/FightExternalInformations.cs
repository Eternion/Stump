

// Generated on 09/01/2014 15:52:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightExternalInformations
    {
        public const short Id = 117;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int fightId;
        public sbyte fightType;
        public int fightStart;
        public bool fightSpectatorLocked;
        public IEnumerable<Types.FightTeamLightInformations> fightTeams;
        public IEnumerable<Types.FightOptionsInformations> fightTeamsOptions;
        
        public FightExternalInformations()
        {
        }
        
        public FightExternalInformations(int fightId, sbyte fightType, int fightStart, bool fightSpectatorLocked, IEnumerable<Types.FightTeamLightInformations> fightTeams, IEnumerable<Types.FightOptionsInformations> fightTeamsOptions)
        {
            this.fightId = fightId;
            this.fightType = fightType;
            this.fightStart = fightStart;
            this.fightSpectatorLocked = fightSpectatorLocked;
            this.fightTeams = fightTeams;
            this.fightTeamsOptions = fightTeamsOptions;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteSByte(fightType);
            writer.WriteInt(fightStart);
            writer.WriteBoolean(fightSpectatorLocked);
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
            fightStart = reader.ReadInt();
            if (fightStart < 0)
                throw new Exception("Forbidden value on fightStart = " + fightStart + ", it doesn't respect the following condition : fightStart < 0");
            fightSpectatorLocked = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var fightTeams_ = new Types.FightTeamLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightTeams_[i] = new Types.FightTeamLightInformations();
                 fightTeams_[i].Deserialize(reader);
            }
            fightTeams = fightTeams_;
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
            return sizeof(int) + sizeof(sbyte) + sizeof(int) + sizeof(bool) + sizeof(short) + fightTeams.Sum(x => x.GetSerializationSize()) + sizeof(short) + fightTeamsOptions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}