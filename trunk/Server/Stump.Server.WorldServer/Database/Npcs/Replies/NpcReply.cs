#region License GNU GPL

// NpcReply.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Conditions;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    public abstract class NpcReply : ParameterizableRecord
    {
        public NpcReply(NpcReplyRecord record)
        {
            Record = record;
        }

        public int Id
        {
            get { return Record.Id; }
            set { Record.Id = value; }
        }


        public int ReplyId
        {
            get { return Record.ReplyId; }
            set { Record.ReplyId = value; }
        }

        public int MessageId
        {
            get { return Record.MessageId; }
            set { Record.MessageId = value; }
        }

        public ConditionExpression CriteriaExpression
        {
            get { return Record.CriteriaExpression; }
            set { Record.CriteriaExpression = value; }
        }

        public NpcMessage Message
        {
            get { return Record.Message; }
            set { Record.Message = value; }
        }

        public NpcReplyRecord Record
        {
            get;
            private set;
        }

        public virtual bool Execute(Npc npc, Character character)
        {
            if (Record.CriteriaExpression != null && !Record.CriteriaExpression.Eval(character))
            {
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 34);
                return false;
            }

            return true;
        }
    }
}