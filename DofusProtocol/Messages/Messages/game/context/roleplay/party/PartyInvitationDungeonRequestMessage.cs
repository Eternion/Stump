

// Generated on 12/29/2014 21:12:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationDungeonRequestMessage : PartyInvitationRequestMessage
    {
        public const uint Id = 6245;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        
        public PartyInvitationDungeonRequestMessage()
        {
        }
        
        public PartyInvitationDungeonRequestMessage(string name, short dungeonId)
         : base(name)
        {
            this.dungeonId = dungeonId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(dungeonId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}