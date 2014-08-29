using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items
{
    public class Pet
    {
        private readonly List<EffectBase> m_effects = new List<EffectBase>();

        public Character Owner
        {
            get;
            private set;
        }

        public BasePlayerItem Item
        {
            get;
            private set;
        }

        public ItemTemplate LastFood
        {
            get;
            private set;
        }

        public PetTemplate PetTemplate
        {
            get;
            private set;
        }

        public ReadOnlyCollection<EffectBase> Effects
        {
            get { return m_effects.AsReadOnly(); }
        }

        public bool TryToFeed(BasePlayerItem item)
        {
            return false;
        }

        private void OnFightFinished(Character character, CharacterFighter fighter)
        {
            
        }
    }
}