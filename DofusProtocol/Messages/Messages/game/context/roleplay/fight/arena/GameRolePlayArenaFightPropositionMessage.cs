

// Generated on 09/01/2015 10:48:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaFightPropositionMessage : Message
    {
        public const uint Id = 6276;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public IEnumerable<int> alliesId;
        public short duration;
        
        public GameRolePlayArenaFightPropositionMessage()
        {
        }
        
        public GameRolePlayArenaFightPropositionMessage(int fightId, IEnumerable<int> alliesId, short duration)
        {
            this.fightId = fightId;
            this.alliesId = alliesId;
            this.duration = duration;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            var alliesId_before = writer.Position;
            var alliesId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in alliesId)
            {
                 writer.WriteInt(entry);
                 alliesId_count++;
            }
            var alliesId_after = writer.Position;
            writer.Seek((int)alliesId_before);
            writer.WriteUShort((ushort)alliesId_count);
            writer.Seek((int)alliesId_after);

            writer.WriteVarShort(duration);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            var alliesId_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 alliesId_[i] = reader.ReadInt();
            }
            alliesId = alliesId_;
            duration = reader.ReadVarShort();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
        }
        
    }
    
}