

// Generated on 10/28/2014 16:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffect
    {
        public const short Id = 76;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short actionId;
        
        public ObjectEffect()
        {
        }
        
        public ObjectEffect(short actionId)
        {
            this.actionId = actionId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(actionId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}