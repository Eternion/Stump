

// Generated on 12/12/2013 16:56:44
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
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(nickname);
        }
        
    }
    
}