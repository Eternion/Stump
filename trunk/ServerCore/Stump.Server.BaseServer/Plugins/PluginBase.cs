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
using System.IO;
using System.Reflection;
using Stump.BaseCore.Framework.Xml;

namespace Stump.Server.BaseServer.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract bool UseConfig
        {
            get;
        }

        public abstract string ConfigFileName
        {
            get;
        }

        public XmlConfigFile ConfigFile
        {
            get;
            protected set;
        }

        #region IPlugin Members

        public abstract string Name
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract string Author
        {
            get;
        }

        public abstract string Version
        {
            get;
        }

        public abstract void Initialize();
        public abstract void Dispose();

        #endregion

        public virtual void LoadConfig(PluginContext context)
        {
            if (UseConfig)
            {
                ConfigFile =
                    new XmlConfigFile(Path.Combine(Path.GetDirectoryName(context.AssemblyPath),
                                                   !string.IsNullOrEmpty(ConfigFileName)
                                                       ? ConfigFileName
                                                       : Name + ".xml"));
                ConfigFile.DefinesVariables(GetType().Assembly);
            }
        }
    }
}