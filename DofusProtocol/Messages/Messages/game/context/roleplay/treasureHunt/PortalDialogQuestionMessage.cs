

// Generated on 09/01/2014 15:52:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PortalDialogQuestionMessage : Message
    {
        public const uint Id = 6495;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int availableUseLeft;
        public int closeDate;
        
        public PortalDialogQuestionMessage()
        {
        }
        
        public PortalDialogQuestionMessage(int availableUseLeft, int closeDate)
        {
            this.availableUseLeft = availableUseLeft;
            this.closeDate = closeDate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(availableUseLeft);
            writer.WriteInt(closeDate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            availableUseLeft = reader.ReadInt();
            if (availableUseLeft < 0)
                throw new Exception("Forbidden value on availableUseLeft = " + availableUseLeft + ", it doesn't respect the following condition : availableUseLeft < 0");
            closeDate = reader.ReadInt();
            if (closeDate < 0)
                throw new Exception("Forbidden value on closeDate = " + closeDate + ", it doesn't respect the following condition : closeDate < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int);
        }
        
    }
    
}