

// Generated on 10/30/2016 16:20:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceBulletinSetErrorMessage : SocialNoticeSetErrorMessage
    {
        public const uint Id = 6692;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceBulletinSetErrorMessage()
        {
        }
        
        public AllianceBulletinSetErrorMessage(sbyte reason)
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