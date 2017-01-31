using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Extensions;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.DESTIN_D_ECAFLIP)]
    [SpellCastHandler(SpellIdEnum.DESTIN_D_ECAFLIP_DU_DOPEUL)]
    public class FateOfEcaflipCastHandler : DefaultSpellCastHandler
    {
        public FateOfEcaflipCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var debuffHandler = Handlers.OfType<MPDebuff>().First();

            if (debuffHandler != null)
                debuffHandler.TriggeredBuffDuration = 2;

            var pullHandler = Handlers.OfType<Push>().First(x => x.Effect.EffectId == EffectsEnum.Effect_PullForward);

            if (pullHandler == null)
                return;

            foreach (var handler in Handlers)
            {
                if (handler is Push && handler.Effect.EffectId == EffectsEnum.Effect_PushBack && pullHandler.PushDirection.HasValue) // push
                    (handler as Push).PushDirection = pullHandler.PushDirection.Value.GetOpposedDirection();

                handler.Apply();
            }
        }
    }
}