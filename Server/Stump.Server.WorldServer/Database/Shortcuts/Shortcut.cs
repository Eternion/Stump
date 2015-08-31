using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Database.Shortcuts
{
    public abstract class Shortcut : ISaveIntercepter
    {
        protected Shortcut()
        {
        }

        protected Shortcut(CharacterRecord owner, int slot)
        {
            OwnerId = owner.Id;
            Slot = slot;
            IsNew = true;
        }

        private int m_id;

        public int Id
        {
            get { return m_id; }
            set
            {
                m_id = value; IsDirty = true;
            }
        }

        private int m_ownerId;

        [Index]
        public int OwnerId
        {
            get { return m_ownerId; }
            set
            {
                m_ownerId = value; IsDirty = true;
            }
        }

        private int m_slot;

        public int Slot
        {
            get { return m_slot; }
            set
            {
                m_slot = value; IsDirty = true;
            }
        }

        [Ignore]
        public bool IsDirty
        {
            get;
            set;
        }

        [Ignore]
        public bool IsNew
        {
            get;
            set;
        }

        public abstract DofusProtocol.Types.Shortcut GetNetworkShortcut();
        public void BeforeSave(bool insert)
        {
            IsDirty = false;
        }
    }
}