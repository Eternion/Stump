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
namespace Stump.Server.WorldServer.Global
{
    /// <summary>
    ///   ParentArea is the representation of a continent. See ParentAreasId for a full list of them.
    ///   We actually don't use ParentArea as it's not needed at the moment.
    /// </summary>
    public class Continent : WorldSpace
    {
        #region Fields

        #endregion

        public override WorldSpaceType Type
        {
            get { return WorldSpaceType.Continent; }
        }
    }
}