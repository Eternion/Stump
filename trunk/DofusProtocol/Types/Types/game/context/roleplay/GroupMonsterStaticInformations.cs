

// Generated on 07/26/2013 22:51:11
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GroupMonsterStaticInformations
    {
        public const short Id = 140;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.MonsterInGroupLightInformations mainCreatureLightInfos;
        public IEnumerable<Types.MonsterInGroupInformations> underlings;
        
        public GroupMonsterStaticInformations()
        {
        }
        
        public GroupMonsterStaticInformations(Types.MonsterInGroupLightInformations mainCreatureLightInfos, IEnumerable<Types.MonsterInGroupInformations> underlings)
        {
            this.mainCreatureLightInfos = mainCreatureLightInfos;
            this.underlings = underlings;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            mainCreatureLightInfos.Serialize(writer);
            writer.WriteUShort((ushort)underlings.Count());
            foreach (var entry in underlings)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            mainCreatureLightInfos = new Types.MonsterInGroupLightInformations();
            mainCreatureLightInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            underlings = new Types.MonsterInGroupInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (underlings as Types.MonsterInGroupInformations[])[i] = new Types.MonsterInGroupInformations();
                 (underlings as Types.MonsterInGroupInformations[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return mainCreatureLightInfos.GetSerializationSize() + sizeof(short) + underlings.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}