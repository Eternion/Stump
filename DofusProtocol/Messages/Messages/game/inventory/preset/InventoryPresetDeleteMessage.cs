

// Generated on 03/02/2014 20:42:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetDeleteMessage : Message
    {
        public const uint Id = 6169;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        
        public InventoryPresetDeleteMessage()
        {
        }
        
        public InventoryPresetDeleteMessage(sbyte presetId)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}