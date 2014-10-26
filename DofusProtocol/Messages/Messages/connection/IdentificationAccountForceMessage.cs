

// Generated on 10/26/2014 23:29:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdentificationAccountForceMessage : IdentificationMessage
    {
        public const uint Id = 6119;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public IdentificationAccountForceMessage()
        {
        }
        
        public IdentificationAccountForceMessage(Types.VersionExtended version, string lang, IEnumerable<sbyte> credentials, short serverId, double sessionOptionalSalt)
         : base(version, lang, credentials, serverId, sessionOptionalSalt)
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
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}