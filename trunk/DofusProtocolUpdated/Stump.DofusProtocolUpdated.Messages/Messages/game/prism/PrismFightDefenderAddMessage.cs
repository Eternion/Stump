

// Generated on 03/05/2014 20:34:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightDefenderAddMessage : Message
    {
        public const uint Id = 5895;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public double fightId;
        public Types.CharacterMinimalPlusLookInformations defender;
        
        public PrismFightDefenderAddMessage()
        {
        }
        
        public PrismFightDefenderAddMessage(short subAreaId, double fightId, Types.CharacterMinimalPlusLookInformations defender)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.defender = defender;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteDouble(fightId);
            writer.WriteShort(defender.TypeId);
            defender.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadDouble();
            defender = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
            defender.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(double) + sizeof(short) + defender.GetSerializationSize();
        }
        
    }
    
}