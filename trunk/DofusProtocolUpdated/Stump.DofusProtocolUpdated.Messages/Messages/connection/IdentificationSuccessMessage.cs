

// Generated on 03/05/2014 20:34:17
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
        public double subscriptionEndDate;
        public double accountCreation;
        
        public IdentificationSuccessMessage()
        {
        }
        
        public IdentificationSuccessMessage(string login, string nickname, int accountId, sbyte communityId, string secretQuestion, double subscriptionEndDate, double accountCreation)
        {
            this.login = login;
            this.nickname = nickname;
            this.accountId = accountId;
            this.communityId = communityId;
            this.secretQuestion = secretQuestion;
            this.subscriptionEndDate = subscriptionEndDate;
            this.accountCreation = accountCreation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(login);
            writer.WriteUTF(nickname);
            writer.WriteInt(accountId);
            writer.WriteSByte(communityId);
            writer.WriteUTF(secretQuestion);
            writer.WriteDouble(subscriptionEndDate);
            writer.WriteDouble(accountCreation);
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
            subscriptionEndDate = reader.ReadDouble();
            if (subscriptionEndDate < 0)
                throw new Exception("Forbidden value on subscriptionEndDate = " + subscriptionEndDate + ", it doesn't respect the following condition : subscriptionEndDate < 0");
            accountCreation = reader.ReadDouble();
            if (accountCreation < 0)
                throw new Exception("Forbidden value on accountCreation = " + accountCreation + ", it doesn't respect the following condition : accountCreation < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(login) + sizeof(short) + Encoding.UTF8.GetByteCount(nickname) + sizeof(int) + sizeof(sbyte) + sizeof(short) + Encoding.UTF8.GetByteCount(secretQuestion) + sizeof(double) + sizeof(double);
        }
        
    }
    
}