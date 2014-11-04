

// Generated on 10/28/2014 16:36:32
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
        
        public bool ssl;
        public bool canCreateNewCharacter;
        public short serverId;
        public string address;
        public ushort port;
        public string ticket;
        
        public SelectedServerDataMessage()
        {
        }
        
        public SelectedServerDataMessage(bool ssl, bool canCreateNewCharacter, short serverId, string address, ushort port, string ticket)
        {
            this.ssl = ssl;
            this.canCreateNewCharacter = canCreateNewCharacter;
            this.serverId = serverId;
            this.address = address;
            this.port = port;
            this.ticket = ticket;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, ssl);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canCreateNewCharacter);
            writer.WriteByte(flag1);
            writer.WriteShort(serverId);
            writer.WriteUTF(address);
            writer.WriteUShort(port);
            writer.WriteUTF(ticket);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            ssl = BooleanByteWrapper.GetFlag(flag1, 0);
            canCreateNewCharacter = BooleanByteWrapper.GetFlag(flag1, 1);
            serverId = reader.ReadShort();
            address = reader.ReadUTF();
            port = reader.ReadUShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            ticket = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + 0 + sizeof(short) + sizeof(short) + Encoding.UTF8.GetByteCount(address) + sizeof(ushort) + sizeof(short) + Encoding.UTF8.GetByteCount(ticket);
        }
        
    }
    
}