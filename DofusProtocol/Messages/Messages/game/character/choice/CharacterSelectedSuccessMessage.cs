

// Generated on 10/26/2014 23:29:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectedSuccessMessage : Message
    {
        public const uint Id = 153;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterBaseInformations infos;
        public bool isCollectingStats;
        
        public CharacterSelectedSuccessMessage()
        {
        }
        
        public CharacterSelectedSuccessMessage(Types.CharacterBaseInformations infos, bool isCollectingStats)
        {
            this.infos = infos;
            this.isCollectingStats = isCollectingStats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            infos.Serialize(writer);
            writer.WriteBoolean(isCollectingStats);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            infos = new Types.CharacterBaseInformations();
            infos.Deserialize(reader);
            isCollectingStats = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return infos.GetSerializationSize() + sizeof(bool);
        }
        
    }
    
}