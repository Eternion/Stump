

// Generated on 12/12/2013 16:56:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KohUpdateMessage : Message
    {
        public const uint Id = 6439;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.AllianceInformations> alliances;
        public IEnumerable<short> allianceNbMembers;
        public IEnumerable<int> allianceRoundWeigth;
        public IEnumerable<sbyte> allianceMatchScore;
        public Types.BasicAllianceInformations allianceMapWinner;
        public int allianceMapWinnerScore;
        public int allianceMapMyAllianceScore;
        
        public KohUpdateMessage()
        {
        }
        
        public KohUpdateMessage(IEnumerable<Types.AllianceInformations> alliances, IEnumerable<short> allianceNbMembers, IEnumerable<int> allianceRoundWeigth, IEnumerable<sbyte> allianceMatchScore, Types.BasicAllianceInformations allianceMapWinner, int allianceMapWinnerScore, int allianceMapMyAllianceScore)
        {
            this.alliances = alliances;
            this.allianceNbMembers = allianceNbMembers;
            this.allianceRoundWeigth = allianceRoundWeigth;
            this.allianceMatchScore = allianceMatchScore;
            this.allianceMapWinner = allianceMapWinner;
            this.allianceMapWinnerScore = allianceMapWinnerScore;
            this.allianceMapMyAllianceScore = allianceMapMyAllianceScore;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)alliances.Count());
            foreach (var entry in alliances)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)allianceNbMembers.Count());
            foreach (var entry in allianceNbMembers)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)allianceRoundWeigth.Count());
            foreach (var entry in allianceRoundWeigth)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)allianceMatchScore.Count());
            foreach (var entry in allianceMatchScore)
            {
                 writer.WriteSByte(entry);
            }
            allianceMapWinner.Serialize(writer);
            writer.WriteInt(allianceMapWinnerScore);
            writer.WriteInt(allianceMapMyAllianceScore);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            alliances = new Types.AllianceInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alliances as Types.AllianceInformations[])[i] = new Types.AllianceInformations();
                 (alliances as Types.AllianceInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            allianceNbMembers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (allianceNbMembers as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            allianceRoundWeigth = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (allianceRoundWeigth as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            allianceMatchScore = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (allianceMatchScore as sbyte[])[i] = reader.ReadSByte();
            }
            allianceMapWinner = new Types.BasicAllianceInformations();
            allianceMapWinner.Deserialize(reader);
            allianceMapWinnerScore = reader.ReadInt();
            if (allianceMapWinnerScore < 0)
                throw new Exception("Forbidden value on allianceMapWinnerScore = " + allianceMapWinnerScore + ", it doesn't respect the following condition : allianceMapWinnerScore < 0");
            allianceMapMyAllianceScore = reader.ReadInt();
            if (allianceMapMyAllianceScore < 0)
                throw new Exception("Forbidden value on allianceMapMyAllianceScore = " + allianceMapMyAllianceScore + ", it doesn't respect the following condition : allianceMapMyAllianceScore < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + alliances.Sum(x => x.GetSerializationSize()) + sizeof(short) + allianceNbMembers.Sum(x => sizeof(short)) + sizeof(short) + allianceRoundWeigth.Sum(x => sizeof(int)) + sizeof(short) + allianceMatchScore.Sum(x => sizeof(sbyte)) + allianceMapWinner.GetSerializationSize() + sizeof(int) + sizeof(int);
        }
        
    }
    
}