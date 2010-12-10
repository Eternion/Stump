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

namespace Stump.BaseCore.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Variable : Attribute
    {
        public Variable()
        {
            DefinableByConfig = true;
            DefinableRunning = false;
        }


        public Variable(bool definableByConfig)
        {
            DefinableByConfig = definableByConfig;
            DefinableRunning = false;
        }

        public Variable(bool definableByConfig, bool definableRunning)
        {
            DefinableByConfig = definableByConfig;
            DefinableRunning = definableRunning;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this variable can be set by the config
        /// </summary>
        /// <value><c>true</c> if this variable can be set by the config; otherwise, <c>false</c>.</value>
        public bool DefinableByConfig
        {
            get;
            set;
        }

        ///<summary>
        ///  ets or sets a value indicating whether this variable can be set when server is running
        ///</summary>
        ///<value><c>true</c> if this variable can be set when server is running; otherwise, <c>false</c>.</value>
        public bool DefinableRunning
        {
            get;
            set;
        }
    }
}