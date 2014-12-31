

// Generated on 12/29/2014 21:14:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var titles_before = writer.Position;
            var titles_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in titles)
            {
                 writer.WriteShort(entry);
                 titles_count++;
            }
            var titles_after = writer.Position;
            writer.Seek((int)titles_before);
            writer.WriteUShort((ushort)titles_count);
            writer.Seek((int)titles_after);

            var ornaments_before = writer.Position;
            var ornaments_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ornaments)
            {
                 writer.WriteShort(entry);
                 ornaments_count++;
            }
            var ornaments_after = writer.Position;
            writer.Seek((int)ornaments_before);
            writer.WriteUShort((ushort)ornaments_count);
            writer.Seek((int)ornaments_after);

            writer.WriteShort(activeTitle);
            writer.WriteShort(activeOrnament);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var titles_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 titles_[i] = reader.ReadShort();
            }
            titles = titles_;
            limit = reader.ReadUShort();
            var ornaments_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 ornaments_[i] = reader.ReadShort();
            }
            ornaments = ornaments_;
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