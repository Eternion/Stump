

// Generated on 12/29/2014 21:11:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterCreationRequestMessage : Message
    {
        public const uint Id = 160;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public sbyte breed;
        public bool sex;
        public IEnumerable<int> colors;
        public short cosmeticId;
        
        public CharacterCreationRequestMessage()
        {
        }
        
        public CharacterCreationRequestMessage(string name, sbyte breed, bool sex, IEnumerable<int> colors, short cosmeticId)
        {
            this.name = name;
            this.breed = breed;
            this.sex = sex;
            this.colors = colors;
            this.cosmeticId = cosmeticId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            foreach (var entry in colors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteShort(cosmeticId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            breed = reader.ReadSByte();
            if (breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Eliatrope)
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : breed < (byte)Enums.PlayableBreedEnum.Feca || breed > (byte)Enums.PlayableBreedEnum.Eliatrope");
            sex = reader.ReadBoolean();
            var colors_ = new int[5];
            for (int i = 0; i < 5; i++)
            {
                 colors_[i] = reader.ReadInt();
            }
            colors = colors_;
            cosmeticId = reader.ReadShort();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(sbyte) + sizeof(bool) + sizeof(short) + colors.Sum(x => sizeof(int)) + sizeof(short);
        }
        
    }
    
}