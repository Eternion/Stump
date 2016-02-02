

// Generated on 02/02/2016 14:14:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceInvitedMessage : Message
    {
        public const uint Id = 6397;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long recruterId;
        public string recruterName;
        public Types.BasicNamedAllianceInformations allianceInfo;
        
        public AllianceInvitedMessage()
        {
        }
        
        public AllianceInvitedMessage(long recruterId, string recruterName, Types.BasicNamedAllianceInformations allianceInfo)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.allianceInfo = allianceInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(recruterId);
            writer.WriteUTF(recruterName);
            allianceInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            recruterId = reader.ReadVarLong();
            if (recruterId < 0 || recruterId > 9.007199254740992E15)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0 || recruterId > 9.007199254740992E15");
            recruterName = reader.ReadUTF();
            allianceInfo = new Types.BasicNamedAllianceInformations();
            allianceInfo.Deserialize(reader);
        }
        
    }
    
}