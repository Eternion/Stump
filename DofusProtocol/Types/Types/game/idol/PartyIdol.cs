

// Generated on 09/01/2015 10:48:38
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
        
        public IEnumerable<int> ownersIds;
        
        public PartyIdol()
        {
        }
        
        public PartyIdol(short id, short xpBonusPercent, short dropBonusPercent, IEnumerable<int> ownersIds)
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
                 writer.WriteInt(entry);
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
            var ownersIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ownersIds_[i] = reader.ReadInt();
            }
            ownersIds = ownersIds_;
        }
        
        
    }
    
}