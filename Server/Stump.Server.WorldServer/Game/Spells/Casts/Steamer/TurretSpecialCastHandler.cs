using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Steamer
{
    [SpellCastHandler(SpellIdEnum.BOUMBOUMF)]
    [SpellCastHandler(SpellIdEnum.BOUMBOUME)]
    [SpellCastHandler(SpellIdEnum.BOUMBOUMT)]
    [SpellCastHandler(SpellIdEnum.TRANSKO)]
    [SpellCastHandler(SpellIdEnum.SAUVETAGE)]
    public class TurretSpecialCastHandler : DefaultSpellCastHandler
    {
        public TurretSpecialCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var requiredState = 0;

            switch (Spell.Id)
            {
                case (int) SpellIdEnum.BOUMBOUMF:
                case (int) SpellIdEnum.BOUMBOUME:
                case (int) SpellIdEnum.BOUMBOUMT:
                    requiredState = (int) SpellStatesEnum.Ambush;
                    break;
                case (int)SpellIdEnum.TRANSKO:
                    requiredState = (int)SpellStatesEnum.Spyglass;
                    break;
                case (int)SpellIdEnum.SAUVETAGE:
                    requiredState = (int)SpellStatesEnum.First_Aid;
                    break;
            }

            if (requiredState == 0)
                return;

            var target = Fight.GetOneFighter(x => x.Team != Caster.Team && x.HasState(requiredState));

            if (target == null)
                return;

            foreach (var handler in Handlers)
                handler.AddAffectedActor(target);

            base.Execute();
        }
    }
}
