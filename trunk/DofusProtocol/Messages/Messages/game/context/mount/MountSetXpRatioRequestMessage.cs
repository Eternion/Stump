
// Generated on 01/04/2013 14:35:47
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountSetXpRatioRequestMessage : Message
    {
        public const uint Id = 5989;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte xpRatio;
        
        public MountSetXpRatioRequestMessage()
        {
        }
        
        public MountSetXpRatioRequestMessage(sbyte xpRatio)
        {
            this.xpRatio = xpRatio;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(xpRatio);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            xpRatio = reader.ReadSByte();
            if (xpRatio < 0)
                throw new Exception("Forbidden value on xpRatio = " + xpRatio + ", it doesn't respect the following condition : xpRatio < 0");
        }
        
    }
    
}