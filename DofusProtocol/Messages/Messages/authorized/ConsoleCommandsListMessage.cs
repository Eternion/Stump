

// Generated on 03/02/2014 20:42:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ConsoleCommandsListMessage : Message
    {
        public const uint Id = 6127;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<string> aliases;
        public IEnumerable<string> arguments;
        public IEnumerable<string> descriptions;
        
        public ConsoleCommandsListMessage()
        {
        }
        
        public ConsoleCommandsListMessage(IEnumerable<string> aliases, IEnumerable<string> arguments, IEnumerable<string> descriptions)
        {
            this.aliases = aliases;
            this.arguments = arguments;
            this.descriptions = descriptions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var aliases_before = writer.Position;
            var aliases_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in aliases)
            {
                 writer.WriteUTF(entry);
                 aliases_count++;
            }
            var aliases_after = writer.Position;
            writer.Seek((int)aliases_before);
            writer.WriteUShort((ushort)aliases_count);
            writer.Seek((int)aliases_after);

            var arguments_before = writer.Position;
            var arguments_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in arguments)
            {
                 writer.WriteUTF(entry);
                 arguments_count++;
            }
            var arguments_after = writer.Position;
            writer.Seek((int)arguments_before);
            writer.WriteUShort((ushort)arguments_count);
            writer.Seek((int)arguments_after);

            var descriptions_before = writer.Position;
            var descriptions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in descriptions)
            {
                 writer.WriteUTF(entry);
                 descriptions_count++;
            }
            var descriptions_after = writer.Position;
            writer.Seek((int)descriptions_before);
            writer.WriteUShort((ushort)descriptions_count);
            writer.Seek((int)descriptions_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var aliases_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 aliases_[i] = reader.ReadUTF();
            }
            aliases = aliases_;
            limit = reader.ReadUShort();
            var arguments_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 arguments_[i] = reader.ReadUTF();
            }
            arguments = arguments_;
            limit = reader.ReadUShort();
            var descriptions_ = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 descriptions_[i] = reader.ReadUTF();
            }
            descriptions = descriptions_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + aliases.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(short) + arguments.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(short) + descriptions.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x));
        }
        
    }
    
}