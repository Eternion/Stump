

// Generated on 09/26/2016 01:49:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightResumeWithSlavesMessage : GameFightResumeMessage
    {
        public const uint Id = 6215;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo;
        
        public GameFightResumeWithSlavesMessage()
        {
        }
        
        public GameFightResumeWithSlavesMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, short gameTurn, int fightStart, IEnumerable<Types.Idol> idols, IEnumerable<Types.GameFightSpellCooldown> spellCooldowns, sbyte summonCount, sbyte bombCount, IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo)
         : base(effects, marks, gameTurn, fightStart, idols, spellCooldowns, summonCount, bombCount)
        {
            this.slavesInfo = slavesInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var slavesInfo_before = writer.Position;
            var slavesInfo_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in slavesInfo)
            {
                 entry.Serialize(writer);
                 slavesInfo_count++;
            }
            var slavesInfo_after = writer.Position;
            writer.Seek((int)slavesInfo_before);
            writer.WriteUShort((ushort)slavesInfo_count);
            writer.Seek((int)slavesInfo_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var slavesInfo_ = new Types.GameFightResumeSlaveInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 slavesInfo_[i] = new Types.GameFightResumeSlaveInfo();
                 slavesInfo_[i].Deserialize(reader);
            }
            slavesInfo = slavesInfo_;
        }
        
    }
    
}