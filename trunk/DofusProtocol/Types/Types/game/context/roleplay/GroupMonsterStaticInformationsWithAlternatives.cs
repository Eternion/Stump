

// Generated on 07/29/2013 23:08:46
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)alternatives.Count());
            foreach (var entry in alternatives)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            alternatives = new Types.AlternativeMonstersInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alternatives as Types.AlternativeMonstersInGroupLightInformations[])[i] = new Types.AlternativeMonstersInGroupLightInformations();
                 (alternatives as Types.AlternativeMonstersInGroupLightInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + alternatives.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}