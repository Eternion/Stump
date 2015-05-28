using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.PARADE_DES_VIEUX_JOUETS)]
    public class OldToyParadeCastHandler : DefaultSpellCastHandler
    {
        public OldToyParadeCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var fighter = Fight.GetFirstFighter<SummonedMonster>(TargetedCell);

            if (fighter == null)
                return;

            if (fighter.Monster.MonsterId != 494)
                return;

            //Handlers[0].Apply();

            base.Execute();
        }
    }
}
