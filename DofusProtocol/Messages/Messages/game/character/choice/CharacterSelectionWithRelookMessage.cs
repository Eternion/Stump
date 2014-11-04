

// Generated on 10/28/2014 16:36:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectionWithRelookMessage : CharacterSelectionMessage
    {
        public const uint Id = 6353;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int cosmeticId;
        
        public CharacterSelectionWithRelookMessage()
        {
        }
        
        public CharacterSelectionWithRelookMessage(int id, int cosmeticId)
         : base(id)
        {
            this.cosmeticId = cosmeticId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(cosmeticId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cosmeticId = reader.ReadInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}