

// Generated on 07/29/2013 23:08:37
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TitlesAndOrnamentsListMessage : Message
    {
        public const uint Id = 6367;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> titles;
        public IEnumerable<short> ornaments;
        public short activeTitle;
        public short activeOrnament;
        
        public TitlesAndOrnamentsListMessage()
        {
        }
        
        public TitlesAndOrnamentsListMessage(IEnumerable<short> titles, IEnumerable<short> ornaments, short activeTitle, short activeOrnament)
        {
            this.titles = titles;
            this.ornaments = ornaments;
            this.activeTitle = activeTitle;
            this.activeOrnament = activeOrnament;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)titles.Count());
            foreach (var entry in titles)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)ornaments.Count());
            foreach (var entry in ornaments)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteShort(activeTitle);
            writer.WriteShort(activeOrnament);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            titles = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (titles as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            ornaments = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ornaments as short[])[i] = reader.ReadShort();
            }
            activeTitle = reader.ReadShort();
            if (activeTitle < 0)
                throw new Exception("Forbidden value on activeTitle = " + activeTitle + ", it doesn't respect the following condition : activeTitle < 0");
            activeOrnament = reader.ReadShort();
            if (activeOrnament < 0)
                throw new Exception("Forbidden value on activeOrnament = " + activeOrnament + ", it doesn't respect the following condition : activeOrnament < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + titles.Sum(x => sizeof(short)) + sizeof(short) + ornaments.Sum(x => sizeof(short)) + sizeof(short) + sizeof(short);
        }
        
    }
    
}