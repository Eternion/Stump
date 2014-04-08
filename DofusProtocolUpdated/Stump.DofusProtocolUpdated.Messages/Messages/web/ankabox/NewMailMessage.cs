

// Generated on 03/06/2014 18:50:29
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
            var sendersAccountId_before = writer.Position;
            var sendersAccountId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in sendersAccountId)
            {
                 writer.WriteInt(entry);
                 sendersAccountId_count++;
            }
            var sendersAccountId_after = writer.Position;
            writer.Seek((int)sendersAccountId_before);
            writer.WriteUShort((ushort)sendersAccountId_count);
            writer.Seek((int)sendersAccountId_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var sendersAccountId_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 sendersAccountId_[i] = reader.ReadInt();
            }
            sendersAccountId = sendersAccountId_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sendersAccountId.Sum(x => sizeof(int));
        }
        
    }
    
}