

// Generated on 04/19/2016 10:17:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SelectedServerDataMessage : Message
    {
        public const uint Id = 42;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short serverId;
        public string address;
        public ushort port;
        public bool canCreateNewCharacter;
        public IEnumerable<sbyte> ticket;
        
        public SelectedServerDataMessage()
        {
        }
        
        public SelectedServerDataMessage(short serverId, string address, ushort port, bool canCreateNewCharacter, IEnumerable<sbyte> ticket)
        {
            this.serverId = serverId;
            this.address = address;
            this.port = port;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.ticket = ticket;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(serverId);
            writer.WriteUTF(address);
            writer.WriteUShort(port);
            writer.WriteBoolean(canCreateNewCharacter);
            writer.WriteVarInt((int)ticket.Count());
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            serverId = reader.ReadVarShort();
            if (serverId < 0)
                throw new Exception("Forbidden value on serverId = " + serverId + ", it doesn't respect the following condition : serverId < 0");
            address = reader.ReadUTF();
            port = reader.ReadUShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            canCreateNewCharacter = reader.ReadBoolean();
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