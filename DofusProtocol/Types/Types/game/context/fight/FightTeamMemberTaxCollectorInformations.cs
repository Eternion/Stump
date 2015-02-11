

// Generated on 02/11/2015 10:21:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamMemberTaxCollectorInformations : FightTeamMemberInformations
    {
        public const short Id = 177;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short firstNameId;
        public short lastNameId;
        public byte level;
        public int guildId;
        public int uid;
        
        public FightTeamMemberTaxCollectorInformations()
        {
        }
        
        public FightTeamMemberTaxCollectorInformations(int id, short firstNameId, short lastNameId, byte level, int guildId, int uid)
         : base(id)
        {
            this.firstNameId = firstNameId;
            this.lastNameId = lastNameId;
            this.level = level;
            this.guildId = guildId;
            this.uid = uid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(firstNameId);
            writer.WriteVarShort(lastNameId);
            writer.WriteByte(level);
            writer.WriteVarInt(guildId);
            writer.WriteVarInt(uid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstNameId = reader.ReadVarShort();
            if (firstNameId < 0)
                throw new Exception("Forbidden value on firstNameId = " + firstNameId + ", it doesn't respect the following condition : firstNameId < 0");
            lastNameId = reader.ReadVarShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            uid = reader.ReadVarInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
        }
        
        
    }
    
}