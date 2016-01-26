using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages.Custom
{
    public class ClearIdentificationMessage : Message
    {
        public const uint Id = 888;
        public override uint MessageId
        {
            get { return Id; }
        }

        public bool autoconnect;
        public string lang;
        public string username;
        public string password;
        public short serverId;
        public string serverIp;
        public string hardwareId;

        public ClearIdentificationMessage()
        {
        }

        public ClearIdentificationMessage(bool autoconnect, string lang, string username, string password, short serverId, string serverIp, string hardwareId)
        {
            this.autoconnect = autoconnect;
            this.lang = lang;
            this.username = username;
            this.password = password;
            this.serverId = serverId;
            this.serverIp = serverIp;
            this.hardwareId = hardwareId;
        }

        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, autoconnect);

            writer.WriteByte(flag1);
            writer.WriteUTF(lang);
            writer.WriteUTF(username);
            writer.WriteUTF(password);
            writer.WriteShort(serverId);
            writer.WriteUTF(serverIp);
            writer.WriteUTF(hardwareId);
        }

        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();

            autoconnect = BooleanByteWrapper.GetFlag(flag1, 0);
            lang = reader.ReadUTF();
            username = reader.ReadUTF();
            password = reader.ReadUTF();
            serverId = reader.ReadShort();
            serverIp = reader.ReadUTF();
            hardwareId = reader.ReadUTF();
        }
    }
}