

// Generated on 12/12/2013 16:56:43
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
        public IEnumerable<string> args;
        public IEnumerable<string> descriptions;
        
        public ConsoleCommandsListMessage()
        {
        }
        
        public ConsoleCommandsListMessage(IEnumerable<string> aliases, IEnumerable<string> args, IEnumerable<string> descriptions)
        {
            this.aliases = aliases;
            this.args = args;
            this.descriptions = descriptions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)aliases.Count());
            foreach (var entry in aliases)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)args.Count());
            foreach (var entry in args)
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
            args = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (args as string[])[i] = reader.ReadUTF();
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
            return sizeof(short) + aliases.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(short) + args.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x)) + sizeof(short) + descriptions.Sum(x => sizeof(short) + Encoding.UTF8.GetByteCount(x));
        }
        
    }
    
}