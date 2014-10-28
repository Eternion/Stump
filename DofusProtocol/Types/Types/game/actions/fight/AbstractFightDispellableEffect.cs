

// Generated on 10/28/2014 16:37:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AbstractFightDispellableEffect
    {
        public const short Id = 206;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int uid;
        public int targetId;
        public short turnDuration;
        public sbyte dispelable;
        public short spellId;
        public int effectId;
        public int parentBoostUid;
        
        public AbstractFightDispellableEffect()
        {
        }
        
        public AbstractFightDispellableEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int effectId, int parentBoostUid)
        {
            this.uid = uid;
            this.targetId = targetId;
            this.turnDuration = turnDuration;
            this.dispelable = dispelable;
            this.spellId = spellId;
            this.effectId = effectId;
            this.parentBoostUid = parentBoostUid;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(uid);
            writer.WriteInt(targetId);
            writer.WriteShort(turnDuration);
            writer.WriteSByte(dispelable);
            writer.WriteShort(spellId);
            writer.WriteInt(effectId);
            writer.WriteInt(parentBoostUid);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            uid = reader.ReadInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            targetId = reader.ReadInt();
            turnDuration = reader.ReadShort();
            dispelable = reader.ReadSByte();
            if (dispelable < 0)
                throw new Exception("Forbidden value on dispelable = " + dispelable + ", it doesn't respect the following condition : dispelable < 0");
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            effectId = reader.ReadInt();
            if (effectId < 0)
                throw new Exception("Forbidden value on effectId = " + effectId + ", it doesn't respect the following condition : effectId < 0");
            parentBoostUid = reader.ReadInt();
            if (parentBoostUid < 0)
                throw new Exception("Forbidden value on parentBoostUid = " + parentBoostUid + ", it doesn't respect the following condition : parentBoostUid < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + sizeof(sbyte) + sizeof(short) + sizeof(int) + sizeof(int);
        }
        
    }
    
}