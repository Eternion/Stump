

// Generated on 02/11/2015 10:20:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightPlayersEnemiesListMessage : Message
    {
        public const uint Id = 5928;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public IEnumerable<Types.CharacterMinimalPlusLookInformations> playerInfo;
        
        public GuildFightPlayersEnemiesListMessage()
        {
        }
        
        public GuildFightPlayersEnemiesListMessage(int fightId, IEnumerable<Types.CharacterMinimalPlusLookInformations> playerInfo)
        {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            var playerInfo_before = writer.Position;
            var playerInfo_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in playerInfo)
            {
                 entry.Serialize(writer);
                 playerInfo_count++;
            }
            var playerInfo_after = writer.Position;
            writer.Seek((int)playerInfo_before);
            writer.WriteUShort((ushort)playerInfo_count);
            writer.Seek((int)playerInfo_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            var playerInfo_ = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 playerInfo_[i] = new Types.CharacterMinimalPlusLookInformations();
                 playerInfo_[i].Deserialize(reader);
            }
            playerInfo = playerInfo_;
        }
        
    }
    
}