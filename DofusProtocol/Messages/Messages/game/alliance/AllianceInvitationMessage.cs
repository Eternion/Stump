

// Generated on 10/26/2014 23:29:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceInvitationMessage : Message
    {
        public const uint Id = 6395;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public AllianceInvitationMessage()
        {
        }
        
        public AllianceInvitationMessage(int targetId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}