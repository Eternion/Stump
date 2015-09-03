

// Generated on 09/01/2015 10:48:00
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
            var ticket_before = writer.Position;
            var ticket_count = 0;
            writer.WriteVarInt(0);
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
                 ticket_count++;
            }
            var credentials_after = writer.Position;
            writer.Seek((int)ticket_before);
            writer.WriteVarInt((int)ticket_count);
            writer.Seek((int)credentials_after);
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