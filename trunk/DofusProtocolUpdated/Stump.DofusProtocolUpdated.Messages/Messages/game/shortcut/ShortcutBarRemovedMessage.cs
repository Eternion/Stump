

// Generated on 03/05/2014 20:34:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarRemovedMessage : Message
    {
        public const uint Id = 6224;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public int slot;
        
        public ShortcutBarRemovedMessage()
        {
        }
        
        public ShortcutBarRemovedMessage(sbyte barType, int slot)
        {
            this.barType = barType;
            this.slot = slot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteInt(slot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            slot = reader.ReadInt();
            if (slot < 0 || slot > 99)
                throw new Exception("Forbidden value on slot = " + slot + ", it doesn't respect the following condition : slot < 0 || slot > 99");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(int);
        }
        
    }
    
}