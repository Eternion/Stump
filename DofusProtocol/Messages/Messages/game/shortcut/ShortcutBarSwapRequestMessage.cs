

// Generated on 02/18/2015 10:46:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarSwapRequestMessage : Message
    {
        public const uint Id = 6230;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public sbyte firstSlot;
        public sbyte secondSlot;
        
        public ShortcutBarSwapRequestMessage()
        {
        }
        
        public ShortcutBarSwapRequestMessage(sbyte barType, sbyte firstSlot, sbyte secondSlot)
        {
            this.barType = barType;
            this.firstSlot = firstSlot;
            this.secondSlot = secondSlot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteSByte(firstSlot);
            writer.WriteSByte(secondSlot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            firstSlot = reader.ReadSByte();
            if (firstSlot < 0 || firstSlot > 99)
                throw new Exception("Forbidden value on firstSlot = " + firstSlot + ", it doesn't respect the following condition : firstSlot < 0 || firstSlot > 99");
            secondSlot = reader.ReadSByte();
            if (secondSlot < 0 || secondSlot > 99)
                throw new Exception("Forbidden value on secondSlot = " + secondSlot + ", it doesn't respect the following condition : secondSlot < 0 || secondSlot > 99");
        }
        
    }
    
}