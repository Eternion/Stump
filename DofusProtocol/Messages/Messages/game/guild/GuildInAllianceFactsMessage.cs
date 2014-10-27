

// Generated on 10/27/2014 19:57:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInAllianceFactsMessage : GuildFactsMessage
    {
        public const uint Id = 6422;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.BasicNamedAllianceInformations allianceInfos;
        
        public GuildInAllianceFactsMessage()
        {
        }
        
        public GuildInAllianceFactsMessage(Types.GuildFactSheetInformations infos, int creationDate, short nbTaxCollectors, bool enabled, IEnumerable<Types.CharacterMinimalInformations> members, Types.BasicNamedAllianceInformations allianceInfos)
         : base(infos, creationDate, nbTaxCollectors, enabled, members)
        {
            this.allianceInfos = allianceInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceInfos = new Types.BasicNamedAllianceInformations();
            allianceInfos.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + allianceInfos.GetSerializationSize();
        }
        
    }
    
}