

// Generated on 09/26/2016 01:50:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyIdol : Idol
    {
        public const short Id = 490;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<long> ownersIds;
        
        public PartyIdol()
        {
        }
        
        public PartyIdol(short id, short xpBonusPercent, short dropBonusPercent, IEnumerable<long> ownersIds)
         : base(id, xpBonusPercent, dropBonusPercent)
        {
            this.ownersIds = ownersIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var ownersIds_before = writer.Position;
            var ownersIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ownersIds)
            {
                 writer.WriteVarLong(entry);
                 ownersIds_count++;
            }
            var ownersIds_after = writer.Position;
            writer.Seek((int)ownersIds_before);
            writer.WriteUShort((ushort)ownersIds_count);
            writer.Seek((int)ownersIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var ownersIds_ = new long[limit];
            for (int i = 0; i < limit; i++)
            {
                 ownersIds_[i] = reader.ReadVarLong();
            }
            ownersIds = ownersIds_;
        }
        
        
    }
    
}