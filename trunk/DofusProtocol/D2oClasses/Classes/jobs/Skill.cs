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
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Skills")]
	public class Skill : Object
	{
		internal const String MODULE = "Skills";
		public int id;
		public uint nameId;
		public int parentJobId;
		public Boolean isForgemagus;
		public int modifiableItemType;
		public int gatheredRessourceItem;
		public List<int> craftableItemIds;
		public int interactiveId;
		public String useAnimation;
		public Boolean isRepair;
		public int cursor;
		public Boolean availableInHouse;
		
	}
}
