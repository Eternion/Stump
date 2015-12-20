

// Generated on 12/20/2015 16:36:50
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
        
        public IEnumerable<double> targetIds;
        public IEnumerable<short> targetCells;
        
        public ChallengeTargetsListMessage()
        {
        }
        
        public ChallengeTargetsListMessage(IEnumerable<double> targetIds, IEnumerable<short> targetCells)
        {
            this.targetIds = targetIds;
            this.targetCells = targetCells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var targetIds_before = writer.Position;
            var targetIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in targetIds)
            {
                 writer.WriteDouble(entry);
                 targetIds_count++;
            }
            var targetIds_after = writer.Position;
            writer.Seek((int)targetIds_before);
            writer.WriteUShort((ushort)targetIds_count);
            writer.Seek((int)targetIds_after);

            var targetCells_before = writer.Position;
            var targetCells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in targetCells)
            {
                 writer.WriteShort(entry);
                 targetCells_count++;
            }
            var targetCells_after = writer.Position;
            writer.Seek((int)targetCells_before);
            writer.WriteUShort((ushort)targetCells_count);
            writer.Seek((int)targetCells_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var targetIds_ = new double[limit];
            for (int i = 0; i < limit; i++)
            {
                 targetIds_[i] = reader.ReadDouble();
            }
            targetIds = targetIds_;
            limit = reader.ReadUShort();
            var targetCells_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 targetCells_[i] = reader.ReadShort();
            }
            targetCells = targetCells_;
        }
        
    }
    
}