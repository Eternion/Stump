using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.REKOP)]
    [SpellCastHandler(SpellIdEnum.REKOP_DU_DOPEUL)]
    public class RekopCastHandler : DefaultSpellCastHandler
    {
        public RekopCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public int CastRound
        {
            get;
            set;
        }

        public override bool Initialize()
        {
            base.Initialize();

            // 1 to 3 rounds
            CastRound = new Random().Next(1, 4);
            Handlers = Handlers.Where(entry => entry.Effect.Duration == CastRound || !(entry is DirectDamage)).ToArray();

            var target = Fight.GetOneFighter(TargetedCell);

            if (target == null)
                return true;

            foreach (var handler in Handlers.OfType<DirectDamage>())
            {
                handler.SetAffectedActors(new[] { target });

                if (handler is DirectDamage)
                    handler.BuffTriggerType = BuffTriggerType.OnBuffEnded;
            }

            return true;
        }
    }
}