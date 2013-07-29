

// Generated on 07/29/2013 23:07:48
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightPlacementPossiblePositionsMessage : Message
    {
        public const uint Id = 703;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> positionsForChallengers;
        public IEnumerable<short> positionsForDefenders;
        public sbyte teamNumber;
        
        public GameFightPlacementPossiblePositionsMessage()
        {
        }
        
        public GameFightPlacementPossiblePositionsMessage(IEnumerable<short> positionsForChallengers, IEnumerable<short> positionsForDefenders, sbyte teamNumber)
        {
            this.positionsForChallengers = positionsForChallengers;
            this.positionsForDefenders = positionsForDefenders;
            this.teamNumber = teamNumber;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)positionsForChallengers.Count());
            foreach (var entry in positionsForChallengers)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)positionsForDefenders.Count());
            foreach (var entry in positionsForDefenders)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteSByte(teamNumber);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            positionsForChallengers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (positionsForChallengers as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            positionsForDefenders = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (positionsForDefenders as short[])[i] = reader.ReadShort();
            }
            teamNumber = reader.ReadSByte();
            if (teamNumber < 0)
                throw new Exception("Forbidden value on teamNumber = " + teamNumber + ", it doesn't respect the following condition : teamNumber < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + positionsForChallengers.Sum(x => sizeof(short)) + sizeof(short) + positionsForDefenders.Sum(x => sizeof(short)) + sizeof(sbyte);
        }
        
    }
    
}