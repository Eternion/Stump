

// Generated on 03/06/2014 18:50:31
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
            var colors_before = writer.Position;
            var colors_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
                 colors_count++;
            }
            var colors_after = writer.Position;
            writer.Seek((int)colors_before);
            writer.WriteUShort((ushort)colors_count);
            writer.Seek((int)colors_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var colors_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 colors_[i] = reader.ReadInt();
            }
            colors = colors_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + colors.Sum(x => sizeof(int));
        }
        
    }
    
}