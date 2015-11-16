

// Generated on 11/16/2015 14:26:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismSettingsRequestMessage : Message
    {
        public const uint Id = 6437;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public sbyte startDefenseTime;
        
        public PrismSettingsRequestMessage()
        {
        }
        
        public PrismSettingsRequestMessage(short subAreaId, sbyte startDefenseTime)
        {
            this.subAreaId = subAreaId;
            this.startDefenseTime = startDefenseTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(subAreaId);
            writer.WriteSByte(startDefenseTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            startDefenseTime = reader.ReadSByte();
            if (startDefenseTime < 0)
                throw new Exception("Forbidden value on startDefenseTime = " + startDefenseTime + ", it doesn't respect the following condition : startDefenseTime < 0");
        }
        
    }
    
}