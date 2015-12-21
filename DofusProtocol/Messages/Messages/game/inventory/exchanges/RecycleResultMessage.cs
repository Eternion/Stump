

// Generated on 12/20/2015 16:37:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class RecycleResultMessage : Message
    {
        public const uint Id = 6601;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int nuggetsForPrism;
        public int nuggetsForPlayer;
        
        public RecycleResultMessage()
        {
        }
        
        public RecycleResultMessage(int nuggetsForPrism, int nuggetsForPlayer)
        {
            this.nuggetsForPrism = nuggetsForPrism;
            this.nuggetsForPlayer = nuggetsForPlayer;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(nuggetsForPrism);
            writer.WriteVarInt(nuggetsForPlayer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nuggetsForPrism = reader.ReadVarInt();
            if (nuggetsForPrism < 0)
                throw new Exception("Forbidden value on nuggetsForPrism = " + nuggetsForPrism + ", it doesn't respect the following condition : nuggetsForPrism < 0");
            nuggetsForPlayer = reader.ReadVarInt();
            if (nuggetsForPlayer < 0)
                throw new Exception("Forbidden value on nuggetsForPlayer = " + nuggetsForPlayer + ", it doesn't respect the following condition : nuggetsForPlayer < 0");
        }
        
    }
    
}