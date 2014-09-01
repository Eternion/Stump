

// Generated on 09/01/2014 15:52:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterInformations : GameContextActorInformations
    {
        public const short Id = 143;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte teamId;
        public uint wave;
        public bool alive;
        public Types.GameFightMinimalStats stats;
        
        public GameFightFighterInformations()
        {
        }
        
        public GameFightFighterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, uint wave, bool alive, Types.GameFightMinimalStats stats)
         : base(contextualId, look, disposition)
        {
            this.teamId = teamId;
            this.wave = wave;
            this.alive = alive;
            this.stats = stats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(teamId);
            writer.WriteUInt(wave);
            writer.WriteBoolean(alive);
            writer.WriteShort(stats.TypeId);
            stats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            wave = reader.ReadUInt();
            if (wave < 0 || wave > 4.294967295E9)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0 || wave > 4.294967295E9");
            alive = reader.ReadBoolean();
            stats = Types.ProtocolTypeManager.GetInstance<Types.GameFightMinimalStats>(reader.ReadShort());
            stats.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(uint) + sizeof(bool) + sizeof(short) + stats.GetSerializationSize();
        }
        
    }
    
}