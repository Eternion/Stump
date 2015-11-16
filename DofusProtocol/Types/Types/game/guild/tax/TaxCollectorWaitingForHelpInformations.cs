

// Generated on 11/16/2015 14:20:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorWaitingForHelpInformations : TaxCollectorComplementaryInformations
    {
        public const short Id = 447;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        
        public TaxCollectorWaitingForHelpInformations()
        {
        }
        
        public TaxCollectorWaitingForHelpInformations(Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            waitingForHelpInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
        }
        
        
    }
    
}