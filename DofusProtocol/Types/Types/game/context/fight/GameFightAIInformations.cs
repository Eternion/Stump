

// Generated on 09/01/2014 15:52:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightAIInformations : GameFightFighterInformations
    {
        public const short Id = 151;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public GameFightAIInformations()
        {
        }
        
        public GameFightAIInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, uint wave, bool alive, Types.GameFightMinimalStats stats)
         : base(contextualId, look, disposition, teamId, wave, alive, stats)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}