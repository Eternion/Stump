using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Server.WorldServer.Database.Characters;
using DofusShortcut = Stump.DofusProtocol.Types.Shortcut;

namespace Stump.Server.WorldServer.Database.Shortcuts
{
    [ActiveRecord("shortcuts", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Abstract")]
    public abstract class Shortcut : WorldBaseRecord<Shortcut>
    {
        protected Shortcut()
        {
            
        }

        protected Shortcut(CharacterRecord owner, int slot)
        {
            OwnerId = owner.Id;
            Slot = slot;
        }

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Owner")]
        public int OwnerId
        {
            get;
            set;
        }

        [Property("Slot")]
        public int Slot
        {
            get;
            set;
        }

        public static Shortcut[] FindByOwnerId(int id)
        {
            return FindAll(Restrictions.Eq("OwnerId", id));
        }

        public abstract DofusShortcut GetNetworkShortcut();
    }
}