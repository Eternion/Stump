

// Generated on 10/26/2014 23:29:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdentificationSuccessMessage : Message
    {
        public const uint Id = 22;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string login;
        public string nickname;
        public int accountId;
        public sbyte communityId;
        public string secretQuestion;
        public double accountCreation;
        public double subscriptionElapsedDuration;
        public double subscriptionEndDate;
        
        public IdentificationSuccessMessage()
        {
        }
        
        public IdentificationSuccessMessage(string login, string nickname, int accountId, sbyte communityId, string secretQuestion, double accountCreation, double subscriptionElapsedDuration, double subscriptionEndDate)
        {
            this.login = login;
            this.nickname = nickname;
            this.accountId = accountId;
            this.communityId = communityId;
            this.secretQuestion = secretQuestion;
            this.accountCreation = accountCreation;
            this.subscriptionElapsedDuration = subscriptionElapsedDuration;
            this.subscriptionEndDate = subscriptionEndDate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(login);
            writer.WriteUTF(nickname);
            writer.WriteInt(accountId);
            writer.WriteSByte(communityId);
            writer.WriteUTF(secretQuestion);
            writer.WriteDouble(accountCreation);
            writer.WriteDouble(subscriptionElapsedDuration);
            writer.WriteDouble(subscriptionEndDate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            login = reader.ReadUTF();
            nickname = reader.ReadUTF();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            communityId = reader.ReadSByte();
            if (communityId < 0)
                throw new Exception("Forbidden value on communityId = " + communityId + ", it doesn't respect the following condition : communityId < 0");
            secretQuestion = reader.ReadUTF();
            accountCreation = reader.ReadDouble();
            if (accountCreation < 0 || accountCreation > 9.007199254740992E15)
                throw new Exception("Forbidden value on accountCreation = " + accountCreation + ", it doesn't respect the following condition : accountCreation < 0 || accountCreation > 9.007199254740992E15");
            subscriptionElapsedDuration = reader.ReadDouble();
            if (subscriptionElapsedDuration < 0 || subscriptionElapsedDuration > 9.007199254740992E15)
                throw new Exception("Forbidden value on subscriptionElapsedDuration = " + subscriptionElapsedDuration + ", it doesn't respect the following condition : subscriptionElapsedDuration < 0 || subscriptionElapsedDuration > 9.007199254740992E15");
            subscriptionEndDate = reader.ReadDouble();
            if (subscriptionEndDate < 0 || subscriptionEndDate > 9.007199254740992E15)
                throw new Exception("Forbidden value on subscriptionEndDate = " + subscriptionEndDate + ", it doesn't respect the following condition : subscriptionEndDate < 0 || subscriptionEndDate > 9.007199254740992E15");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(login) + sizeof(short) + Encoding.UTF8.GetByteCount(nickname) + sizeof(int) + sizeof(sbyte) + sizeof(short) + Encoding.UTF8.GetByteCount(secretQuestion) + sizeof(double) + sizeof(double) + sizeof(double);
        }
        
    }
    
}