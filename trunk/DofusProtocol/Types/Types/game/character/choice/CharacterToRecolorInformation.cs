
// Generated on 03/25/2013 19:24:27
using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }
    
}