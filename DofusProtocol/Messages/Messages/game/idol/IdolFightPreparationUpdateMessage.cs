

// Generated on 08/04/2015 13:25:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolFightPreparationUpdateMessage : Message
    {
        public const uint Id = 6586;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte idolSource;
        public IEnumerable<Types.Idol> idols;
        
        public IdolFightPreparationUpdateMessage()
        {
        }
        
        public IdolFightPreparationUpdateMessage(sbyte idolSource, IEnumerable<Types.Idol> idols)
        {
            this.idolSource = idolSource;
            this.idols = idols;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(idolSource);
            var idols_before = writer.Position;
            var idols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in idols)
            {
                 writer.WriteShort(entry.TypeId);
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
            idolSource = reader.ReadSByte();
            if (idolSource < 0)
                throw new Exception("Forbidden value on idolSource = " + idolSource + ", it doesn't respect the following condition : idolSource < 0");
            var limit = reader.ReadUShort();
            var idols_ = new Types.Idol[limit];
            for (int i = 0; i < limit; i++)
            {
                 idols_[i] = Types.ProtocolTypeManager.GetInstance<Types.Idol>(reader.ReadShort());
                 idols_[i].Deserialize(reader);
            }
            idols = idols_;
        }
        
    }
    
}