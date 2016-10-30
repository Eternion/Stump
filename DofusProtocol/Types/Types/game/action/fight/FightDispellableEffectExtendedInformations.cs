

// Generated on 10/30/2016 16:20:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightDispellableEffectExtendedInformations
    {
        public const short Id = 208;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short actionId;
        public double sourceId;
        public Types.AbstractFightDispellableEffect effect;
        
        public FightDispellableEffectExtendedInformations()
        {
        }
        
        public FightDispellableEffectExtendedInformations(short actionId, double sourceId, Types.AbstractFightDispellableEffect effect)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
            this.effect = effect;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(actionId);
            writer.WriteDouble(sourceId);
            writer.WriteShort(effect.TypeId);
            effect.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadDouble();
            if (sourceId < -9007199254740990 || sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < -9007199254740990 || sourceId > 9007199254740990");
            effect = Types.ProtocolTypeManager.GetInstance<Types.AbstractFightDispellableEffect>(reader.ReadShort());
            effect.Deserialize(reader);
        }
        
        
    }
    
}