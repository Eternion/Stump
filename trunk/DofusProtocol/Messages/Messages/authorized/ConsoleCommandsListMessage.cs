

// Generated on 07/26/2013 22:50:48
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)aliases.Count());
            foreach (var entry in aliases)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)arguments.Count());
            foreach (var entry in arguments)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)descriptions.Count());
            foreach (var entry in descriptions)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            aliases = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (aliases as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            arguments = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (arguments as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            descriptions = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (descriptions as string[])[i] = reader.ReadUTF();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + aliases.Sum(x => sizeof(short) + x.Length) + sizeof(short) + arguments.Sum(x => sizeof(short) + x.Length) + sizeof(short) + descriptions.Sum(x => sizeof(short) + x.Length);
        }
        
    }
    
}