
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public class Mount : NamedActor
    {

        public Mount(long id, ExtendedLook look, VectorIsometric position, string name, string owner, uint level)
            : base(id, look, position, name)
        {
            Owner = owner;
            Level = level;
        }

        public string Owner
        {
            get;
            set;
        }

        public uint Level
        {
            get;
            set;
        }

        public GameRolePlayMountInformations ToGameRolePlayMountInformations()
        {
            return new GameRolePlayMountInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, Owner, Level);
        }

    }
}