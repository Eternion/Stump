
// Generated on 03/25/2013 19:24:21
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkJobIndexMessage : Message
    {
        public const uint Id = 5819;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> jobs;
        
        public ExchangeStartOkJobIndexMessage()
        {
        }
        
        public ExchangeStartOkJobIndexMessage(IEnumerable<int> jobs)
        {
            this.jobs = jobs;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)jobs.Count());
            foreach (var entry in jobs)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            jobs = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (jobs as int[])[i] = reader.ReadInt();
            }
        }
        
    }
    
}