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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Dialog;
using NpcEx = Stump.DofusProtocol.D2oClasses.Npc;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcTemplate
    {
        private NpcEx m_npc;
        public NpcTemplate(NpcEx npc)
        {
            m_npc = npc;
            Look = npc.look.ToEntityLook();
        }

        public int Id
        {
            get { return m_npc.id; }
        }

        public string Name
        {
            get;
            set;
        }

        public SexTypeEnum Sex
        {
            get { return m_npc.gender != 0 ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE; }
        }

        public EntityLook Look
        {
            get;
            set;
        }

        public NpcDialogQuestion MessageEntry
        {
            get;
            set;
        }

        public bool CanSpeak
        {
            get { return MessageEntry != null; }
        }
    }
}