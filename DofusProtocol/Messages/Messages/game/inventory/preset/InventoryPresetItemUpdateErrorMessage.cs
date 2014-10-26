

// Generated on 10/26/2014 23:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetItemUpdateErrorMessage : Message
    {
        public const uint Id = 6211;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte code;
        
        public InventoryPresetItemUpdateErrorMessage()
        {
        }
        
        public InventoryPresetItemUpdateErrorMessage(sbyte code)
        {
            this.code = code;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(code);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            code = reader.ReadSByte();
            if (code < 0)
                throw new Exception("Forbidden value on code = " + code + ", it doesn't respect the following condition : code < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}