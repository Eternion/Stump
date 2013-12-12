

// Generated on 12/12/2013 16:57:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyGuestInformations
    {
        public const short Id = 374;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int guestId;
        public int hostId;
        public string name;
        public Types.EntityLook guestLook;
        public sbyte breed;
        public bool sex;
        public Types.PlayerStatus status;
        public IEnumerable<Types.PartyCompanionBaseInformations> companions;
        
        public PartyGuestInformations()
        {
        }
        
        public PartyGuestInformations(int guestId, int hostId, string name, Types.EntityLook guestLook, sbyte breed, bool sex, Types.PlayerStatus status, IEnumerable<Types.PartyCompanionBaseInformations> companions)
        {
            this.guestId = guestId;
            this.hostId = hostId;
            this.name = name;
            this.guestLook = guestLook;
            this.breed = breed;
            this.sex = sex;
            this.status = status;
            this.companions = companions;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guestId);
            writer.WriteInt(hostId);
            writer.WriteUTF(name);
            guestLook.Serialize(writer);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
            writer.WriteUShort((ushort)companions.Count());
            foreach (var entry in companions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guestId = reader.ReadInt();
            if (guestId < 0)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0");
            hostId = reader.ReadInt();
            if (hostId < 0)
                throw new Exception("Forbidden value on hostId = " + hostId + ", it doesn't respect the following condition : hostId < 0");
            name = reader.ReadUTF();
            guestLook = new Types.EntityLook();
            guestLook.Deserialize(reader);
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
            var limit = reader.ReadUShort();
            companions = new Types.PartyCompanionBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (companions as Types.PartyCompanionBaseInformations[])[i] = new Types.PartyCompanionBaseInformations();
                 (companions as Types.PartyCompanionBaseInformations[])[i].Deserialize(reader);
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(name) + guestLook.GetSerializationSize() + sizeof(sbyte) + sizeof(bool) + sizeof(short) + status.GetSerializationSize() + sizeof(short) + companions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}