

// Generated on 12/29/2014 21:14:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PortalInformation
    {
        public const short Id = 466;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short portalId;
        public short areaId;
        
        public PortalInformation()
        {
        }
        
        public PortalInformation(short portalId, short areaId)
        {
            this.portalId = portalId;
            this.areaId = areaId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(portalId);
            writer.WriteShort(areaId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            portalId = reader.ReadShort();
            if (portalId < 0)
                throw new Exception("Forbidden value on portalId = " + portalId + ", it doesn't respect the following condition : portalId < 0");
            areaId = reader.ReadShort();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short);
        }
        
    }
    
}