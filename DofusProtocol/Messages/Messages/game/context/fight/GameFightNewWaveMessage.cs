

// Generated on 10/26/2014 23:29:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightNewWaveMessage : Message
    {
        public const uint Id = 6490;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public uint id;
        public sbyte teamId;
        public int nbTurnBeforeNextWave;
        
        public GameFightNewWaveMessage()
        {
        }
        
        public GameFightNewWaveMessage(uint id, sbyte teamId, int nbTurnBeforeNextWave)
        {
            this.id = id;
            this.teamId = teamId;
            this.nbTurnBeforeNextWave = nbTurnBeforeNextWave;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUInt(id);
            writer.WriteSByte(teamId);
            writer.WriteInt(nbTurnBeforeNextWave);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadUInt();
            if (id < 0 || id > 4.294967295E9)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 4.294967295E9");
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            nbTurnBeforeNextWave = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(uint) + sizeof(sbyte) + sizeof(int);
        }
        
    }
    
}