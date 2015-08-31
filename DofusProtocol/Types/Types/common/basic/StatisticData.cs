

// Generated on 08/04/2015 00:35:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticData
    {
        public const short Id = 484;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short actionId;
        
        public StatisticData()
        {
        }
        
        public StatisticData(short actionId)
        {
            this.actionId = actionId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(actionId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
        }
        
        
    }
    
}