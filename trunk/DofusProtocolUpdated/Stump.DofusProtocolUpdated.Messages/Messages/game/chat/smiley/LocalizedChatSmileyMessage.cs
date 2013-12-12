

// Generated on 12/12/2013 16:56:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LocalizedChatSmileyMessage : ChatSmileyMessage
    {
        public const uint Id = 6185;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cellId;
        
        public LocalizedChatSmileyMessage()
        {
        }
        
        public LocalizedChatSmileyMessage(int entityId, sbyte smileyId, int accountId, short cellId)
         : base(entityId, smileyId, accountId)
        {
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}