

// Generated on 10/28/2014 16:36:32
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
        
        public bool autoconnect;
        public bool useCertificate;
        public bool useLoginToken;
        public Types.VersionExtended version;
        public string lang;
        public IEnumerable<sbyte> credentials;
        public short serverId;
        public double sessionOptionalSalt;
        
        public IdentificationMessage()
        {
        }
        
        public IdentificationMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, IEnumerable<sbyte> credentials, short serverId, double sessionOptionalSalt)
        {
            this.autoconnect = autoconnect;
            this.useCertificate = useCertificate;
            this.useLoginToken = useLoginToken;
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.sessionOptionalSalt = sessionOptionalSalt;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, autoconnect);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, useCertificate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, useLoginToken);
            writer.WriteByte(flag1);
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
            byte flag1 = reader.ReadByte();
            autoconnect = BooleanByteWrapper.GetFlag(flag1, 0);
            useCertificate = BooleanByteWrapper.GetFlag(flag1, 1);
            useLoginToken = BooleanByteWrapper.GetFlag(flag1, 2);
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
            if (sessionOptionalSalt < -9.007199254740992E15 || sessionOptionalSalt > 9.007199254740992E15)
                throw new Exception("Forbidden value on sessionOptionalSalt = " + sessionOptionalSalt + ", it doesn't respect the following condition : sessionOptionalSalt < -9.007199254740992E15 || sessionOptionalSalt > 9.007199254740992E15");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + 0 + 0 + version.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(lang) + sizeof(short) + credentials.Sum(x => sizeof(sbyte)) + sizeof(short) + sizeof(double);
        }
        
    }
    
}