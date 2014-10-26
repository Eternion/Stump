

// Generated on 10/26/2014 23:29:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseGenericItemRemovedMessage : Message
    {
        public const uint Id = 5948;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objGenericId;
        
        public ExchangeBidHouseGenericItemRemovedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemRemovedMessage(int objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objGenericId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}