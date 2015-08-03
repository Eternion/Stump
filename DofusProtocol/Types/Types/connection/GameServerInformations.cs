

// Generated on 08/04/2015 00:35:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameServerInformations
    {
        public const short Id = 25;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        public sbyte status;
        public sbyte completion;
        public bool isSelectable;
        public sbyte charactersCount;
        public double date;
        
        public GameServerInformations()
        {
        }
        
        public GameServerInformations(short id, sbyte status, sbyte completion, bool isSelectable, sbyte charactersCount, double date)
        {
            this.id = id;
            this.status = status;
            this.completion = completion;
            this.isSelectable = isSelectable;
            this.charactersCount = charactersCount;
            this.date = date;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
            writer.WriteSByte(status);
            writer.WriteSByte(completion);
            writer.WriteBoolean(isSelectable);
            writer.WriteSByte(charactersCount);
            writer.WriteDouble(date);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            completion = reader.ReadSByte();
            if (completion < 0)
                throw new Exception("Forbidden value on completion = " + completion + ", it doesn't respect the following condition : completion < 0");
            isSelectable = reader.ReadBoolean();
            charactersCount = reader.ReadSByte();
            if (charactersCount < 0)
                throw new Exception("Forbidden value on charactersCount = " + charactersCount + ", it doesn't respect the following condition : charactersCount < 0");
            date = reader.ReadDouble();
            if (date < -9.007199254740992E15 || date > 9.007199254740992E15)
                throw new Exception("Forbidden value on date = " + date + ", it doesn't respect the following condition : date < -9.007199254740992E15 || date > 9.007199254740992E15");
        }
        
        
    }
    
}