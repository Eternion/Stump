

// Generated on 12/12/2013 16:57:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInformationsGeneralMessage : Message
    {
        public const uint Id = 5557;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte level;
        public double expLevelFloor;
        public double experience;
        public double expNextLevelFloor;
        public int creationDate;
        
        public GuildInformationsGeneralMessage()
        {
        }
        
        public GuildInformationsGeneralMessage(byte level, double expLevelFloor, double experience, double expNextLevelFloor, int creationDate)
        {
            this.level = level;
            this.expLevelFloor = expLevelFloor;
            this.experience = experience;
            this.expNextLevelFloor = expNextLevelFloor;
            this.creationDate = creationDate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(level);
            writer.WriteDouble(expLevelFloor);
            writer.WriteDouble(experience);
            writer.WriteDouble(expNextLevelFloor);
            writer.WriteInt(creationDate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            level = reader.ReadByte();
            if (level < 0 || level > 255)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 255");
            expLevelFloor = reader.ReadDouble();
            if (expLevelFloor < 0)
                throw new Exception("Forbidden value on expLevelFloor = " + expLevelFloor + ", it doesn't respect the following condition : expLevelFloor < 0");
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            expNextLevelFloor = reader.ReadDouble();
            if (expNextLevelFloor < 0)
                throw new Exception("Forbidden value on expNextLevelFloor = " + expNextLevelFloor + ", it doesn't respect the following condition : expNextLevelFloor < 0");
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(byte) + sizeof(double) + sizeof(double) + sizeof(double) + sizeof(int);
        }
        
    }
    
}