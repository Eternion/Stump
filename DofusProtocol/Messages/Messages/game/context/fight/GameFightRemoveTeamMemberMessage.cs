

// Generated on 12/20/2015 16:36:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightRemoveTeamMemberMessage : Message
    {
        public const uint Id = 711;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short fightId;
        public sbyte teamId;
        public double charId;
        
        public GameFightRemoveTeamMemberMessage()
        {
        }
        
        public GameFightRemoveTeamMemberMessage(short fightId, sbyte teamId, double charId)
        {
            this.fightId = fightId;
            this.teamId = teamId;
            this.charId = charId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(fightId);
            writer.WriteSByte(teamId);
            writer.WriteDouble(charId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadShort();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            charId = reader.ReadDouble();
            if (charId < -9.007199254740992E15 || charId > 9.007199254740992E15)
                throw new Exception("Forbidden value on charId = " + charId + ", it doesn't respect the following condition : charId < -9.007199254740992E15 || charId > 9.007199254740992E15");
        }
        
    }
    
}