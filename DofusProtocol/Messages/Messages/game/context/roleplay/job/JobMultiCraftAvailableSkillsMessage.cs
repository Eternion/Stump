

// Generated on 12/29/2014 21:12:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage
    {
        public const uint Id = 5747;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        public IEnumerable<short> skills;
        
        public JobMultiCraftAvailableSkillsMessage()
        {
        }
        
        public JobMultiCraftAvailableSkillsMessage(bool enabled, int playerId, IEnumerable<short> skills)
         : base(enabled)
        {
            this.playerId = playerId;
            this.skills = skills;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(playerId);
            var skills_before = writer.Position;
            var skills_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in skills)
            {
                 writer.WriteShort(entry);
                 skills_count++;
            }
            var skills_after = writer.Position;
            writer.Seek((int)skills_before);
            writer.WriteUShort((ushort)skills_count);
            writer.Seek((int)skills_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            var limit = reader.ReadUShort();
            var skills_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 skills_[i] = reader.ReadShort();
            }
            skills = skills_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short) + skills.Sum(x => sizeof(short));
        }
        
    }
    
}