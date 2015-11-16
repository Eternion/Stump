

// Generated on 11/16/2015 14:26:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaSwitchToGameServerMessage : Message
    {
        public const uint Id = 6574;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool validToken;
        public IEnumerable<sbyte> ticket;
        public short homeServerId;
        
        public GameRolePlayArenaSwitchToGameServerMessage()
        {
        }
        
        public GameRolePlayArenaSwitchToGameServerMessage(bool validToken, IEnumerable<sbyte> ticket, short homeServerId)
        {
            this.validToken = validToken;
            this.ticket = ticket;
            this.homeServerId = homeServerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(validToken);
            writer.WriteVarInt((int)ticket.Count());
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
            }
            writer.WriteShort(homeServerId);
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
            homeServerId = reader.ReadShort();
        }
        
    }
    
}