

// Generated on 12/12/2013 16:57:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NewMailMessage : MailStatusMessage
    {
        public const uint Id = 6292;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> sendersAccountId;
        
        public NewMailMessage()
        {
        }
        
        public NewMailMessage(short unread, short total, IEnumerable<int> sendersAccountId)
         : base(unread, total)
        {
            this.sendersAccountId = sendersAccountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)sendersAccountId.Count());
            foreach (var entry in sendersAccountId)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            sendersAccountId = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (sendersAccountId as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sendersAccountId.Sum(x => sizeof(int));
        }
        
    }
    
}