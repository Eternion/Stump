

// Generated on 10/30/2016 16:20:49
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
        
        public long reportedId;
        public sbyte reason;
        
        public CharacterReportMessage()
        {
        }
        
        public CharacterReportMessage(long reportedId, sbyte reason)
        {
            this.reportedId = reportedId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(reportedId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reportedId = reader.ReadVarLong();
            if (reportedId < 0 || reportedId > 9007199254740990)
                throw new Exception("Forbidden value on reportedId = " + reportedId + ", it doesn't respect the following condition : reportedId < 0 || reportedId > 9007199254740990");
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}