

// Generated on 12/29/2014 21:14:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightMonsterInformations : GameFightAIInformations
    {
        public const short Id = 29;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short creatureGenericId;
        public sbyte creatureGrade;
        
        public GameFightMonsterInformations()
        {
        }
        
        public GameFightMonsterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, sbyte wave, bool alive, Types.GameFightMinimalStats stats, IEnumerable<short> previousPositions, short creatureGenericId, sbyte creatureGrade)
         : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions)
        {
            this.creatureGenericId = creatureGenericId;
            this.creatureGrade = creatureGrade;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(creatureGenericId);
            writer.WriteSByte(creatureGrade);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creatureGenericId = reader.ReadShort();
            if (creatureGenericId < 0)
                throw new Exception("Forbidden value on creatureGenericId = " + creatureGenericId + ", it doesn't respect the following condition : creatureGenericId < 0");
            creatureGrade = reader.ReadSByte();
            if (creatureGrade < 0)
                throw new Exception("Forbidden value on creatureGrade = " + creatureGrade + ", it doesn't respect the following condition : creatureGrade < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(sbyte);
        }
        
    }
    
}