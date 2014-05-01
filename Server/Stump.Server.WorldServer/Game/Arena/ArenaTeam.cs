using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Parties;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaTeam : FightTeamWithLeader<CharacterFighter>
    {
        private List<ArenaWaitingCharacter> m_requestedCharacters = new List<ArenaWaitingCharacter>();

        public ArenaTeam(sbyte id, Cell[] placementCells)
            : base(id, placementCells)
        {
        }

        public void AddQueueMember(ArenaQueueMember member)
        {
            if (Fight.State != FightState.NotStarted)
                return;

            if (member.Party != null)
                m_requestedCharacters.AddRange(member.Party.Members.Select(x => new ArenaWaitingCharacter(x, this)));
            else m_requestedCharacters.Add(new ArenaWaitingCharacter(member.Character, this));

            member.Team = this;
        }

        public Character[] GetAlliesInQueue()
        {
            return m_requestedCharacters.Select(x => x.Character).ToArray();
        }

        public bool IsReadyToFight(Character character)
        {
            var tuple = m_requestedCharacters.FirstOrDefault(x => x.Character == character);

            if (tuple == null)
                return false;

            return tuple.Ready;
        }

        public void ToggleReadyToFight(Character character, bool ready)
        {
            var tuple = m_requestedCharacters.FirstOrDefault(x => x.Character == character);

            if (tuple == null)
                return;

            tuple.Ready = ready;
            if (AreAllReadyToFight() && (((ArenaTeam) OpposedTeam).AreAllReadyToFight()))
            {
                Fight.Map.Area.ExecuteInContext(TeleportCharactersToFight);
            }
        }
        public bool AreAllReadyToFight()
        {
            return m_requestedCharacters.All(x => x.Ready);
        }

        private void TeleportCharactersToFight()
        {
            // a bit tricky
            int count = m_requestedCharacters.Count + ((ArenaTeam) OpposedTeam).m_requestedCharacters.Count;
            foreach (var character in m_requestedCharacters.Concat(((ArenaTeam) OpposedTeam).m_requestedCharacters).Select(x => x.Character))
            {
                Character character1 = character;
                character.Area.ExecuteInContext(() =>
                {
                    try
                    {
                        if (character1.IsFighting())
                        {
                            character1.NextMap = Fight.Map;
                            character1.Fighter.LeaveFight();
                        }
                        else if (character1.IsSpectator())
                        {
                            character1.NextMap = Fight.Map;
                            character1.Spectator.Leave();
                        }
                        else
                        {
                            character1.NextMap = Fight.Map;
                            character1.Teleport(Fight.Map, Fight.Map.Cells[character1.Cell.Id]);
                        }
                    }
                    finally
                    {
                        if (Interlocked.Decrement(ref count) <= 0)
                        {
                            Fight.Map.Area.ExecuteInContext(PrepareFight);
                        }
                    }
                });
            }
        }

        private void PrepareFight()
        {
            foreach (var character in m_requestedCharacters.Select(x => x.Character))
            {
                AddFighter(character.CreateFighter(this));
            }

            foreach (var character in ((ArenaTeam)OpposedTeam).m_requestedCharacters.Select(x => x.Character))
            {
                OpposedTeam.AddFighter(character.CreateFighter(this));
            }

            Fight.StartPlacement();
        }


        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }
    }
}