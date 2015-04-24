

// Generated on 04/24/2015 03:38:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultTaxCollectorListEntry : FightResultFighterListEntry
    {
        public const short Id = 84;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public byte level;
        public Types.BasicGuildInformations guildInfo;
        public int experienceForGuild;
        
        public FightResultTaxCollectorListEntry()
        {
        }
        
        public FightResultTaxCollectorListEntry(short outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, byte level, Types.BasicGuildInformations guildInfo, int experienceForGuild)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
            this.guildInfo = guildInfo;
            this.experienceForGuild = experienceForGuild;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(level);
            guildInfo.Serialize(writer);
            writer.WriteInt(experienceForGuild);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            guildInfo = new Types.BasicGuildInformations();
            guildInfo.Deserialize(reader);
            experienceForGuild = reader.ReadInt();
        }
        
        
    }
    
}