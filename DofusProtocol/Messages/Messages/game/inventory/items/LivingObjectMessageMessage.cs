

// Generated on 02/11/2015 10:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LivingObjectMessageMessage : Message
    {
        public const uint Id = 6065;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short msgId;
        public int timeStamp;
        public string owner;
        public short objectGenericId;
        
        public LivingObjectMessageMessage()
        {
        }
        
        public LivingObjectMessageMessage(short msgId, int timeStamp, string owner, short objectGenericId)
        {
            this.msgId = msgId;
            this.timeStamp = timeStamp;
            this.owner = owner;
            this.objectGenericId = objectGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(msgId);
            writer.WriteInt(timeStamp);
            writer.WriteUTF(owner);
            writer.WriteVarShort(objectGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            msgId = reader.ReadVarShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            timeStamp = reader.ReadInt();
            if (timeStamp < 0)
                throw new Exception("Forbidden value on timeStamp = " + timeStamp + ", it doesn't respect the following condition : timeStamp < 0");
            owner = reader.ReadUTF();
            objectGenericId = reader.ReadVarShort();
            if (objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
        }
        
    }
    
}