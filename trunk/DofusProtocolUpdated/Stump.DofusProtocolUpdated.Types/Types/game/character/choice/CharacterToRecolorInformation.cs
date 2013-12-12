

// Generated on 12/12/2013 16:57:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterToRecolorInformation : AbstractCharacterInformation
    {
        public const short Id = 212;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> colors;
        
        public CharacterToRecolorInformation()
        {
        }
        
        public CharacterToRecolorInformation(int id, IEnumerable<int> colors)
         : base(id)
        {
            this.colors = colors;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)colors.Count());
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            colors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (colors as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + colors.Sum(x => sizeof(int));
        }
        
    }
    
}