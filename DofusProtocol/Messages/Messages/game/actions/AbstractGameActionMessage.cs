

// Generated on 12/20/2015 16:36:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AbstractGameActionMessage : Message
    {
        public const uint Id = 1000;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short actionId;
        public double sourceId;
        
        public AbstractGameActionMessage()
        {
        }
        
        public AbstractGameActionMessage(short actionId, double sourceId)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(actionId);
            writer.WriteDouble(sourceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadDouble();
            if (sourceId < -9.007199254740992E15 || sourceId > 9.007199254740992E15)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < -9.007199254740992E15 || sourceId > 9.007199254740992E15");
        }
        
    }
    
}