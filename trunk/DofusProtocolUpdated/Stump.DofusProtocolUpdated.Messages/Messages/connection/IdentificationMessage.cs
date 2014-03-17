

// Generated on 03/06/2014 18:49:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdentificationMessage : Message
    {
        public const uint Id = 4;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.VersionExtended version;
        public string lang;
        public IEnumerable<sbyte> credentials;
        public short serverId;
        public double sessionOptionalSalt;
        
        public IdentificationMessage()
        {
        }
        
        public IdentificationMessage(Types.VersionExtended version, string lang, IEnumerable<sbyte> credentials, short serverId, double sessionOptionalSalt)
        {
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.sessionOptionalSalt = sessionOptionalSalt;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            version.Serialize(writer);
            writer.WriteUTF(lang);
            var credentials_before = writer.Position;
            var credentials_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in credentials)
            {
                 writer.WriteSByte(entry);
                 credentials_count++;
            }
            var credentials_after = writer.Position;
            writer.Seek((int)credentials_before);
            writer.WriteUShort((ushort)credentials_count);
            writer.Seek((int)credentials_after);

            writer.WriteShort(serverId);
            writer.WriteDouble(sessionOptionalSalt);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            version = new Types.VersionExtended();
            version.Deserialize(reader);
            lang = reader.ReadUTF();
            var limit = reader.ReadUShort();
            var credentials_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 credentials_[i] = reader.ReadSByte();
            }
            credentials = credentials_;
            serverId = reader.ReadShort();
            sessionOptionalSalt = reader.ReadDouble();
        }
        
        public override int GetSerializationSize()
        {
            return version.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(lang) + sizeof(short) + credentials.Sum(x => sizeof(sbyte)) + sizeof(short) + sizeof(double);
        }
        
    }
    
}