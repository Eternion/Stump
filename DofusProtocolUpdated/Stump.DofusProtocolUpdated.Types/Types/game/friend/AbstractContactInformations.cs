

// Generated on 03/06/2014 18:50:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AbstractContactInformations
    {
        public const short Id = 380;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int accountId;
        public string accountName;
        
        public AbstractContactInformations()
        {
        }
        
        public AbstractContactInformations(int accountId, string accountName)
        {
            this.accountId = accountId;
            this.accountName = accountName;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteUTF(accountName);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            accountName = reader.ReadUTF();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(accountName);
        }
        
    }
    
}