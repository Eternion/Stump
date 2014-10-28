

// Generated on 10/28/2014 16:38:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class BasicAllianceInformations : AbstractSocialGroupInfos
    {
        public const short Id = 419;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int allianceId;
        public string allianceTag;
        
        public BasicAllianceInformations()
        {
        }
        
        public BasicAllianceInformations(int allianceId, string allianceTag)
        {
            this.allianceId = allianceId;
            this.allianceTag = allianceTag;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(allianceId);
            writer.WriteUTF(allianceTag);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceId = reader.ReadInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
            allianceTag = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(allianceTag);
        }
        
    }
    
}