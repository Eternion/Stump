

// Generated on 12/12/2013 16:57:29
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
            writer.WriteUShort((ushort)fightTeams.Count());
            foreach (var entry in fightTeams)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fightTeamsOptions.Count());
            foreach (var entry in fightTeamsOptions)
            {
                 entry.Serialize(writer);
            }
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
            fightTeams = new Types.FightTeamLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightTeams as Types.FightTeamLightInformations[])[i] = new Types.FightTeamLightInformations();
                 (fightTeams as Types.FightTeamLightInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fightTeamsOptions = new Types.FightOptionsInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightTeamsOptions as Types.FightOptionsInformations[])[i] = new Types.FightOptionsInformations();
                 (fightTeamsOptions as Types.FightOptionsInformations[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(sbyte) + sizeof(int) + sizeof(bool) + sizeof(short) + fightTeams.Sum(x => x.GetSerializationSize()) + sizeof(short) + fightTeamsOptions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}