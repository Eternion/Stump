

// Generated on 02/02/2016 14:14:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AdditionalTaxCollectorInformations
    {
        public const short Id = 165;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public string collectorCallerName;
        public int date;
        
        public AdditionalTaxCollectorInformations()
        {
        }
        
        public AdditionalTaxCollectorInformations(string collectorCallerName, int date)
        {
            this.collectorCallerName = collectorCallerName;
            this.date = date;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(collectorCallerName);
            writer.WriteInt(date);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            collectorCallerName = reader.ReadUTF();
            date = reader.ReadInt();
            if (date < 0)
                throw new Exception("Forbidden value on date = " + date + ", it doesn't respect the following condition : date < 0");
        }
        
        
    }
    
}