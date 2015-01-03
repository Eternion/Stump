

// Generated on 12/29/2014 21:13:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MimicryObjectFeedAndAssociateRequestMessage : SymbioticObjectAssociateRequestMessage
    {
        public const uint Id = 6460;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int foodUID;
        public byte foodPos;
        public bool preview;
        
        public MimicryObjectFeedAndAssociateRequestMessage()
        {
        }
        
        public MimicryObjectFeedAndAssociateRequestMessage(int symbioteUID, byte symbiotePos, int hostUID, byte hostPos, int foodUID, byte foodPos, bool preview)
         : base(symbioteUID, symbiotePos, hostUID, hostPos)
        {
            this.foodUID = foodUID;
            this.foodPos = foodPos;
            this.preview = preview;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(foodUID);
            writer.WriteByte(foodPos);
            writer.WriteBoolean(preview);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            foodUID = reader.ReadInt();
            if (foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + foodUID + ", it doesn't respect the following condition : foodUID < 0");
            foodPos = reader.ReadByte();
            if (foodPos < 0 || foodPos > 255)
                throw new Exception("Forbidden value on foodPos = " + foodPos + ", it doesn't respect the following condition : foodPos < 0 || foodPos > 255");
            preview = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(byte) + sizeof(bool);
        }
        
    }
    
}