

// Generated on 08/11/2013 11:28:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeTargetsListMessage : Message
    {
        public const uint Id = 5613;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> targetIds;
        public IEnumerable<short> targetCells;
        
        public ChallengeTargetsListMessage()
        {
        }
        
        public ChallengeTargetsListMessage(IEnumerable<int> targetIds, IEnumerable<short> targetCells)
        {
            this.targetIds = targetIds;
            this.targetCells = targetCells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)targetIds.Count());
            foreach (var entry in targetIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)targetCells.Count());
            foreach (var entry in targetCells)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            targetIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (targetIds as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            targetCells = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (targetCells as short[])[i] = reader.ReadShort();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + targetIds.Sum(x => sizeof(int)) + sizeof(short) + targetCells.Sum(x => sizeof(short));
        }
        
    }
    
}