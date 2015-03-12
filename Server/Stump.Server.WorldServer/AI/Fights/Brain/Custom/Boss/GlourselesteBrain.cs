using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier(2864)]
    public class GlourselesteBrain : Brain
    {
        public GlourselesteBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.ActorPushed += OnActorPushed;
            fighter.FightPointsVariation += OnFightPointsVariation;
            fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;

            fighter.Fight.FightStarted += OnFightStarted;
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        private SpellEffectHandler GetEffectHandler(SpellIdEnum spellId, Cell cell, int offset, FightActor fighter = null)
        {
            if (fighter == null)
                fighter = Fighter;

            var spell = SpellManager.Instance.GetSpellCastHandler(fighter, new Spell((int)spellId, 1), cell, false);
            spell.Initialize();

            return spell.GetEffectHandlers().ToArray()[offset];
        }

        private void OnFightStarted(IFight fight)
        {
            //State Lourd
            GetEffectHandler(SpellIdEnum.GLOURSONGEUR, Fighter.Cell, 4).Apply();

            Fighter.OpposedTeam.FighterAdded += OnFighterAdded;

            foreach (var fighter in Fighter.OpposedTeam.GetAllFighters())
            {
                fighter.ActorPushed += OnActorPushed;
            }
        }

        private void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (team == Fighter.Team)
                return;

            fighter.ActorPushed += OnActorPushed;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player == Fighter && player.HasState((int)SpellStatesEnum.Beark_to_Life))
            {
                var spellGlours = new Spell((int)SpellIdEnum.PETIT_GLOURS_BRUN, 1);

                for (var i = 0; i < 3; i++)
                {
                    if (player.AP < spellGlours.CurrentSpellLevel.ApCost)
                        continue;

                    var deadFighter = player.Team.GetLastDeadFighter();
                    if (deadFighter == null)
                        continue;

                    player.CastSpell(spellGlours, deadFighter.Cell, true);
                }

                var state = SpellManager.Instance.GetSpellState((int)SpellStatesEnum.Beark_to_Life);
                player.RemoveState(state);
            }

            if (!(player is CharacterFighter) || player.Team.Id == Fighter.Team.Id)
                return;

            if (Fighter.HasState((int) SpellStatesEnum.Invulnerable))
                return;

            var spell = new Spell((int) SpellIdEnum.GLOURSOMPTUEUX, 1);
            player.CastSpell(spell, Fighter.Cell, true, true);
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (damage.Source == Fighter)
                return;

            if (!Fighter.Position.Point.IsAdjacentTo(damage.Source.Position.Point))
                return;

            if (Fighter.HasState((int)SpellStatesEnum.Invulnerable))
            {
                damage.IgnoreDamageBoost = true;
                damage.IgnoreDamageReduction = true;
                damage.Generated = true;
                damage.Amount = 0;
            }

            var spell = new Spell((int)SpellIdEnum.HYDROMEL, 1);
            Fighter.CastSpell(spell, Fighter.Cell, true, true);
        }

        private void OnActorPushed(FightActor fighter, bool takeDamage)
        {
            if (fighter == Fighter)
            {
                //State Résuglours
                GetEffectHandler(SpellIdEnum.GLOURSONGEUR, Fighter.Cell, 3).Apply();

                return;
            }

            if (!takeDamage)
                return;

            //Kill
            var handler = GetEffectHandler(SpellIdEnum.GLOURSONGEUR, fighter.Cell, 1);

            handler.AddAffectedActor(fighter);
            handler.Apply();
        }

        private void OnFightPointsVariation(FightActor fighter, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            if (action != ActionsEnum.ACTION_CHARACTER_ACTION_POINTS_LOST && action != ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST)
                return;

            //State Résuglours
            GetEffectHandler(SpellIdEnum.GLOURSONGEUR, Fighter.Cell, 3).Apply();
        }
    }
}