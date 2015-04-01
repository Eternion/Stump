using System.Linq;
using Stump.DofusProtocol.Enums;
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

        public override bool Initialize()
        {
            base.Initialize();

            foreach (var handler in Handlers)
            {
                handler.SetAffectedActors(new FightActor[0]);
            }

            var requiredState = 0;

            switch (Spell.Id)
            {
                case (int)SpellIdEnum.BOUMBOUMF:
                case (int)SpellIdEnum.BOUMBOUME:
                case (int)SpellIdEnum.BOUMBOUMT:
                    requiredState = (int)SpellStatesEnum.Ambush;
                    break;
                case (int)SpellIdEnum.TRANSKO:
                    requiredState = (int)SpellStatesEnum.Periscope;
                    break;
                case (int)SpellIdEnum.SAUVETAGE:
                    requiredState = (int)SpellStatesEnum.First_Aid;
                    break;
            }

            if (requiredState == 0)
                return false;

            var turret = Caster as SummonedTurret;
            if (turret == null)
                return false;

            var target = Fight.GetFirstFighter<FightActor>(x => x.GetBuffs(y => y.Caster == turret.Summoner && (y is StateBuff)
                && ((StateBuff)y).State.Id == requiredState).Any());

            if (target == null)
                return false;

            if (Caster.CanCastSpell(Spell, target.Position.Cell) != SpellCastResult.OK)
                return false;

            TargetedActor = target;

            foreach (var handler in Handlers)
            {
                handler.AddAffectedActor(target);
            }

            return true;
        }
    }
}
