

// Generated on 09/26/2016 01:50:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorInformations
    {
        public const short Id = 167;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int uniqueId;
        public short firtNameId;
        public short lastNameId;
        public Types.AdditionalTaxCollectorInformations additionalInfos;
        public short worldX;
        public short worldY;
        public short subAreaId;
        public sbyte state;
        public Types.EntityLook look;
        public IEnumerable<Types.TaxCollectorComplementaryInformations> complements;
        
        public TaxCollectorInformations()
        {
        }
        
        public TaxCollectorInformations(int uniqueId, short firtNameId, short lastNameId, Types.AdditionalTaxCollectorInformations additionalInfos, short worldX, short worldY, short subAreaId, sbyte state, Types.EntityLook look, IEnumerable<Types.TaxCollectorComplementaryInformations> complements)
        {
            this.uniqueId = uniqueId;
            this.firtNameId = firtNameId;
            this.lastNameId = lastNameId;
            this.additionalInfos = additionalInfos;
            this.worldX = worldX;
            this.worldY = worldY;
            this.subAreaId = subAreaId;
            this.state = state;
            this.look = look;
            this.complements = complements;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(uniqueId);
            writer.WriteVarShort(firtNameId);
            writer.WriteVarShort(lastNameId);
            additionalInfos.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteVarShort(subAreaId);
            writer.WriteSByte(state);
            look.Serialize(writer);
            var complements_before = writer.Position;
            var complements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in complements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 complements_count++;
            }
            var complements_after = writer.Position;
            writer.Seek((int)complements_before);
            writer.WriteUShort((ushort)complements_count);
            writer.Seek((int)complements_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            uniqueId = reader.ReadInt();
            firtNameId = reader.ReadVarShort();
            if (firtNameId < 0)
                throw new Exception("Forbidden value on firtNameId = " + firtNameId + ", it doesn't respect the following condition : firtNameId < 0");
            lastNameId = reader.ReadVarShort();
            if (lastNameId < 0)
                throw new Exception("Forbidden value on lastNameId = " + lastNameId + ", it doesn't respect the following condition : lastNameId < 0");
            additionalInfos = new Types.AdditionalTaxCollectorInformations();
            additionalInfos.Deserialize(reader);
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
            look = new Types.EntityLook();
            look.Deserialize(reader);
            var limit = reader.ReadUShort();
            var complements_ = new Types.TaxCollectorComplementaryInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 complements_[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorComplementaryInformations>(reader.ReadShort());
                 complements_[i].Deserialize(reader);
            }
            complements = complements_;
        }
        
        
    }
    
}