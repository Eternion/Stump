

// Generated on 12/29/2014 21:14:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShortcutBarContentMessage : Message
    {
        public const uint Id = 6231;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public IEnumerable<Types.Shortcut> shortcuts;
        
        public ShortcutBarContentMessage()
        {
        }
        
        public ShortcutBarContentMessage(sbyte barType, IEnumerable<Types.Shortcut> shortcuts)
        {
            this.barType = barType;
            this.shortcuts = shortcuts;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            var shortcuts_before = writer.Position;
            var shortcuts_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in shortcuts)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 shortcuts_count++;
            }
            var shortcuts_after = writer.Position;
            writer.Seek((int)shortcuts_before);
            writer.WriteUShort((ushort)shortcuts_count);
            writer.Seek((int)shortcuts_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            var limit = reader.ReadUShort();
            var shortcuts_ = new Types.Shortcut[limit];
            for (int i = 0; i < limit; i++)
            {
                 shortcuts_[i] = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
                 shortcuts_[i].Deserialize(reader);
            }
            shortcuts = shortcuts_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + shortcuts.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}