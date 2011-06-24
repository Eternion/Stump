using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stump.Core.Threading;
using Stump.Database.Data.Communication;
using Stump.Database.Interfaces;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities.Actors
{
    public abstract class Humanoid : NamedActor, ISpellsOwner
    {
        protected Humanoid(long id, EntityLook look, ObjectPosition position, string name)
            : base(id, look, position, name)
        {
        }

        public SpellInventory SpellInventory
        {
            get;
            private set;
        }

        public EmoticonRecord Emote
        {
            get;
            protected set;
        }

        private CancellationTokenSource m_emoteCancellationToken;

        public IRestrictable Restrictions
        {
            get;
            protected set;
        }

        public Title ActorTitle
        {
            get;
            protected set;
        }

        public void DoEmote(EmotesEnum emote)
        {
            // todo : check conditions

            if (m_emoteCancellationToken != null && Emote != null && Emote.Id != (uint) EmotesEnum.EMOTE_SIT)
                m_emoteCancellationToken.Cancel();
            else if (m_emoteCancellationToken != null && Emote != null && Emote.Id == (uint)EmotesEnum.EMOTE_SIT)
            {
                m_emoteCancellationToken.Cancel();

                Emote = null;
                return;
            }

            if (EmotesManager.Emotes.ContainsKey((byte)emote))
            {
                Emote = EmotesManager.Emotes[(byte)emote];

                Context.Do(entry =>
                {
                    ContextHandler.SendEmotePlayMessage(entry.Client, this, emote, Emote.Duration);
                });

                m_emoteCancellationToken = new CancellationTokenSource();
                Task.Factory.StartNewDelayed((int)Emote.Duration, () => Emote = null, m_emoteCancellationToken.Token);
            }
        }

        public override GameContextActorInformations GetActorInformations()
        {
            return new GameRolePlayHumanoidInformations((int)Id, Look, GetDispositionInformations(), Name, GetHumanInformations());
        }

        public ActorRestrictionsInformations GetActorRestrictions()
        {
            return new ActorRestrictionsInformations(
                Restrictions.CantBeAggressed, Restrictions.CantBeChallenged,
                Restrictions.CantTrade, Restrictions.CantBeAttackedByMutant,
                Restrictions.CantRun, Restrictions.ForceSlowWalk,
                Restrictions.CantMinimize, Restrictions.CantMove,
                Restrictions.CantAggress, Restrictions.CantChallenge,
                Restrictions.CantExchange, Restrictions.CantAttack,
                Restrictions.CantChat, Restrictions.CantBeMerchant,
                Restrictions.CantUseObject, Restrictions.CantUseTaxCollector,
                Restrictions.CantUseInteractive, Restrictions.CantSpeakToNpc,
                Restrictions.CantChangeZone, Restrictions.CantAttackMonster, Restrictions.CantWalk8Directions);
        }

        public List<EntityLook> GetFollowingCharacters()
        {
            return new List<EntityLook>(); //todo
        }

        public virtual HumanInformations GetHumanInformations()
        {
            return new HumanInformations(
                GetFollowingCharacters(), // followers
                (int) Emote.Id,
                Emote.Duration,
                GetActorRestrictions(),
                ActorTitle.Id,
                ActorTitle.Params);
        }
    }
}