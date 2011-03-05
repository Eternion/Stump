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

using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Folder which contains all of sql file to update db
        /// </summary>
        public string UpdateFileDir = "./sql_update/";

        /// <summary>
        /// Database user
        /// </summary>
        public string User = "root";

        /// <summary>
        /// Database password
        /// </summary>
        public string Password = "";

        /// <summary>
        /// Database host
        /// </summary>
        public string Host = "localhost";

        /// <summary>
        /// Database name to connect to
        /// </summary>
        public string Name = "";
    }
}