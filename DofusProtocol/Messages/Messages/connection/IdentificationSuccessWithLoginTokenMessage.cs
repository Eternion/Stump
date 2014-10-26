

// Generated on 10/26/2014 23:29:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdentificationSuccessWithLoginTokenMessage : IdentificationSuccessMessage
    {
        public const uint Id = 6209;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string loginToken;
        
        public IdentificationSuccessWithLoginTokenMessage()
        {
        }
        
        public IdentificationSuccessWithLoginTokenMessage(string login, string nickname, int accountId, sbyte communityId, string secretQuestion, double accountCreation, double subscriptionElapsedDuration, double subscriptionEndDate, string loginToken)
         : base(login, nickname, accountId, communityId, secretQuestion, accountCreation, subscriptionElapsedDuration, subscriptionEndDate)
        {
            this.loginToken = loginToken;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(loginToken);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            loginToken = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(loginToken);
        }
        
    }
    
}