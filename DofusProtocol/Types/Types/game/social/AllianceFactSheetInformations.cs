

// Generated on 02/11/2015 10:21:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AllianceFactSheetInformations : AllianceInformations
    {
        public const short Id = 421;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int creationDate;
        
        public AllianceFactSheetInformations()
        {
        }
        
        public AllianceFactSheetInformations(int allianceId, string allianceTag, string allianceName, Types.GuildEmblem allianceEmblem, int creationDate)
         : base(allianceId, allianceTag, allianceName, allianceEmblem)
        {
            this.creationDate = creationDate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(creationDate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
        }
        
        
    }
    
}