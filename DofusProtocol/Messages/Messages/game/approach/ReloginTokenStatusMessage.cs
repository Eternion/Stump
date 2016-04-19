

// Generated on 04/19/2016 10:17:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ReloginTokenStatusMessage : Message
    {
        public const uint Id = 6539;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool validToken;
        public IEnumerable<sbyte> ticket;
        
        public ReloginTokenStatusMessage()
        {
        }
        
        public ReloginTokenStatusMessage(bool validToken, IEnumerable<sbyte> ticket)
        {
            this.validToken = validToken;
            this.ticket = ticket;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(validToken);
            writer.WriteVarInt((int)ticket.Count());
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            validToken = reader.ReadBoolean();
            var limit = reader.ReadVarInt();
            var ticket_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 ticket_[i] = reader.ReadSByte();
            }
            ticket = ticket_;
        }
        
    }
    
}