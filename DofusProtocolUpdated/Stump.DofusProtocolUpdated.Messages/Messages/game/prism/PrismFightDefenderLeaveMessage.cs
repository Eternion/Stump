

// Generated on 03/06/2014 18:50:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightDefenderLeaveMessage : Message
    {
        public const uint Id = 5892;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public double fightId;
        public int fighterToRemoveId;
        
        public PrismFightDefenderLeaveMessage()
        {
        }
        
        public PrismFightDefenderLeaveMessage(short subAreaId, double fightId, int fighterToRemoveId)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteDouble(fightId);
            writer.WriteInt(fighterToRemoveId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadDouble();
            fighterToRemoveId = reader.ReadInt();
            if (fighterToRemoveId < 0)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(double) + sizeof(int);
        }
        
    }
    
}