namespace Stump.Server.WorldServer.Database
{
    public abstract class Shortcut
    {
        protected Shortcut()
        {
        }

        protected Shortcut(CharacterRecord owner, int slot)
        {
            OwnerId = owner.Id;
            Slot = slot;
        }

        public int Id
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        public int Slot
        {
            get;
            set;
        }

        public abstract DofusProtocol.Types.Shortcut GetNetworkShortcut();
    }
}