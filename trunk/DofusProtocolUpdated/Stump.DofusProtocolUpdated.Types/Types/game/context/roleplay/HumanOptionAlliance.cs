

// Generated on 12/12/2013 16:57:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOptionAlliance : HumanOption
    {
        public const short Id = 425;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.AllianceInformations allianceInformations;
        public sbyte aggressable;
        
        public HumanOptionAlliance()
        {
        }
        
        public HumanOptionAlliance(Types.AllianceInformations allianceInformations, sbyte aggressable)
        {
            this.allianceInformations = allianceInformations;
            this.aggressable = aggressable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceInformations.Serialize(writer);
            writer.WriteSByte(aggressable);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceInformations = new Types.AllianceInformations();
            allianceInformations.Deserialize(reader);
            aggressable = reader.ReadSByte();
            if (aggressable < 0)
                throw new Exception("Forbidden value on aggressable = " + aggressable + ", it doesn't respect the following condition : aggressable < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + allianceInformations.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}