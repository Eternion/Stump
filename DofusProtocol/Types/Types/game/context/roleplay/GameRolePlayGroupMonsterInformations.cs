

// Generated on 08/04/2015 00:35:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations
    {
        public const short Id = 160;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public bool keyRingBonus;
        public bool hasHardcoreDrop;
        public bool hasAVARewardToken;
        public Types.GroupMonsterStaticInformations staticInfos;
        public short ageBonus;
        public sbyte lootShare;
        public sbyte alignmentSide;
        
        public GameRolePlayGroupMonsterInformations()
        {
        }
        
        public GameRolePlayGroupMonsterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, bool keyRingBonus, bool hasHardcoreDrop, bool hasAVARewardToken, Types.GroupMonsterStaticInformations staticInfos, short ageBonus, sbyte lootShare, sbyte alignmentSide)
         : base(contextualId, look, disposition)
        {
            this.keyRingBonus = keyRingBonus;
            this.hasHardcoreDrop = hasHardcoreDrop;
            this.hasAVARewardToken = hasAVARewardToken;
            this.staticInfos = staticInfos;
            this.ageBonus = ageBonus;
            this.lootShare = lootShare;
            this.alignmentSide = alignmentSide;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, keyRingBonus);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, hasHardcoreDrop);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, hasAVARewardToken);
            writer.WriteByte(flag1);
            writer.WriteShort(staticInfos.TypeId);
            staticInfos.Serialize(writer);
            writer.WriteShort(ageBonus);
            writer.WriteSByte(lootShare);
            writer.WriteSByte(alignmentSide);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            keyRingBonus = BooleanByteWrapper.GetFlag(flag1, 0);
            hasHardcoreDrop = BooleanByteWrapper.GetFlag(flag1, 1);
            hasAVARewardToken = BooleanByteWrapper.GetFlag(flag1, 2);
            staticInfos = Types.ProtocolTypeManager.GetInstance<Types.GroupMonsterStaticInformations>(reader.ReadShort());
            staticInfos.Deserialize(reader);
            ageBonus = reader.ReadShort();
            if (ageBonus < -1 || ageBonus > 1000)
                throw new Exception("Forbidden value on ageBonus = " + ageBonus + ", it doesn't respect the following condition : ageBonus < -1 || ageBonus > 1000");
            lootShare = reader.ReadSByte();
            if (lootShare < -1 || lootShare > 8)
                throw new Exception("Forbidden value on lootShare = " + lootShare + ", it doesn't respect the following condition : lootShare < -1 || lootShare > 8");
            alignmentSide = reader.ReadSByte();
        }
        
        
    }
    
}