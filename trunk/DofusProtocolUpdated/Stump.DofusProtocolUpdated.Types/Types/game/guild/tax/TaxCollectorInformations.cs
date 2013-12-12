

// Generated on 12/12/2013 16:57:34
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
            writer.WriteShort(firtNameId);
            writer.WriteShort(lastNameId);
            additionalInfos.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteShort(subAreaId);
            writer.WriteSByte(state);
            look.Serialize(writer);
            writer.WriteUShort((ushort)complements.Count());
            foreach (var entry in complements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            uniqueId = reader.ReadInt();
            firtNameId = reader.ReadShort();
            if (firtNameId < 0)
                throw new Exception("Forbidden value on firtNameId = " + firtNameId + ", it doesn't respect the following condition : firtNameId < 0");
            lastNameId = reader.ReadShort();
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
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            state = reader.ReadSByte();
            look = new Types.EntityLook();
            look.Deserialize(reader);
            var limit = reader.ReadUShort();
            complements = new Types.TaxCollectorComplementaryInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (complements as Types.TaxCollectorComplementaryInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorComplementaryInformations>(reader.ReadShort());
                 (complements as Types.TaxCollectorComplementaryInformations[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + sizeof(short) + additionalInfos.GetSerializationSize() + sizeof(short) + sizeof(short) + sizeof(short) + sizeof(sbyte) + look.GetSerializationSize() + sizeof(short) + complements.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}