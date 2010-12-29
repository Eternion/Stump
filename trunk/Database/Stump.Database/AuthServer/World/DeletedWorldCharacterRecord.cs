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

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("worlds_characters_deleted")]
    public sealed class DeletedWorldCharacterRecord : ActiveRecordBase<DeletedWorldCharacterRecord>
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

        [KeyProperty(Column = "AccountId", NotNull = true)]
        public uint AccountId
        {
            get;
            set;
        }

        [KeyProperty(Column = "WorldId", NotNull = true)]
        public int WorldId
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


        public AccountRecord Account
        {
            get { return AccountRecord.FindByPrimaryKey(AccountId); }
            set { AccountId = value.Id; }
        }

        public WorldRecord World
        {
            get { return WorldRecord.FindByPrimaryKey(WorldId); }
            set { WorldId = value.Id; }
        }


        public static DeletedWorldCharacterRecord FindCharacterById(long id)
        {
            return FindByPrimaryKey(id);
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByAccountId(uint accountId)
        {
            return FindAll((Restrictions.Eq("AccountId", accountId)));
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByServerId(int worldId)
        {
            return FindAll((Restrictions.Eq("WorldId", worldId)));
        }

        public static DeletedWorldCharacterRecord FindCharacterByServerIdAndCharacterId(int worldId, uint characterId)
        {
            return FindOne(Restrictions.And(Restrictions.Eq("WorldId", worldId), Restrictions.Eq("CharacterId", characterId)));
        }

        public static DeletedWorldCharacterRecord[] FindCharactersByAccountIdAndServerId(uint accountid, int worldId)
        {
            return FindAll(Restrictions.And(Restrictions.Eq("AccountId", accountid), Restrictions.Eq("WorldId", worldId)));
        }

    }
}