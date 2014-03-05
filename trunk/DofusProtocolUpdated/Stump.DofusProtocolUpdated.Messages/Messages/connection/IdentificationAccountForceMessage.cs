

// Generated on 03/05/2014 20:34:17
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
        
        public string forcedAccountLogin;
        
        public IdentificationAccountForceMessage()
        {
        }
        
        public IdentificationAccountForceMessage(Types.VersionExtended version, string lang, IEnumerable<sbyte> credentials, short serverId, double sessionOptionalSalt, string forcedAccountLogin)
         : base(version, lang, credentials, serverId, sessionOptionalSalt)
        {
            this.forcedAccountLogin = forcedAccountLogin;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(forcedAccountLogin);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            forcedAccountLogin = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(forcedAccountLogin);
        }
        
    }
    
}