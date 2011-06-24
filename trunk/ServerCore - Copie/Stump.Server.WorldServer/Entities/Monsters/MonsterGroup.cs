
using System;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.World.Entities.Actors;

namespace Stump.Server.WorldServer.World.Entities.Monsters
{
    public class MonsterGroup : Actor,IAttackable, IAligned
    {

        protected MonsterGroup(long id, ExtendedLook look, VectorIsometric position, uint firstNameId, uint lastNameId)
            :base(id,"",look,position)
        {
            //MainCreature = mainCreature;
            // Monsters = monsters;
            //AgeBonus = ageBonus;
            //AlignmentSide = alignmentSide;
        }

        //public Monster MainCreature
        //{
        //    get;
        //    set;
        //}

        //public List<Monster> Monsters
        //{
        //    get;
        //    set;
        //}

        //public int Agebonus
        //{
        //    get;
        //    set;
        //}

        //public int AlignmentSide
        //{
        //    get;
        //    set;
        //}


        public GameRolePlayGroupMonsterInformations ToGameRolePlayGroupMonsterInformations()
        {
            return new GameRolePlayGroupMonsterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(),Monster.GID, Monster.Level, Monsters.ToMonsterInGroupInformations(),AgeBonus, AlignmentSide));
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public uint Level
        {
            get { throw new NotImplementedException(); }
        }
    }
}