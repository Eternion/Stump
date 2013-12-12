

// Generated on 12/12/2013 16:56:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextMoveMultipleElementsMessage : Message
    {
        public const uint Id = 254;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.EntityMovementInformations> movements;
        
        public GameContextMoveMultipleElementsMessage()
        {
        }
        
        public GameContextMoveMultipleElementsMessage(IEnumerable<Types.EntityMovementInformations> movements)
        {
            this.movements = movements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)movements.Count());
            foreach (var entry in movements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            movements = new Types.EntityMovementInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (movements as Types.EntityMovementInformations[])[i] = new Types.EntityMovementInformations();
                 (movements as Types.EntityMovementInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + movements.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}