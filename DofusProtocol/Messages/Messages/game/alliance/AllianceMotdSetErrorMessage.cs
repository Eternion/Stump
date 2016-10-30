

// Generated on 10/30/2016 16:20:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceMotdSetErrorMessage : SocialNoticeSetErrorMessage
    {
        public const uint Id = 6683;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceMotdSetErrorMessage()
        {
        }
        
        public AllianceMotdSetErrorMessage(sbyte reason)
         : base(reason)
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