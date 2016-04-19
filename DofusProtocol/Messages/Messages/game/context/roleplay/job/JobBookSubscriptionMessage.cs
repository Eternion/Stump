

// Generated on 04/19/2016 10:17:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobBookSubscriptionMessage : Message
    {
        public const uint Id = 6593;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobBookSubscription> subscriptions;
        
        public JobBookSubscriptionMessage()
        {
        }
        
        public JobBookSubscriptionMessage(IEnumerable<Types.JobBookSubscription> subscriptions)
        {
            this.subscriptions = subscriptions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var subscriptions_before = writer.Position;
            var subscriptions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in subscriptions)
            {
                 entry.Serialize(writer);
                 subscriptions_count++;
            }
            var subscriptions_after = writer.Position;
            writer.Seek((int)subscriptions_before);
            writer.WriteUShort((ushort)subscriptions_count);
            writer.Seek((int)subscriptions_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var subscriptions_ = new Types.JobBookSubscription[limit];
            for (int i = 0; i < limit; i++)
            {
                 subscriptions_[i] = new Types.JobBookSubscription();
                 subscriptions_[i].Deserialize(reader);
            }
            subscriptions = subscriptions_;
        }
        
    }
    
}