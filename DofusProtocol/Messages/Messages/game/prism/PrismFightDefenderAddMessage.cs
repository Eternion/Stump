

// Generated on 03/02/2014 20:42:55
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
        
        public double fightId;
        public Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
        public bool inMain;
        
        public PrismFightDefenderAddMessage()
        {
        }
        
        public PrismFightDefenderAddMessage(double fightId, Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations, bool inMain)
        {
            this.fightId = fightId;
            this.fighterMovementInformations = fighterMovementInformations;
            this.inMain = inMain;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            fighterMovementInformations.Serialize(writer);
            writer.WriteBoolean(inMain);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            fighterMovementInformations = new Types.CharacterMinimalPlusLookAndGradeInformations();
            fighterMovementInformations.Deserialize(reader);
            inMain = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + fighterMovementInformations.GetSerializationSize() + sizeof(bool);
        }
        
    }
    
}