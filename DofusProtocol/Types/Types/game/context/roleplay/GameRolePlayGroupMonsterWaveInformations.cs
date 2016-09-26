

// Generated on 09/26/2016 01:50:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayGroupMonsterWaveInformations : GameRolePlayGroupMonsterInformations
    {
        public const short Id = 464;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte nbWaves;
        public IEnumerable<Types.GroupMonsterStaticInformations> alternatives;
        
        public GameRolePlayGroupMonsterWaveInformations()
        {
        }
        
        public GameRolePlayGroupMonsterWaveInformations(double contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, bool keyRingBonus, bool hasHardcoreDrop, bool hasAVARewardToken, Types.GroupMonsterStaticInformations staticInfos, double creationTime, int ageBonusRate, sbyte lootShare, sbyte alignmentSide, sbyte nbWaves, IEnumerable<Types.GroupMonsterStaticInformations> alternatives)
         : base(contextualId, look, disposition, keyRingBonus, hasHardcoreDrop, hasAVARewardToken, staticInfos, creationTime, ageBonusRate, lootShare, alignmentSide)
        {
            this.nbWaves = nbWaves;
            this.alternatives = alternatives;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(nbWaves);
            var alternatives_before = writer.Position;
            var alternatives_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in alternatives)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 alternatives_count++;
            }
            var alternatives_after = writer.Position;
            writer.Seek((int)alternatives_before);
            writer.WriteUShort((ushort)alternatives_count);
            writer.Seek((int)alternatives_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            nbWaves = reader.ReadSByte();
            if (nbWaves < 0)
                throw new Exception("Forbidden value on nbWaves = " + nbWaves + ", it doesn't respect the following condition : nbWaves < 0");
            var limit = reader.ReadUShort();
            var alternatives_ = new Types.GroupMonsterStaticInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 alternatives_[i] = Types.ProtocolTypeManager.GetInstance<Types.GroupMonsterStaticInformations>(reader.ReadShort());
                 alternatives_[i].Deserialize(reader);
            }
            alternatives = alternatives_;
        }
        
        
    }
    
}