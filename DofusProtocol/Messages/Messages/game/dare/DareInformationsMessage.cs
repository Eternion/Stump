

// Generated on 10/30/2016 16:20:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareInformationsMessage : Message
    {
        public const uint Id = 6656;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.DareInformations dareFixedInfos;
        public Types.DareVersatileInformations dareVersatilesInfos;
        
        public DareInformationsMessage()
        {
        }
        
        public DareInformationsMessage(Types.DareInformations dareFixedInfos, Types.DareVersatileInformations dareVersatilesInfos)
        {
            this.dareFixedInfos = dareFixedInfos;
            this.dareVersatilesInfos = dareVersatilesInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            dareFixedInfos.Serialize(writer);
            dareVersatilesInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dareFixedInfos = new Types.DareInformations();
            dareFixedInfos.Deserialize(reader);
            dareVersatilesInfos = new Types.DareVersatileInformations();
            dareVersatilesInfos.Deserialize(reader);
        }
        
    }
    
}