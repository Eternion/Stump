

// Generated on 09/01/2015 10:48:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StatsUpgradeResultMessage : Message
    {
        public const uint Id = 5609;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte result;
        public short nbCharacBoost;
        
        public StatsUpgradeResultMessage()
        {
        }
        
        public StatsUpgradeResultMessage(sbyte result, short nbCharacBoost)
        {
            this.result = result;
            this.nbCharacBoost = nbCharacBoost;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(result);
            writer.WriteVarShort(nbCharacBoost);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            result = reader.ReadSByte();
            nbCharacBoost = reader.ReadVarShort();
            if (nbCharacBoost < 0)
                throw new Exception("Forbidden value on nbCharacBoost = " + nbCharacBoost + ", it doesn't respect the following condition : nbCharacBoost < 0");
        }
        
    }
    
}