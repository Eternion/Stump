
// Generated on 03/25/2013 19:24:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightTackledMessage : AbstractGameActionMessage
    {
        public const uint Id = 1004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> tacklersIds;
        
        public GameActionFightTackledMessage()
        {
        }
        
        public GameActionFightTackledMessage(short actionId, int sourceId, IEnumerable<int> tacklersIds)
         : base(actionId, sourceId)
        {
            this.tacklersIds = tacklersIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)tacklersIds.Count());
            foreach (var entry in tacklersIds)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            tacklersIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (tacklersIds as int[])[i] = reader.ReadInt();
            }
        }
        
    }
    
}