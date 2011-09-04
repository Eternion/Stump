using System;
using Castle.ActiveRecord;
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
            Owner = owner;
            Slot = slot;
        }

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [BelongsTo("CharacterId")]
        public CharacterRecord Owner
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

        public abstract DofusShortcut GetNetworkShortcut();
    }
}