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
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [Serializable]
    [ActiveRecord("worlds_characters_deleted")]
    public sealed class DeletedWorldCharacterRecord : AuthRecord<DeletedWorldCharacterRecord>
    {

        public DeletedWorldCharacterRecord()
        {
            DeletionDate = DateTime.Now;
        }

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo(Column = "AccountId", NotNull = true)]
        public AccountRecord Account
        {
            get;
            set;
        }

        [BelongsTo(Column = "WorldId", NotNull = true)]
        public WorldRecord World
        {
            get;
            set;
        }

        [KeyProperty(Column = "CharacterId", NotNull = true)]
        public uint CharacterId
        {
            get;
            set;
        }

        [KeyProperty(Column = "DeletionDate", NotNull = true)]
        public DateTime DeletionDate
        {
            get;
            set;
        }


        public static DeletedWorldCharacterRecord FindCharacterById(long id)
        {
            return FindByPrimaryKey(id);
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByAccount(AccountRecord account)
        {
            return FindAll((Restrictions.Eq("Account", account)));
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByServer(WorldRecord world)
        {
            return FindAll((Restrictions.Eq("World", world)));
        }

        public static DeletedWorldCharacterRecord FindCharacterByServerAndCharacterId(WorldRecord world, uint characterId)
        {
            return FindOne(Restrictions.And(Restrictions.Eq("World", world), Restrictions.Eq("CharacterId", characterId)));
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByAccountAndServer(AccountRecord account, WorldRecord world)
        {
            return FindAll(Restrictions.And(Restrictions.Eq("Account", account), Restrictions.Eq("World", world)));
        }

    }
}