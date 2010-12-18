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
using System.Collections.Generic;

namespace Stump.Tools.UtilityBot.FileWriter
{
    public class MethodInfo
    {
        #region MethodModifiers enum

        public enum MethodModifiers
        {
            NONE,
            ABSTRACT,
            CONSTANT,
            STATIC,
            NEW,
            OVERRIDE,
            VIRTUAL
        } ;

        #endregion

        public MethodInfo()
        {
            Modifiers = new List<MethodModifiers>();
        }

        public List<MethodModifiers> Modifiers
        {
            get;
            set;
        }

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool ReturnsArray
        {
            get;
            set;
        }

        public string[] Args
        {
            get;
            set;
        }

        public string[] ArgsType
        {
            get;
            set;
        }

        public string[] ArgsDefaultValue
        {
            get;
            set;
        }
    }
}