using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    public class ItemHasEffectAttribute : Attribute
    {
        public ItemHasEffectAttribute(EffectsEnum effect)
        {
            Effect = effect;
        }

        public EffectsEnum Effect
        {
            get;
            set;
        }
    }
}