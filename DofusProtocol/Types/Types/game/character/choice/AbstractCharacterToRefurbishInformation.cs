

// Generated on 10/26/2014 23:30:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AbstractCharacterToRefurbishInformation : AbstractCharacterInformation
    {
        public const short Id = 475;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> colors;
        public int cosmeticId;
        
        public AbstractCharacterToRefurbishInformation()
        {
        }
        
        public AbstractCharacterToRefurbishInformation(int id, IEnumerable<int> colors, int cosmeticId)
         : base(id)
        {
            this.colors = colors;
            this.cosmeticId = cosmeticId;
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

            writer.WriteInt(cosmeticId);
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
            cosmeticId = reader.ReadInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + colors.Sum(x => sizeof(int)) + sizeof(int);
        }
        
    }
    
}