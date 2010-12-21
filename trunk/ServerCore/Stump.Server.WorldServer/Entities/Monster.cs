// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Groups;
using MonsterGradeTemplate = Stump.DofusProtocol.D2oClasses.MonsterGrade;
using MonsterRaceTemplate = Stump.DofusProtocol.D2oClasses.MonsterRace;
using MonsterTemplate = Stump.DofusProtocol.D2oClasses.Monster;

namespace Stump.Server.WorldServer.Entities
{
    public class Monster : LivingEntity
    {
        #region Fields

        /// <summary>
        ///   Our global array containing every monsters of our world.
        /// </summary>
        public static List<Monster> Monsters = new List<Monster>();

        private readonly List<int> m_levelsRange = new List<int>();
        private readonly List<MapIdEnum> m_mapIds = new List<MapIdEnum>();

        #endregion

        public Monster(long id)
        {
            Id = id;
            Race = MonsterRaceIdEnum.NotListed; // default race if not defined later.
            Grades = new List<MonsterGrade>();
        }

        public Monster(long id, MonsterRaceIdEnum monsterRace)
        {
            Id = id;
            Race = monsterRace;
            Grades = new List<MonsterGrade>();
        }

        #region Loading

        [StageStep(Stages.Two, "Loaded Monsters")]
        public static void LoadAll()
        {
            var monstertemplates = DataLoader.LoadData<MonsterTemplate>();
            var monsterRaces = DataLoader.LoadData<MonsterRaceTemplate>();

            foreach (MonsterTemplate monstertemplate in monstertemplates)
            {
                var monster = new Monster(monstertemplate.id)
                    {
                        GfxId = monstertemplate.gfxId,
                        Race = (MonsterRaceIdEnum) monstertemplate.race,
                        Scale = 150,
                        BonesId = 1
                    };

                // todo : this is totally wrong !
                MonsterRaceTemplate race = monsterRaces.SingleOrDefault(monsterRace => monsterRace.id == monster.Id);
                if (race != null)
                    monster.ParentMonsterId = (MonsterRaceIdEnum) race.superRaceId;

                foreach (MonsterGradeTemplate grade in monstertemplate.grades)
                {
                    var mg = new MonsterGrade
                        {
                            ActionPoints = grade.actionPoints,
                            AirResistance = grade.airResistance,
                            EarthResistance = grade.earthResistance,
                            FireResistance = grade.fireResistance,
                            Grade = grade.grade,
                            Level = grade.level,
                            LifePoints = grade.lifePoints,
                            MonsterId = grade.monsterId,
                            MovementPoints = grade.movementPoints,
                            NeutralResistance = grade.neutralResistance,
                            PaDodge = grade.paDodge,
                            PmDodge = grade.pmDodge,
                            WaterResistance = grade.waterResistance
                        };

                    monster.Grades.Add(mg);
                }

                Monsters.Add(monster);
            }

            LoadSpawnData();
        }


        public static void LoadSpawnData()
        {
            // todo
        }

        #endregion

        #region Generation

        public static MonsterGroup GenerateGroup(MapIdEnum mapid)
        {
            var group = new MonsterGroup();

            // 1) Get how munch monsters will join this group.
            var random = new AsyncRandom();
            int number = random.NextInt(1, 8);

            // 2) Get succeptibles monsters to land on this map
            IEnumerable<Monster> monsters = (from entry in Monsters
                                             where entry.m_mapIds.Contains(mapid)
                                             select entry);
            IEnumerator<Monster> enumerator = monsters.GetEnumerator();
            enumerator.Reset();
            int i = 0;
            while (enumerator.MoveNext() && i < number)
            {
                group.AddMember(enumerator.Current);
                i++;
            }

            MonsterGroup = group;
            return group;
        }

        #endregion

        #region Properties

        public uint GfxId
        {
            get;
            set;
        }

        public MonsterRaceIdEnum Race
        {
            get;
            set;
        }

        public List<MonsterGrade> Grades
        {
            get;
            set;
        }

        public MonsterRaceIdEnum ParentMonsterId
        {
            get;
            set;
        }

        public static MonsterGroup MonsterGroup
        {
            get;
            set;
        }

        #endregion

        public override string ToString()
        {
            return String.Format("Monster \"{0}\" <Id:{1}>", Enum.GetName(typeof (MonsterIdEnum), Id), Id);
        }

        public override FightTeamMemberInformations ToNetworkTeamMember()
        {
            throw new NotImplementedException();
        }

        public override GameFightFighterInformations ToNetworkFighter()
        {
            throw new NotImplementedException();
        }
    }
}