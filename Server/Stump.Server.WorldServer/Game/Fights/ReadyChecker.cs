using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class ReadyChecker
    {
        /// <summary>
        /// Delay in ms before a fighter is declared as lagger.
        /// </summary>
        [Variable(true)]
        public static readonly int CheckTimeout = 5000;

        public event Action<ReadyChecker> Success;

        private void NotifySuccess()
        {
            Success?.Invoke(this);
        }

        public event Action<ReadyChecker, CharacterFighter[]> Timeout;

        private void NotifyTimeout(CharacterFighter[] laggers)
        {
            Timeout?.Invoke(this, laggers);
        }

        private readonly List<CharacterFighter> m_fighters;
        private readonly IFight m_fight;
        private bool m_started;
        private TimedTimerEntry m_timer;

        public ReadyChecker(IFight fight, IEnumerable<CharacterFighter> actors)
        {
            m_fight = fight;
            m_fighters = actors.ToList();
        }

        public void Start()
        {
            if (m_started)
                return;

            m_started = true;

            if (m_fighters.Count <= 0)
            {
                m_timer = m_fight.Map.Area.CallDelayed(0, NotifySuccess);
                return;
            }

            foreach (var fighter in m_fighters)
            {
                ContextHandler.SendGameFightTurnReadyRequestMessage(fighter.Character.Client, m_fight.TimeLine.Current);
            }

            m_timer = m_fight.Map.Area.CallDelayed(Math.Max(0, (int)(m_fight.LastSequenceEndTime - DateTime.Now).TotalMilliseconds) + CheckTimeout, TimedOut);
        }

        public void Cancel()
        {
            m_started = false;

            if (m_timer != null)
                m_timer.Dispose();
        }

        public void ToggleReady(CharacterFighter actor, bool ready = true)
        {
            if (!m_started)
                return;

            if (ready && m_fighters.Contains(actor))
            {
                m_fighters.Remove(actor);
            }
            else if (!ready)
            {
                m_fighters.Add(actor);
            }

            if (m_fighters.Count != 0)
                return;

            if (m_timer != null)
                m_timer.Dispose();

            NotifySuccess();
        }

        private void TimedOut()
        {
            if (!m_started)
                return;

            if (m_fighters.Count > 0)
                NotifyTimeout(m_fighters.ToArray());
        }

        public static ReadyChecker RequestCheck(IFight fight, Action success, Action<CharacterFighter[]> fail)
        {
            var checker = new ReadyChecker(fight, fight.GetAllFighters<CharacterFighter>(entry => !entry.HasLeft()).ToList());
            checker.Success += chk => success();
            checker.Timeout += (chk, laggers) => fail(laggers);
            checker.Start();

            return checker;
        }
    }
}