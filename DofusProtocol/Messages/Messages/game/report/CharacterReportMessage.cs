

// Generated on 02/18/2015 10:46:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterReportMessage : Message
    {
        public const uint Id = 6079;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int reportedId;
        public sbyte reason;
        
        public CharacterReportMessage()
        {
        }
        
        public CharacterReportMessage(int reportedId, sbyte reason)
        {
            this.reportedId = reportedId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(reportedId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reportedId = reader.ReadVarInt();
            if (reportedId < 0)
                throw new Exception("Forbidden value on reportedId = " + reportedId + ", it doesn't respect the following condition : reportedId < 0");
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}