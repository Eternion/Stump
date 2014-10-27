

// Generated on 10/27/2014 19:58:01
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightDefendersSwapMessage : Message
    {
        public const uint Id = 5902;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public double fightId;
        public int fighterId1;
        public int fighterId2;
        
        public PrismFightDefendersSwapMessage()
        {
        }
        
        public PrismFightDefendersSwapMessage(short subAreaId, double fightId, int fighterId1, int fighterId2)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.fighterId1 = fighterId1;
            this.fighterId2 = fighterId2;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteDouble(fightId);
            writer.WriteInt(fighterId1);
            writer.WriteInt(fighterId2);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadDouble();
            if (fightId < -9.007199254740992E15 || fightId > 9.007199254740992E15)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < -9.007199254740992E15 || fightId > 9.007199254740992E15");
            fighterId1 = reader.ReadInt();
            if (fighterId1 < 0)
                throw new Exception("Forbidden value on fighterId1 = " + fighterId1 + ", it doesn't respect the following condition : fighterId1 < 0");
            fighterId2 = reader.ReadInt();
            if (fighterId2 < 0)
                throw new Exception("Forbidden value on fighterId2 = " + fighterId2 + ", it doesn't respect the following condition : fighterId2 < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(double) + sizeof(int) + sizeof(int);
        }
        
    }
    
}