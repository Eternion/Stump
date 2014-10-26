

// Generated on 10/26/2014 23:29:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectsRemovedMessage : ExchangeObjectMessage
    {
        public const uint Id = 6532;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> objectUID;
        
        public ExchangeObjectsRemovedMessage()
        {
        }
        
        public ExchangeObjectsRemovedMessage(bool remote, IEnumerable<int> objectUID)
         : base(remote)
        {
            this.objectUID = objectUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var objectUID_before = writer.Position;
            var objectUID_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectUID)
            {
                 writer.WriteInt(entry);
                 objectUID_count++;
            }
            var objectUID_after = writer.Position;
            writer.Seek((int)objectUID_before);
            writer.WriteUShort((ushort)objectUID_count);
            writer.Seek((int)objectUID_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var objectUID_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectUID_[i] = reader.ReadInt();
            }
            objectUID = objectUID_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + objectUID.Sum(x => sizeof(int));
        }
        
    }
    
}