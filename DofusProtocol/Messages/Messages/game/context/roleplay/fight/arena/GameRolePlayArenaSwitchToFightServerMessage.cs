

// Generated on 08/04/2015 00:37:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaSwitchToFightServerMessage : Message
    {
        public const uint Id = 6575;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string address;
        public ushort port;
        public IEnumerable<sbyte> ticket;
        
        public GameRolePlayArenaSwitchToFightServerMessage()
        {
        }
        
        public GameRolePlayArenaSwitchToFightServerMessage(string address, ushort port, IEnumerable<sbyte> ticket)
        {
            this.address = address;
            this.port = port;
            this.ticket = ticket;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(address);
            writer.WriteUShort(port);
            var ticket_before = writer.Position;
            var ticket_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
                 ticket_count++;
            }
            var ticket_after = writer.Position;
            writer.Seek((int)ticket_before);
            writer.WriteUShort((ushort)ticket_count);
            writer.Seek((int)ticket_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            address = reader.ReadUTF();
            port = reader.ReadUShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            var limit = reader.ReadUShort();
            var ticket_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 ticket_[i] = reader.ReadSByte();
            }
            ticket = ticket_;
        }
        
    }
    
}