

// Generated on 08/04/2015 00:35:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class IgnoredInformations : AbstractContactInformations
    {
        public const short Id = 106;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public IgnoredInformations()
        {
        }
        
        public IgnoredInformations(int accountId, string accountName)
         : base(accountId, accountName)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        
    }
    
}