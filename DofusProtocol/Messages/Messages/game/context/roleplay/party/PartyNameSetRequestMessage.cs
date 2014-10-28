

// Generated on 10/28/2014 16:36:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyNameSetRequestMessage : AbstractPartyMessage
    {
        public const uint Id = 6503;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string partyName;
        
        public PartyNameSetRequestMessage()
        {
        }
        
        public PartyNameSetRequestMessage(int partyId, string partyName)
         : base(partyId)
        {
            this.partyName = partyName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(partyName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyName = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(partyName);
        }
        
    }
    
}