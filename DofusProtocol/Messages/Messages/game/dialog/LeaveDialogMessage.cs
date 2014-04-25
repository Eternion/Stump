

// Generated on 03/02/2014 20:42:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LeaveDialogMessage : Message
    {
        public const uint Id = 5502;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte dialogType;
        
        public LeaveDialogMessage()
        {
        }
        
        public LeaveDialogMessage(sbyte dialogType)
        {
            this.dialogType = dialogType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(dialogType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dialogType = reader.ReadSByte();
            if (dialogType < 0)
                throw new Exception("Forbidden value on dialogType = " + dialogType + ", it doesn't respect the following condition : dialogType < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}