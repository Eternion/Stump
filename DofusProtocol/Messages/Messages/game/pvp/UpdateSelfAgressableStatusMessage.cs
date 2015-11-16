

// Generated on 11/16/2015 14:26:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class UpdateSelfAgressableStatusMessage : Message
    {
        public const uint Id = 6456;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte status;
        public int probationTime;
        
        public UpdateSelfAgressableStatusMessage()
        {
        }
        
        public UpdateSelfAgressableStatusMessage(sbyte status, int probationTime)
        {
            this.status = status;
            this.probationTime = probationTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(status);
            writer.WriteInt(probationTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            probationTime = reader.ReadInt();
            if (probationTime < 0)
                throw new Exception("Forbidden value on probationTime = " + probationTime + ", it doesn't respect the following condition : probationTime < 0");
        }
        
    }
    
}