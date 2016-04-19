

// Generated on 04/19/2016 10:17:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NicknameChoiceRequestMessage : Message
    {
        public const uint Id = 5639;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string nickname;
        
        public NicknameChoiceRequestMessage()
        {
        }
        
        public NicknameChoiceRequestMessage(string nickname)
        {
            this.nickname = nickname;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(nickname);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nickname = reader.ReadUTF();
        }
        
    }
    
}