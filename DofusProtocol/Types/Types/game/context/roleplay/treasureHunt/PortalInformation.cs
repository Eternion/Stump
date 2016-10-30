

// Generated on 10/30/2016 16:20:56
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
        
        public int portalId;
        public short areaId;
        
        public PortalInformation()
        {
        }
        
        public PortalInformation(int portalId, short areaId)
        {
            this.portalId = portalId;
            this.areaId = areaId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(portalId);
            writer.WriteShort(areaId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            portalId = reader.ReadInt();
            areaId = reader.ReadShort();
        }
        
        
    }
    
}