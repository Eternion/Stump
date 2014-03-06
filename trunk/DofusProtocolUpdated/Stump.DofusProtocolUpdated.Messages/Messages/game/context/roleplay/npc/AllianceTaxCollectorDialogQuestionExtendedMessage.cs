

// Generated on 03/06/2014 18:50:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceTaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionExtendedMessage
    {
        public const uint Id = 6445;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.BasicNamedAllianceInformations alliance;
        
        public AllianceTaxCollectorDialogQuestionExtendedMessage()
        {
        }
        
        public AllianceTaxCollectorDialogQuestionExtendedMessage(Types.BasicGuildInformations guildInfo, short maxPods, short prospecting, short wisdom, sbyte taxCollectorsCount, int taxCollectorAttack, int kamas, double experience, int pods, int itemsValue, Types.BasicNamedAllianceInformations alliance)
         : base(guildInfo, maxPods, prospecting, wisdom, taxCollectorsCount, taxCollectorAttack, kamas, experience, pods, itemsValue)
        {
            this.alliance = alliance;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alliance.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alliance = new Types.BasicNamedAllianceInformations();
            alliance.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + alliance.GetSerializationSize();
        }
        
    }
    
}