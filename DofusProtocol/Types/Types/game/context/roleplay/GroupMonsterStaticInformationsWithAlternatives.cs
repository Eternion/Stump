

// Generated on 09/01/2015 10:48:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GroupMonsterStaticInformationsWithAlternatives : GroupMonsterStaticInformations
    {
        public const short Id = 396;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.AlternativeMonstersInGroupLightInformations> alternatives;
        
        public GroupMonsterStaticInformationsWithAlternatives()
        {
        }
        
        public GroupMonsterStaticInformationsWithAlternatives(Types.MonsterInGroupLightInformations mainCreatureLightInfos, IEnumerable<Types.MonsterInGroupInformations> underlings, IEnumerable<Types.AlternativeMonstersInGroupLightInformations> alternatives)
         : base(mainCreatureLightInfos, underlings)
        {
            this.alternatives = alternatives;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var alternatives_before = writer.Position;
            var alternatives_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in alternatives)
            {
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
            var limit = reader.ReadUShort();
            var alternatives_ = new Types.AlternativeMonstersInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 alternatives_[i] = new Types.AlternativeMonstersInGroupLightInformations();
                 alternatives_[i].Deserialize(reader);
            }
            alternatives = alternatives_;
        }
        
        
    }
    
}