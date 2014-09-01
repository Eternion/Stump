

// Generated on 09/01/2014 15:52:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarRefreshMessage : Message
    {
        public const uint Id = 6229;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public Types.Shortcut shortcut;
        
        public ShortcutBarRefreshMessage()
        {
        }
        
        public ShortcutBarRefreshMessage(sbyte barType, Types.Shortcut shortcut)
        {
            this.barType = barType;
            this.shortcut = shortcut;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteShort(shortcut.TypeId);
            shortcut.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            shortcut = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
            shortcut.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + shortcut.GetSerializationSize();
        }
        
    }
    
}