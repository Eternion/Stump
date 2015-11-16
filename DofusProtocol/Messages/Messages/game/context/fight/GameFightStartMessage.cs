

// Generated on 11/16/2015 14:26:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightStartMessage : Message
    {
        public const uint Id = 712;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.Idol> idols;
        
        public GameFightStartMessage()
        {
        }
        
        public GameFightStartMessage(IEnumerable<Types.Idol> idols)
        {
            this.idols = idols;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var idols_before = writer.Position;
            var idols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in idols)
            {
                 entry.Serialize(writer);
                 idols_count++;
            }
            var idols_after = writer.Position;
            writer.Seek((int)idols_before);
            writer.WriteUShort((ushort)idols_count);
            writer.Seek((int)idols_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var idols_ = new Types.Idol[limit];
            for (int i = 0; i < limit; i++)
            {
                 idols_[i] = new Types.Idol();
                 idols_[i].Deserialize(reader);
            }
            idols = idols_;
        }
        
    }
    
}