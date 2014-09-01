

// Generated on 09/01/2014 15:52:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var underlings_before = writer.Position;
            var underlings_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in underlings)
            {
                 entry.Serialize(writer);
                 underlings_count++;
            }
            var underlings_after = writer.Position;
            writer.Seek((int)underlings_before);
            writer.WriteUShort((ushort)underlings_count);
            writer.Seek((int)underlings_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            mainCreatureLightInfos = new Types.MonsterInGroupLightInformations();
            mainCreatureLightInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            var underlings_ = new Types.MonsterInGroupInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 underlings_[i] = new Types.MonsterInGroupInformations();
                 underlings_[i].Deserialize(reader);
            }
            underlings = underlings_;
        }
        
        public virtual int GetSerializationSize()
        {
            return mainCreatureLightInfos.GetSerializationSize() + sizeof(short) + underlings.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}