using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Handlers.Context;

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
            Action<ReadyChecker> handler = Success;
            if (handler != null)
                handler(this);
        }

        public event Action<ReadyChecker, CharacterFighter[]> Timeout;

        private void NotifyTimeout(CharacterFighter[] laggers)
        {
            Action<ReadyChecker, CharacterFighter[]> handler = Timeout;
            if (handler != null)
                handler(this, laggers);
        }

        private readonly List<CharacterFighter> m_fighters;
        private readonly Fight m_fight;
        private bool m_started;
        private TimerEntry m_timer;

        public ReadyChecker(Fight fight, IEnumerable<CharacterFighter> actors)
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
                NotifySuccess();
                return;
            }

            foreach (var fighter in m_fighters)
            {
                ContextHandler.SendGameFightTurnReadyRequestMessage(fighter.Character.Client, m_fight.TimeLine.Current);
            }

            m_timer = m_fight.Map.Area.CallDelayed(CheckTimeout, TimedOut);
        }

        public void Cancel()
        {
            m_started = false;

            if (m_timer != null)
                m_timer.Stop();
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

            if (m_fighters.Count == 0)
                NotifySuccess();
        }

        private void TimedOut()
        {
            if (!m_started)
                return;

            if (m_fighters.Count > 0)
                NotifyTimeout(m_fighters.ToArray());
        }

        public static ReadyChecker RequestCheck(Fight fight, Action success, Action<CharacterFighter[]> fail)
        {
            var checker = new ReadyChecker(fight, fight.GetAllFighters<CharacterFighter>(entry => !entry.HasLeft()).ToList());
            checker.Success += (chk) => success();
            checker.Timeout += (chk, laggers) => fail(laggers);
            checker.Start();

            return checker;
        }
    }
}