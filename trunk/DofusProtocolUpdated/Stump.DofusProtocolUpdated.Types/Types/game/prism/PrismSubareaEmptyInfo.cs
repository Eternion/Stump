

// Generated on 12/12/2013 16:57:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PrismSubareaEmptyInfo
    {
        public const short Id = 438;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public int allianceId;
        
        public PrismSubareaEmptyInfo()
        {
        }
        
        public PrismSubareaEmptyInfo(short subAreaId, int allianceId)
        {
            this.subAreaId = subAreaId;
            this.allianceId = allianceId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteInt(allianceId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            allianceId = reader.ReadInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int);
        }
        
    }
    
}