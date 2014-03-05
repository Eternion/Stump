

// Generated on 03/05/2014 20:34:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionFinishedMessage : Message
    {
        public const uint Id = 1304;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int actionId;
        
        public StartupActionFinishedMessage()
        {
        }
        
        public StartupActionFinishedMessage(int actionId)
        {
            this.actionId = actionId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(actionId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}