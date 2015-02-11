

// Generated on 02/11/2015 10:21:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntFlag
    {
        public const short Id = 473;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int mapId;
        public sbyte state;
        
        public TreasureHuntFlag()
        {
        }
        
        public TreasureHuntFlag(int mapId, sbyte state)
        {
            this.mapId = mapId;
            this.state = state;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteSByte(state);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
        
        
    }
    
}