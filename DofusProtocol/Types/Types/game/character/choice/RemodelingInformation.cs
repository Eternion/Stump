

// Generated on 10/30/2016 16:20:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class RemodelingInformation
    {
        public const short Id = 480;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        public sbyte breed;
        public bool sex;
        public short cosmeticId;
        public IEnumerable<int> colors;
        
        public RemodelingInformation()
        {
        }
        
        public RemodelingInformation(string name, sbyte breed, bool sex, short cosmeticId, IEnumerable<int> colors)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.cosmeticId = cosmeticId;
            this.colors = colors;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteVarShort(cosmeticId);
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
        
        public virtual void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
            cosmeticId = reader.ReadVarShort();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
            var limit = reader.ReadUShort();
            var colors_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 colors_[i] = reader.ReadInt();
            }
            colors = colors_;
        }
        
        
    }
    
}