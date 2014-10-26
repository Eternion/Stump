

// Generated on 10/26/2014 23:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightAttackerAddMessage : Message
    {
        public const uint Id = 5893;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public double fightId;
        public Types.CharacterMinimalPlusLookInformations attacker;
        
        public PrismFightAttackerAddMessage()
        {
        }
        
        public PrismFightAttackerAddMessage(short subAreaId, double fightId, Types.CharacterMinimalPlusLookInformations attacker)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.attacker = attacker;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteDouble(fightId);
            writer.WriteShort(attacker.TypeId);
            attacker.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadDouble();
            if (fightId < -9.007199254740992E15 || fightId > 9.007199254740992E15)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < -9.007199254740992E15 || fightId > 9.007199254740992E15");
            attacker = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
            attacker.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(double) + sizeof(short) + attacker.GetSerializationSize();
        }
        
    }
    
}