

// Generated on 10/30/2016 16:20:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SelectedServerRefusedMessage : Message
    {
        public const uint Id = 41;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short serverId;
        public sbyte error;
        public sbyte serverStatus;
        
        public SelectedServerRefusedMessage()
        {
        }
        
        public SelectedServerRefusedMessage(short serverId, sbyte error, sbyte serverStatus)
        {
            this.serverId = serverId;
            this.error = error;
            this.serverStatus = serverStatus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(serverId);
            writer.WriteSByte(error);
            writer.WriteSByte(serverStatus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            serverId = reader.ReadVarShort();
            if (serverId < 0)
                throw new Exception("Forbidden value on serverId = " + serverId + ", it doesn't respect the following condition : serverId < 0");
            error = reader.ReadSByte();
            if (error < 0)
                throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
            serverStatus = reader.ReadSByte();
            if (serverStatus < 0)
                throw new Exception("Forbidden value on serverStatus = " + serverStatus + ", it doesn't respect the following condition : serverStatus < 0");
        }
        
    }
    
}