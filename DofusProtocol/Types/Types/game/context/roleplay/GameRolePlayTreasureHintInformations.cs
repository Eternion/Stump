

// Generated on 10/26/2014 23:30:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayTreasureHintInformations : GameRolePlayActorInformations
    {
        public const short Id = 471;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short npcId;
        
        public GameRolePlayTreasureHintInformations()
        {
        }
        
        public GameRolePlayTreasureHintInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, short npcId)
         : base(contextualId, look, disposition)
        {
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            npcId = reader.ReadShort();
            if (npcId < 0)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}