

// Generated on 10/26/2014 23:29:17
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
        
        public int recruterId;
        public string recruterName;
        public Types.BasicNamedAllianceInformations allianceInfo;
        
        public AllianceInvitedMessage()
        {
        }
        
        public AllianceInvitedMessage(int recruterId, string recruterName, Types.BasicNamedAllianceInformations allianceInfo)
        {
            this.recruterId = recruterId;
            this.recruterName = recruterName;
            this.allianceInfo = allianceInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(recruterId);
            writer.WriteUTF(recruterName);
            allianceInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            recruterId = reader.ReadInt();
            if (recruterId < 0)
                throw new Exception("Forbidden value on recruterId = " + recruterId + ", it doesn't respect the following condition : recruterId < 0");
            recruterName = reader.ReadUTF();
            allianceInfo = new Types.BasicNamedAllianceInformations();
            allianceInfo.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(recruterName) + allianceInfo.GetSerializationSize();
        }
        
    }
    
}